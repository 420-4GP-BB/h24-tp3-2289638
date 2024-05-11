using UnityEngine;

public class Oeuf : MonoBehaviour, IRamassable
{
    [SerializeField] private float pourcentageEclore = 25;
    [SerializeField] private GameObject Poule;
    private Soleil _soleil;
    private float tempsPondu;
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
        tempsPondu = 1440 - _soleil.ProportionRestante * 1440;
        _soleil.OnJourneeTerminee += AjouterJournee;
    }
    void Update()
    {
        float tempsActuel = 1440 - _soleil.ProportionRestante * 1440;
        float tempsGap = tempsPondu - tempsActuel;
        if (jourPassees >2 && tempsGap<15)          // Pour s'assurer qu'on ne le manque pas.
        {
            float roll = Random.value * 100;
            if (roll < pourcentageEclore)
            {
                GameObject poule = Instantiate(Poule);
                poule.GetComponent<MouvementPoulet>().PouleAchetee=false;
                poule.GetComponent<MouvementPoulet>().PositionOeuf=transform.position;
            }
            Destroy(gameObject);
        }
    }
    private void AjouterJournee()
    {
        jourPassees++;
    }
}