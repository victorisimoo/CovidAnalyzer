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

        public bool addPatientHold(Patient newPatient) {
            this.beadsAvailable++;
            bool response = false;
            try {
                if ((beadsAvailable <= 10)){
                    Storage.Instance.bedsTable.insert(newPatient.DPI, newPatient);
                    patientsHold.EnqueuePatient(newPatient, Patient.compareByName, Patient.compareByHour);
                    response = true;
                }else{
                    response = false;
                }
            }catch {
                response = false;
            }
            return response;
        }

        public bool addPatientCared(Patient newPatient){
            try {
                patientsCared.EnqueuePatient(newPatient, Patient.compareByName, Patient.compareByHour);
                return true;
            } catch {
                return false;
            }
        }
    }
}