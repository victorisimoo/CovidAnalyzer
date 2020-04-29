using CovidAnalyzer.Models;
using CustomGenerics.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidAnalyzer.Services {
    public class Storage {

        private static Storage _instance = null;

        public static Storage  Instance {
            get {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        public AVLStructure<Patient> patientTree = new AVLStructure<Patient>();
        public List<Patient> patientList = new List<Patient>();

    }
}