using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouvementPoulet : MonoBehaviour
{
    // private UnityEngine.GameObject _zoneRelachement;
    // private float _angleDerriere;  // L'angle pour que le poulet soit derrière le joueur
    // private UnityEngine.GameObject joueur;
    // private bool _suivreJoueur = true;

    private NavMeshAgent _agent;
    private Animator _animator;

    private GameObject[] _pointsDeDeplacement;

    [SerializeField] private Vector3 SortieMagasin = new Vector3 (-37, 0, -15);// Variable afin de faire apparaitre la poule sur la sortie du magasin s'elle est achetée
    [SerializeField] private Vector3 Ferme = new Vector3 (58.5f, 0, -47f);     // Variable afin que la poule qui suit le joueur sache quand arrêter de le suivre
    [SerializeField] private float DistanceDuJoueur = 2.5f;                    // Variable afin de décider la distance minimum que la poule garde avec le joueur afin de pas le bloquer
    [SerializeField] private float DistanceEntreeFerme = 5.0f;                 // Variable afin de décider à quelle distance la poule arrête de suivre le joueur et prend son comportement habituel
    public bool PouleAchetee;   // booléean initialisée à l'extérieure de la classe (public) qui sert à savoir d'ou origine la poule.
    public Vector3 PositionOeuf;// Variable initialisée à l'externe, est utilisée si la booléan en haut est true ; elle sert à savoir la location de l'oeuf duquel origine la poule
    private bool EstDansFerme;  // boolean pour que la poule sache quelle comportement prendre dans le Update()
    private GameObject Joueur;  // On a besoin du joueur afin de le suivre
    private const float progression21h = 16.0f / 24;    // J'ai la flemme de penser à pourquoi les valeurs doivent être inversées pour que ca fonctionne, mais 
                                                        // j'ai trouvé la solution, ca marche, je suis content.
    private const float progression8h = 3.0f / 24;
    private Soleil Etoile => GameObject.FindFirstObjectByType<Soleil>();// Le soleil est nécessaire afin de savoir il est quelle heure (pour savoir quand s'aventurer)
    public bool TempsAventureux => Etoile.ProportionRestante >= progression21h || Etoile.ProportionRestante <= progression8h;
    void Start()
    {
        // _zoneRelachement = UnityEngine.GameObject.Find("ZoneRelachePoulet");
        // joueur = UnityEngine.GameObject.Find("Joueur");
        // _suivreJoueur = true;
        // _angleDerriere = Random.Range(-60.0f, 60.0f);

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        GameObject pointSpecial = GameObject.FindGameObjectWithTag("PointSpecial");// On s'assure que le point spécial est le dernier pour l'aventure. (Il pourrait être premier aussi)
        List<GameObject> points = new List<GameObject>(GameObject.FindGameObjectsWithTag("PointsPoulet"));
        points.Add(pointSpecial);
        _pointsDeDeplacement = points.ToArray();
        Joueur = GameObject.FindGameObjectWithTag("Player");
        if (PouleAchetee)                   // Si la poule est achetée, elle apparait devant la magasin et ''EstDansFerme'' est faux. Elle suivra donc le joueur.
        {
            InitialiserAchat();
        } else                              // Si la poule n'est pas achetée, elle est donc éclot, et est déjà dans la ferme. Elle commencera donc à pondre ses oeufs et à se promener.
        {
            InitilaiserEclot(PositionOeuf);
        }
    }

    private void InitialiserAchat()
    {
        // Position initiale sur la ferme
        _agent.enabled = false;
        transform.position = SortieMagasin;
        EstDansFerme = false;
        _agent.enabled = true;
    }
    private void InitilaiserEclot(Vector3 positionOeuf)
    {
        _agent.enabled = false;
        transform.position = positionOeuf;
        EstDansFerme = true;
        _agent.enabled = true;
        ArriverFerme();
    }
    void ChoisirDestinationAleatoire()
    {
        GameObject point;
        if (TempsAventureux)
        {
            point = _pointsDeDeplacement[Random.Range(0, _pointsDeDeplacement.Length)]; // Si on est entre 21h et 8h, la poule a accès à l'entièreté de sa liste de points.
        } else                                                                          // incluant ainsi le dernier, qui est le point spécial.
        {
            point = _pointsDeDeplacement[Random.Range(0, _pointsDeDeplacement.Length-1)];
        }
        _agent.SetDestination(point.transform.position);
    }
    public void ArriverFerme()
    {
        EstDansFerme = true;
        gameObject.GetComponent<PondreOeufs>().enabled = true;
        ChoisirDestinationAleatoire();
        _animator.SetBool("Walk", true);    // Pas de repos pour la pauvre poule! Une fois arriver à la ferme, elle se promène jour et nuit. L'animation peut donc être activé en permanence.
    }
    void Update()
    {
        if (!EstDansFerme)
        {
            SuivreJoueur();         // Si la poule n'est pas dans la ferme, elle suit le joueur et:
            VerifierDistanceFerme();// - elle vérifie sa distance à la ferme (pour ArriverFerme() et changer son comportement)
            VerifierAnimationWalk();// - elle vérifie si elle est entrain de marcher (animation). Il se peut qu'elle se retrouve immobile si le joueur est immobile.
        }
        else
        {
            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                ChoisirDestinationAleatoire();
            }
        }
    }
    void SuivreJoueur()
    {
        Vector3 direction = (Joueur.transform.position - _agent.transform.position).normalized;
        Vector3 position = Joueur.transform.position - direction * DistanceDuJoueur;// La poule calcule ou elle doit être afin de maintenir la distance minimum entre elle et son beau
        _agent.destination = position;
    }
    void VerifierDistanceFerme()
    {
        float distance = Vector3.Distance(_agent.transform.position, Ferme);
        if (distance < DistanceEntreeFerme)
        {
            ArriverFerme();
        }
    }
    void VerifierAnimationWalk()                // Code très simple car les cas pour la poule ne sont pas trop complexe
    {
        if (_agent.velocity.magnitude>0f)
        {
            _animator.SetBool("Walk", true);
        } else
        {
            _animator.SetBool("Walk", false);
        }
    }
}