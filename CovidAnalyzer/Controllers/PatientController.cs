using CovidAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CovidAnalyzer.Services;

namespace CovidAnalyzer.Controllers {
    public class PatientController : Controller {
        public ActionResult PatientsList(FormCollection collection, string searchButton, string searchString, string id, string create) {
           
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
                            Storage.Instance.patientReturn.Add(Storage.Instance.patientList.Find(x => x.DPI.Contains(item.DPI)));
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
                            Storage.Instance.patientReturn.Add(Storage.Instance.patientList.Find(x => x.DPI.Contains(item.DPI)));
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
                            Storage.Instance.patientReturn.Add(Storage.Instance.patientList.Find(x => x.DPI.Contains(item.DPI)));
                        }
                        return View(Storage.Instance.patientReturn);
                    }
                }
                else {
                    Storage.Instance.patientReturn.Clear();
                    return View(Storage.Instance.patientList);
                }
                
            }

            foreach (var item in Storage.Instance.patientList) {
                if(item.analyzed == false) {
                    if (item.infected) {
                        Storage.Instance.patientConfirmed.Add(item);
                        item.analyzed = true;
                    }
                }
            }

            if (!String.IsNullOrEmpty(id)) {
                Random randomCovid = new Random();
                int posOrNeg = randomCovid.Next(1, 10);
                if (Storage.Instance.patientList.Find(x => x.Name.Contains(id)).analyzed==false) {
                    if (posOrNeg >= 5)
                    {
                        Storage.Instance.patientList.Find(x => x.Name.Contains(id)).infected = true;
                        Storage.Instance.patientList.Find(x => x.Name.Contains(id)).analyzed = true;
                        Storage.Instance.patientConfirmed.Add(Storage.Instance.patientList.Find(x => x.Name.Contains(id)));
                        TempData["smsPositive"] = "El paciente está contagiado con COVID-19.";
                        ViewBag.smsPositive = TempData["smsPositive"].ToString();
                    }
                    else
                    {
                        Storage.Instance.patientList.Find(x => x.Name.Contains(id)).infected = false;
                        Storage.Instance.patientList.Find(x => x.Name.Contains(id)).analyzed = true;
                        TempData["smsNegative"] = "El paciente no está contagiado con COVID-19.";
                        ViewBag.smsNegative = TempData["smsNegative"].ToString();
                    }
                }
            }

            if (!String.IsNullOrEmpty(create)) {
                return RedirectToAction("Create");
            }
            Storage.Instance.patientReturn.Clear();
            

            return View(Storage.Instance.patientList);
        }


        // GET: Patient
        public ActionResult Index() {
            return View();
        }

        public ActionResult PatientList() {
            return View("PatientsList", Storage.Instance.patientList);
        }

        public ActionResult Create(){
            return View("Create");
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id) {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection) {
            try
            {
                var newPatient = new Patient {
                    Name = collection["name"],
                    Lastname = collection["lastname"],
                    DPI = collection["dpi"],
                    Years = Convert.ToInt16(collection["years"]),
                    Departament = collection["department"],
                    Municipality = collection["municipality"],
                    Symptom = collection["symptom"],
                    Description = collection["description"]
                };
                newPatient.savePatient();
                return RedirectToAction("PatientsList", "Patient");
            } catch {
                return View();
            }
}

        // GET: Patient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
