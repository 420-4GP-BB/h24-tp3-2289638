using UnityEngine;

public abstract class EtatJoueur
{
    public ComportementJoueur Sujet
    {
        get;
        private set;
    }

    public Animator Animateur => Sujet.GetComponent<Animator>();

    public EtatJoueur(ComportementJoueur sujet)
    {
        Sujet = sujet;
    }

    public EnergieJoueur EnergieJoueur => Sujet.GetComponent<EnergieJoueur>();

    public CharacterController ControleurMouvement => Sujet.GetComponent<CharacterController>();

    public Inventaire Inventaire => Sujet.GetComponent<Inventaire>();

    public abstract bool EstActif { get; }
    public abstract bool DansDialogue { get; }
    public abstract float EnergieDepensee { get; }
    public virtual float MultiplicateurScale => 1.0f;

    public abstract void Enter();
    public abstract void Handle();
    public abstract void Exit();
}

