using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour, IRamassable
{
    // Le code du prof est très optimisé! Ajouter des objets ramassable est très facile grace à ce dernier.
    // J'implemente donc l'interface IRamassable, et le code du prof s'occupe du reste (Dans comportementJoueur, le code vérifie si le joueur clique sur un objet qui implémente
    // l'interface IActionnable (qui est implementée par IRamassable))
    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        return new EtatRamasserObjet(sujet,this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;    // Ramasser la buche est toujours permis. Dès qu'elle est visible, elle est ramassable.
    }

    public void Ramasser(Inventaire inventaireJoueur)
    {
        inventaireJoueur.Buches++;
        Destroy(gameObject);
    }
}
