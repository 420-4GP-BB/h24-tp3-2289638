using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour, IRamassable
{
    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        return new EtatRamasserObjet(sujet,this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;
    }

    public void Ramasser(Inventaire inventaireJoueur)
    {
        inventaireJoueur.Buches++;
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
