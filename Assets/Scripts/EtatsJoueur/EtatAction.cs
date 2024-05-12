using UnityEngine;
using UnityEngine.AI;

public class EtatAction : EtatJoueur
{
    public override bool EstActif => true;
    public override bool DansDialogue => false;
    public override float EnergieDepensee => ConstantesJeu.COUT_MARCHER;


    private GameObject _destination;
    private NavMeshAgent _navMeshAgent;

    private Vector3 pointDestination;

    // Tourner de maniere non brusque vers le sujet
    Quaternion rotationVisee;
    private float DureeRotation = 0.25f;
    private float DureeEcoulee;
    private bool EstEnRotation = false;
    private Logger _logger;
    private float timeStuck = 0.0f;

    public EtatAction(ComportementJoueur sujet, GameObject destination) : base(sujet)
    {
        _destination = destination;
        _navMeshAgent = Sujet.GetComponent<NavMeshAgent>();
        _logger = new Logger();
    }

    public override void Enter()
    {
        // Garder la valeure en tete au lieu de immediatement tourner
        Vector3 direction = _destination.transform.position - Sujet.transform.position;
        rotationVisee = Quaternion.LookRotation(direction);
        EstEnRotation = true;
        DureeEcoulee = 0.00f;
    }

    // On doit se rendre au point pour faire l'action
    public override void Handle()
    {
        if (EstEnRotation)
        {
            if (DureeRotation > DureeEcoulee && Sujet.transform.rotation!=rotationVisee)
            {
                float lerp = DureeEcoulee / DureeRotation;                                                  // Code standard afin de faire rotationner un objet sur une p�riode fixe.
                Sujet.transform.rotation = Quaternion.Slerp(Sujet.transform.rotation, rotationVisee, lerp);
                DureeEcoulee += Time.deltaTime;
            } else
            {
                CommencerTrajet();
            }
        } else
        {
            // Parfois, il arrive que l'agent se coince entre des buches, surtout lorsqu'il commence son path tres proche de sa destination. 
            // Ceci evite cela.
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                timeStuck += Time.deltaTime;
            } else
            {
                timeStuck = 0;
            }

    //        _logger.Log($"Path pending: {_navMeshAgent.pathPending}, Remaining Distance: {_navMeshAgent.remainingDistance}, Stopping Distance: {_navMeshAgent.stoppingDistance}," +
    //$"Agent has path: {_navMeshAgent.hasPath}, Time Stuck: {timeStuck}");

            if ((!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.hasPath) || timeStuck>0.35f)
                // Si toute les autres conditions ici � l'exception de 'hasPath' sont vrai pendant plus que 350ms, l'agent sera d�coinc�.
            {
                _navMeshAgent.enabled = false;
                pointDestination.y = Sujet.transform.position.y;
                Sujet.transform.position = pointDestination;

                var actionnable = _destination.GetComponent<IActionnable>();
                if (actionnable != null)
                {
                    Sujet.ChangerEtat(actionnable.EtatAUtiliser(Sujet));
                }
            }
        }
        //Vector3 direction = _destination.transform.position - Sujet.transform.position;
        //Sujet.transform.rotation = Quaternion.LookRotation(direction);
        //Vector3 pointProche = _destination.GetComponent<Collider>().ClosestPoint(Sujet.transform.position);
        //Vector3 pointDestination = pointProche - direction.normalized * 0.1f;

        //if (Vector3.Distance(Sujet.transform.position, pointDestination) > 0.1f)
        //{
        //    float distanceAvant = Vector3.Distance(Sujet.transform.position, pointDestination);
        //    ControleurMouvement.SimpleMove(Sujet.transform.forward * (Sujet.VitesseDeplacement));

        //    Il faudrait peut - ?tre essayer avec un NavMesh ici
        //    Sujet.transform.Translate(Sujet.transform.forward * (Sujet.VitesseDeplacement * Time.deltaTime), Space.World);
        //    Sujet.transform.rotation = Quaternion.Euler(0, Sujet.transform.rotation.eulerAngles.y, 0);
        //    float distanceApres = Vector3.Distance(Sujet.transform.position, pointDestination);

        //}
        //else
        //{
        //    ControleurMouvement.enabled = false;
        //    Sujet.transform.position = pointDestination;

        //    Chou chou = _destination.GetComponent<Chou>();
        //    if (chou != null)
        //    {
        //        Sujet.ChangerEtat(new PlanterChou(Sujet, chou));
        //    }

        //    Oeuf oeuf = _destination.GetComponent<Oeuf>();
        //    if (oeuf != null)
        //    {
        //        Sujet.ChangerEtat(new RamasserOeuf(Sujet, oeuf));
        //    }
        //    ControleurMouvement.enabled = true;
        //}
    }

    public override void Exit()
    {
        ControleurMouvement.enabled = true;
        _navMeshAgent.enabled = false;
        Animateur.SetBool("Walking", false);
    }

    private void CommencerTrajet()
    {
        Sujet.transform.rotation = rotationVisee;   // On s'assure que le sujet fait face � l'endroit
        EstEnRotation = false;                      // Pour que la m�thode handle s'occupe du chemin au lieu de la rotation
        Animateur.SetBool("Walking", true);
        ControleurMouvement.enabled = false;        
        _navMeshAgent.enabled = true;               // Code du prof pour faire le trajet.
        Vector3 direction = _destination.transform.position - Sujet.transform.position;
        Vector3 pointProche = _destination.GetComponent<Collider>().ClosestPoint(Sujet.transform.position);
        pointDestination = pointProche - direction.normalized * 0.3f;
        _navMeshAgent.SetDestination(pointDestination);
    }
}