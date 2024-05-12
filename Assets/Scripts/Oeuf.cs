using UnityEngine;

public class Oeuf : MonoBehaviour, IRamassable
{
    [SerializeField] private float pourcentageEclore = 25;
    [SerializeField] private GameObject Poule;              // Le prefab de la poule est n�cessaire afin que l'oeuf puisse la faire apparaitre.
    private Soleil _soleil;
    private float tempsPondu;                               // Ces trois variables servent � pouvoir calculer quand trois jours auront pass�e depuis la cr�ation de ce script
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
        tempsPondu = 1440 - _soleil.ProportionRestante * 1440;  // Je suis au courant que �a fonctionnerait aussi sans la soustraction, mais je pr�f�re travailler avec le temps qu'on est
                                                                // et non le temps restant.
        _soleil.OnJourneeTerminee += AjouterJournee;
    }
    void Update()
    {
        float tempsActuel = 1440 - _soleil.ProportionRestante * 1440;
        float tempsGap = tempsPondu - tempsActuel;
        if (jourPassees >2 && tempsGap<15)          // Pour s'assurer qu'on ne le manque pas.
        {
            float roll = Random.value * 100;        // Et non Random.Range(0,4) afin de donner l'option de customiser la chance d'�closion en pourcentage � partir du editor.
            if (roll < pourcentageEclore)
            {
                GameObject poule = Instantiate(Poule);
                poule.GetComponent<MouvementPoulet>().PouleAchetee=false;               // Signale au code que cette poule n'a pas �t� achet�e, pour que ce dernier puisse
                poule.GetComponent<MouvementPoulet>().PositionOeuf=transform.position;  // r�agir de mani�re appropri� (faire apparaitre la poule sur l'emplacement de l'oeuf)
            }
            Destroy(gameObject);
        }
    }
    private void AjouterJournee()   // M�thode abonn�e � l'event du soleil, pour compter le nombre de jour brute qui a passer depuis que l'oeuf a �t� pondu
    {
        jourPassees++;
    }
}