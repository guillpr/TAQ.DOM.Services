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
    public class DomaineSousValeurController : Controller
    {
        private readonly IConfiguration Configuration;
        public DomaineSousValeurController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        /// <summary>
        /// Obtenir une liste d'un seul domaine de sous-valeur par ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("ObtenirDomSousValUniqueId/{id}")]
        public IEnumerable<DomaineSousValeur> ObtenirDomaineSousValeurUniqueId(int id)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            List<DomaineSousValeur> nomDomaineValeurListe = new List<DomaineSousValeur>();
            nomDomaineValeurListe = fBD.ObtenirDomaineSousValeurParID(id);
            return nomDomaineValeurListe;
        }

        /// <summary>
        /// Obtenir une liste de tous les sous-domaines de valeurs par ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valeurTri"></param>
        /// <returns></returns>
        [HttpGet, Route("ObtenirTousDomaineSousValeurId/{id}")]
        public IEnumerable<DomaineSousValeur> ObtenirTousDomaineSousValeurParId(int id, int valeurTri)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            List<DomaineSousValeur> nomSousDomaineValeurListe = new List<DomaineSousValeur>();
            nomSousDomaineValeurListe = fBD.ObtenirTousDomaineSousValeurParId(id, valeurTri);
            return nomSousDomaineValeurListe;
        }

        /// <summary>
        /// Ajouter un domaine sous-valeur
        /// </summary>
        /// <param name="Criteres">Critères d'ajout domaine sous-valeur</param>
        /// <returns></returns>
        [HttpPost, Route("AjouterDomaineSousValeur")]
        public DomaineSousValeur AjouterDomaineSousValeur([FromBody] DomaineSousValeur Criteres)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            DomaineSousValeur domaineSousValeur = fBD.AjouterDomaineSousValeur(Criteres);
            return domaineSousValeur;
        }
        /// <summary>
        /// Modification d'un domaine sous-valeur
        /// </summary>
        /// <param name="Criteres">Critères à modifier</param>
        /// <returns></returns>
        [Route("ModifierDomaineSousValeur")]
        [HttpPut]
        public bool ModifierDomaineSousValeur([FromBody] DomaineSousValeur Criteres)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            bool resultat = fBD.MiseAJourDomaineSousValeur(Criteres);
            return resultat;
        }
        /// <summary>
        /// Suppression d'un domaine sous-valeur
        /// </summary>
        /// <param name="idDomaineSousValeur">Identifiant à supprimer</param>
        /// <returns></returns>
        [Route("SupprimerDomaineSousValeur")]
        [HttpDelete]
        public bool SupprimerDomaineSousValeur(int idDomaineSousValeur)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            bool resultat = fBD.SupprimerDomaineSousValeur(idDomaineSousValeur);
            return resultat;
        }

    }
}
