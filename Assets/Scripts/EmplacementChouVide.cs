using System;
using UnityEngine;

public class EmplacementChouVide : MonoBehaviour, IPlantable
{
    private ChouMesh3D _chouMesh3D;

    private void Start()
    {
        _chouMesh3D = GetComponent<ChouMesh3D>();
    }

    public void Planter(Inventaire inventaireJoueur)
    {
        inventaireJoueur.Graines--;
        _chouMesh3D.ObjetBebe.SetActive(true);
        gameObject.AddComponent<ChouCroissant>();
        Destroy(this);
    }


    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        return new EtatPlanter(sujet, this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return sujet.GetComponent<Inventaire>().Graines > 0;
    }
}