using System;
using UnityEngine;

public class EnergieJoueur : MonoBehaviour
{
    private bool _modeImmortel;

    public event Action OnEnergieVide;

    private Soleil _soleil;
    private ComportementJoueur _mouvementJoueur;
    private float _energie = 1.0f;

    public float Energie
    {
        get => _energie;
        set
        {
            _energie = value;
            _energie = Mathf.Clamp(_energie, 0, 1);
        }
    }

    public bool EnergieFaible => Energie < 0.2f;

    void Start()
    {
        _energie = 1.0f;
        _mouvementJoueur = GetComponent<ComportementJoueur>();
        _soleil = GameObject.Find("Directional Light").GetComponent<Soleil>();
    }

    void Update()
    {
        float quantite = _mouvementJoueur.ConsommationEnergie;
        float minutesEcoulees = _soleil.DeltaMinutesEcoulees;

        if (_soleil.EstNuit)
        {
            quantite *= 2;
        }

        if (_modeImmortel)
        {
            quantite = 0;
        }

        Energie -= quantite * minutesEcoulees;

        if (Energie <= 0)
        {
            OnEnergieVide?.Invoke();
        }
    }

    public void BasculerImmortel()
    {
        _modeImmortel = !_modeImmortel;

        if (_modeImmortel)
            Energie = 1;
    }
}