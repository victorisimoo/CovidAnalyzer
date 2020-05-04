using System.Web.Mvc;
using CovidAnalyzer.Services;
using CovidAnalyzer.Models;
using System;
using System.Linq;

namespace CovidAnalyzer.Controllers {
    public class HospitalController : Controller {
        // GET: Hospital
        public ActionResult Index() {
            return View();
        }
    
        public ActionResult HospitalList(string idHospital) {
            if (!String.IsNullOrEmpty((idHospital))) {
                if(idHospital == "Hospital de Guatemala") { Storage.Instance.hospitalSelected = 1; }
                else if (idHospital == "Hospital de Quetzaltenango") { Storage.Instance.hospitalSelected = 2; }
                else if (idHospital == "Hospital de Oriente") { Storage.Instance.hospitalSelected = 3; }
                else if (idHospital == "Hospital de Escuintla") { Storage.Instance.hospitalSelected = 4; }
                else if (idHospital == "Hospital de Peten") { Storage.Instance.hospitalSelected = 5; }
                return RedirectToAction("Hospital");
            }
            return View("HospitalList");
        }

        public ActionResult Hospital(FormCollection collection, string searchButton, string searchString, string idPatient) {
            
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
                            if (item.region == Storage.Instance.hospitalSelected) {
                                Storage.Instance.patientReturn.Add(Storage.Instance.patientConfirmed.Find(x => x.DPI.Contains(item.DPI)));
                            }
                        }
                        return View(Storage.Instance.patientReturn);
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
                            if(item.region == Storage.Instance.hospitalSelected) {
                                Storage.Instance.patientReturn.Add(Storage.Instance.patientConfirmed.Find(x => x.DPI.Contains(item.DPI)));
                            }
                        }
                        return View(Storage.Instance.patientReturn);
                    }
                    //If the option selected was Name
                } else if (collection["options"] == "name") {
                    var searchElementLastname = new Patient {
                        Name = searchString
                    };
                    var foundName = Storage.Instance.patientTree.searchValue(searchElementLastname, Patient.compareByName);
                    int count = foundName.Count();
                    if (foundName != null && count != 0) {
                        foreach (var item in foundName){
                            if (item.region == Storage.Instance.hospitalSelected) {
                                Storage.Instance.patientReturn.Add(Storage.Instance.patientConfirmed.Find(x => x.DPI.Contains(item.DPI)));
                            }
                        }
                        return View(Storage.Instance.patientReturn);
                    }
                }    
            }
            if (!String.IsNullOrEmpty(idPatient)) {
                var patientRecovered = new Patient() { Name = idPatient };
                var found = Storage.Instance.patientTree.searchValue(patientRecovered, Patient.compareByName)[0];
                foreach (var item in Storage.Instance.hospitalsActives) {
                    if (item.regionHospital == Storage.Instance.hospitalSelected) {
                        if (Storage.Instance.bedsTable.find(found.DPI) != null) {
                            if (item.healPatient(found)) {
                                TempData["smsRecovered"] = "El paciente ha sido dada de alta.";
                                ViewBag.ssmsRecovered = TempData["smsRecovered"].ToString();
                                Storage.Instance.patientConfirmed.RemoveAll(x => x.DPI.Contains(found.DPI));
                                Storage.Instance.patientList.Find(x => x.DPI.Contains(found.DPI)).infected = false;
                                Storage.Instance.patientsRecovered.Add(found);
                            }
                        }
                    }
                }
            }
            Storage.Instance.patientReturn.Clear();
            foreach (var item in Storage.Instance.patientConfirmed) {
                if (item.region == Storage.Instance.hospitalSelected) {
                    Storage.Instance.patientReturn.Add(item);
                }
            }
            return View(Storage.Instance.patientReturn);
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
