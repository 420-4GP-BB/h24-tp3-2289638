using UnityEngine;

public class EtatRamasserObjet : EtatJoueur
{
    private IRamassable _objetACueillir;
    private float _tempsDeCueillette = 0.0f;
    private bool _estRammasse;

    public override bool EstActif => true;
    public override bool DansDialogue => false;
    public override float EnergieDepensee => ConstantesJeu.COUT_PLANTER;

    public EtatRamasserObjet(ComportementJoueur sujet, IRamassable gameObject) : base(sujet)
    {
        _objetACueillir = gameObject;
        _estRammasse = false;
        Time.timeScale = 2f;
    }

    public override void Enter()
    {
        Animateur.SetBool("Ramasse", true);
    }

    public override void Handle()
    {
        _tempsDeCueillette += Time.deltaTime;

        // Le personnage se relève après avoir ramassé l'objet
        if (_tempsDeCueillette >= 3.5f && ! _estRammasse)
        {
            _objetACueillir.Ramasser(Inventaire);
            _estRammasse = true;
        }
        else if (_tempsDeCueillette >= 5.0f)
        {
            Sujet.ChangerEtat(Sujet.EtatNormal);
        }
    }

    public override void Exit()
    {
        Animateur.SetBool("Ramasse", false);
        Time.timeScale = 1f;
    }
}