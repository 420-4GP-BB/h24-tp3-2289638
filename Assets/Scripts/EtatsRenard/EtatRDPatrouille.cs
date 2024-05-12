using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EtatRDPatrouille : EtatRenard
{
    private GameObject[] PointsPatrouille;
    private float RadiusDetection = 5f; // Zone autour du renard � laquelle il peut sentir les poule
    public bool RenardSeReveille;       // Bool�an qui sert � assurer que lorsque le renard passe de mode pourchasse � patrouille, ce dernier ne 'respawn' pas.
    public EtatRDPatrouille(ComportementRenard renard, GameObject[] pointsPatrouille) : base(renard)
    {
        PointsPatrouille = pointsPatrouille;
        RenardSeReveille = true;
    }

    public override void Enter()
    {
        if (RenardSeReveille)                   // Seulement s'il se r�veille (qu'il est entrain de sortir de l'�tat absent), le renard respawn � un point al�atoire.
        {
            Respawn();                          // S'il ne se r�veille pas (qu'il arrive a l'�tat patrouille � partir de l'�tat pourchasse), le renard continue sa patrouille de sa
        }                                       // location actuelle.
        _Animator.SetBool("isWalking", true);   // Pourquoi le renard re-apparait � une location al�atoire apr�s une journ�e?:
        SetNewDestination();                    // Si ce ne serait pas le cas, cela voudrait dire que le renard tuerait tout les poules si jamais il arrive � se retrouver dans la ferme.
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
        CheckTime();    // Si �a devient la nuit, le renard change d'�tat � absent.
        DetectChicks();
    }
    public void SetNewDestination()
    {
        GameObject point = PointsPatrouille[Random.Range(0, PointsPatrouille.Length)];
        Agent.SetDestination(point.transform.position);
    }
    public void Respawn() // Pour �viter des cas ou les poules s'�chappent � peine du renard, mais que ce dernier reapparait sur eux, guarantissant la mort de toute les poules
                          // lorsque le renard arrive � la ferme.
    {
        GameObject point = PointsPatrouille[Random.Range(0, PointsPatrouille.Length)];
        Agent.Warp(point.transform.position);
    }
    public void DetectChicks() // Chicks comme "Chickens", � ne pas mal comprendre ( arr�tez de choisir l'ours D: )
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
