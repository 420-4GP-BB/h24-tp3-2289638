using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportementRenard : MonoBehaviour
{
    // Code inspirée de celui du prof, afin de rester consistent avec les ÉtatsJoueurs
    private EtatRenard _etat;                   // l'état qui alterne entre Absent, Patrouille et Poursuite.
    [SerializeField] public Soleil soleil;      // Afin d'être recuperée par les états à travers cette classe (public)
    [SerializeField] public NavMeshAgent agent; // idem
    public EtatRDPatrouille _etatPatrouille;    // l'état-patrouille reste sauvegardé afin de ne pas devoir y repasser les points de patrouille à chaque fois
    void Start()
    {
        agent.Warp(agent.transform.position);   // Afin de s'assurer que l'agent n'est pas sur un endroit sans NavMesh (un obstacle ou quelconque)
        List<GameObject> pointsList = new List<GameObject>(GameObject.FindGameObjectsWithTag("PointsRenard"));
        GameObject point = GameObject.FindGameObjectWithTag("PointSpecial");
        pointsList.Add(point);
        GameObject[] points = pointsList.ToArray(); // Le renard traite le point comme un point spécial. Pas besoin de en faire une distinction.
        _etatPatrouille = new EtatRDPatrouille(this, points);
        _etat = _etatPatrouille;
        _etat.Enter();
    }
    void Update()
    {
        _etat.Handle();
    }
    public void ChangerEtat(EtatRenard etat)
    {
        _etat.Exit();
        _etat = etat;
        _etat.Enter();
    }
}
