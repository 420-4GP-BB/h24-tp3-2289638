using System;
using UnityEngine;

public class ChouCroissant : MonoBehaviour
{
    private ChouMesh3D _chouMesh3D;
    private float _tempsCroissance;

    private Soleil _soleil;

    private int journeesDeVie = 0;

    public bool Pret
    {
        get => journeesDeVie >= ParametresParties.Instance.TempsCroissance;
    }

    private void Start()
    {
        _soleil = FindObjectOfType<Soleil>();
        _chouMesh3D = GetComponent<ChouMesh3D>();
    }


    void Update()
    {
        _tempsCroissance += _soleil.DeltaMinutesEcoulees;
        if (_tempsCroissance >= ConstantesJeu.MINUTES_PAR_JOUR)
        {
            _tempsCroissance = 0.0f;
            JourneePassee();
        }
    }


    public void JourneePassee()
    {
        journeesDeVie++;

        // Si on ne cueille pas le chou assez vite
        if (journeesDeVie >= ParametresParties.Instance.TempsCroissance + ParametresParties.Instance.DelaiCueillete)
        {
            // Le chou est perdu
            ChouPerdu();
            return;
        }

        // Utilise le mesh approprié
        if (Pret)
        {
            ChouPret();
        }
        else
        {
            // En pousse
            _chouMesh3D.CacherObjets();
            _chouMesh3D.ObjetPetit.SetActive(true);
        }
    }

    private void ChouPerdu()
    {
        _chouMesh3D.CacherObjets();
    }

    private void ChouPret()
    {
        _chouMesh3D.CacherObjets();
        _chouMesh3D.ObjetPret.SetActive(true);
        gameObject.AddComponent<ChouPret>();
        Destroy(this);
    }
}