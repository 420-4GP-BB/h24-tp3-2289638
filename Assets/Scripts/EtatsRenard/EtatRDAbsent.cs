using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatRDAbsent : EtatRenard
{
    public EtatRDAbsent(ComportementRenard renard) : base(renard)
    {
    }

    public override void CheckTime()    // Seulement dans cet �tat, cette m�thode est overriden
    {                                   // �videmment, vu qu'on est d�j� dans l'�tat absent, la m�thode v�rifie maintenant si on devrait sortir de cet �tat
        if (RenardEstPresent)
        {
            Renard._etatPatrouille.RenardSeReveille = true; // On signale � l'�tat patrouille qu'on y rentre � partir de l'�tat absent.
            Renard.ChangerEtat(Renard._etatPatrouille);
        }
    }
    public override void Enter()
    {
        Vector3 RenardCurrentPos = Renard.transform.position;   // Afin de ne pas avoir besoin d'un gameObject autre que le renard qui viendrait activer et desactiver ce dernier,
        float NewY = RenardCurrentPos.y-=10;                    // le renard fait simplement se cacher sous le sol lors de son absence. Pendant ce moment, il ne bouge pas et fait rien.
        Vector3 RenardNewPos = new Vector3(RenardCurrentPos.x, NewY,RenardCurrentPos.z);
        Agent.enabled = false;
        Renard.transform.position = RenardNewPos;
    }

    public override void Exit()
    {                                                           // De toute �vidence, � son retour, le renard revient � hauteur du sol et son agent est r�activ�.
        Vector3 RenardCurrentPos = Renard.transform.position;
        float NewY = RenardCurrentPos.y += 10;
        Vector3 RenardNewPos = new Vector3(RenardCurrentPos.x, NewY, RenardCurrentPos.z);
        Agent.enabled = true;
        Agent.Warp(RenardNewPos);                               // Warp pour s'assurer qu'il revienne � une zone NavMesh
    }

    public override void Handle()
    {
        CheckTime();
    }
}
