using CovidAnalyzer.Models;
using CustomGenerics.Structures;
using System.Collections.Generic;

namespace CovidAnalyzer.Services {
    public class Storage {

        private static Storage _instance = null;

        public static Storage  Instance {
            get {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        public int hospitalSelected;
        public AVLStructure<Patient> patientTree = new AVLStructure<Patient>();
        public HashTable<Patient> bedsTable = new HashTable<Patient>();
        public List<Hospital> hospitalsActives = new List<Hospital>();

        public List<Patient> patientList = new List<Patient>();
        public List<Patient> patientConfirmed = new List<Patient>();
        public List<Patient> patientReturn = new List<Patient>();


    }
}