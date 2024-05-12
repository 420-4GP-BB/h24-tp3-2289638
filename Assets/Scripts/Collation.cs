using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collation : MonoBehaviour, IRamassable
{
    ComportementJoueur Joueur;              // N�cessaire afin de pouvoir appeler la fonction manger lorsque la collation est ramass�e.
    public ArbreCollation ArbreParent;      // N�cessaire afin de savoir de quelle arbre provient la collation, et de pouvoir alerter cette derni�re lorsque la collation est mang�e.
    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        Joueur = sujet;
        return new EtatRamasserObjet(sujet,this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;                        // D�s qu'elle est visible, la collation est ramassable.
    }

    public void Ramasser(Inventaire inventaireJoueur)
    {
        Joueur.Manger(true);                // Un param�tre boolean a �t� ajout� � la fonction Manger de la classe ComportementJoueur afin de differ�ncier le cas de manger
                                            // imm�diatement un objet ramass� de celui de chercher un objet � manger (maison).
        ArbreParent.CollationPrete = false; // C'est la collation qui s'occupe de dire � l'arbre qu'elle a �t� ramass�e.
        Destroy(gameObject);
    }
}
