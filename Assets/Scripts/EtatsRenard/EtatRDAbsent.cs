using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatRDAbsent : EtatRenard
{
    public EtatRDAbsent(ComportementRenard renard) : base(renard)
    {
    }

    public override void CheckTime()
    {
        if (RenardEstPresent)
        {
            Renard._etatPatrouille.RenardSeReveille = true;
            Renard.ChangerEtat(Renard._etatPatrouille);
        }
    }
    public override void Enter()
    {
        Vector3 RenardCurrentPos = Renard.transform.position;
        float NewY = RenardCurrentPos.y-=10;
        Vector3 RenardNewPos = new Vector3(RenardCurrentPos.x, NewY,RenardCurrentPos.z);
        Agent.enabled = false;
        Renard.transform.position = RenardNewPos;
    }

    public override void Exit()
    {
        Vector3 RenardCurrentPos = Renard.transform.position;
        float NewY = RenardCurrentPos.y += 10;
        Vector3 RenardNewPos = new Vector3(RenardCurrentPos.x, NewY, RenardCurrentPos.z);
        Agent.enabled = true;
        Agent.Warp(RenardNewPos);
    }

    public override void Handle()
    {
        CheckTime();
    }
}
