using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collation : MonoBehaviour, IRamassable
{
    ComportementJoueur Joueur;
    public ArbreCollation ArbreParent;
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
        ArbreParent.CollationPrete = false;
        Destroy(gameObject);
    }
}
