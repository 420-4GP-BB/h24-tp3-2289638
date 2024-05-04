using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatPousser : EtatJoueur
{
    private IPoussable _poussable;
    private float DureeTombee = 2.0f;
    float vitesseTombee;
    public EtatPousser(ComportementJoueur sujet, IPoussable poussable) : base(sujet)
    {
        _poussable = poussable;
    }

    public override bool EstActif => true;

    public override bool DansDialogue => false;

    public override float EnergieDepensee => 0.005f;

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Handle()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
