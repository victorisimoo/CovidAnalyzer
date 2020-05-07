using CovidAnalyzer.Models;
using CustomGenerics.Structures;
using System.Collections.Generic;
using System.Linq;

namespace CovidAnalyzer.Services {
    public class Storage {

        private static Storage _instance = null;

        public static Storage  Instance {
            get {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        //Get cant infected
        public int cantInfected(int regionSelected){
            return patientList.Count((x => x.region == regionSelected && x.infected == true));
        }

        public int cantInfected() {
            return patientList.Count((x => x.infected == true));
        }

        public int cantRecovered(int regionSelected){
            return patientList.Count(x => x.region == regionSelected && x.infected == false && x.analyzed == true && x.recovered == true);
        }

        public int cantRecovered() {
            return patientList.Count(x => x.infected == false && x.analyzed == true && x.recovered == true);
        }


        public int cantAttended(int regionSelected){
            return patientList.Count(x => x.region == regionSelected && x.infected == true && x.analyzed == false && x.recovered == false);
        }
        public int cantSuspect() {
            return patientList.Count(x => x.infected == false && x.analyzed == false && x.recovered == false);
        }

        public string getHospitalName(int hospitalSelecte) {
            return (hospitalsActives.Find(x => x.regionHospital == hospitalSelecte).nameHostpial);
        }


        public int hospitalSelected;
        public AVLStructure<Patient> patientTree = new AVLStructure<Patient>();
        public HashTable<Patient> bedsTable = new HashTable<Patient>();
        public List<Hospital> hospitalsActives = new List<Hospital>();

        public List<Patient> patientList = new List<Patient>();
        public List<Patient> patientReturn = new List<Patient>();
        public List<Patient> patientsRecovered = new List<Patient>();
    }
}