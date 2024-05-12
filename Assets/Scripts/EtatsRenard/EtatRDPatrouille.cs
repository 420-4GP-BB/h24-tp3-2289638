using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EtatRDPatrouille : EtatRenard
{
    private GameObject[] PointsPatrouille;
    private float RadiusDetection = 5f; // Zone autour du renard à laquelle il peut sentir les poule
    public bool RenardSeReveille;       // Booléan qui sert à assurer que lorsque le renard passe de mode pourchasse à patrouille, ce dernier ne 'respawn' pas.
    public EtatRDPatrouille(ComportementRenard renard, GameObject[] pointsPatrouille) : base(renard)
    {
        PointsPatrouille = pointsPatrouille;
        RenardSeReveille = true;
    }

    public override void Enter()
    {
        if (RenardSeReveille)                   // Seulement s'il se réveille (qu'il est entrain de sortir de l'état absent), le renard respawn à un point aléatoire.
        {
            Respawn();                          // S'il ne se réveille pas (qu'il arrive a l'état patrouille à partir de l'état pourchasse), le renard continue sa patrouille de sa
        }                                       // location actuelle.
        _Animator.SetBool("isWalking", true);   // Pourquoi le renard re-apparait à une location aléatoire après une journée?:
        SetNewDestination();                    // Si ce ne serait pas le cas, cela voudrait dire que le renard tuerait tout les poules si jamais il arrive à se retrouver dans la ferme.
    }                                           // au final, c'est un 'game-design choice'.

    public override void Exit()
    {
        _Animator.SetBool("isWalking", false);
        Agent.SetDestination(Agent.transform.position);
    }

    public override void Handle()
    {
        if (!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance+0.3f)
        {
            SetNewDestination();
        }
        CheckTime();    // Si ça devient la nuit, le renard change d'état à absent.
        DetectChicks();
    }
    public void SetNewDestination()
    {
        GameObject point = PointsPatrouille[Random.Range(0, PointsPatrouille.Length)];
        Agent.SetDestination(point.transform.position);
    }
    public void Respawn() // Pour éviter des cas ou les poules s'échappent à peine du renard, mais que ce dernier reapparait sur eux, guarantissant la mort de toute les poules
                          // lorsque le renard arrive à la ferme.
    {
        GameObject point = PointsPatrouille[Random.Range(0, PointsPatrouille.Length)];
        Agent.Warp(point.transform.position);
    }
    public void DetectChicks() // Chicks comme "Chickens", à ne pas mal comprendre ( arrêtez de choisir l'ours D: )
    {                          // Recherche standard de gameObjects d'un certain tags autour du renard
        Collider[] hitColliders = Physics.OverlapSphere(Agent.transform.position, RadiusDetection);
        int i = 0;             // Lorsqu'il trouve un object avec tag 'Poule' , il le pourchasse.
        while (i < hitColliders.Length)
        {
            GameObject obj = hitColliders[i].gameObject;
            if (obj.CompareTag("Poule"))
            {
                Renard.ChangerEtat(new EtatRDPoursuite(Renard,obj));
            }
            i++;
        }
    }
}
