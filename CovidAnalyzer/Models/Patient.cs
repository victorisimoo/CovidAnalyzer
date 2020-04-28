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


    }
}