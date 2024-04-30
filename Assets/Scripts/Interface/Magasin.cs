using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Magasin : MonoBehaviour
{
    const int PRIX_OEUF = 25;
    const int PRIX_POULE = 100;
    const int PRIX_GRAINES = 3;
    const int PRIX_CHOUX = 5;

    public static int ITEM_LE_MOINS_CHER
    {
        get => Math.Min(
            Math.Min(PRIX_OEUF, PRIX_POULE),
            Math.Min(PRIX_GRAINES, PRIX_CHOUX)
        );
    }

    [SerializeField] private GameObject _modelePoule;

    [SerializeField] private TMP_Text _prixOeuf;

    [SerializeField] private TMP_Text _prixPoule;

    [SerializeField] private TMP_Text _prixGraines;

    [SerializeField] private TMP_Text _prixChoux;


    [SerializeField] private TMP_Text _messageBienvenue;

    [SerializeField] private GameObject _sortieMagasin;

    [SerializeField] private GameObject _panelMagasin;

    [SerializeField] private Button _boutonViande;

    [SerializeField] private Button _boutonPoule;

    [SerializeField] private Button _boutonGraines;

    [SerializeField] private Button _boutonChoux;

    private GameObject _joueur;
    private Inventaire _inventaire;

// Start is called before the first frame update
    void Start()
    {
        // Les prix
        _prixOeuf.text = PRIX_OEUF.ToString();
        _prixPoule.text = PRIX_POULE.ToString();
        _prixGraines.text = PRIX_GRAINES.ToString();
        _prixChoux.text = PRIX_CHOUX.ToString();


        _joueur = GameObject.Find("Joueur");
        _messageBienvenue.text = $"Bonjour {_joueur.GetComponent<DonneesJoueur>().Nom}, " +
                                 "que puis-je faire pour toi aujourd'hui ?";
        _inventaire = _joueur.GetComponent<Inventaire>();
        _panelMagasin.SetActive(false);

        _boutonViande.onClick.AddListener(() =>
        {
            _inventaire.Or -= PRIX_OEUF;
            _inventaire.Oeuf += 1;
        });

        _boutonPoule.onClick.AddListener(() =>
        {
            _inventaire.Or -= PRIX_POULE;
            GameObject poule = Instantiate(_modelePoule);
        });

        _boutonGraines.onClick.AddListener(() =>
        {
            _inventaire.Or -= PRIX_GRAINES;
            _inventaire.Graines += 1;
        });

        _boutonChoux.onClick.AddListener(() =>
        {
            _inventaire.Choux -= 1;
            _inventaire.Or += PRIX_CHOUX;
        });
    }

// Update is called once per frame
    void Update()
    {
        _boutonViande.interactable = _inventaire.Or >= PRIX_OEUF;
        _boutonPoule.interactable = _inventaire.Or >= PRIX_POULE;
        _boutonGraines.interactable = _inventaire.Or >= PRIX_GRAINES;
        _boutonChoux.interactable = _inventaire.Choux >= 1;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _joueur)
        {
            _panelMagasin.SetActive(true);
            _joueur.GetComponent<ComportementJoueur>().ChangerEtat(new EtatDansMenu(_joueur.GetComponent<ComportementJoueur>()));
        }
    }


    public void SortirMagasin()
    {
        _panelMagasin.SetActive(false);
        _joueur.GetComponent<CharacterController>().enabled = false;
        _joueur.transform.position = _sortieMagasin.transform.position;
        _joueur.transform.LookAt(_sortieMagasin.transform);
        _joueur.transform.Rotate(0, 180, 0);
        _joueur.GetComponent<CharacterController>().enabled = true;
        _joueur.GetComponent<ComportementJoueur>().ChangerEtat(_joueur.GetComponent<ComportementJoueur>().EtatNormal);
    }
}