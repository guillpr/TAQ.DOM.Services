using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAQ.DOM.Services.Modeles;

namespace TAQ.DOM.Services.Traitements
{
    public class FonctionsBD
    {
        private string connexion = string.Empty;
        private Journalisation journal;
        ConversionBd conv = new ConversionBd();
        string provenance = string.Empty;

        public FonctionsBD(IConfiguration _configuration)
        {
            connexion = _configuration.GetConnectionString("myCon");
            journal = new Journalisation(_configuration["EmplacementJournal"]);
        }
        public List<Domaine> RechercheDomaine(Domaine Criteres)
        {
            List<Domaine> lstDomaine = new List<Domaine>();
            provenance = "RechercheDomaine";

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SEL_RECH_DOMN";

            //Paramètre Code Domaine 
            if (!String.IsNullOrEmpty(Criteres.Code))
            {
                OracleParameter ocpCodeDom = new OracleParameter();
                ocpCodeDom.OracleDbType = OracleDbType.Varchar2;
                ocpCodeDom.ParameterName = "P_CODE_DOMN";
                ocpCodeDom.Direction = System.Data.ParameterDirection.Input;
                ocpCodeDom.Value = Criteres.Code;
                ocm.Parameters.Add(ocpCodeDom);
            }
            //Paramètre NomDomaine 
            if (!String.IsNullOrEmpty(Criteres.Nom))
            {
                OracleParameter ocpNomDom = new OracleParameter();
                ocpNomDom.OracleDbType = OracleDbType.Varchar2;
                ocpNomDom.ParameterName = "P_NOM";
                ocpNomDom.Direction = System.Data.ParameterDirection.Input;
                ocpNomDom.Value = Criteres.Nom;
                ocm.Parameters.Add(ocpNomDom);
            }
            //Paramètre DescDom 
            if (!String.IsNullOrEmpty(Criteres.Description))
            {
                OracleParameter ocpDescDom = new OracleParameter();
                ocpDescDom.OracleDbType = OracleDbType.Varchar2;
                ocpDescDom.ParameterName = "P_DESCR_DOMN";
                ocpDescDom.Direction = System.Data.ParameterDirection.Input;
                ocpDescDom.Value = Criteres.Description;
                ocm.Parameters.Add(ocpDescDom);
            }
            //Paramètre Statut ValeurDomaine 
            if (!String.IsNullOrEmpty(Criteres.Statut))
            {
                OracleParameter ocpStatDom = new OracleParameter();
                ocpStatDom.OracleDbType = OracleDbType.Varchar2;
                ocpStatDom.ParameterName = "P_STAT_DOMN";
                ocpStatDom.Direction = System.Data.ParameterDirection.Input;
                ocpStatDom.Value = conv.StatutDomaineToChar(Criteres.Statut);
                ocm.Parameters.Add(ocpStatDom);
            }
            //Paramètre DateDebut Domaine 
            if ((Criteres.DateDebut != DateTime.MinValue) || (Criteres.DateDebut.Year != 1))
            {
                OracleParameter ocpDateDebutDom = new OracleParameter();
                ocpDateDebutDom.OracleDbType = OracleDbType.Date;
                ocpDateDebutDom.ParameterName = "P_DATE_DEBUT";
                ocpDateDebutDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateDebutDom.Value = Convert.ToDateTime(Criteres.DateDebut.ToShortDateString());
                ocm.Parameters.Add(ocpDateDebutDom);

                //Paramètre Opérateur date début  
                if (!String.IsNullOrEmpty(Criteres.OpDateDebut))
                {
                    OracleParameter ocpOpDateDebutDom = new OracleParameter();
                    ocpOpDateDebutDom.OracleDbType = OracleDbType.Varchar2;
                    ocpOpDateDebutDom.ParameterName = "P_OPER_DATE_DEBUT";
                    ocpOpDateDebutDom.Direction = System.Data.ParameterDirection.Input;
                    ocpOpDateDebutDom.Value = Criteres.OpDateDebut;
                    ocm.Parameters.Add(ocpOpDateDebutDom);
                }
            }
            //Paramètre DateFin Domaine 
            if ((Criteres.DateFin != DateTime.MinValue) || (Criteres.DateFin.Year != 1))
            {
                OracleParameter ocpDateFinDom = new OracleParameter();
                ocpDateFinDom.OracleDbType = OracleDbType.Date;
                ocpDateFinDom.ParameterName = "P_DATE_FIN";
                ocpDateFinDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateFinDom.Value = Convert.ToDateTime(Criteres.DateFin.ToShortDateString());
                ocm.Parameters.Add(ocpDateFinDom);

                //Paramètre Opérateur date fin  
                if (!String.IsNullOrEmpty(Criteres.OpDateFin))
                {
                    OracleParameter ocpOpDateFinDom = new OracleParameter();
                    ocpOpDateFinDom.OracleDbType = OracleDbType.Varchar2;
                    ocpOpDateFinDom.ParameterName = "P_OPER_DATE_FIN";
                    ocpOpDateFinDom.Direction = System.Data.ParameterDirection.Input;
                    ocpOpDateFinDom.Value = Criteres.OpDateFin;
                    ocm.Parameters.Add(ocpOpDateFinDom);
                }
            }
            //Paramètre Code Domaine 
            if (!String.IsNullOrEmpty(Criteres.Ordre.ToString()))
            {
                OracleParameter ocpOrdreDom = new OracleParameter();
                ocpOrdreDom.OracleDbType = OracleDbType.Int32;
                ocpOrdreDom.ParameterName = "P_TRI";
                ocpOrdreDom.Direction = System.Data.ParameterDirection.Input;
                ocpOrdreDom.Value = Criteres.Ordre;
                ocm.Parameters.Add(ocpOrdreDom);
            }
            //Paramètre Resultats recherche domaine
            OracleParameter ocpResultatsDomaine = new OracleParameter();
            ocpResultatsDomaine.OracleDbType = OracleDbType.RefCursor;
            ocpResultatsDomaine.ParameterName = "P_RESULTATS";
            ocpResultatsDomaine.Direction = System.Data.ParameterDirection.Output;
            ocpResultatsDomaine.Value = Criteres.IDDomaine;
            ocm.Parameters.Add(ocpResultatsDomaine);

            return LierBdDomaine(provenance, ocn, ocm);


        }
        public List<Domaine> ObtenirDomaineParID(int id)
        {
            Domaine dom = new Domaine();

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SEL_SEQ_DOMN";

            //Paramètre ID Domaine  
            OracleParameter ocpIDDom = new OracleParameter();
            ocpIDDom.OracleDbType = OracleDbType.Int32;
            ocpIDDom.ParameterName = "P_NO_SEQ_DOMN";
            ocpIDDom.Direction = System.Data.ParameterDirection.Input;
            ocpIDDom.Value = id;
            ocm.Parameters.Add(ocpIDDom);


            //Paramètre Resultats recherche domaine
            OracleParameter ocpResultatsDomaine = new OracleParameter();
            ocpResultatsDomaine.OracleDbType = OracleDbType.RefCursor;
            ocpResultatsDomaine.ParameterName = "P_RESULTATS";
            ocpResultatsDomaine.Direction = System.Data.ParameterDirection.Output;
            //ocpResultatsDomaine.Value = dom.IDDomaine;
            ocm.Parameters.Add(ocpResultatsDomaine);

            return LierBdDomaine(provenance, ocn, ocm);
        }

