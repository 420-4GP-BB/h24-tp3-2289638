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

    [SerializeField] private Vector3 SortieMagasin = new Vector3 (-37, 0, -15);
    [SerializeField] private Vector3 Ferme = new Vector3 (58.5f, 0, -47f);
    [SerializeField] private float DistanceDuJoueur = 2.5f;
    [SerializeField] private float DistanceEntreeFerme = 5.0f;
    private bool EstDansFerme;
    private GameObject Joueur;
    private const float progression21h = 16.0f / 24;    // J'ai la flemme de penser à pourquoi les valeurs doivent être inversées pour que ca fonctionne, mais 
                                                        // j'ai trouvé la solution, ca marche, je suis content.
    private const float progression8h = 3.0f / 24;
    private Soleil Etoile => GameObject.FindFirstObjectByType<Soleil>();
    public bool TempsAventureux => Etoile.ProportionRestante >= progression21h || Etoile.ProportionRestante <= progression8h;
    void Start()
    {
        // _zoneRelachement = UnityEngine.GameObject.Find("ZoneRelachePoulet");
        // joueur = UnityEngine.GameObject.Find("Joueur");
        // _suivreJoueur = true;
        // _angleDerriere = Random.Range(-60.0f, 60.0f);

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        GameObject pointSpecial = GameObject.FindGameObjectWithTag("PointSpecial");
        List<GameObject> points = new List<GameObject>(GameObject.FindGameObjectsWithTag("PointsPoulet"));
        points.Add(pointSpecial);
        _pointsDeDeplacement = points.ToArray();
        Joueur = GameObject.FindGameObjectWithTag("Player");
        Initialiser();
    }

    void Initialiser()
    {
        // Position initiale sur la ferme
        _agent.enabled = false;
        transform.position = SortieMagasin;
        EstDansFerme = false;
        _agent.enabled = true;
    }

    void ChoisirDestinationAleatoire()
    {
        GameObject point;
        if (TempsAventureux)
        {
            point = _pointsDeDeplacement[Random.Range(0, _pointsDeDeplacement.Length)];
        } else
        {
            point = _pointsDeDeplacement[Random.Range(0, _pointsDeDeplacement.Length-1)];
        }
        _agent.SetDestination(point.transform.position);
    }
    private void ArriverFerme()
    {
        EstDansFerme = true;
        gameObject.GetComponent<PondreOeufs>().enabled = true;
        ChoisirDestinationAleatoire();
        _animator.SetBool("Walk", true);
    }
    void Update()
    {
        if (!EstDansFerme)
        {
            SuivreJoueur();
            VerifierDistanceFerme();
            VerifierAnimationWalk();
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
        Vector3 position = Joueur.transform.position - direction * DistanceDuJoueur;
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
    void VerifierAnimationWalk()
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