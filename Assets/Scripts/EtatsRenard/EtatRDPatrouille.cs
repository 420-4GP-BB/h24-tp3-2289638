using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EtatRDPatrouille : EtatRenard
{
    Logger logger = new Logger();
    private const int nombreSecondesJournees = 24 * 60 * 60;
    private GameObject[] PointsPatrouille;
    private float RadiusDetection = 5f;
    public bool RenardSeReveille;
    public EtatRDPatrouille(ComportementRenard renard, GameObject[] pointsPatrouille) : base(renard)
    {
        PointsPatrouille = pointsPatrouille;
        RenardSeReveille = true;
    }

    public override void Enter()
    {
        if (RenardSeReveille)
        {
            Respawn();
        }
        _Animator.SetBool("isWalking", true);
        SetNewDestination();
    }

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
        CheckTime();
        DetectChicks();
    }
    public void LogState()
    {
        int nombreSecondesEcoulees =
    nombreSecondesJournees - (int)(nombreSecondesJournees * Etoile.ProportionRestante);
        int nombreHeures = nombreSecondesEcoulees / 3600;
        int nombreMinutes = (nombreSecondesEcoulees % 3600) / 60;
        string text = $"{nombreHeures:00}:{nombreMinutes:00}";
        logger.Log("Renard! : est actif? " + RenardEstPresent + "Time: " + text);
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
    {
        Collider[] hitColliders = Physics.OverlapSphere(Agent.transform.position, RadiusDetection);
        int i = 0;
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
