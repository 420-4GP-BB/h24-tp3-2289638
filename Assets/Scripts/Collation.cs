using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collation : MonoBehaviour, IRamassable
{
    ComportementJoueur Joueur;              // Nécessaire afin de pouvoir appeler la fonction manger lorsque la collation est ramassée.
    public ArbreCollation ArbreParent;      // Nécessaire afin de savoir de quelle arbre provient la collation, et de pouvoir alerter cette dernière lorsque la collation est mangée.
    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        Joueur = sujet;
        return new EtatRamasserObjet(sujet,this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;                        // Dès qu'elle est visible, la collation est ramassable.
    }

    public void Ramasser(Inventaire inventaireJoueur)
    {
        Joueur.Manger(true);                // Un paramètre boolean a été ajouté à la fonction Manger de la classe ComportementJoueur afin de differéncier le cas de manger
                                            // immédiatement un objet ramassé de celui de chercher un objet à manger (maison).
        ArbreParent.CollationPrete = false; // C'est la collation qui s'occupe de dire à l'arbre qu'elle a été ramassée.
        Destroy(gameObject);
    }
}
