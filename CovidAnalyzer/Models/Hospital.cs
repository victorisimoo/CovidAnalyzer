using CovidAnalyzer.Services;
using CustomGenerics.Structures;

namespace CovidAnalyzer.Models {
    public class Hospital {

        //Parameters class.
        public string nameHostpial;
        public string address;
        public int regionHospital;
        public int beadsAvailable;
        public int patientsRegisters;
        public AVLStructure<Patient> waitingPatients;
        public PriorityQueue<Patient> attendedPatients;

        public Hospital(){
            waitingPatients = new AVLStructure<Patient>();
            attendedPatients = new PriorityQueue<Patient>();
        }

        public bool saveHospital() {
            try {
                Storage.Instance.hospitalsActives.Add(this);
                return true;
            } catch {
                return false;
            }
        }

        //Method for add patient in hospital
        public bool addPatient(Patient patientAdd) {
            try { 
                //Validation for elected internal structur 
                if (patientAdd.infected) {
                    if (beadsAvailable <= 10) {
                        Storage.Instance.bedsTable.insert(patientAdd.DPI, patientAdd);
                    }else {
                        attendedPatients.EnqueuePatient(patientAdd, Patient.compareByName, Patient.compareByHour);
                    }
                }else {
                    waitingPatients.addElement(patientAdd, Patient.compareByDPI);
                }
                return true;
            }catch {
                return false;
            }
        }

        //Method for change patient status
        public bool changeStatus(Patient patientChange) {
            if (!patientChange.infected) {
                waitingPatients.deleteElement(patientChange, Patient.compareByDPI);
                addPatient(patientChange);
                return true;
            }else {
                waitingPatients.deleteElement(patientChange, Patient.compareByDPI);
                return false;
            }
        }

        //Method to change hospital beds
        public bool healPatient(Patient patient) {
            try {                 
                Storage.Instance.bedsTable.delete(patient.DPI);
                Patient newPatient = attendedPatients.DequeuePatient(null, Patient.compareByName, Patient.compareByHour);
                Storage.Instance.bedsTable.insert(newPatient.Name, newPatient);
                return true;
            }catch {
                return false;
            }

        }
    }
}