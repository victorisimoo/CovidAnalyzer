using CovidAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CovidAnalyzer.Services;

namespace CovidAnalyzer.Controllers {
    public class PatientController : Controller {
        public ActionResult PatientsList(FormCollection collection, string search, string searchString) {
            if (!String.IsNullOrEmpty(search)) {
                if (collection["options"] == "dpi") {
                    var searchElementDPI = new Patient {
                        DPI = searchString
                    };
                    var foundDPI = Storage.Instance.patientTree.searchValue((searchElementDPI), Patient.compareByDPI);
                    if (foundDPI != null) {
                        Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.DPI.Contains(foundDPI.DPI));
                    }
                } else if (collection["options"] == "name") {
                    var searchElementName = new Patient {
                        Name = searchString
                    };
                    var foundLastname = Storage.Instance.patientTree.searchValue(searchElementName, Patient.compareByName);
                    if (foundLastname != null) {
                        Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.Lastname.Contains(foundLastname.Lastname));
                    }
                } else if (collection["options"] == "lastname") {
                    var searchElementLastname = new Patient {
                        Lastname = searchString
                    };
                    var foundName = Storage.Instance.patientTree.searchValue(searchElementLastname, Patient.compareByLastName);
                    if (foundName != null) {
                        Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.Name.Contains(foundName.Name));
                    }
                }
            }
            return View();
        }

        

        public void modal(object sender, EventArgs e, FormCollection collection, string search, string searchString)
        {
            if (!String.IsNullOrEmpty(search))
            {
                if (collection["options"] == "dpi")
                {
                    var searchElementDPI = new Patient
                    {
                        DPI = searchString
                    };
                    var foundDPI = Storage.Instance.patientTree.searchValue((searchElementDPI), Patient.compareByDPI);
                    if (foundDPI != null)
                    {
                        Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.DPI.Contains(foundDPI.DPI));
                    }
                }
                else if (collection["options"] == "name")
                {
                    var searchElementName = new Patient
                    {
                        Name = searchString
                    };
                    var foundLastname = Storage.Instance.patientTree.searchValue(searchElementName, Patient.compareByName);
                    if (foundLastname != null)
                    {
                        Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.Lastname.Contains(foundLastname.Lastname));
                    }
                }
                else if (collection["options"] == "lastname")
                {
                    var searchElementLastname = new Patient
                    {
                        Lastname = searchString
                    };
                    var foundName = Storage.Instance.patientTree.searchValue(searchElementLastname, Patient.compareByLastName);
                    if (foundName != null)
                    {
                        Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.Name.Contains(foundName.Name));
                    }
                }
            }
        }

        // GET: Patient
        public ActionResult Index() {
            return View();
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
            try {
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
