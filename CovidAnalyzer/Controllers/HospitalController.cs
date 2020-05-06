using System.Web.Mvc;
using CovidAnalyzer.Services;
using CovidAnalyzer.Models;
using System;
using System.Linq;
using PagedList;
using System.Collections.Generic;

namespace CovidAnalyzer.Controllers {
    public class HospitalController : Controller {
        // GET: Hospital
        public ActionResult Index() {
            return View();
        }

        public ActionResult HospitalList(string idHospital) {
            if (!String.IsNullOrEmpty((idHospital))) {
                if (idHospital == "Hospital de Guatemala") { Storage.Instance.hospitalSelected = 1; }
                else if (idHospital == "Hospital de Quetzaltenango") { Storage.Instance.hospitalSelected = 2; }
                else if (idHospital == "Hospital de Oriente") { Storage.Instance.hospitalSelected = 3; }
                else if (idHospital == "Hospital de Escuintla") { Storage.Instance.hospitalSelected = 4; }
                else if (idHospital == "Hospital de Peten") { Storage.Instance.hospitalSelected = 5; }
                return RedirectToAction("Hospital");
            }

            return View("HospitalList");
        }

        public ActionResult Hospital(FormCollection collection, int? page, string searchButton, string searchString, string idRecovered, string idAnalyzed) {

            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            int regionSelected = Storage.Instance.hospitalSelected;
            Storage.Instance.patientReturn.Clear();
            if (!String.IsNullOrEmpty(searchButton)) {
                //If the option selected was DPI
                if (collection["options"] == "dpi") {
                    var searchElementDPI = new Patient {
                        DPI = searchString
                    };
                    var foundDPI = Storage.Instance.patientTree.searchValue((searchElementDPI), Patient.compareByDPI);
                    int count = foundDPI.Count();
                    if (foundDPI != null && count != 0) {
                        foreach (var item in foundDPI) {
                            var foundStatus = Storage.Instance.patientList.Find(x => x.DPI.Contains(item.DPI));
                            if (Storage.Instance.bedsTable.find(item.DPI) != null) {
                                Storage.Instance.patientReturn.Add(foundStatus);
                            } else {
                                if(foundStatus.analyzed == false && foundStatus.recovered == false) {
                                    Storage.Instance.patientReturn.Add(foundStatus);
                                }
                            }
                        }
                        return View(Storage.Instance.patientReturn.ToPagedList(pageIndex, pageSize));
                    }
                    //If the option selected was Lastname
                } else if (collection["options"] == "lastname") {
                    var searchElementLastname = new Patient {
                        Lastname = searchString
                    };
                    var foundLastName = Storage.Instance.patientTree.searchValue((searchElementLastname), Patient.compareByLastName);
                    int count = foundLastName.Count();
                    if (foundLastName != null && count != 0) {
                        foreach (var item in foundLastName) {
                            var foundStatus = Storage.Instance.patientList.Find(x => x.DPI.Contains(item.DPI));
                            if (Storage.Instance.bedsTable.find(item.DPI) != null) {
                                Storage.Instance.patientReturn.Add(foundStatus);
                            } else {
                                if(foundStatus.analyzed == false && foundStatus.recovered == false) {
                                    Storage.Instance.patientReturn.Add(foundStatus);
                                }
                            }
                        }
                        return View(Storage.Instance.patientReturn.ToPagedList(pageIndex, pageSize));
                    }
                    //If the option selected was Name
                } else if (collection["options"] == "name") {
                    var searchElementName = new Patient {
                        Name = searchString
                    };
                    var foundName = Storage.Instance.patientTree.searchValue((searchElementName), Patient.compareByName);
                    int count = foundName.Count();
                    if (foundName != null && count != 0) {
                        foreach (var item in foundName) {
                            var foundStatus = Storage.Instance.patientList.Find(x => x.DPI.Contains(item.DPI));
                            if (Storage.Instance.bedsTable.find(item.DPI) != null) {
                                Storage.Instance.patientReturn.Add(foundStatus);
                            } else {
                                if(foundStatus.analyzed == false && foundStatus.recovered == false) {
                                    Storage.Instance.patientReturn.Add(foundStatus);
                                }
                            }
                        }
                        return View(Storage.Instance.patientReturn.ToPagedList(pageIndex, pageSize));
                    }
                } else {
                    foreach (var itemRegion in Storage.Instance.patientList) {
                        if (Storage.Instance.hospitalsActives[regionSelected - 1].regionHospital == itemRegion.region) {
                            if (Storage.Instance.bedsTable.find(itemRegion.DPI) != null) {
                                Storage.Instance.patientReturn.Add(itemRegion);
                            } else if (itemRegion.analyzed == false) {
                                Storage.Instance.patientReturn.Add(itemRegion);
                            }
                        }
                    }
                }
            }
            //Recovered patients
            if (!String.IsNullOrEmpty(idRecovered)) {
                var patientRecovered = new Patient() { DPI = idRecovered };
                var found = Storage.Instance.patientTree.searchValue(patientRecovered, Patient.compareByDPI)[0];
                var foundStatus = Storage.Instance.patientList.Find(x => x.DPI.Contains(found.DPI));
                if (found.region == Storage.Instance.hospitalSelected) {
                    if (Storage.Instance.bedsTable.find(found.DPI) != null && foundStatus.recovered == false) {
                        if (Storage.Instance.hospitalsActives[regionSelected - 1].healPatient(found)) {
                            TempData["smsRecovered"] = "El paciente ha sido dada de alta.";
                            ViewBag.smsRecovered = TempData["smsRecovered"].ToString();
                            foundStatus.infected = false;
                            foundStatus.recovered = true;
                            Storage.Instance.patientsRecovered.Add(foundStatus);
                        }
                    }
                }
            }
            //Analyzed Patient
            if (!String.IsNullOrEmpty(idAnalyzed)) {
                Random randomCovid = new Random();
                int posOrNeg = randomCovid.Next(1, 10);
                var patientAnalyzed = Storage.Instance.patientList.Find(x => x.DPI.Contains(idAnalyzed));
                if (Storage.Instance.hospitalSelected == patientAnalyzed.region) {
                    if (patientAnalyzed.analyzed == false) {
                        if (posOrNeg >= 5) {
                            patientAnalyzed.infected = true;
                            patientAnalyzed.analyzed = true;
                            Storage.Instance.hospitalsActives[regionSelected - 1].changeStatus(patientAnalyzed);

                            TempData["smsPositive"] = "el paciente está contagiado con COVID-19.";
                            ViewBag.smsPositive = TempData["smsPositive"].ToString();
                        } else {
                            patientAnalyzed.infected = false;
                            patientAnalyzed.analyzed = true;
                            Storage.Instance.hospitalsActives[regionSelected - 1].changeStatus(patientAnalyzed);

                            TempData["smsNegative"] = "el paciente no está contagiado con COVID-19.";
                            ViewBag.smsNegative = TempData["smsNegative"].ToString();
                        }
                    }
                }
            }
            //Show the patients
            Storage.Instance.patientReturn.Clear();
            foreach (var itemRegion in Storage.Instance.patientList) {
                if (Storage.Instance.hospitalsActives[regionSelected - 1].regionHospital == itemRegion.region) {
                    if (Storage.Instance.bedsTable.find(itemRegion.DPI) != null) {
                        Storage.Instance.patientReturn.Add(itemRegion);
                    } else if (itemRegion.analyzed == false) {
                        Storage.Instance.patientReturn.Add(itemRegion);
                    }
                }
            }
            IPagedList<Patient> listPatient = null;
            List<Patient> auxiliarPatientList = new List<Patient>();
            auxiliarPatientList = Storage.Instance.patientReturn;
            listPatient = auxiliarPatientList.ToPagedList(pageIndex, pageSize);
            return View(listPatient);
        }

        // GET: Hospital/Details/5
        public ActionResult Details(int id) {
            return View();
        }

        // GET: Hospital/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Hospital/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection) {
            try {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }

        // GET: Hospital/Edit/5
        public ActionResult Edit(int id) {
            return View();
        }

        // POST: Hospital/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection) {
            try {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }

        // GET: Hospital/Delete/5
        public ActionResult Delete(int id) {
            return View();
        }

        // POST: Hospital/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection) {
            try {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }
    }
}
