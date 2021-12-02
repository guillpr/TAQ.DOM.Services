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
    public class DomaineValeurController : Controller
    {
        private readonly IConfiguration Configuration;
        public DomaineValeurController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        /// <summary>
        /// Obtenir une liste d'un seul domaine de valeur par ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("ObtenirDomValUniqueId/{id}")]
        public IEnumerable<DomaineValeur> GetDomaineValeurUniqueId(int id)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            List<DomaineValeur> nomDomaineValeurListe = new List<DomaineValeur>();
            nomDomaineValeurListe = fBD.ObtenirDomaineValeurParID(id);
            return nomDomaineValeurListe;
        }
        /// <summary>
        /// Obtenir une liste de tous les domaines de valeurs par ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valeurTri"></param>
        /// <returns></returns>
        [HttpGet, Route("ObtenirTousDomaineValeurId/{id}")]
        public IEnumerable<DomaineValeur> ObtenirTousDomaineValeurId(int id, int valeurTri)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            List<DomaineValeur> nomDomaineValeurListe = new List<DomaineValeur>();
            nomDomaineValeurListe = fBD.ObtenirTousDomaineValeurParId(id, valeurTri);

            return nomDomaineValeurListe;
        }
        /// <summary>
        /// Obtenir une liste de tous les opérateurs
        /// </summary>
        /// <param name="typeTri">Type de tri: 0 ou NULL = ASC , 1 = DESC </param>
        /// <returns></returns>
        [Route("ObtenirOperateurs")]
        [HttpGet]
        public IEnumerable<string> ObtenirOperateurs(int typeTri)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            List<string> lstOperateur = new List<string>();
            lstOperateur = fBD.ObtenirOperateurs(typeTri);
            return lstOperateur;
        }
        /// <summary>
        /// Ajouter un domaine valeur
        /// </summary>
        /// <param name="Criteres">Critères d'ajout domaine valeur</param>
        /// <returns></returns>
        [HttpPost, Route("AjouterDomaineValeur")]
        public DomaineValeur AjouterDomaineValeur([FromBody] DomaineValeur Criteres)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            DomaineValeur domaineValeur = fBD.AjouterDomaineValeur(Criteres);
            return domaineValeur;
        }
        /// <summary>
        /// Modification d'un domaine valeur
        /// </summary>
        /// <param name="Criteres">Critères à modifier</param>
        /// <returns></returns>
        [Route("ModifierDomaineValeur")]
        [HttpPut]
        public bool ModifierDomaineValeur([FromBody] DomaineValeur Criteres)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            bool resultat = fBD.MiseAJourDomaineValeur(Criteres);
            return resultat;
        }
        /// <summary>
        /// Suppression d'un domaine valeur
        /// </summary>
        /// <param name="idValeurDomaine">Identifiant à supprimer</param>
        /// <returns></returns>
        [Route("SupprimerDomaineValeur")]
        [HttpDelete]
        public bool SupprimerDomaineValeur(int idValeurDomaine)
        {
            FonctionsBD fBD = new FonctionsBD(Configuration);
            bool resultat = fBD.SupprimerDomaineValeur(idValeurDomaine);
            return resultat;
        }
    }
}
