using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAQ.DOM.Services.Traitements
{
    public class ConversionBd
    {
        string resultat = string.Empty;

        public ConversionBd()
        {
        }

        public string TypeDomaineToString(string type)
        {

            switch (type)
            {
                case "C":
                    resultat = "Caractères";
                    break;
                case "N":
                    resultat = "Numérique";
                    break;
                case "D":
                    resultat = "Date";
                    break;
            }
            return resultat;
        }

        public string TypeDomaineToChar(string type)
        {
            switch (type)
            {
                case "Caractères":
                case "Characters":
                    resultat = "C";
                    break;
                case "Numérique":
                case "Numeric":
                    resultat = "N";
                    break;
                case "Date":
                    resultat = "D";
                    break;
            }
            return resultat;

        }
        public string StatutDomaineToChar(string statut)
        {
            switch (statut)
            {
                case "Actif":
                case "Active":
                    resultat = "A";
                    break;
                case "Inactif":
                case "Inactive":
                    resultat = "I";
                    break;

            }
            return resultat;

        }
        public string StatutDomaineToString(string statut)
        {
            switch (statut)
            {
                case "A":
                    resultat = "Actif";
                    break;
                case "I":
                    resultat = "Inactif";
                    break;
            }
            return resultat;

        }
        public string ValeurDefautToChar(string valDefaut)
        {
            switch (valDefaut)
            {
                case "Oui":
                case "Yes":
                    resultat = "O";
                    break;
                case "Non":
                case "No":
                    resultat = "N";
                    break;
            }
            return resultat;

        }
        public string ValeurDefautToString(string valDefaut)
        {
            switch (valDefaut)
            {
                case "O":
                    resultat = "Oui";
                    break;
                case "N":

                    resultat = "Non";
                    break;
            }
            return resultat;

        }

    }

}
