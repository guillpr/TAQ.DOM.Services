using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TAQ.DOM.Services.Traitements
{
    public class Journalisation
    {
        private string EmplacementJournal = string.Empty;

        public Journalisation(string Emplacement)
        {
            EmplacementJournal = Emplacement;
        }
        /// <summary>
        /// Journaliser le message ou erreur
        /// </summary>
        /// <param name="sMessage">Message à journaliser</param>

        public void Journaliser(string sMessage)
        {
            StreamWriter sw = new StreamWriter(EmplacementJournal, true);
            sw.WriteLine(DateTime.Now.ToString() + " : " + sMessage);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }
}
