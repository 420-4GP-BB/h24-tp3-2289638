using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatRDAbsent : EtatRenard
{
    public EtatRDAbsent(ComportementRenard renard) : base(renard)
    {
    }

    public override void CheckTime()    // Seulement dans cet état, cette méthode est overriden
    {                                   // Évidemment, vu qu'on est déjà dans l'état absent, la méthode vérifie maintenant si on devrait sortir de cet état
        if (RenardEstPresent)
        {
            Renard._etatPatrouille.RenardSeReveille = true; // On signale à l'état patrouille qu'on y rentre à partir de l'état absent.
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
    {                                                           // De toute évidence, à son retour, le renard revient à hauteur du sol et son agent est réactivé.
        Vector3 RenardCurrentPos = Renard.transform.position;
        float NewY = RenardCurrentPos.y += 10;
        Vector3 RenardNewPos = new Vector3(RenardCurrentPos.x, NewY, RenardCurrentPos.z);
        Agent.enabled = true;
        Agent.Warp(RenardNewPos);                               // Warp pour s'assurer qu'il revienne à une zone NavMesh
    }

    public override void Handle()
    {
        CheckTime();
    }
}
