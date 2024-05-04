using UnityEngine;

public class ParametresParties
{
    public static ParametresParties Instance { get; private set; } = new ParametresParties();

    public string NomJoueur { get; set; } = "Mathurin";
    public int OrDepart { get; set; } = 200;
    public int OeufsDepart { get; set; } = 5;
    public int SemencesDepart { get; set; } = 5;
    public GameObject ModelJoueur { get; set; }

    ///// <summary>
    ///// Nombre de jours nécessaires à un chou pour être prêts
    ///// 0 = le chou est déjà prêt dès qu'on le plante
    ///// </summary>
    public int TempsCroissance { get; set; } = 3;

    ///// <summary>
    ///// Nombre de jours pendant lesquels on peut cueillir un chou prêt
    ///// Plus cette valeur est petite, plus on doit se dépêcher avant qu'ils ne soient plus bons
    ///// </summary>
    public int DelaiCueillete { get; set; } = 5;

    private ParametresParties()
    {
    }
}