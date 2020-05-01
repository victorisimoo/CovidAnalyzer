using CovidAnalyzer.Models;
using CovidAnalyzer.Services;
using System;
using System.IO;
using System.Web.Mvc;

namespace CovidAnalyzer.Controllers {
    public class HomeController : Controller {

        public ActionResult InitialPage() {
            createHostpitals();
            return View();
        }

        public ActionResult Virus() {
            return View("Virus");
        }

        public ActionResult Panel(){
            return View("Panel");
        }

        public ActionResult PatientsList(){
            return View();
        }

        public void createHostpitals() {
            if (Storage.Instance.hospitalsActives.Count == 0) {
                var ubication = Server.MapPath($"~/info/hospitals.csv");
                using (var fileStream = new FileStream(ubication, FileMode.Open)) {
                    using (var streamReader = new StreamReader(fileStream)) {
                        Hospital newHospital;
                        while (streamReader.Peek() >= 0) {
                            newHospital = new Hospital();
                            String lineReader = streamReader.ReadLine();
                            String[] parts = lineReader.Split(',');
                            newHospital.nameHostpial = parts[0];
                            newHospital.regionHospital = Convert.ToInt32(parts[1]);
                            newHospital.beadsAvailable = Convert.ToInt32(parts[2]);
                            newHospital.address = parts[3];
                            newHospital.saveHospital();
                        }
                    }
                }
            }

        }

    }
}