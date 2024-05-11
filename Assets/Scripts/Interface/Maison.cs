using UnityEngine;
using UnityEngine.UI;

public class Maison : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject _panelMaison;
    [SerializeField] private GameManager _gameManager;
    
    [SerializeField] private Button _boutonManger;
    
    private ComportementJoueur _joueur;

    void Start()
    {
        _panelMaison.SetActive(false);
        _joueur = UnityEngine.GameObject.Find("Joueur").GetComponent<ComportementJoueur>();
    }

    private void OnGUI()
    {
        _boutonManger.interactable = _joueur.PeutManger;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _joueur.gameObject)
        {
            _panelMaison.SetActive(true);
            _joueur.ChangerEtat(new EtatDansMenu(_joueur));
        }
    }
    
    public void Dodo()
    {
        _joueur.ChangerEtat(new EtatDormir(_joueur, _panelMaison));
    }

    void FermerMenu()
    {
        _panelMaison.SetActive(false);
        _joueur.ChangerEtat(_joueur.EtatNormal);
        _joueur.ReplacerPositionDepart();
    }

    public void Manger()
    {
//        FermerMenu();
        _joueur.Manger(false);
    }

    public void SortirMaison()
    {
        FermerMenu();
    }
}