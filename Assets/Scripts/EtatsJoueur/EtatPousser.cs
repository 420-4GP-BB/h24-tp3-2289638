using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatPousser : EtatJoueur
{
    private IPoussable _poussable;
    private float DureeTombee = 2.0f;
    private float TempsPoussee;
    float vitesseTombee;
    GameObject Joueur;
    public EtatPousser(ComportementJoueur sujet, IPoussable poussable) : base(sujet)
    {
        _poussable = poussable;
        Joueur = Sujet.gameObject;
    }

    public override bool EstActif => true;

    public override bool DansDialogue => false;

    public override float EnergieDepensee => 0.005f;

    public override void Enter()
    {
        vitesseTombee = 90.0f / DureeTombee;
        TempsPoussee = 0.0f;
        Animateur.SetBool("Pousser", true);
    }

    public override void Exit()
    {
        Animateur.SetBool("Pousser", false);
    }

    public override void Handle()
    {
        TempsPoussee += Time.deltaTime;
        _poussable.Pousser(vitesseTombee, Joueur.transform.right);
        if (TempsPoussee > DureeTombee)
        {
            _poussable.Tomber();
            Sujet.ChangerEtat(Sujet.EtatNormal);
        }
    }
}
