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

    void Start()
    {
        // _zoneRelachement = UnityEngine.GameObject.Find("ZoneRelachePoulet");
        // joueur = UnityEngine.GameObject.Find("Joueur");
        // _suivreJoueur = true;
        // _angleDerriere = Random.Range(-60.0f, 60.0f);

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _pointsDeDeplacement = GameObject.FindGameObjectsWithTag("PointsPoulet");
        _animator.SetBool("Walk", true);
        Initialiser();
    }

    void Initialiser()
    {
        // Position initiale sur la ferme
        _agent.enabled = false;
        var point = _pointsDeDeplacement[Random.Range(0, _pointsDeDeplacement.Length)];
        transform.position = point.transform.position;
        _agent.enabled = true;

        gameObject.GetComponent<PondreOeufs>().enabled = true;

        ChoisirDestinationAleatoire();
    }

    void ChoisirDestinationAleatoire()
    {
        GameObject point = _pointsDeDeplacement[Random.Range(0, _pointsDeDeplacement.Length)];
        _agent.SetDestination(point.transform.position);
    }

    void Update()
    {
        // if (_suivreJoueur)
        // {
        //     Vector3 directionAvecJoueur = Quaternion.AngleAxis(_angleDerriere, Vector3.up) * joueur.transform.forward;
        //     transform.position = joueur.transform.position - directionAvecJoueur;
        //     transform.rotation = joueur.transform.rotation;
        // }

        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            ChoisirDestinationAleatoire();
        }
    }
}