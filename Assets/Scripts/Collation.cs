using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collation : MonoBehaviour, IRamassable
{
    ComportementJoueur Joueur;
    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        Joueur = sujet;
        return new EtatRamasserObjet(sujet,this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;
    }

    public void Ramasser(Inventaire inventaireJoueur)
    {
        Joueur.Manger(true);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
