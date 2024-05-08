using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EtatRDPatrouille : EtatRenard
{
    Logger logger = new Logger();
    private const int nombreSecondesJournees = 24 * 60 * 60;
    private GameObject[] PointsPatrouille;
    public EtatRDPatrouille(ComportementRenard renard, GameObject[] pointsPatrouille) : base(renard)
    {
        PointsPatrouille = pointsPatrouille;
    }

    public override void Enter()
    {
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
}
