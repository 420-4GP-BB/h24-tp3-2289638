using UnityEngine;

public class EtatDansMenu : EtatJoueur
{
    public override bool EstActif => true;
    public override bool DansDialogue => true;
    public override float EnergieDepensee => ConstantesJeu.COUT_IMMOBILE;
    public override float MultiplicateurScale => 0.0f;

    public EtatDansMenu(ComportementJoueur joueur) : base(joueur)
    {
    }

    public override void Enter()
    {
        Animateur.SetBool("Walking", false);
    }

    public override void Handle()
    {
    }

    public override void Exit()
    {
    }

}

