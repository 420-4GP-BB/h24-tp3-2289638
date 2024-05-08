using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportementRenard : MonoBehaviour
{
    private EtatRenard _etat;
    [SerializeField] public Soleil soleil;
    [SerializeField] public NavMeshAgent agent;
    public EtatRDPatrouille _etatPatrouille;
    void Start()
    {
        Debug.Log("Agent active?: " + agent.isActiveAndEnabled);
        agent.Warp(agent.transform.position);
        Debug.Log("Agent on NavMesh?: " + agent.isOnNavMesh);
        List<GameObject> pointsList = new List<GameObject>(GameObject.FindGameObjectsWithTag("PointsRenard"));
        GameObject point = GameObject.FindGameObjectWithTag("PointSpecial");
        pointsList.Add(point);
        GameObject[] points = pointsList.ToArray();
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
