using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TAQ.DOM.Services.Modeles;
using TAQ.DOM.Services.Traitements;

namespace TAQ.DOM.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomaineController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public DomaineController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        /// <summary>
        /// Obtenir un domaine par ID
        /// </summary>
        /// <param name="id">Numéro d'identifiant du domaine</param>
        /// <returns></returns>
        //GET
        [HttpGet("{id}")]
        public IEnumerable<Domaine> GetDomaineId(int id)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            List<Domaine> nomDomaineListe = new List<Domaine>();
            nomDomaineListe = fBD.ObtenirDomaineParID(id);
            return nomDomaineListe;
        }


        //POST
        /// <summary>
        /// Obtenir le résultat de la recherche
        /// </summary>
        /// <param name="Criteres">Critères de la recherche</param>
        /// <returns></returns>
        [HttpPost, Route("ResultatRecherche")]

        public IEnumerable<Domaine> GetResultatRecherche([FromBody] Domaine Criteres)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            List<Domaine> lstResuRech = new List<Domaine>();
            lstResuRech = fBD.RechercheDomaine(Criteres);
            return lstResuRech;
        }
        /// <summary>
        /// Ajouter un domaine
        /// </summary>
        /// <param name="Criteres">Critères d'ajout domaine</param>
        /// <returns></returns>
        [HttpPost, Route("AjouterDomaine")]
        public Domaine AjouterDomaine([FromBody] Domaine Criteres)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            Domaine domaine = fBD.AjouterDomaine(Criteres);
            return domaine;
        }
        //PUT
        /// <summary>
        /// Modification d'un domaine
        /// </summary>
        /// <param name="Criteres">Critères à modifier</param>
        /// <returns></returns>
        [Route("ModifierDomaine")]
        [HttpPut]
        public bool ModifierDomaine([FromBody] Domaine Criteres)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            List<string> lstOperateurASC = new List<string>();
            bool resultat = fBD.MiseAJourDomaine(Criteres);
            return resultat;
        }

        //DELETE
        /// <summary>
        /// Suppression d'un domaine
        /// </summary>
        /// <param name="idDomaine">Identifiant à supprimer</param>
        /// <returns></returns>
        [Route("SupprimerDomaine")]
        [HttpDelete]
        public bool SupprimerDomaine(int idDomaine)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            bool resultat = fBD.SupprimerDomaine(idDomaine);
            return resultat;
        }
    }
}
