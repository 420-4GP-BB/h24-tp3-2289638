using UnityEngine;

public class EtatPlanter : EtatJoueur
{
    public override bool EstActif => true;
    public override bool DansDialogue => false;
    public override float EnergieDepensee => ConstantesJeu.COUT_PLANTER;

    private IPlantable _plantable;
    private float _tempsDePlantation = 0.0f;

    public EtatPlanter(ComportementJoueur sujet, IPlantable plantable) : base(sujet)
    {
        _plantable = plantable;
    }

    public override void Enter()
    {
        Animateur.SetBool("Planter", true);
    }

    public override void Handle()
    {
        _tempsDePlantation += Time.deltaTime;
        if (_tempsDePlantation >= 5.0f)
        {
            _plantable.Planter(Inventaire);
            Sujet.ChangerEtat(Sujet.EtatNormal);
        }
    }

    public override void Exit()
    {
        Animateur.SetBool("Planter", false);
    }
}