using System;
using UnityEngine;
using UnityEngine.AI;

public class ComportementJoueur : MonoBehaviour
{
    private EtatJoueur _etat;

    public EtatNormal EtatNormal { get; private set; }
    public float TempsDepuisDernierRepas { private set; get; }

    [SerializeField] private float _vitesseDeplacement;
    [SerializeField] private float _vitesseRotation;
    [SerializeField] private float _facteurCourse;

    public float VitesseDeplacement
    {
        get { return _vitesseDeplacement; }
    }

    public float VitesseRotation
    {
        get { return _vitesseRotation; }
    }

    public float FacteurCourse
    {
        get { return _facteurCourse; }
    }

    public float ConsommationEnergie => _etat.EnergieDepensee;
    public float MultiplicateurScale => _etat.MultiplicateurScale;

    public bool EstActif => _etat.EstActif;
    public bool DansDialogue => _etat.DansDialogue;

    private CharacterController _controller;
    private EnergieJoueur _energieJoueur;
    private Inventaire _inventaire;

    private Vector3 _positionDepart;
    private Quaternion _rotationDepart;

    private Animator _animator;

    private Soleil _soleil;


    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(),
            UnityEngine.GameObject.Find("NavMeshObstacle").GetComponent<Collider>());
        EtatNormal = new EtatNormal(this);

        _soleil = UnityEngine.GameObject.Find("Directional Light").GetComponent<Soleil>();

        _etat = EtatNormal;

        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _energieJoueur = GetComponent<EnergieJoueur>();
        _inventaire = GetComponent<Inventaire>();

        _positionDepart = transform.position;
        _rotationDepart = transform.rotation;

        TempsDepuisDernierRepas = 0.0f;

        GetComponent<NavMeshAgent>().enabled = false;
    }

    void Update()
    {
        TempsDepuisDernierRepas += _soleil.DeltaMinutesEcoulees;
        _etat.Handle();
    }

    public void ChangerEtat(EtatJoueur nouvelEtat)
    {
        _etat.Exit();
        _etat = nouvelEtat;
        _etat.Enter();
    }

    public void ReplacerPositionDepart()
    {
        _controller.enabled = false;
        transform.position = _positionDepart;
        transform.rotation = _rotationDepart;
        _controller.enabled = true;
    }

    public void Manger()
    {
        if (_inventaire.Oeuf > 0)
        {
            // Peut-être qu'on devrait offrir le choix de ce qu'on mange.
            // Ce qui implique de faire une nouvelle interface pour ça.
            _inventaire.Oeuf--;
            _energieJoueur.Energie += ConstantesJeu.GAIN_ENERGIE_MANGER_OEUF;
            GestionnaireMessages.Instance.AfficherMessage("Vous mangez un oeuf", "C'était délicieux");
            TempsDepuisDernierRepas = 0.0f;
        }
        else if (_inventaire.Choux > 0)
        {
            _inventaire.Choux--;
            _energieJoueur.Energie += ConstantesJeu.GAIN_ENERGIE_MANGER_CHOU;
            GestionnaireMessages.Instance.AfficherMessage("Vous mangez un chou",
                "C'était... Correct. En salade ça aurait été meilleur");
            TempsDepuisDernierRepas = 0.0f;
        }
    }

    internal bool DansMenu()
    {
        return _etat is EtatDansMenu;
    }

    public bool PeutManger
    {
        get => _inventaire.Oeuf > 0 || _inventaire.Choux > 0;
    }
}