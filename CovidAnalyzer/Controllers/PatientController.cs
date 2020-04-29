using CovidAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CovidAnalyzer.Services;

namespace CovidAnalyzer.Controllers
{
    public class PatientController : Controller
    {
        public ActionResult PatientsList(string search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                var searchElement = new Patient
                {
                    Name = search,
                    DPI = search,
                    Lastname = search
                };
                var foundDPI = Storage.Instance.patientTree.searchValue((searchElement), Patient.compareByDPI);
                var foundLastname = Storage.Instance.patientTree.searchValue(searchElement, Patient.compareByLastName);
                var foundName = Storage.Instance.patientTree.searchValue(searchElement, Patient.compareByName);

                if (foundDPI != null) {
                    Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.DPI.Contains(foundDPI.DPI));
                }
                else if (foundLastname != null) {
                    Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.Lastname.Contains(foundLastname.Lastname));
                }
                else if (foundName != null) {
                    Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.Name.Contains(foundName.Name));
                }
            }
            

            return View();
        }

        [System.Web.Services.WebMethod]
        public static void Search(string search)
        {
            var searchElement = new Patient
            {
                Name = search,
                DPI = search,
                Lastname = search
            };
            var foundDPI = Storage.Instance.patientTree.searchValue((searchElement), Patient.compareByDPI);
            var foundLastname = Storage.Instance.patientTree.searchValue(searchElement, Patient.compareByLastName);
            var foundName = Storage.Instance.patientTree.searchValue(searchElement, Patient.compareByName);

            if (foundDPI != null)
            {
                Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.DPI.Contains(foundDPI.DPI));
            }
            else if (foundLastname != null)
            {
                Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.Lastname.Contains(foundLastname.Lastname));
            }
            else if (foundName != null)
            {
                Storage.Instance.patientReturn = Storage.Instance.patientList.Find(x => x.Name.Contains(foundName.Name));
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
                return RedirectToAction("InitialPage", "Home");
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
