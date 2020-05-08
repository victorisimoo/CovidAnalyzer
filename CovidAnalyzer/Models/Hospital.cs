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

        //Class constructor
        public Hospital(){
            waitingPatients = new AVLStructure<Patient>();
            attendedPatients = new PriorityQueue<Patient>();
        }

        //Method for save hospital
        public bool SaveHospital() {
            try {
                Storage.Instance.hospitalsActives.Add(this);
                return true;
            } catch {
                return false;
            }
        }

        //Method for add patient in hospital
        public bool AddPatient(Patient patientAdd) {
            try { 
                //Validation for elected internal structur 
                if (patientAdd.infected) {
                    if (beadsAvailable <= 10) {
                        Storage.Instance.bedsTable.insert(patientAdd.DPI, patientAdd);
                    }else {
                        attendedPatients.EnqueuePatient(patientAdd, Patient.compareByName, Patient.compareByHour);
                    }
                }else {
                    if (waitingPatients.isEmpty()) {
                        waitingPatients = new AVLStructure<Patient>();
                        waitingPatients.addElement(patientAdd, Patient.compareByDPI);
                    }else {
                        waitingPatients.addElement(patientAdd, Patient.compareByDPI);
                    }
                    
                }
                return true;
            }catch {
                return false;
            }
        }

        //Method for change patient status
        public bool ChangeStatus(Patient patientChange) {
            if (patientChange.infected) {
                waitingPatients.deleteElement(patientChange, Patient.compareByDPI);
                AddPatient(patientChange);
                return true;
            }else {
                waitingPatients.deleteElement(patientChange, Patient.compareByDPI);
                return false;
            }
        }

        //Method to change hospital beds
        public bool HealPatient(Patient patient) {
            try {
                Storage.Instance.bedsTable.delete(patient.DPI);
                Patient newPatient = attendedPatients.DequeuePatient(Patient.compareByName, Patient.compareByHour);
                if (newPatient != null) {
                    Storage.Instance.bedsTable.insert(newPatient.Name, newPatient);
                }
                return true;
            }catch {
                return false;
            }

        }
    }
}