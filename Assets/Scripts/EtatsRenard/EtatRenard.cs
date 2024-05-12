using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EtatRenard
{
    public ComportementRenard Renard { get; private set; }  // Consistence avec ComportementJoueur et EtatJoueur
    public Soleil Etoile => Renard.soleil;
    private const float progression21h = 16.0f / 24;    // J'ai la flemme de penser � pourquoi les valeurs doivent �tre invers�es pour que ca fonctionne, mais 
                                                        // j'ai trouv� la solution, ca marche, je suis content.
    private const float progression8h = 3.0f / 24;
    public bool RenardEstPresent => Etoile.ProportionRestante >= progression21h || Etoile.ProportionRestante <= progression8h;
    public Animator _Animator => Renard.GetComponent<Animator>();   // Afin d'animer les mouvements du renard (En ce moment, il a juste l'animation de marcher)
    public NavMeshAgent Agent;  // Afin de faire promener le renard (mouvement du renard)
    public EtatRenard(ComportementRenard renard)
    {
        Renard = renard;
        Agent = Renard.agent;
    }

    public abstract void Handle();
    public abstract void Enter();
    public abstract void Exit();
    public virtual void CheckTime()//M�thode de base car les autres �tats, � l'exception de EtatRDAbsent, n'ont pas besoin de la modifier.
    {                              // Si c'est la journ�e; le renard est en �tat absent.
        if (!RenardEstPresent)
        {
            Renard.ChangerEtat(new EtatRDAbsent(Renard));
        }
    }
}
