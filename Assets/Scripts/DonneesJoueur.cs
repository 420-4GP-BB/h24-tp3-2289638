using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonneesJoueur : MonoBehaviour
{
    public string Nom { get; private set; }

    void Awake()
    {
        Nom = ParametresParties.Instance.NomJoueur;
    }
}
