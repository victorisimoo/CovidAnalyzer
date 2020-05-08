using CovidAnalyzer.Models;
using CustomGenerics.Structures;
using System.Collections.Generic;
using System.Linq;

namespace CovidAnalyzer.Services {
    public class Storage {

        //Instance element
        private static Storage _instance = null;

        //Method for instance
        public static Storage Instance {
            get {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        //Get cant infected
        public int CantInfected(int regionSelected) {
            return patientList.Count((x => x.region == regionSelected && x.infected == true));
        }

        //Get cant infected
        public int CantInfected() {
            return patientList.Count((x => x.infected == true));
        }

        //Get cant recovered
        public int CantRecovered(int regionSelected){
            return patientList.Count(x => x.region == regionSelected && x.infected == false && x.analyzed == true && x.recovered == true);
        }

        //Get cant recovered
        public int CantRecovered() {
            return patientList.Count(x => x.recovered == true);
        }

        //Get cant attended
        public int CantAttended(int regionSelected){
            return patientList.Count(x => x.region == regionSelected && x.infected == false && x.analyzed == false && x.recovered == false);
        }

        //Get cant suspect
        public int CantSuspect() {
            return patientList.Count(x => x.infected == false && x.analyzed == false && x.recovered == false);
        }

        //Get hospital name
        public string GetHospitalName(int hospitalSelecte) {
                return (hospitalsActives.Find(x => x.regionHospital == hospitalSelecte).nameHostpial);
        }

        //Storage variables
        public int hospitalSelected;
        public AVLStructure<Patient> patientTree = new AVLStructure<Patient>();
        public HashTable<Patient> bedsTable = new HashTable<Patient>();
        public List<Hospital> hospitalsActives = new List<Hospital>();
        public List<Patient> patientList = new List<Patient>();
        public List<Patient> patientReturn = new List<Patient>();
    }
}