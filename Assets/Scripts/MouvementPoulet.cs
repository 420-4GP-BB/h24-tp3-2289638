using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouvementPoulet : MonoBehaviour
{
    // private UnityEngine.GameObject _zoneRelachement;
    // private float _angleDerriere;  // L'angle pour que le poulet soit derri�re le joueur
    // private UnityEngine.GameObject joueur;
    // private bool _suivreJoueur = true;

    private NavMeshAgent _agent;
    private Animator _animator;

    private GameObject[] _pointsDeDeplacement;

    [SerializeField] private Vector3 SortieMagasin = new Vector3 (-37, 0, -15);// Variable afin de faire apparaitre la poule sur la sortie du magasin s'elle est achet�e
    [SerializeField] private Vector3 Ferme = new Vector3 (58.5f, 0, -47f);     // Variable afin que la poule qui suit le joueur sache quand arr�ter de le suivre
    [SerializeField] private float DistanceDuJoueur = 2.5f;                    // Variable afin de d�cider la distance minimum que la poule garde avec le joueur afin de pas le bloquer
    [SerializeField] private float DistanceEntreeFerme = 5.0f;                 // Variable afin de d�cider � quelle distance la poule arr�te de suivre le joueur et prend son comportement habituel
    public bool PouleAchetee;   // bool�ean initialis�e � l'ext�rieure de la classe (public) qui sert � savoir d'ou origine la poule.
    public Vector3 PositionOeuf;// Variable initialis�e � l'externe, est utilis�e si la bool�an en haut est true ; elle sert � savoir la location de l'oeuf duquel origine la poule
    private bool EstDansFerme;  // boolean pour que la poule sache quelle comportement prendre dans le Update()
    private GameObject Joueur;  // On a besoin du joueur afin de le suivre
    private const float progression21h = 16.0f / 24;    // J'ai la flemme de penser � pourquoi les valeurs doivent �tre invers�es pour que ca fonctionne, mais 
                                                        // j'ai trouv� la solution, ca marche, je suis content.
    private const float progression8h = 3.0f / 24;
    private Soleil Etoile => GameObject.FindFirstObjectByType<Soleil>();// Le soleil est n�cessaire afin de savoir il est quelle heure (pour savoir quand s'aventurer)
    public bool TempsAventureux => Etoile.ProportionRestante >= progression21h || Etoile.ProportionRestante <= progression8h;
    void Start()
    {
        // _zoneRelachement = UnityEngine.GameObject.Find("ZoneRelachePoulet");
        // joueur = UnityEngine.GameObject.Find("Joueur");
        // _suivreJoueur = true;
        // _angleDerriere = Random.Range(-60.0f, 60.0f);

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        GameObject pointSpecial = GameObject.FindGameObjectWithTag("PointSpecial");// On s'assure que le point sp�cial est le dernier pour l'aventure. (Il pourrait �tre premier aussi)
        List<GameObject> points = new List<GameObject>(GameObject.FindGameObjectsWithTag("PointsPoulet"));
        points.Add(pointSpecial);
        _pointsDeDeplacement = points.ToArray();
        Joueur = GameObject.FindGameObjectWithTag("Player");
        if (PouleAchetee)                   // Si la poule est achet�e, elle apparait devant la magasin et ''EstDansFerme'' est faux. Elle suivra donc le joueur.
        {
            InitialiserAchat();
        } else                              // Si la poule n'est pas achet�e, elle est donc �clot, et est d�j� dans la ferme. Elle commencera donc � pondre ses oeufs et � se promener.
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
            point = _pointsDeDeplacement[Random.Range(0, _pointsDeDeplacement.Length)]; // Si on est entre 21h et 8h, la poule a acc�s � l'enti�ret� de sa liste de points.
        } else                                                                          // incluant ainsi le dernier, qui est le point sp�cial.
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
        _animator.SetBool("Walk", true);    // Pas de repos pour la pauvre poule! Une fois arriver � la ferme, elle se prom�ne jour et nuit. L'animation peut donc �tre activ� en permanence.
    }
    void Update()
    {
        if (!EstDansFerme)
        {
            SuivreJoueur();         // Si la poule n'est pas dans la ferme, elle suit le joueur et:
            VerifierDistanceFerme();// - elle v�rifie sa distance � la ferme (pour ArriverFerme() et changer son comportement)
            VerifierAnimationWalk();// - elle v�rifie si elle est entrain de marcher (animation). Il se peut qu'elle se retrouve immobile si le joueur est immobile.
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
        Vector3 position = Joueur.transform.position - direction * DistanceDuJoueur;// La poule calcule ou elle doit �tre afin de maintenir la distance minimum entre elle et son beau
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
    void VerifierAnimationWalk()                // Code tr�s simple car les cas pour la poule ne sont pas trop complexe
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