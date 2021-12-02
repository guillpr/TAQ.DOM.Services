using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAQ.DOM.Services.Modeles
{
    public class Domaine
    {
        public int IDDomaine { get; set; }
        public string Code { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string Statut { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Type { get; set; }
        public string NomAnglais { get; set; }

        // Ordre 0 = valeur asc 1 = valeur desc 2 = description asc 3 = description desc
        public int Ordre { get; set; }
        // Utilisé pour la selection de plusieurs case à cocher selection à false par défaut
        public bool Select { get; set; } = false;
        public string OpDateDebut { get; set; }
        public string OpDateFin { get; set; }

        // Description de l'erreur
        public string Erreur { get; set; }

    }

}
