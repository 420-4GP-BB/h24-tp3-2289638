using UnityEngine;

public class CodesTriche : MonoBehaviour
{
    private GameObject _magasin;

    private GameObject _joueur;
    private CharacterController _controller;

    [SerializeField] private GameObject[] _pointsTeleportation;

    private Inventaire _inventaire;


    // Start is called before the first frame update
    void Start()
    {
        _magasin = GameObject.Find("Magasin");
        _joueur = GameObject.Find("Joueur");
        _inventaire = _joueur.GetComponent<Inventaire>();
        _controller = _joueur.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _controller.enabled = false;
            _joueur.transform.position = _pointsTeleportation[0].transform.position;
            _joueur.transform.LookAt(_magasin.transform);
            _controller.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _controller.enabled = false;
            _joueur.transform.position = _pointsTeleportation[1].transform.position;
            _joueur.transform.rotation = _pointsTeleportation[1].transform.rotation *= Quaternion.Euler(0, 180, 0);
            _controller.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _inventaire.Oeuf += 10;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            _inventaire.Or += 100;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _inventaire.Choux += 10;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _inventaire.Graines += 10;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _inventaire.ToutPerdre();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            var energie = FindObjectOfType<EnergieJoueur>();
            energie.BasculerImmortel();
        }

        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    _inventaire.RemplirPourTester();
        //}
    }
}