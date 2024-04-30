using UnityEngine;

public class PondreOeufs : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject _modeleOeufs;

    private Soleil _soleil;

    private bool _doitPondre = false;
    private float _proportionCible;

    // Start is called before the first frame update
    void Start()
    {
        _soleil = UnityEngine.GameObject.Find("Directional Light").GetComponent<Soleil>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_soleil.ProportionRestante <= _proportionCible && _doitPondre)
        {
            UnityEngine.GameObject nouvelOeuf = Instantiate(_modeleOeufs, transform.position, Quaternion.identity);
            Vector3 position = transform.position + Vector3.up * nouvelOeuf.transform.localScale.y / 2;
            _doitPondre = false;
        }
    }

    public void DeterminerPonte()
    {
        if (! this.isActiveAndEnabled)
        {
            return;
        }

        _doitPondre = Random.Range(0, 2) == 0;  // On a 50% de chance de pondre aujourd'hui
        
        if (_doitPondre)
        {
            _proportionCible = Random.Range(0.01f, 0.99f);
        }
    }
}
