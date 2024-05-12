using UnityEngine;

public class Oeuf : MonoBehaviour, IRamassable
{
    [SerializeField] private float pourcentageEclore = 25;
    [SerializeField] private GameObject Poule;              // Le prefab de la poule est nécessaire afin que l'oeuf puisse la faire apparaitre.
    private Soleil _soleil;
    private float tempsPondu;                               // Ces trois variables servent à pouvoir calculer quand trois jours auront passée depuis la création de ce script
    private int jourPassees;
    public void Ramasser(Inventaire inventaireJoueur)
    {
        inventaireJoueur.Oeuf++;
        Destroy(gameObject);
    }

    public EtatJoueur EtatAUtiliser(ComportementJoueur Sujet)
    {
        return new EtatRamasserObjet(Sujet, this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;
    }
    void Start()
    {
        _soleil = FindFirstObjectByType<Soleil>();
        jourPassees = 0;
        tempsPondu = 1440 - _soleil.ProportionRestante * 1440;  // Je suis au courant que ça fonctionnerait aussi sans la soustraction, mais je préfère travailler avec le temps qu'on est
                                                                // et non le temps restant.
        _soleil.OnJourneeTerminee += AjouterJournee;
    }
    void Update()
    {
        float tempsActuel = 1440 - _soleil.ProportionRestante * 1440;
        float tempsGap = tempsPondu - tempsActuel;
        if (jourPassees >2 && tempsGap<15)          // Pour s'assurer qu'on ne le manque pas.
        {
            float roll = Random.value * 100;        // Et non Random.Range(0,4) afin de donner l'option de customiser la chance d'éclosion en pourcentage à partir du editor.
            if (roll < pourcentageEclore)
            {
                GameObject poule = Instantiate(Poule);
                poule.GetComponent<MouvementPoulet>().PouleAchetee=false;               // Signale au code que cette poule n'a pas été achetée, pour que ce dernier puisse
                poule.GetComponent<MouvementPoulet>().PositionOeuf=transform.position;  // réagir de manière approprié (faire apparaitre la poule sur l'emplacement de l'oeuf)
            }
            Destroy(gameObject);
        }
    }
    private void AjouterJournee()   // Méthode abonnée à l'event du soleil, pour compter le nombre de jour brute qui a passer depuis que l'oeuf a été pondu
    {
        jourPassees++;
    }
}