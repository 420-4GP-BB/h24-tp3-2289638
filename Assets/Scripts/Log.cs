using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour, IRamassable
{
    // Le code du prof est tr�s optimis�! Ajouter des objets ramassable est tr�s facile grace � ce dernier.
    // J'implemente donc l'interface IRamassable, et le code du prof s'occupe du reste (Dans comportementJoueur, le code v�rifie si le joueur clique sur un objet qui impl�mente
    // l'interface IActionnable (qui est implement�e par IRamassable))
    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        return new EtatRamasserObjet(sujet,this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;    // Ramasser la buche est toujours permis. D�s qu'elle est visible, elle est ramassable.
    }

    public void Ramasser(Inventaire inventaireJoueur)
    {
        inventaireJoueur.Buches++;
        Destroy(gameObject);
    }
}
