using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAQ.DOM.Services.Modeles
{
    public class DomaineValeur
    {
        public int ValeurDomID { get; set; }
        public int DomaineID { get; set; }
        public string Valeur { get; set; }
        public string DescAbrg { get; set; }
        public string DescLong { get; set; }
        public string Statut { get; set; }
        public string ValMax { get; set; }
        public string ValMin { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string ValeurDefaut { get; set; }
        public int SeqTri { get; set; }
        public string DescAbrgAngl { get; set; }
        // Ordre 0 = valeur asc 1 = valeur desc 2 = description asc 3 = description desc
        public int Ordre { get; set; }
        public bool select { get; set; } = false;
        // Description de l'erreur
        public string Erreur { get; set; }



    
}
}
