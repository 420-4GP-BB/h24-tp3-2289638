using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbre : MonoBehaviour , IPoussable
{
    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        return new EtatPousser(sujet,this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;
    }
    public void Pousser()
    {
        throw new System.NotImplementedException();
    }
}