        public List<DomaineValeur> ObtenirDomaineValeurParID(int id)
        {
            DomaineValeur valDom = new DomaineValeur();

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SEL_SEQ_DOMN_VALR";

            //Paramètre ID Domaine  
            OracleParameter ocpIDDom = new OracleParameter();
            ocpIDDom.OracleDbType = OracleDbType.Int32;
            ocpIDDom.ParameterName = "P_NO_SEQ_DOMN_VALR";
            ocpIDDom.Direction = System.Data.ParameterDirection.Input;
            ocpIDDom.Value = id;
            ocm.Parameters.Add(ocpIDDom);


            //Paramètre Resultats recherche domaine
            OracleParameter ocpResultatsDomaine = new OracleParameter();
            ocpResultatsDomaine.OracleDbType = OracleDbType.RefCursor;
            ocpResultatsDomaine.ParameterName = "P_RESULTATS";
            ocpResultatsDomaine.Direction = System.Data.ParameterDirection.Output;
            ocpResultatsDomaine.Value = valDom.ValeurDomID;
            ocm.Parameters.Add(ocpResultatsDomaine);

            return LierBdValeurDomaine(provenance, ocn, ocm);
        }
        public List<DomaineSousValeur> ObtenirDomaineSousValeurParID(int id)
        {
            DomaineSousValeur valDom = new DomaineSousValeur();

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SEQ_DOMN_SOUS_VALR";

            //Paramètre ID Domaine  
            OracleParameter ocpIDDom = new OracleParameter();
            ocpIDDom.OracleDbType = OracleDbType.Int32;
            ocpIDDom.ParameterName = "P_NO_SEQ_DOMN_SOUS_VALR";
            ocpIDDom.Direction = System.Data.ParameterDirection.Input;
            ocpIDDom.Value = id;
            ocm.Parameters.Add(ocpIDDom);


            //Paramètre Resultats recherche domaine
            OracleParameter ocpResultatsDomaine = new OracleParameter();
            ocpResultatsDomaine.OracleDbType = OracleDbType.RefCursor;
            ocpResultatsDomaine.ParameterName = "P_RESULTATS";
            ocpResultatsDomaine.Direction = System.Data.ParameterDirection.Output;
            //ocpResultatsDomaine.Value = valDom.ValeurDomID;
            ocm.Parameters.Add(ocpResultatsDomaine);

            return LierBdSousValeurDomaine(provenance, ocn, ocm);
        }
        public List<DomaineValeur> ObtenirTousDomaineValeurParId(int id, int valeurTri)
        {
            DomaineValeur dom = new DomaineValeur();

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SEL_DOMN_VALR";

            //Paramètre ID Domaine  
            OracleParameter ocpIDDom = new OracleParameter();
            ocpIDDom.OracleDbType = OracleDbType.Int32;
            ocpIDDom.ParameterName = "P_NO_SEQ_DOMN";
            ocpIDDom.Direction = System.Data.ParameterDirection.Input;
            ocpIDDom.Value = id;
            ocm.Parameters.Add(ocpIDDom);


            //Paramètre Tri 
            OracleParameter ocpPTri = new OracleParameter();
            ocpPTri.OracleDbType = OracleDbType.Int32;
            ocpPTri.ParameterName = "P_TRI";
            ocpPTri.Direction = System.Data.ParameterDirection.Input;
            ocpPTri.Value = valeurTri;
            ocm.Parameters.Add(ocpPTri);


            //Paramètre Resultats recherche domaine
            OracleParameter ocpResultatsDomaine = new OracleParameter();
            ocpResultatsDomaine.OracleDbType = OracleDbType.RefCursor;
            ocpResultatsDomaine.ParameterName = "P_RESULTATS";
            ocpResultatsDomaine.Direction = System.Data.ParameterDirection.Output;
            //ocpResultatsDomaine.Value = dom.IDDomaine;
            ocm.Parameters.Add(ocpResultatsDomaine);

            return LierBdValeurDomaine(provenance, ocn, ocm);
        }
        public List<DomaineSousValeur> ObtenirTousDomaineSousValeurParId(int id, int valeurTri)
        {
            DomaineSousValeur valDom = new DomaineSousValeur();

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SEL_DOMN_SOUS_VALR";

            //Paramètre ID Domaine  
            OracleParameter ocpIDDom = new OracleParameter();
            ocpIDDom.OracleDbType = OracleDbType.Int32;
            ocpIDDom.ParameterName = "P_NO_SEQ_DOMN_VALR";
            ocpIDDom.Direction = System.Data.ParameterDirection.Input;
            ocpIDDom.Value = id;
            ocm.Parameters.Add(ocpIDDom);

            //Paramètre valeurTir  
            OracleParameter ocpPTri = new OracleParameter();
            ocpPTri.OracleDbType = OracleDbType.Int32;
            ocpPTri.ParameterName = "P_TRI";
            ocpPTri.Direction = System.Data.ParameterDirection.Input;
            ocpPTri.Value = valeurTri;
            ocm.Parameters.Add(ocpPTri);


            //Paramètre Resultats recherche domaine
            OracleParameter ocpResultatsDomaine = new OracleParameter();
            ocpResultatsDomaine.OracleDbType = OracleDbType.RefCursor;
            ocpResultatsDomaine.ParameterName = "P_RESULTATS";
            ocpResultatsDomaine.Direction = System.Data.ParameterDirection.Output;
            //ocpResultatsDomaine.Value = valDom.ValeurDomID;
            ocm.Parameters.Add(ocpResultatsDomaine);

            return LierBdSousValeurDomaine(provenance, ocn, ocm);
        }

