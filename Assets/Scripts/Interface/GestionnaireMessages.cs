using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GestionnaireMessages : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private UnityEngine.GameObject _panel;
    [SerializeField] private UnityEngine.GameObject _menuMaison;
    [SerializeField] private TMP_Text _titre;
    [SerializeField] private TMP_Text _contenu;
    [SerializeField] private ComportementJoueur _mouvementJoueur;

    private bool _finDePartie;

    /// <summary>
    /// Semi-singleton. Pas exactement un "singleton Unity" comme vu en cours,
    /// puisque sa portée est limitée à une seule scène
    /// </summary>
    public static GestionnaireMessages Instance { get; private set; } = null;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GetComponent<GameManager>();
        Assert.IsNull(Instance, "Un seul GestionnaireMessage est permis par scène");
        Instance = this;

        _panel.SetActive(false);
        _finDePartie = false;
    }

    public void AfficherMessage(string titre, string message)
    {
        _titre.text = titre;
        _contenu.text = message;
        _panel.SetActive(true);
    }


    public void AfficherMessageFin(string titre, string message)
    {
        AfficherMessage(titre, message);
        _finDePartie = true;
    }

    public void Fermer()
    {
        _panel.SetActive(false);
//        _mouvementJoueur.DansDialogue = false;
        if (_finDePartie)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuConfiguration");
        }
    }
}