using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatRDPoursuite : EtatRenard
{
    private GameObject Proie;   // Proie � suivre
    public EtatRDPoursuite(ComportementRenard renard, GameObject proie) : base(renard)  // Constructeur prend en param�tre la proie
    {
        Proie = proie;
    }

    public override void Enter()
    {
        ChasserProie();
    }

    public override void Exit()
    {
        // Rien � faire ici
    }

    public override void Handle()
    {
        ChasserProie();
        CheckTime();    // CheckTime() provenant de la classe de base fait en sorte que le renard peut disparaitre m�me s'il est en pleine pourchasse.
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
            Debug.Log("Renard s'est servit de ta poule pref�r�e");
            Object.Destroy(Proie);
            Proie.tag = "Untagged"; // Pour �viter un bogue ou l'�tatPatrouille detecte la m�me poule avant qu'elle ne disparaisse.
            Proie = null;
            Renard._etatPatrouille.RenardSeReveille = false;    // Pour signaler qu'on rentre � l'�tat patrouille � partir de l'�tat poursuite, et non absent
            Renard.ChangerEtat(Renard._etatPatrouille);
        }
    }
}
