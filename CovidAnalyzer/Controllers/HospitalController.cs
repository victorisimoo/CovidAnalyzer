using System.Web.Mvc;
using CovidAnalyzer.Services;
using CovidAnalyzer.Models;

namespace CovidAnalyzer.Controllers {
    public class HospitalController : Controller {
        // GET: Hospital
        public ActionResult Index() {
            return View();
        }

        public ActionResult HospitalList(string idHospital) {
            if (!string.IsNullOrEmpty(idHospital)) {
                TempData["Hospital"] = idHospital;
                ViewBag.Hospital = TempData["Hospital"].ToString();
            }
            return View("HospitalList");
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
