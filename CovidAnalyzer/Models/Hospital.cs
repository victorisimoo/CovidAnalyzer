using CovidAnalyzer.Services;
using CustomGenerics.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidAnalyzer.Models {
    public class Hospital {

        //Parameters class.
        public string nameHostpial;
        public string address;
        public int regionHospital;
        public int beadsAvailable;
        public int patientsRegisters;
        public PriorityQueue<Patient> patientsHold;
        public PriorityQueue<Patient> patientsCared;

        public Hospital(){
            patientsHold = new PriorityQueue<Patient>();
            patientsCared = new PriorityQueue<Patient>();
        }

        public bool saveHospital() {
            try {
                Storage.Instance.hospitalsActives.Add(this);
                return true;
            } catch {
                return false;
            }
        }

        public bool addPatientHold(Patient newPatient){
            try {
                //patientsHold.AddNode(newPatient, );
                return true;
            }catch {
                return false;
            }
        }

        public bool addPatientCared(Patient newPatient){
            try {
                //patientsHold.AddNode(newPatient, );
                return true;
            } catch {
                return false;
            }
        }










    }
}