using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public int Or { get; set; }
    public int Oeuf { get; set; }
    public int Choux { get; set; }
    public int Graines { get; set; }
    public int Buches { get; set; }

    void Awake()
    {
        Or = ParametresParties.Instance.OrDepart;
        Oeuf = ParametresParties.Instance.OeufsDepart;
        Graines = ParametresParties.Instance.SemencesDepart;
        Choux = 0;
        Buches = 0;
    }

    public void ToutPerdre()
    {
        Or = 0;
        Oeuf = 0;
        Choux = 0;
        Graines = 0;
        Buches = 0;
    }

    public void RemplirPourTester()
    {
        Or = 1000;
        Oeuf = 1000;
        Choux = 1000;
        Graines = 1000;
        Buches = 1000;
    }
}