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
                            if (Storage.Instance.bedsTable.find(item.DPI) != null) {
                                Storage.Instance.patientReturn.Add(Storage.Instance.patientConfirmed.Find(x => x.DPI.Contains(item.DPI)));
                            }
                            if (Storage.Instance.hospitalsActives[Storage.Instance.hospitalSelected - 1].waitingPatients.searchValue(item, Patient.compareByName).Count != 0) {
                                Storage.Instance.patientReturn.Add(Storage.Instance.patientSuspect.Find(x => x.DPI.Contains(item.DPI)));
                            }
                        }
                        return View(Storage.Instance.patientReturn.ToPagedList(pageIndex, pageSize));
                    }
                    //If the option selected was Lastname
                } else if (collection["options"] == "lastname") {
                    var searchElementName = new Patient {
                        Lastname = searchString
                    };
                    var foundLastname = Storage.Instance.patientTree.searchValue(searchElementName, Patient.compareByLastName);
                    int count = foundLastname.Count();
                    if (foundLastname != null && count != 0) {
                        foreach (var item in foundLastname) {
                            if (Storage.Instance.bedsTable.find(item.DPI) != null) {
                                Storage.Instance.patientReturn.Add(Storage.Instance.patientConfirmed.Find(x => x.DPI.Contains(item.DPI)));
                            }
                            if (Storage.Instance.hospitalsActives[Storage.Instance.hospitalSelected - 1].waitingPatients.searchValue(item, Patient.compareByName).Count != 0) {
                                Storage.Instance.patientReturn.Add(Storage.Instance.patientSuspect.Find(x => x.DPI.Contains(item.DPI)));
                            }
                        }
                        return View(Storage.Instance.patientReturn.ToPagedList(pageIndex, pageSize));
                    }
                    //If the option selected was Name
                } else if (collection["options"] == "name") {
                    var searchElementLastname = new Patient {
                        Name = searchString
                    };
                    var foundName = Storage.Instance.patientTree.searchValue(searchElementLastname, Patient.compareByName);
                    int count = foundName.Count();
                    if (foundName != null && count != 0) {
                        foreach (var item in foundName) {
                            if (item.region == Storage.Instance.hospitalSelected) {
                                if (Storage.Instance.bedsTable.find(item.DPI) != null) {
                                    Storage.Instance.patientReturn.Add(Storage.Instance.patientConfirmed.Find(x => x.DPI.Contains(item.DPI)));
                                }
                                if(Storage.Instance.patientSuspect.Count != 0) {
                                    if (Storage.Instance.hospitalsActives[Storage.Instance.hospitalSelected - 1].waitingPatients.searchValue(item, Patient.compareByName).Count != 0){
                                        Storage.Instance.patientReturn.Add(Storage.Instance.patientSuspect.Find(x => x.DPI.Contains(item.DPI)));
                                    }
                                }

                            }
                        }
                        return View(Storage.Instance.patientReturn.ToPagedList(pageIndex, pageSize));
                    }
                }
            }
            if (!String.IsNullOrEmpty(idRecovered)) {
                var patientRecovered = new Patient() { DPI = idRecovered };
                var found = Storage.Instance.patientTree.searchValue(patientRecovered, Patient.compareByDPI)[0];
                if (found.region == Storage.Instance.hospitalSelected) {
                    if (Storage.Instance.bedsTable.find(found.DPI) != null) {
                        if (Storage.Instance.hospitalsActives[Storage.Instance.hospitalSelected - 1].healPatient(found)) {
                            TempData["smsRecovered"] = "El paciente ha sido dada de alta.";
                            ViewBag.smsRecovered = TempData["smsRecovered"].ToString();
                            Storage.Instance.patientConfirmed.RemoveAll(x => x.DPI.Contains(found.DPI));
                            Storage.Instance.patientList.Find(x => x.DPI.Contains(found.DPI)).infected = false;
                            Storage.Instance.patientList.Find(x => x.DPI.Contains(found.DPI)).recovered = true;
                            Storage.Instance.patientsRecovered.Add(Storage.Instance.patientList.Find(x => x.DPI.Contains(found.DPI)));
                        }
                    }
                }
            }
        
    

            if (!String.IsNullOrEmpty(idAnalyzed)) {
                Random randomCovid = new Random();
                int posOrNeg = randomCovid.Next(1, 10);
                if (Storage.Instance.patientList.Find(x => x.DPI.Contains(idAnalyzed)).analyzed == false) {
                    if (posOrNeg >= 5) {
                        Storage.Instance.patientList.Find(x => x.DPI.Contains(idAnalyzed)).infected = true;
                        Storage.Instance.patientList.Find(x => x.DPI.Contains(idAnalyzed)).analyzed = true;
                        Storage.Instance.patientConfirmed.Add(Storage.Instance.patientList.Find(x => x.DPI.Contains(idAnalyzed)));
                            if (Storage.Instance.hospitalSelected == Storage.Instance.patientConfirmed.Find(x => x.DPI.Contains(idAnalyzed)).region) {
                                Storage.Instance.hospitalsActives[Storage.Instance.hospitalSelected-1].changeStatus(Storage.Instance.patientConfirmed.Find(x => x.DPI.Contains(idAnalyzed)));
                                Storage.Instance.patientSuspect.RemoveAll(x => x.DPI.Contains(idAnalyzed));
                            }
                        TempData["smsPositive"] = "el paciente está contagiado con COVID-19.";
                        ViewBag.smsPositive = TempData["smsPositive"].ToString();
                    } else {
                        Storage.Instance.patientList.Find(x => x.DPI.Contains(idAnalyzed)).infected = false;
                        Storage.Instance.patientList.Find(x => x.DPI.Contains(idAnalyzed)).analyzed = true;
                        if (Storage.Instance.hospitalSelected == Storage.Instance.patientSuspect.Find(x => x.DPI.Contains(idAnalyzed)).region) {
                            Storage.Instance.hospitalsActives[Storage.Instance.hospitalSelected - 1].changeStatus(Storage.Instance.patientSuspect.Find(x => x.DPI.Contains(idAnalyzed)));
                            Storage.Instance.patientSuspect.RemoveAll(x => x.DPI.Contains(idAnalyzed));
                        }
                        TempData["smsNegative"] = "el paciente no está contagiado con COVID-19.";
                        ViewBag.smsNegative = TempData["smsNegative"].ToString();
                    }
                }
            }

            Storage.Instance.patientReturn.Clear();
            foreach (var item in Storage.Instance.hospitalsActives) {
                if (item.regionHospital == Storage.Instance.hospitalSelected) {
                    foreach (var itemReturn in Storage.Instance.patientConfirmed) {
                        var foundInTree = Storage.Instance.patientTree.searchValue(itemReturn, Patient.compareByName);
                        foreach (var itemFoundTree in foundInTree) {
                            if (Storage.Instance.bedsTable.find(itemFoundTree.DPI) != null && Storage.Instance.patientReturn.Find(x => x.DPI.Contains(itemFoundTree.DPI)) == null) {
                                Storage.Instance.patientReturn.Add(Storage.Instance.patientConfirmed.Find(x => x.DPI.Contains(itemFoundTree.DPI)));
                            }
                        }  
                    }
                    foreach(var itemSuspects in Storage.Instance.patientSuspect) {
                        var foundInWaiting = item.waitingPatients.searchValue(itemSuspects, Patient.compareByName);
                        if (foundInWaiting != null) {
                            foreach (var itemFound in foundInWaiting) {
                                if (itemFound.region == item.regionHospital && itemSuspects.analyzed == false && Storage.Instance.patientReturn.Find(x => x.DPI.Contains(itemFound.DPI)) == null) {
                                    Storage.Instance.patientReturn.Add(Storage.Instance.patientSuspect.Find(x => x.DPI.Contains(itemFound.DPI)));
                                }
                            }
                        }

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
