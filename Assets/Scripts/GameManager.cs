using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.AI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Soleil _soleil;
    [SerializeField] private GameObject WoodLog;
    [SerializeField] private GameObject JoueurParDefaut;
    [SerializeField] private float TailleCarreeDelimiteurArbre = 4f;    // On peut décider dynamiquement à partir du éditeur de la distance entre les arbres.
    [SerializeField] private GameObject ArbrePrefab;

    private ComportementJoueur _joueur;

    private const float DISTANCE_ACTION = 3.0f;

    private Inventaire _inventaireJoueur;
    private EnergieJoueur _energieJoueur;
    private ChouMesh3D[] _chous;
    public int NumeroJour = 1;
    private Vector3 SpawnJoueur = new Vector3(58.5f, 0, -52f);

    private StrategieForet strategieForet;

    void Start()
    {                                                                   
        strategieForet = new StrategieGrille();
        CreerJoueur();
        _joueur = GameObject.Find("Joueur").GetComponent<ComportementJoueur>();
        _inventaireJoueur = _joueur.GetComponent<Inventaire>();
        _energieJoueur = _joueur.GetComponent<EnergieJoueur>();
        _chous = FindObjectsByType<ChouMesh3D>(FindObjectsSortMode.None);
        GameObject[] arbres = FindObjectsStartingWith("Arbre");
        strategieForet.GenererForet(TailleCarreeDelimiteurArbre, ArbrePrefab, arbres);  // Utilisaiton de patron stratégie.
        // Patron de conception: Observateur
        FindObjectOfType<Soleil>().OnJourneeTerminee += NouvelleJournee;

        _energieJoueur.OnEnergieVide += EnergieVide;
    }

    void NouvelleJournee()
    {
        NumeroJour++;

        GameObject[] poules = GameObject.FindGameObjectsWithTag("Poule");
        foreach (var poule in poules)
        {
            poule.GetComponent<PondreOeufs>().DeterminerPonte();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuConfiguration");
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            Time.timeScale = 45;
        }
        else
        {
            Time.timeScale = 1;
        }

        // L'?tat du joueur peut affecter le passage du temps (ex.: Dodo: tout va vite, menus: le temps est stopp?, etc)
        Time.timeScale *= _joueur.GetComponent<ComportementJoueur>().MultiplicateurScale;
    }

    /// <summary>
    /// Partie perdue quand l'?nergie tombe ? z?ro
    /// </summary>
    private void EnergieVide()
    {
        _joueur.ChangerEtat(new EtatDansMenu(_joueur));
        GestionnaireMessages.Instance.AfficherMessageFin(
            "Plus d'?nergie!",
            "Vous n'avez pas r?ussi ? vous garder en vie, vous tombez sans connaissance au milieu du champ." +
            "Un loup passe et vous d?guste en guise de d?ner. Meilleure chance la prochaine partie!");
        Time.timeScale = 0;
    }
    GameObject[] FindObjectsStartingWith(string prefix)
    {
        // Source de ce code : ChatGPT
        return GameObject.FindObjectsOfType<GameObject>()
            .Where(obj => obj.name.StartsWith(prefix))
            .ToArray();
    }
    private void CreerJoueur()
    {
        if (!ParametresParties.Instance.ModelJoueur)
        {
            ParametresParties.Instance.ModelJoueur = JoueurParDefaut;
        }
        GameObject Joueur = Instantiate(ParametresParties.Instance.ModelJoueur);
        Joueur.name = "Joueur";
        Joueur.GetComponent<CharacterController>().enabled = false;
        Joueur.transform.position = SpawnJoueur;
        Joueur.GetComponent<CharacterController>().enabled = true;

        // enlever les
        foreach (Transform child in Joueur.transform)
        {
            if (child.name.Contains("Naked"))
            {
                child.gameObject.SetActive(false);
            }
        }
        //_inventaireJoueur = Joueur.AddComponent<Inventaire>();
        //_energieJoueur = Joueur.AddComponent<EnergieJoueur>();
        //Joueur.AddComponent<CharacterController>();
        //Joueur.AddComponent<NavMeshAgent>();
        //_joueur = Joueur.AddComponent<ComportementJoueur>();
    }
}