using UnityEngine;

public class EtatNormal : EtatJoueur
{
    private bool _estActif;
    private bool _enCourse;

    public override bool EstActif => _estActif;
    public override bool DansDialogue => false;


    public override float EnergieDepensee
    {
        get
        {
            if (_enCourse)
            {
                return ConstantesJeu.COUT_COURIR;
            }
            else if (_estActif)
            {
                return ConstantesJeu.COUT_MARCHER;
            }
            else
            {
                return ConstantesJeu.COUT_IMMOBILE;
            }
        }
    }

    public EtatNormal(ComportementJoueur sujet) : base(sujet)
    {
    }

    public override void Enter()
    {
    }

    public override void Handle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var actionnable = Utilitaires.ObjetSousSouris();
            if (actionnable != null &&
                actionnable.GetComponent<IActionnable>().Permis(Sujet))
            {
                Sujet.ChangerEtat(new EtatAction(Sujet, actionnable));
            }
        }
        else
        {
            float translation = Input.GetAxis("Vertical") * Sujet.VitesseDeplacement;
            float rotation = Input.GetAxis("Horizontal") * Sujet.VitesseRotation * Time.deltaTime;

            bool faible = EnergieJoueur.EnergieFaible;

            if (!faible && Input.GetKey(KeyCode.LeftShift))
            {
                translation *= Sujet.FacteurCourse;
                _enCourse = true;
            }
            else
            {
                _enCourse = false;
            }

            if (faible)
            {
                translation *= 0.5f;
            }

            ControleurMouvement.SimpleMove(translation * Sujet.transform.forward);
            Sujet.transform.Rotate(rotation * Vector3.up);

            _estActif = !Mathf.Approximately(translation, 0.0f);
            Animateur.SetBool("Walking", EstActif);

            Sujet.transform.rotation = Quaternion.Euler(0, Sujet.transform.rotation.eulerAngles.y, 0);
        }
    }

    public override void Exit()
    {
    }
}