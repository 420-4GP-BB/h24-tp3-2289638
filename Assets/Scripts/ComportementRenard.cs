using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportementRenard : MonoBehaviour
{
    // Code inspir�e de celui du prof, afin de rester consistent avec les �tatsJoueurs
    private EtatRenard _etat;                   // l'�tat qui alterne entre Absent, Patrouille et Poursuite.
    [SerializeField] public Soleil soleil;      // Afin d'�tre recuper�e par les �tats � travers cette classe (public)
    [SerializeField] public NavMeshAgent agent; // idem
    public EtatRDPatrouille _etatPatrouille;    // l'�tat-patrouille reste sauvegard� afin de ne pas devoir y repasser les points de patrouille � chaque fois
    void Start()
    {
        agent.Warp(agent.transform.position);   // Afin de s'assurer que l'agent n'est pas sur un endroit sans NavMesh (un obstacle ou quelconque)
        List<GameObject> pointsList = new List<GameObject>(GameObject.FindGameObjectsWithTag("PointsRenard"));
        GameObject point = GameObject.FindGameObjectWithTag("PointSpecial");
        pointsList.Add(point);
        GameObject[] points = pointsList.ToArray(); // Le renard traite le point comme un point sp�cial. Pas besoin de en faire une distinction.
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