        public Domaine AjouterDomaine(Domaine Criteres)
        {
            Domaine domaine = new Domaine();
            int idDomaine = 0;
            provenance = "AjouterDomaine";

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_INS_DOMN";

            //Paramètre Code Domaine Obligatoire
            OracleParameter ocpCodeDom = new OracleParameter();
            ocpCodeDom.OracleDbType = OracleDbType.Varchar2;
            ocpCodeDom.ParameterName = "P_CODE_DOMN";
            ocpCodeDom.Direction = System.Data.ParameterDirection.Input;
            ocpCodeDom.Value = Criteres.Code;
            ocm.Parameters.Add(ocpCodeDom);

            //Paramètre Nom Domaine Obligatoire
            OracleParameter ocpNomDom = new OracleParameter();
            ocpNomDom.OracleDbType = OracleDbType.Varchar2;
            ocpNomDom.ParameterName = "P_NOM";
            ocpNomDom.Direction = System.Data.ParameterDirection.Input;
            ocpNomDom.Value = Criteres.Nom;
            ocm.Parameters.Add(ocpNomDom);

            //Paramètre Statut Domaine Obligatoire
            OracleParameter ocpStatDom = new OracleParameter();
            ocpStatDom.OracleDbType = OracleDbType.Varchar2;
            ocpStatDom.ParameterName = "P_STAT_DOMN";
            ocpStatDom.Direction = System.Data.ParameterDirection.Input;
            ocpStatDom.Value = conv.StatutDomaineToChar(Criteres.Statut);
            ocm.Parameters.Add(ocpStatDom);
            //Paramètre Type Domaine Obligatoire
            if (!String.IsNullOrEmpty(Criteres.Type))
            {
                OracleParameter ocpTypeDom = new OracleParameter();
                ocpTypeDom.OracleDbType = OracleDbType.Varchar2;
                ocpTypeDom.ParameterName = "P_TYPE_DOMN";
                ocpTypeDom.Direction = System.Data.ParameterDirection.Input;
                ocpTypeDom.Value = conv.TypeDomaineToChar(Criteres.Type);
                ocm.Parameters.Add(ocpTypeDom);
            }
            //Paramètre DateDebut Domaine 
            if ((Criteres.DateDebut != DateTime.MinValue) || (Criteres.DateDebut.Year != 1))
            {
                OracleParameter ocpDateDebutDom = new OracleParameter();
                ocpDateDebutDom.OracleDbType = OracleDbType.Date;
                ocpDateDebutDom.ParameterName = "P_DATE_DEBUT";
                ocpDateDebutDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateDebutDom.Value = Convert.ToDateTime(Criteres.DateDebut.ToShortDateString());
                ocm.Parameters.Add(ocpDateDebutDom);
            }
            //Paramètre DateFin Domaine 
            if ((Criteres.DateFin != DateTime.MinValue) || (Criteres.DateFin.Year != 1))
            {
                OracleParameter ocpDateFinDom = new OracleParameter();
                ocpDateFinDom.OracleDbType = OracleDbType.Date;
                ocpDateFinDom.ParameterName = "P_DATE_FIN";
                ocpDateFinDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateFinDom.Value = Convert.ToDateTime(Criteres.DateFin.ToShortDateString());
                ocm.Parameters.Add(ocpDateFinDom);
            }
            //Paramètre Description Domaine 
            if (!String.IsNullOrEmpty(Criteres.Description))
            {
                OracleParameter ocpDescDom = new OracleParameter();
                ocpDescDom.OracleDbType = OracleDbType.Varchar2;
                ocpDescDom.ParameterName = "P_DESCR_DOMN";
                ocpDescDom.Direction = System.Data.ParameterDirection.Input;
                ocpDescDom.Value = Criteres.Description;
                ocm.Parameters.Add(ocpDescDom);
            }
            ////Paramètre Nom anglais Domaine 
            if (!String.IsNullOrEmpty(Criteres.NomAnglais))
            {
                OracleParameter ocpNomAnglDom = new OracleParameter();
                ocpNomAnglDom.OracleDbType = OracleDbType.Varchar2;
                ocpNomAnglDom.ParameterName = "P_NOM_ANGLS";
                ocpNomAnglDom.Direction = System.Data.ParameterDirection.Input;
                ocpNomAnglDom.Value = Criteres.NomAnglais;
                ocm.Parameters.Add(ocpNomAnglDom);
            }
            //Paramètre Identifiant domaine
            OracleParameter ocpIdDomaine = new OracleParameter();
            ocpIdDomaine.OracleDbType = OracleDbType.Int32;
            ocpIdDomaine.ParameterName = "P_NO_SEQ_DOMN";
            ocpIdDomaine.Direction = System.Data.ParameterDirection.Output;
            ocpIdDomaine.Value = Criteres.IDDomaine;
            ocm.Parameters.Add(ocpIdDomaine);

            ocm.BindByName = true;

            try
            {
                //Exécution de la requête
                ocn.Open();
                ocm.ExecuteScalar();
                ocn.Close();

                //Récuperation du résultat
                if ((OracleDecimal)ocpIdDomaine.Value != OracleDecimal.Null)
                {
                    int.TryParse(ocpIdDomaine.Value.ToString(), out idDomaine);
                    domaine.IDDomaine = idDomaine;
                }
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
                if (ex.Message.Contains("ORA-00001"))
                {
                    if (ex.Message.Contains("COM.DOM_1_UK"))
                    {
                        domaine.IDDomaine = idDomaine;
                        domaine.Erreur = "CODE UNIQUE";


                    }
                }
            }
            return domaine;
        }
        public DomaineValeur AjouterDomaineValeur(DomaineValeur Criteres)
        {
            DomaineValeur domaineValeur = new DomaineValeur();
            int idValeurDomaine = 0;
            provenance = "AjouterDomaineValeur";

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_INS_DOMN_VALR";

            //Paramètre IDDomaine ValeurDomaine Obligatoire
            OracleParameter ocpIdDomaineValDom = new OracleParameter();
            ocpIdDomaineValDom.OracleDbType = OracleDbType.Int32;
            ocpIdDomaineValDom.ParameterName = "P_NO_SEQ_DOMN";
            ocpIdDomaineValDom.Direction = System.Data.ParameterDirection.Input;
            ocpIdDomaineValDom.Value = Criteres.DomaineID;
            ocm.Parameters.Add(ocpIdDomaineValDom);

            //Paramètre Valeur  ValeurDomaine Obligatoire
            OracleParameter ocpValeurDom = new OracleParameter();
            ocpValeurDom.OracleDbType = OracleDbType.Varchar2;
            ocpValeurDom.ParameterName = "P_VALR";
            ocpValeurDom.Direction = System.Data.ParameterDirection.Input;
            ocpValeurDom.Value = Criteres.Valeur;
            ocm.Parameters.Add(ocpValeurDom);

            //Paramètre DescAbrg ValeurDomaine 
            OracleParameter ocpDescAbrgDom = new OracleParameter();
            ocpDescAbrgDom.OracleDbType = OracleDbType.Varchar2;
            ocpDescAbrgDom.ParameterName = "P_DESCR_ABRG";
            ocpDescAbrgDom.Direction = System.Data.ParameterDirection.Input;
            ocpDescAbrgDom.Value = Criteres.DescAbrg;
            ocm.Parameters.Add(ocpDescAbrgDom);

            //Paramètre Statut ValeurDomaine Obligatoire
            OracleParameter ocpStatValDom = new OracleParameter();
            ocpStatValDom.OracleDbType = OracleDbType.Varchar2;
            ocpStatValDom.ParameterName = "P_STAT_DOMN_VALR";
            ocpStatValDom.Direction = System.Data.ParameterDirection.Input;
            ocpStatValDom.Value = conv.StatutDomaineToChar(Criteres.Statut);
            ocm.Parameters.Add(ocpStatValDom);

            //Paramètre Valeur defaut ValeurDomaine Obligatoire
            OracleParameter ocpValDefValDom = new OracleParameter();
            ocpValDefValDom.OracleDbType = OracleDbType.Varchar2;
            ocpValDefValDom.ParameterName = "P_VALR_DEFT";
            ocpValDefValDom.Direction = System.Data.ParameterDirection.Input;
            ocpValDefValDom.Value = conv.ValeurDefautToChar(Criteres.ValeurDefaut);
            ocm.Parameters.Add(ocpValDefValDom);

            //Paramètre Valeur description longue ValeurDomaine 
            OracleParameter ocpDescLongValDom = new OracleParameter();
            ocpDescLongValDom.OracleDbType = OracleDbType.Varchar2;
            ocpDescLongValDom.ParameterName = "P_DESCR_LONG";
            ocpDescLongValDom.Direction = System.Data.ParameterDirection.Input;
            ocpDescLongValDom.Value = Criteres.DescLong;
            ocm.Parameters.Add(ocpDescLongValDom);

            //Paramètre Valeur valeur min ValeurDomaine 
            if (!String.IsNullOrEmpty(Criteres.ValMin))
            {
                OracleParameter ocpValMinValDom = new OracleParameter();
                ocpValMinValDom.OracleDbType = OracleDbType.Varchar2;
                ocpValMinValDom.ParameterName = "P_VALR_MIN";
                ocpValMinValDom.Direction = System.Data.ParameterDirection.Input;
                ocpValMinValDom.Value = Criteres.ValMin;
                ocm.Parameters.Add(ocpValMinValDom);
            }
            //Paramètre Valeur valeur max ValeurDomaine 
            if (!String.IsNullOrEmpty(Criteres.ValMax))
            {
                OracleParameter ocpValMaxValDom = new OracleParameter();
                ocpValMaxValDom.OracleDbType = OracleDbType.Varchar2;
                ocpValMaxValDom.ParameterName = "P_VALR_MAX";
                ocpValMaxValDom.Direction = System.Data.ParameterDirection.Input;
                ocpValMaxValDom.Value = Criteres.ValMax;
                ocm.Parameters.Add(ocpValMaxValDom);
            }
            //Paramètre DateDebut ValeurDomaine 
            if ((Criteres.DateDebut != DateTime.MinValue) || (Criteres.DateDebut.Year != 1))
            {
                OracleParameter ocpDateDebutDom = new OracleParameter();
                ocpDateDebutDom.OracleDbType = OracleDbType.Date;
                ocpDateDebutDom.ParameterName = "P_DATE_DEBUT";
                ocpDateDebutDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateDebutDom.Value = Convert.ToDateTime(Criteres.DateDebut.ToShortDateString());
                ocm.Parameters.Add(ocpDateDebutDom);
            }
            //Paramètre DateFin ValeurDomaine 
            if ((Criteres.DateFin != DateTime.MinValue) || (Criteres.DateFin.Year != 1))
            {
                OracleParameter ocpDateFinDom = new OracleParameter();
                ocpDateFinDom.OracleDbType = OracleDbType.Date;
                ocpDateFinDom.ParameterName = "P_DATE_FIN";
                ocpDateFinDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateFinDom.Value = Convert.ToDateTime(Criteres.DateFin.ToShortDateString());
                ocm.Parameters.Add(ocpDateFinDom);
            }
            ////Paramètre seqTri ValeurDomaine 
            if (!String.IsNullOrEmpty(Criteres.SeqTri.ToString()))
            {
                OracleParameter ocpSeqTriValDom = new OracleParameter();
                ocpSeqTriValDom.OracleDbType = OracleDbType.Int32;
                ocpSeqTriValDom.ParameterName = "P_SEQNC_TRI";
                ocpSeqTriValDom.Direction = System.Data.ParameterDirection.Input;
                ocpSeqTriValDom.Value = Criteres.SeqTri;
                ocm.Parameters.Add(ocpSeqTriValDom);
            }
            if (!String.IsNullOrEmpty(Criteres.DescAbrgAngl))
            {
                //Paramètre Valeur descAbrg anglais ValeurDomaine 
                OracleParameter ocpDescAbrgAngValDom = new OracleParameter();
                ocpDescAbrgAngValDom.OracleDbType = OracleDbType.Varchar2;
                ocpDescAbrgAngValDom.ParameterName = "P_DESCR_ABRG_ANGLS";
                ocpDescAbrgAngValDom.Direction = System.Data.ParameterDirection.Input;
                ocpDescAbrgAngValDom.Value = Criteres.DescAbrgAngl;
                ocm.Parameters.Add(ocpDescAbrgAngValDom);
            }
            //Paramètre Identifiant Valeurdomaine
            OracleParameter ocpIdValDomaine = new OracleParameter();
            ocpIdValDomaine.OracleDbType = OracleDbType.Int32;
            ocpIdValDomaine.ParameterName = "P_NO_SEQ_DOMN_VALR";
            ocpIdValDomaine.Direction = System.Data.ParameterDirection.Output;
            ocpIdValDomaine.Value = Criteres.ValeurDomID;
            ocm.Parameters.Add(ocpIdValDomaine);

            ocm.BindByName = true;

            try
            {
                //Exécution de la requête
                ocn.Open();
                ocm.ExecuteScalar();
                ocn.Close();

                //Récuperation du résultat
                if ((OracleDecimal)ocpIdValDomaine.Value != OracleDecimal.Null)
                {
                    int.TryParse(ocpIdValDomaine.Value.ToString(), out idValeurDomaine);
                    domaineValeur.ValeurDomID = idValeurDomaine;
                }
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
                if (ex.Message.Contains("ORA-00001"))
                {
                    if (ex.Message.Contains("COM.DOM_VA_1_UK"))
                    {
                        domaineValeur.ValeurDomID = idValeurDomaine;
                        domaineValeur.Erreur = "VALEUR UNIQUE";
                    }
                }
            }
            return domaineValeur;
        }
        public DomaineSousValeur AjouterDomaineSousValeur(DomaineSousValeur Criteres)
        {
            DomaineSousValeur domaineSousValeur = new DomaineSousValeur();
            int idSousValeurDomaine = 0;
            provenance = "AjouterSousValeurDomaine";

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_INS_DOMN_SOUS_VALR";

            //Paramètre IDValeurDomaine SousValeurDomaine Obligatoire
            OracleParameter ocpIdValDom = new OracleParameter();
            ocpIdValDom.OracleDbType = OracleDbType.Int32;
            ocpIdValDom.ParameterName = "P_NO_SEQ_DOMN_VALR";
            ocpIdValDom.Direction = System.Data.ParameterDirection.Input;
            ocpIdValDom.Value = Criteres.ValeurDomnID;
            ocm.Parameters.Add(ocpIdValDom);

            //Paramètre Valeur  SousValeurDomaine Obligatoire
            OracleParameter ocpSousValeurDom = new OracleParameter();
            ocpSousValeurDom.OracleDbType = OracleDbType.Varchar2;
            ocpSousValeurDom.ParameterName = "P_VALR";
            ocpSousValeurDom.Direction = System.Data.ParameterDirection.Input;
            ocpSousValeurDom.Value = Criteres.Valeur;
            ocm.Parameters.Add(ocpSousValeurDom);

            //Paramètre DescAbrg SousValeurDomaine 
            OracleParameter ocpDescAbrgDom = new OracleParameter();
            ocpDescAbrgDom.OracleDbType = OracleDbType.Varchar2;
            ocpDescAbrgDom.ParameterName = "P_DESCR_ABRG";
            ocpDescAbrgDom.Direction = System.Data.ParameterDirection.Input;
            ocpDescAbrgDom.Value = Criteres.DescAbrg;
            ocm.Parameters.Add(ocpDescAbrgDom);

            //Paramètre Statut SousValeurDomaine Obligatoire
            OracleParameter ocpStatValDom = new OracleParameter();
            ocpStatValDom.OracleDbType = OracleDbType.Varchar2;
            ocpStatValDom.ParameterName = "P_STAT_DOMN_SOUS_VALR";
            ocpStatValDom.Direction = System.Data.ParameterDirection.Input;
            ocpStatValDom.Value = conv.StatutDomaineToChar(Criteres.Statut);
            ocm.Parameters.Add(ocpStatValDom);

            //Paramètre Valeur defaut SousValeurDomaine Obligatoire
            OracleParameter ocpValDefValDom = new OracleParameter();
            ocpValDefValDom.OracleDbType = OracleDbType.Varchar2;
            ocpValDefValDom.ParameterName = "P_VALR_DEFT";
            ocpValDefValDom.Direction = System.Data.ParameterDirection.Input;
            ocpValDefValDom.Value = conv.ValeurDefautToChar(Criteres.ValeurDefaut);
            ocm.Parameters.Add(ocpValDefValDom);

            //Paramètre Valeur description longue SousValeurDomaine 
            OracleParameter ocpDescLongValDom = new OracleParameter();
            ocpDescLongValDom.OracleDbType = OracleDbType.Varchar2;
            ocpDescLongValDom.ParameterName = "P_DESCR_LONG";
            ocpDescLongValDom.Direction = System.Data.ParameterDirection.Input;
            ocpDescLongValDom.Value = Criteres.DescLong;
            ocm.Parameters.Add(ocpDescLongValDom);

            //Paramètre Valeur valeur min SousValeurDomaine 
            if (!String.IsNullOrEmpty(Criteres.ValMin))
            {
                OracleParameter ocpValMinValDom = new OracleParameter();
                ocpValMinValDom.OracleDbType = OracleDbType.Varchar2;
                ocpValMinValDom.ParameterName = "P_VALR_MIN";
                ocpValMinValDom.Direction = System.Data.ParameterDirection.Input;
                ocpValMinValDom.Value = Criteres.ValMin;
                ocm.Parameters.Add(ocpValMinValDom);
            }
            //Paramètre Valeur valeur max SousValeurDomaine 
            if (!String.IsNullOrEmpty(Criteres.ValMax))
            {
                OracleParameter ocpValMaxValDom = new OracleParameter();
                ocpValMaxValDom.OracleDbType = OracleDbType.Varchar2;
                ocpValMaxValDom.ParameterName = "P_VALR_MAX";
                ocpValMaxValDom.Direction = System.Data.ParameterDirection.Input;
                ocpValMaxValDom.Value = Criteres.ValMax;
                ocm.Parameters.Add(ocpValMaxValDom);
            }
            //Paramètre DateDebut SousValeurDomaine 
            if ((Criteres.DateDebut != DateTime.MinValue) || (Criteres.DateDebut.Year != 1))
            {
                OracleParameter ocpDateDebutDom = new OracleParameter();
                ocpDateDebutDom.OracleDbType = OracleDbType.Date;
                ocpDateDebutDom.ParameterName = "P_DATE_DEBUT";
                ocpDateDebutDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateDebutDom.Value = Convert.ToDateTime(Criteres.DateDebut.ToShortDateString());
                ocm.Parameters.Add(ocpDateDebutDom);
            }
            //Paramètre DateFin SousValeurDomaine 
            if ((Criteres.DateFin != DateTime.MinValue) || (Criteres.DateFin.Year != 1))
            {
                OracleParameter ocpDateFinDom = new OracleParameter();
                ocpDateFinDom.OracleDbType = OracleDbType.Date;
                ocpDateFinDom.ParameterName = "P_DATE_FIN";
                ocpDateFinDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateFinDom.Value = Convert.ToDateTime(Criteres.DateFin.ToShortDateString());
                ocm.Parameters.Add(ocpDateFinDom);
            }
            ////Paramètre seqTri SousValeurDomaine 
            if (!String.IsNullOrEmpty(Criteres.SeqTri.ToString()))
            {
                OracleParameter ocpSeqTriValDom = new OracleParameter();
                ocpSeqTriValDom.OracleDbType = OracleDbType.Int32;
                ocpSeqTriValDom.ParameterName = "P_SEQNC_TRI";
                ocpSeqTriValDom.Direction = System.Data.ParameterDirection.Input;
                ocpSeqTriValDom.Value = Criteres.SeqTri;
                ocm.Parameters.Add(ocpSeqTriValDom);
            }
            if (!String.IsNullOrEmpty(Criteres.DescAbrgAngl))
            {
                //Paramètre Valeur descAbrg anglais SousValeurDomaine 
                OracleParameter ocpDescAbrgAngValDom = new OracleParameter();
                ocpDescAbrgAngValDom.OracleDbType = OracleDbType.Varchar2;
                ocpDescAbrgAngValDom.ParameterName = "P_DESCR_ABRG_ANGLS";
                ocpDescAbrgAngValDom.Direction = System.Data.ParameterDirection.Input;
                ocpDescAbrgAngValDom.Value = Criteres.DescAbrgAngl;
                ocm.Parameters.Add(ocpDescAbrgAngValDom);
            }
            //Paramètre Identifiant SousValeurdomaine
            OracleParameter ocpIdValDomaine = new OracleParameter();
            ocpIdValDomaine.OracleDbType = OracleDbType.Int32;
            ocpIdValDomaine.ParameterName = "P_NO_SEQ_DOMN_SOUS_VALR";
            ocpIdValDomaine.Direction = System.Data.ParameterDirection.Output;
            ocpIdValDomaine.Value = Criteres.ValeurSousDomnID;
            ocm.Parameters.Add(ocpIdValDomaine);

            ocm.BindByName = true;

            try
            {
                //Exécution de la requête
                ocn.Open();
                ocm.ExecuteScalar();
                ocn.Close();

                //Récuperation du résultat
                if ((OracleDecimal)ocpIdValDomaine.Value != OracleDecimal.Null)
                {
                    int.TryParse(ocpIdValDomaine.Value.ToString(), out idSousValeurDomaine);
                    domaineSousValeur.ValeurSousDomnID = idSousValeurDomaine;
                }
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
                if (ex.Message.Contains("ORA-00001"))
                {
                    if (ex.Message.Contains("COM.DOM_SO_VA_1_UK"))
                    {
                        domaineSousValeur.ValeurSousDomnID = idSousValeurDomaine;
                        domaineSousValeur.Erreur = "SOUS-VALEUR UNIQUE";
                    }
                }
            }
            return domaineSousValeur;
        }
        public bool MiseAJourDomaine(Domaine Criteres)
        {
            bool resultat = true;
            provenance = "MiseAJourDomaine";

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_MAJ_DOMN";

            //Paramètre IDDomaine Obligatoire
            OracleParameter ocpIDDom = new OracleParameter();
            ocpIDDom.OracleDbType = OracleDbType.Int32;
            ocpIDDom.ParameterName = "P_NO_SEQ_DOMN";
            ocpIDDom.Direction = System.Data.ParameterDirection.Input;
            ocpIDDom.Value = Criteres.IDDomaine;
            ocm.Parameters.Add(ocpIDDom);

            //Paramètre Code Domaine Obligatoire
            OracleParameter ocpCodeDom = new OracleParameter();
            ocpCodeDom.OracleDbType = OracleDbType.Varchar2;
            ocpCodeDom.ParameterName = "P_CODE_DOMN";
            ocpCodeDom.Direction = System.Data.ParameterDirection.Input;
            ocpCodeDom.Value = Criteres.Code;
            ocm.Parameters.Add(ocpCodeDom);

            //Paramètre Nom Domaine Obligatoire
            OracleParameter ocpNomDom = new OracleParameter();
            ocpNomDom.OracleDbType = OracleDbType.Varchar2;
            ocpNomDom.ParameterName = "P_NOM";
            ocpNomDom.Direction = System.Data.ParameterDirection.Input;
            ocpNomDom.Value = Criteres.Nom;
            ocm.Parameters.Add(ocpNomDom);

            //Paramètre Statut Domaine Obligatoire
            OracleParameter ocpStatDom = new OracleParameter();
            ocpStatDom.OracleDbType = OracleDbType.Varchar2;
            ocpStatDom.ParameterName = "P_STAT_DOMN";
            ocpStatDom.Direction = System.Data.ParameterDirection.Input;
            ocpStatDom.Value = conv.StatutDomaineToChar(Criteres.Statut);
            ocm.Parameters.Add(ocpStatDom);
            //Paramètre Type Domaine Obligatoire
            if (!String.IsNullOrEmpty(Criteres.Type))
            {
                OracleParameter ocpTypeDom = new OracleParameter();
                ocpTypeDom.OracleDbType = OracleDbType.Varchar2;
                ocpTypeDom.ParameterName = "P_TYPE_DOMN";
                ocpTypeDom.Direction = System.Data.ParameterDirection.Input;
                ocpTypeDom.Value = conv.TypeDomaineToChar(Criteres.Type);
                ocm.Parameters.Add(ocpTypeDom);
            }
            //Paramètre DateDebut Domaine 
            if ((Criteres.DateDebut != DateTime.MinValue) || (Criteres.DateDebut.Year != 1))
            {
                OracleParameter ocpDateDebutDom = new OracleParameter();
                ocpDateDebutDom.OracleDbType = OracleDbType.Date;
                ocpDateDebutDom.ParameterName = "P_DATE_DEBUT";
                ocpDateDebutDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateDebutDom.Value = Convert.ToDateTime(Criteres.DateDebut.ToShortDateString());
                ocm.Parameters.Add(ocpDateDebutDom);
            }
            //Paramètre DateFin Domaine 
            if ((Criteres.DateFin != DateTime.MinValue) || (Criteres.DateFin.Year != 1))
            {
                OracleParameter ocpDateFinDom = new OracleParameter();
                ocpDateFinDom.OracleDbType = OracleDbType.Date;
                ocpDateFinDom.ParameterName = "P_DATE_FIN";
                ocpDateFinDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateFinDom.Value = Convert.ToDateTime(Criteres.DateFin.ToShortDateString());
                ocm.Parameters.Add(ocpDateFinDom);
            }
            //Paramètre Description Domaine 
            if (!String.IsNullOrEmpty(Criteres.Description))
            {
                OracleParameter ocpDescDom = new OracleParameter();
                ocpDescDom.OracleDbType = OracleDbType.Varchar2;
                ocpDescDom.ParameterName = "P_DESCR_DOMN";
                ocpDescDom.Direction = System.Data.ParameterDirection.Input;
                ocpDescDom.Value = Criteres.Description;
                ocm.Parameters.Add(ocpDescDom);
            }
            ////Paramètre Nom anglais Domaine 
            if (!String.IsNullOrEmpty(Criteres.NomAnglais))
            {
                OracleParameter ocpNomAnglDom = new OracleParameter();
                ocpNomAnglDom.OracleDbType = OracleDbType.Varchar2;
                ocpNomAnglDom.ParameterName = "P_NOM_ANGLS";
                ocpNomAnglDom.Direction = System.Data.ParameterDirection.Input;
                ocpNomAnglDom.Value = Criteres.NomAnglais;
                ocm.Parameters.Add(ocpNomAnglDom);
            }
            ocm.BindByName = true;

            try
            {
                //Exécution de la requête
                ocn.Open();
                ocm.ExecuteScalar();
                ocn.Close();
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
                resultat = false;
            }

            return resultat;
        }
        public bool MiseAJourDomaineValeur(DomaineValeur Criteres)
        {
            bool resultat = true;
            provenance = "MiseAJourValeurDomaine";

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_MAJ_DOMN_VALR";

            //Paramètre IDValeurDomaine ValeurDomaine Obligatoire
            OracleParameter ocpIDDom = new OracleParameter();
            ocpIDDom.OracleDbType = OracleDbType.Int32;
            ocpIDDom.ParameterName = "P_NO_SEQ_DOMN_VALR";
            ocpIDDom.Direction = System.Data.ParameterDirection.Input;
            ocpIDDom.Value = Criteres.ValeurDomID;
            ocm.Parameters.Add(ocpIDDom);

            //Paramètre Valeur ValeurDomaine Obligatoire
            OracleParameter ocpValeurDom = new OracleParameter();
            ocpValeurDom.OracleDbType = OracleDbType.Varchar2;
            ocpValeurDom.ParameterName = "P_VALR";
            ocpValeurDom.Direction = System.Data.ParameterDirection.Input;
            ocpValeurDom.Value = Criteres.Valeur;
            ocm.Parameters.Add(ocpValeurDom);

            //Paramètre DescAbrg ValeurDomaine Obligatoire
            OracleParameter ocpDescAbrgDom = new OracleParameter();
            ocpDescAbrgDom.OracleDbType = OracleDbType.Varchar2;
            ocpDescAbrgDom.ParameterName = "P_DESCR_ABRG";
            ocpDescAbrgDom.Direction = System.Data.ParameterDirection.Input;
            ocpDescAbrgDom.Value = Criteres.DescAbrg;
            ocm.Parameters.Add(ocpDescAbrgDom);

            //Paramètre Statut ValeurDomaine Obligatoire
            OracleParameter ocpStatDom = new OracleParameter();
            ocpStatDom.OracleDbType = OracleDbType.Varchar2;
            ocpStatDom.ParameterName = "P_STAT_DOMN_VALR";
            ocpStatDom.Direction = System.Data.ParameterDirection.Input;
            ocpStatDom.Value = conv.StatutDomaineToChar(Criteres.Statut);
            ocm.Parameters.Add(ocpStatDom);

            //Paramètre Valeur Defaut ValeurDomaine Obligatoire
            OracleParameter ocpDefValDom = new OracleParameter();
            ocpDefValDom.OracleDbType = OracleDbType.Varchar2;
            ocpDefValDom.ParameterName = "P_VALR_DEFT";
            ocpDefValDom.Direction = System.Data.ParameterDirection.Input;
            ocpDefValDom.Value = conv.ValeurDefautToChar(Criteres.ValeurDefaut);
            ocm.Parameters.Add(ocpDefValDom);

            //Paramètre DescLong Domaine 
            if (!String.IsNullOrEmpty(Criteres.DescLong))
            {
                OracleParameter ocpDescLongDom = new OracleParameter();
                ocpDescLongDom.OracleDbType = OracleDbType.Varchar2;
                ocpDescLongDom.ParameterName = "P_DESCR_LONG";
                ocpDescLongDom.Direction = System.Data.ParameterDirection.Input;
                ocpDescLongDom.Value = Criteres.DescLong;
                ocm.Parameters.Add(ocpDescLongDom);
            }
            //Paramètre Val min Domaine 
            if (!String.IsNullOrEmpty(Criteres.ValMin))
            {
                OracleParameter ocpValMinDom = new OracleParameter();
                ocpValMinDom.OracleDbType = OracleDbType.Varchar2;
                ocpValMinDom.ParameterName = "P_VALR_MIN";
                ocpValMinDom.Direction = System.Data.ParameterDirection.Input;
                ocpValMinDom.Value = Criteres.ValMin;
                ocm.Parameters.Add(ocpValMinDom);
            }
            //Paramètre Val max Domaine 
            if (!String.IsNullOrEmpty(Criteres.ValMax))
            {
                OracleParameter ocpValMaxDom = new OracleParameter();
                ocpValMaxDom.OracleDbType = OracleDbType.Varchar2;
                ocpValMaxDom.ParameterName = "P_VALR_MAX";
                ocpValMaxDom.Direction = System.Data.ParameterDirection.Input;
                ocpValMaxDom.Value = Criteres.ValMax;
                ocm.Parameters.Add(ocpValMaxDom);
            }

            //Paramètre DateDebut Domaine 
            if ((Criteres.DateDebut != DateTime.MinValue) || (Criteres.DateDebut.Year != 1))
            {
                OracleParameter ocpDateDebutDom = new OracleParameter();
                ocpDateDebutDom.OracleDbType = OracleDbType.Date;
                ocpDateDebutDom.ParameterName = "P_DATE_DEBUT";
                ocpDateDebutDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateDebutDom.Value = Convert.ToDateTime(Criteres.DateDebut.ToShortDateString());
                ocm.Parameters.Add(ocpDateDebutDom);
            }
            //Paramètre DateFin Domaine 
            if ((Criteres.DateFin != DateTime.MinValue) || (Criteres.DateFin.Year != 1))
            {
                OracleParameter ocpDateFinDom = new OracleParameter();
                ocpDateFinDom.OracleDbType = OracleDbType.Date;
                ocpDateFinDom.ParameterName = "P_DATE_FIN";
                ocpDateFinDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateFinDom.Value = Convert.ToDateTime(Criteres.DateFin.ToShortDateString());
                ocm.Parameters.Add(ocpDateFinDom);
            }

            ////Paramètre SeqTri Domaine 
            if (!String.IsNullOrEmpty(Criteres.SeqTri.ToString()))
            {
                OracleParameter ocpSeqTriDom = new OracleParameter();
                ocpSeqTriDom.OracleDbType = OracleDbType.Int32;
                ocpSeqTriDom.ParameterName = "P_SEQNC_TRI";
                ocpSeqTriDom.Direction = System.Data.ParameterDirection.Input;
                ocpSeqTriDom.Value = Criteres.SeqTri;
                ocm.Parameters.Add(ocpSeqTriDom);
            }
            ////Paramètre SeqTri Domaine 
            if (!String.IsNullOrEmpty(Criteres.DescAbrgAngl))
            {
                OracleParameter ocpDescAbrgAngDom = new OracleParameter();
                ocpDescAbrgAngDom.OracleDbType = OracleDbType.Varchar2;
                ocpDescAbrgAngDom.ParameterName = "P_DESCR_ABRG_ANGLS";
                ocpDescAbrgAngDom.Direction = System.Data.ParameterDirection.Input;
                ocpDescAbrgAngDom.Value = Criteres.DescAbrgAngl;
                ocm.Parameters.Add(ocpDescAbrgAngDom);
            }

            ocm.BindByName = true;

            try
            {
                //Exécution de la requête
                ocn.Open();
                ocm.ExecuteScalar();
                ocn.Close();
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
                resultat = false;
            }

            return resultat;
        }
        public bool MiseAJourDomaineSousValeur(DomaineSousValeur Criteres)
        {
            bool resultat = true;
            provenance = "MiseAJourSousValeurDomaine";

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_MAJ_DOMN_SOUS_VALR";

            //Paramètre IDValeurDomaine ValeurDomaine Obligatoire
            OracleParameter ocpIDDom = new OracleParameter();
            ocpIDDom.OracleDbType = OracleDbType.Int32;
            ocpIDDom.ParameterName = "P_NO_SEQ_DOMN_SOUS_VALR";
            ocpIDDom.Direction = System.Data.ParameterDirection.Input;
            ocpIDDom.Value = Criteres.ValeurSousDomnID;
            ocm.Parameters.Add(ocpIDDom);

            //Paramètre Valeur ValeurDomaine Obligatoire
            OracleParameter ocpValeurDom = new OracleParameter();
            ocpValeurDom.OracleDbType = OracleDbType.Varchar2;
            ocpValeurDom.ParameterName = "P_VALR";
            ocpValeurDom.Direction = System.Data.ParameterDirection.Input;
            ocpValeurDom.Value = Criteres.Valeur;
            ocm.Parameters.Add(ocpValeurDom);

            //Paramètre DescAbrg ValeurDomaine Obligatoire
            OracleParameter ocpDescAbrgDom = new OracleParameter();
            ocpDescAbrgDom.OracleDbType = OracleDbType.Varchar2;
            ocpDescAbrgDom.ParameterName = "P_DESCR_ABRG";
            ocpDescAbrgDom.Direction = System.Data.ParameterDirection.Input;
            ocpDescAbrgDom.Value = Criteres.DescAbrg;
            ocm.Parameters.Add(ocpDescAbrgDom);

            //Paramètre Statut ValeurDomaine Obligatoire
            OracleParameter ocpStatDom = new OracleParameter();
            ocpStatDom.OracleDbType = OracleDbType.Varchar2;
            ocpStatDom.ParameterName = "P_STAT_DOMN_SOUS_VALR";
            ocpStatDom.Direction = System.Data.ParameterDirection.Input;
            ocpStatDom.Value = conv.StatutDomaineToChar(Criteres.Statut);
            ocm.Parameters.Add(ocpStatDom);

            //Paramètre Valeur Defaut ValeurDomaine Obligatoire
            OracleParameter ocpDefValDom = new OracleParameter();
            ocpDefValDom.OracleDbType = OracleDbType.Varchar2;
            ocpDefValDom.ParameterName = "P_VALR_DEFT";
            ocpDefValDom.Direction = System.Data.ParameterDirection.Input;
            ocpDefValDom.Value = conv.ValeurDefautToChar(Criteres.ValeurDefaut);
            ocm.Parameters.Add(ocpDefValDom);

            //Paramètre DescLong Domaine 
            if (!String.IsNullOrEmpty(Criteres.DescLong))
            {
                OracleParameter ocpDescLongDom = new OracleParameter();
                ocpDescLongDom.OracleDbType = OracleDbType.Varchar2;
                ocpDescLongDom.ParameterName = "P_DESCR_LONG";
                ocpDescLongDom.Direction = System.Data.ParameterDirection.Input;
                ocpDescLongDom.Value = Criteres.DescLong;
                ocm.Parameters.Add(ocpDescLongDom);
            }
            //Paramètre Val min Domaine 
            if (!String.IsNullOrEmpty(Criteres.ValMin))
            {
                OracleParameter ocpValMinDom = new OracleParameter();
                ocpValMinDom.OracleDbType = OracleDbType.Varchar2;
                ocpValMinDom.ParameterName = "P_VALR_MIN";
                ocpValMinDom.Direction = System.Data.ParameterDirection.Input;
                ocpValMinDom.Value = Criteres.ValMin;
                ocm.Parameters.Add(ocpValMinDom);
            }
            //Paramètre Val max Domaine 
            if (!String.IsNullOrEmpty(Criteres.ValMax))
            {
                OracleParameter ocpValMaxDom = new OracleParameter();
                ocpValMaxDom.OracleDbType = OracleDbType.Varchar2;
                ocpValMaxDom.ParameterName = "P_VALR_MAX";
                ocpValMaxDom.Direction = System.Data.ParameterDirection.Input;
                ocpValMaxDom.Value = Criteres.ValMax;
                ocm.Parameters.Add(ocpValMaxDom);
            }

            //Paramètre DateDebut Domaine 
            if ((Criteres.DateDebut != DateTime.MinValue) || (Criteres.DateDebut.Year != 1))
            {
                OracleParameter ocpDateDebutDom = new OracleParameter();
                ocpDateDebutDom.OracleDbType = OracleDbType.Date;
                ocpDateDebutDom.ParameterName = "P_DATE_DEBUT";
                ocpDateDebutDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateDebutDom.Value = Convert.ToDateTime(Criteres.DateDebut.ToShortDateString());
                ocm.Parameters.Add(ocpDateDebutDom);
            }
            //Paramètre DateFin Domaine 
            if ((Criteres.DateFin != DateTime.MinValue) || (Criteres.DateFin.Year != 1))
            {
                OracleParameter ocpDateFinDom = new OracleParameter();
                ocpDateFinDom.OracleDbType = OracleDbType.Date;
                ocpDateFinDom.ParameterName = "P_DATE_FIN";
                ocpDateFinDom.Direction = System.Data.ParameterDirection.Input;
                ocpDateFinDom.Value = Convert.ToDateTime(Criteres.DateFin.ToShortDateString());
                ocm.Parameters.Add(ocpDateFinDom);
            }

            ////Paramètre SeqTri Domaine 
            if (!String.IsNullOrEmpty(Criteres.SeqTri.ToString()))
            {
                OracleParameter ocpSeqTriDom = new OracleParameter();
                ocpSeqTriDom.OracleDbType = OracleDbType.Int32;
                ocpSeqTriDom.ParameterName = "P_SEQNC_TRI";
                ocpSeqTriDom.Direction = System.Data.ParameterDirection.Input;
                ocpSeqTriDom.Value = Criteres.SeqTri;
                ocm.Parameters.Add(ocpSeqTriDom);
            }
            ////Paramètre SeqTri Domaine 
            if (!String.IsNullOrEmpty(Criteres.DescAbrgAngl))
            {
                OracleParameter ocpDescAbrgAngDom = new OracleParameter();
                ocpDescAbrgAngDom.OracleDbType = OracleDbType.Varchar2;
                ocpDescAbrgAngDom.ParameterName = "P_DESCR_ABRG_ANGLS";
                ocpDescAbrgAngDom.Direction = System.Data.ParameterDirection.Input;
                ocpDescAbrgAngDom.Value = Criteres.DescAbrgAngl;
                ocm.Parameters.Add(ocpDescAbrgAngDom);
            }

            ocm.BindByName = true;

            try
            {
                //Exécution de la requête
                ocn.Open();
                ocm.ExecuteScalar();
                ocn.Close();
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
                resultat = false;
            }

            return resultat;
        }
        public bool SupprimerDomaine(int idDomaine)
        {
            provenance = "SupprimerDomaine";
            bool resultat = true;


            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SPR_DOMN";

            //Paramètre Identifiant d'activité
            OracleParameter ocpIdDomaine = new OracleParameter();
            ocpIdDomaine.OracleDbType = OracleDbType.Int32;
            ocpIdDomaine.ParameterName = "P_NO_SEQ_DOMN";
            ocpIdDomaine.Direction = System.Data.ParameterDirection.Input;
            ocpIdDomaine.Value = idDomaine;
            ocm.Parameters.Add(ocpIdDomaine);
            try
            {
                //Exécution de la requête
                ocn.Open();
                ocm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
                resultat = false;
            }
            ocn.Close();
            ocpIdDomaine.Dispose();
            ocn.Dispose();
            ocm.Dispose();

            return resultat;

        }
        public bool SupprimerDomaineValeur(int idValeurDomaine)
        {
            provenance = "SupprimerValeurDomaine";
            bool resultat = true;


            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SPR_DOMN_VALR";

            //Paramètre Identifiant d'activité
            OracleParameter ocpIdDomaine = new OracleParameter();
            ocpIdDomaine.OracleDbType = OracleDbType.Int32;
            ocpIdDomaine.ParameterName = "P_NO_SEQ_DOMN_VALR";
            ocpIdDomaine.Direction = System.Data.ParameterDirection.Input;
            ocpIdDomaine.Value = idValeurDomaine;
            ocm.Parameters.Add(ocpIdDomaine);
            try
            {
                //Exécution de la requête
                ocn.Open();
                ocm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
                resultat = false;
            }
            ocn.Close();
            ocpIdDomaine.Dispose();
            ocn.Dispose();
            ocm.Dispose();

            return resultat;

        }
        public bool SupprimerDomaineSousValeur(int idSousValeurDomaine)
        {
            provenance = "SupprimerSousValeurDomaine";
            bool resultat = true;


            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SPR_DOMN_SOUS_VALR";

            //Paramètre Identifiant d'activité
            OracleParameter ocpIdDomaine = new OracleParameter();
            ocpIdDomaine.OracleDbType = OracleDbType.Int32;
            ocpIdDomaine.ParameterName = "P_NO_SEQ_DOMN_SOUS_VALR";
            ocpIdDomaine.Direction = System.Data.ParameterDirection.Input;
            ocpIdDomaine.Value = idSousValeurDomaine;
            ocm.Parameters.Add(ocpIdDomaine);
            try
            {
                //Exécution de la requête
                ocn.Open();
                ocm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
                resultat = false;
            }
            ocn.Close();
            ocpIdDomaine.Dispose();
            ocn.Dispose();
            ocm.Dispose();

            return resultat;

        }
        public List<string> ObtenirOperateurs(int typeTri)
        {
            provenance = "ObtenirOperateurs";
            List<string> lstOperateurs = new List<string>();

            OracleConnection ocn = new OracleConnection(connexion);
            OracleCommand ocm = new OracleCommand();
            ocm.Connection = ocn;

            ocm.CommandType = System.Data.CommandType.StoredProcedure;
            ocm.CommandText = "BCOM_P_SEL_OPERT_COMPR";

            //Paramètre type de tri
            OracleParameter ocpTypeTri = new OracleParameter();
            ocpTypeTri.OracleDbType = OracleDbType.Int32;
            ocpTypeTri.ParameterName = "P_TYPE_TRI";
            ocpTypeTri.Direction = System.Data.ParameterDirection.Input;
            ocpTypeTri.Value = typeTri;
            ocm.Parameters.Add(ocpTypeTri);

            //Paramètre Résultat
            OracleParameter ocpResultat = new OracleParameter();
            ocpResultat.OracleDbType = OracleDbType.RefCursor;
            ocpResultat.ParameterName = "P_RESULTATS";
            ocpResultat.Direction = System.Data.ParameterDirection.Output;
            ocm.Parameters.Add(ocpResultat);

            ocm.BindByName = true;

            try
            {
                ocn.Open();
                OracleDataReader odr = ocm.ExecuteReader();
                while (odr.Read())
                {
                    lstOperateurs.Add(odr[0].ToString());
                }
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
            }

            return lstOperateurs;

        }
        public List<Domaine> LierBdDomaine(string provenance, OracleConnection ocn, OracleCommand ocm)
        {
            List<Domaine> lstNomDomaine = new List<Domaine>();

            ocm.BindByName = true;

            try
            {
                //Exécution de la requête
                ocn.Open();
                // ocm.ExecuteScalar();

                OracleDataReader reader = ocm.ExecuteReader();
                while (reader.Read())
                {
                    Domaine domaine = new Domaine();
                    domaine.IDDomaine = Convert.ToInt32(reader[0]);
                    domaine.Code = reader[1].ToString();
                    domaine.Nom = reader[2].ToString();
                    domaine.Description = reader[3].ToString();
                    domaine.Statut = conv.StatutDomaineToString(reader[4].ToString());
                    DateTime dtDateDebut = DateTime.MinValue;
                    DateTime dtDateFin = DateTime.MinValue;
                    DateTime.TryParse(reader[5].ToString(), out dtDateDebut);
                    DateTime.TryParse(reader[6].ToString(), out dtDateFin);
                    domaine.DateDebut = dtDateDebut;
                    domaine.DateFin = dtDateFin;
                    domaine.NomAnglais = reader[7].ToString();
                    domaine.Type = conv.TypeDomaineToString(reader[8].ToString());

                    lstNomDomaine.Add(domaine);
                }
                reader.Dispose();

                ocn.Close();
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
            }
            return lstNomDomaine;

        }
        public List<DomaineValeur> LierBdValeurDomaine(string provenance, OracleConnection ocn, OracleCommand ocm)
        {
            List<DomaineValeur> lstValeurDomaine = new List<DomaineValeur>();

            ocm.BindByName = true;

            try
            {
                //Exécution de la requête
                ocn.Open();
                OracleDataReader reader = ocm.ExecuteReader();
                while (reader.Read())
                {
                    DomaineValeur valDomaine = new DomaineValeur();
                    valDomaine.ValeurDomID = Convert.ToInt32(reader[0]);

                    valDomaine.DomaineID = Convert.ToInt32(reader[1]);
                    valDomaine.Valeur = reader[2].ToString();
                    valDomaine.DescAbrg = reader[3].ToString();
                    valDomaine.DescLong = reader[4].ToString();
                    valDomaine.Statut = conv.StatutDomaineToString(reader[5].ToString());
                    valDomaine.ValMin = reader[6].ToString();
                    valDomaine.ValMax = reader[7].ToString();
                    DateTime dtDateDebut = DateTime.MinValue;
                    DateTime dtDateFin = DateTime.MinValue;
                    DateTime.TryParse(reader[8].ToString(), out dtDateDebut);
                    DateTime.TryParse(reader[9].ToString(), out dtDateFin);
                    valDomaine.DateDebut = dtDateDebut;
                    valDomaine.DateFin = dtDateFin;
                    valDomaine.ValeurDefaut = conv.ValeurDefautToString(reader[10].ToString());
                    int seqTriInt = 0;
                    Int32.TryParse(reader[11].ToString(), out seqTriInt);
                    valDomaine.SeqTri = seqTriInt;
                    valDomaine.DescAbrgAngl = reader[12].ToString();

                    lstValeurDomaine.Add(valDomaine);
                }
                reader.Dispose();

                ocn.Close();
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
            }
            return lstValeurDomaine;

        }
        public List<DomaineSousValeur> LierBdSousValeurDomaine(string provenance, OracleConnection ocn, OracleCommand ocm)
        {
            List<DomaineSousValeur> lstSousValeurDomaine = new List<DomaineSousValeur>();

            ocm.BindByName = true;

            try
            {
                //Exécution de la requête
                ocn.Open();
                OracleDataReader reader = ocm.ExecuteReader();
                while (reader.Read())
                {
                    DomaineSousValeur sousValDomaine = new DomaineSousValeur();
                    sousValDomaine.ValeurSousDomnID = Convert.ToInt32(reader[0]);

                    sousValDomaine.ValeurDomnID = Convert.ToInt32(reader[1]);
                    sousValDomaine.Valeur = reader[2].ToString();
                    sousValDomaine.DescAbrg = reader[3].ToString();
                    sousValDomaine.DescLong = reader[4].ToString();
                    sousValDomaine.Statut = conv.StatutDomaineToString(reader[5].ToString());
                    sousValDomaine.ValMin = reader[6].ToString();
                    sousValDomaine.ValMax = reader[7].ToString();
                    DateTime dtDateDebut = DateTime.MinValue;
                    DateTime dtDateFin = DateTime.MinValue;
                    DateTime.TryParse(reader[8].ToString(), out dtDateDebut);
                    DateTime.TryParse(reader[9].ToString(), out dtDateFin);
                    sousValDomaine.DateDebut = dtDateDebut;
                    sousValDomaine.DateFin = dtDateFin;
                    sousValDomaine.ValeurDefaut = conv.ValeurDefautToString(reader[10].ToString());
                    int seqTriInt = 0;
                    Int32.TryParse(reader[11].ToString(), out seqTriInt);
                    sousValDomaine.SeqTri = seqTriInt;
                    sousValDomaine.DescAbrgAngl = reader[12].ToString();

                    lstSousValeurDomaine.Add(sousValDomaine);
                }
                reader.Dispose();

                ocn.Close();
            }
            catch (Exception ex)
            {
                journal.Journaliser("FonctionsBD " + provenance + " " + ex.Message);
            }
            return lstSousValeurDomaine;

        }
    }
}
