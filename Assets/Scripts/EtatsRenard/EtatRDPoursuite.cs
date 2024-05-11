using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatRDPoursuite : EtatRenard
{
    private GameObject Proie;
    Logger logger = new Logger();
    public EtatRDPoursuite(ComportementRenard renard, GameObject proie) : base(renard)
    {
        Proie = proie;
    }

    public override void Enter()
    {
        ChasserProie();
    }

    public override void Exit()
    {

    }

    public override void Handle()
    {
        ChasserProie();
        CheckTime();
    }
    private void ChasserProie()
    {
        if (Proie != null)
        {
            Agent.SetDestination(Proie.transform.position);
            VerifierDistance();
        }
    }
    private void VerifierDistance()
    {
        float distance = Vector3.Distance(Agent.transform.position, Proie.transform.position);
        if (distance < 1.5f)
        {
            Debug.Log("Renard s'est servit de ta poule preférée");
            Object.Destroy(Proie);
            Proie.tag = "Untagged";
            Proie = null;
            Renard._etatPatrouille.RenardSeReveille = false;
            Renard.ChangerEtat(Renard._etatPatrouille);
        }
    }
}
