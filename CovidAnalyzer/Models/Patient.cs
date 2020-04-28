using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidAnalyzer.Models {
    public class Patient {

        //Patient parameters.
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int DPI { get; set; }
        public int Years { get; set; }
        public string Departament { get; set; }
        public string Municipality { get; set; }
        public string Symptom { get; set; }
        public string Description { get; set; }

        //Parameters for defined patient status.
        bool infected { get; set; }
        int typePatient { get; set; }

        //Method for return infected probability.
        public bool getProbability(string description){
            int probability = 5;
            //Modificar linea
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
                this.infected = true;
                return true;
            }else {
                this.infected = true;
                return false;
            }
        }

        //Method for defined patient type.
        public void getTypePatient(){
            if ((this.Years >= 60 && this.Years > 0)){
                if (this.infected){
                    this.typePatient = 1;
                }else {
                    this.typePatient = 4;
                }
            }else if (this.Years < 60 && this.Years > 18){
                if (this.infected) {
                    this.typePatient = 3;
                }else {
                    this.typePatient = 7;
                }
            }else if (this.Years <= 18 && this.Years > 0) {
                if (this.infected) {
                    this.typePatient = 5;
                } else {
                    this.typePatient = 8;
                }
            }else if (this.Years == 0) {
                if (this.infected){
                    this.typePatient = 2;
                }else {
                    this.typePatient = 6;
                }
            }
        }

        public string getHospitalRegion() {
            
            string[] region_1 = { "Guatemala", "Chimaltenango", "Sacatepéquez" };
            string[] region_2 = { "Quetzaltenango", "Totonicapan", "Huehuetenango", "San Marcos" };
            string[] region_3 = { "Izabal", "Zacapa", "Chiquimula", "Jalapa", "El Progreso" };
            string[] region_4 = { "Jutiapa", "Santa Rosa", "Escuintla", "Suchitepéquez", "Retalhuleu" };
            string[] region_5 = { "Alta Verapaz", "Baja Verapaz", "Petén", "Quiché", "Sololá" };
            //Hospital 1
            foreach (var departament in region_1) {
                if(departament == Departament) {
                    return "Guatemala";
                }
            }
            //Hospital 2
            foreach (var departament in region_2) {
                if(departament == Departament) {
                    return "Quetzaltenango";
                }
            }
            //Hospital 3
            foreach (var departament in region_3) {
                if (departament == Departament) { 
                    return "Oriente";
                }
            }
            //Hospital 4
            foreach (var departament in region_4) {
                if (departament == Departament){
                    return "Escuintla";
                }
            }
            //Hospital 5
            foreach (var departament in region_5) {
                if (departament == Departament) {
                    return "Petén";
                }
            }
            //Not found
            return "No se encontró";
        }


    }
}