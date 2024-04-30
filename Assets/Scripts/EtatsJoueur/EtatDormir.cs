using UnityEngine;

public class EtatDormir : EtatJoueur
{
    private const float TEMPS_DODO = 600.0f; // 10 heures de sommeil
    private Soleil _soleil;
    private UnityEngine.GameObject _parent;

    public EtatDormir(ComportementJoueur sujet, UnityEngine.GameObject parent) : base(sujet)
    {
        _parent = parent;
        _soleil = UnityEngine.GameObject.FindObjectOfType<Soleil>();
    }

    public override bool EstActif => false;
    public override bool DansDialogue => false;
    public override float MultiplicateurScale => 90.0f;

    /// <summary>
    /// Énergie dépensée, possiblement négative si on gagne de l'énergie
    /// </summary>
    public override float EnergieDepensee =>
        Sujet.TempsDepuisDernierRepas > 720.0f
            ? ConstantesJeu.COUT_IMMOBILE
            : -ConstantesJeu.GAIN_ENERGIE_SOMMEIL;

    private float _tempsDormi = 0f; // Temps en minutes de jeu

    public override void Enter()
    {
        _parent.SetActive(false);
    }

    public override void Handle()
    {
        _tempsDormi += _soleil.DeltaMinutesEcoulees;

        if (_tempsDormi >= TEMPS_DODO)
        {
            Sujet.ChangerEtat(Sujet.EtatNormal);
        }
    }

    public override void Exit()
    {
    }
}