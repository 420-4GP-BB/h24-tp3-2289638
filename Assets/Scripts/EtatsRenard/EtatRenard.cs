using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EtatRenard
{
    public ComportementRenard Renard { get; private set; }
    public Soleil Etoile => Renard.soleil;
    private const float progression21h = 16.0f / 24;    // J'ai la flemme de penser à pourquoi les valeurs doivent être inversées pour que ca fonctionne, mais 
                                                        // j'ai trouvé la solution, ca marche, je suis content.
    private const float progression8h = 3.0f / 24;
    public bool RenardEstPresent => Etoile.ProportionRestante >= progression21h || Etoile.ProportionRestante <= progression8h;
    public Animator _Animator => Renard.GetComponent<Animator>();
    public NavMeshAgent Agent;
    public EtatRenard(ComportementRenard renard)
    {
        Renard = renard;
        Agent = Renard.agent;
    }

    public abstract void Handle();
    public abstract void Enter();
    public abstract void Exit();
    public virtual void CheckTime()
    {
        if (!RenardEstPresent)
        {
            Renard.ChangerEtat(new EtatRDAbsent(Renard));
        }
    }
}
