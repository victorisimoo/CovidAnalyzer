using CovidAnalyzer.Services;
using System.Linq;
using System;

namespace CovidAnalyzer.Models {
    public class Patient {

        //Patient parameters
        static int CodeUser = 0;
        public int IdPatient { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string DPI { get; set; }
        public int Years { get; set; }
        public string Departament { get; set; }
        public string Municipality { get; set; }
        public string Symptom { get; set; }
        public string Description { get; set; }
        public DateTime dateHourIngress { get; set;}

        //Parameters for defined patient status.
        public bool infected { get; set; }
        public bool analyzed { get; set; }
        public int typePatient { get; set; }
        public int region { get; set; }


        public Patient(int id,string name, string lastname, string dpi){
            this.IdPatient = id;
            this.Name = name;
            this.Lastname = lastname;
            this.DPI = dpi;
            this.analyzed = false;
        }

        //Method for return infected probability.
        public bool getProbability(string description){
            int probability = 5;
            description = description.ToLower();
            string[] descriptionAnalicer = description.Split(' ');
            string[] travel = { "europa", "viaje", "china", "italia", "avión" };
            string[] relatives = { "familiar", "conocido", "vecino", "amigo", "compañero" };
            string[] family = { "hermana", "papá", "mamá", "novia", "esposa", "hermano", "esposo" };
            string[] reunions = { "reunió", "juntó", "vió", "saludó", "mano", "abrazo" };

            foreach (var words in descriptionAnalicer){
                foreach (var item in travel){
                    if (words == item){
                        probability += 10;
                    }
                }
                foreach (var item in relatives){
                    if (words == item){
                        probability += 15;
                    }
                }
                foreach (var item in family){
                    if (words == item) {
                        probability += 30;
                    }
                }
                foreach (var item in reunions){
                    if (words == item){
                        probability += 30;
                    }
                }
            }
            if (probability >= 60){
                return true;
            }else {
                return false;
            }
        }

        //Method for defined patient type.
        public int getTypePatient(){
            if ((this.Years >= 60 && this.Years > 0)){
                if (this.infected){
                    return 1;
                }else {
                    return 4;
                }
            }else if (this.Years < 60 && this.Years > 18){
                if (this.infected) {
                    return 3;
                }else {
                    return 7;
                }
            }else if (this.Years <= 18 && this.Years > 0) {
                if (this.infected) {
                    return 5;
                } else {
                    return 8;
                }
            }else if (this.Years == 0) {
                if (this.infected){
                    return 2;
                }else {
                    return 6;
                }
            }
            return 0;
        }

        //Method for save patient
        public bool savePatient() {
            CodeUser++;
            this.IdPatient = CodeUser;
            try {
                this.dateHourIngress = DateTime.Now;
                this.infected = getProbability(this.Description);
                this.typePatient = getTypePatient();
                this.region = getRegion(this.Departament);
                if (Storage.Instance.hospitalsActives[(this.region-1)].addPatientHold(this)){
                    saveInStructures();
                } else if (Storage.Instance.hospitalsActives[(this.region - 1)].addPatientCared(this)) {
                    saveInStructures();
                }
                return true;
            } catch {
                return false;
            }
        }

        public void saveInStructures(){
            Storage.Instance.patientTree.addElement(new Patient(this.IdPatient, this.Name, this.Lastname, this.DPI), Patient.compareByDPI);
            Storage.Instance.patientList.Add(this);
        }

        public int getRegion(string userDep) {
            string[] region_1 = { "guatemala", "chimaltenango", "sacatepequez" };
            string[] region_2 = { "quetzaltenango", "totonicapan", "huehuetenango", "san marcos" };
            string[] region_3 = { "izabal", "zacapa", "chiquimula", "jalapa", "el progreso" };
            string[] region_4 = { "jutiapa", "santa rosa", "escuintla", "suchitepequez", "retalhuleu" };
            string[] region_5 = { "alta verapaz", "baja verapaz", "peten", "quiche", "solola" };
            //Hospital 1
            foreach (var departament in region_1) {
                if(departament == userDep) {
                    return 1;
                }
            }
            //Hospital 2
            foreach (var departament in region_2) {
                if(departament == userDep) {
                    return 2;
                }
            }
            //Hospital 3
            foreach (var departament in region_3) {
                if (departament == userDep) {
                    return 3;
                }
            }
            //Hospital 4
            foreach (var departament in region_4) {
                if (departament == userDep){
                    return 4;
                }
            }
            //Hospital 5
            foreach (var departament in region_5) {
                if (departament == userDep) {
                    return 5;
                }
            }
            return 0;
        }

        public Patient() { }

        //Method for compare elements
        public static Comparison<Patient> compareByDPI = delegate (Patient patient_one, Patient patient_two) {
            return patient_one.DPI.CompareTo(patient_two.DPI);
        };

        public static Comparison<Patient> compareByName = delegate (Patient patient_one, Patient patient_two) {
            return patient_one.Name.CompareTo(patient_two.Name);
        };

        public static Comparison<Patient> compareByLastName = delegate (Patient patient_one, Patient patient_two) {
            return patient_one.Lastname.CompareTo(patient_two.Lastname);
        };

        public static Comparison<Patient> compareByHour = delegate (Patient patient_one, Patient patient_two) {
            return patient_one.dateHourIngress.CompareTo(patient_two.dateHourIngress);
        };

    }
}