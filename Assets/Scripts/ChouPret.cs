using System;
using Unity.VisualScripting;
using UnityEngine;

public class ChouPret : MonoBehaviour, IRamassable
{
    private ChouMesh3D _chouMesh3D;
    private void Start()
    {
        _chouMesh3D = GetComponent<ChouMesh3D>();
        _chouMesh3D.ObjetPret.SetActive(true);
    }

    public void Ramasser(Inventaire inventaireJoueur)
    {
        // Cueillir
        _chouMesh3D.CacherObjets();
        inventaireJoueur.Choux++;
        Destroy(this);
    }

    public EtatJoueur EtatAUtiliser(ComportementJoueur Sujet)
    {
        return new EtatRamasserObjet(Sujet, this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;
    }
}