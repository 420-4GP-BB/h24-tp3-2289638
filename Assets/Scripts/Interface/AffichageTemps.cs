using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class AffichageTemps : MonoBehaviour
{
    // Le jour va de 0:00 a 24:00
    private const int nombreSecondesJournees = 24 * 60 * 60;

    [SerializeField] private GameManager _gameManager; // Pour obtenir le jour
    [SerializeField] private Soleil soleil; // Pour obtenir la proportion de la journée qui reste
    [SerializeField] private TMP_Text texte; // Pour afficher l'heure
    [SerializeField] private TMP_Text texteJour; // Pour afficher l'heure

    void Start()
    {
    }

    void OnGUI()
    {
        int nombreSecondesEcoulees =
            nombreSecondesJournees - (int)(nombreSecondesJournees * soleil.ProportionRestante);
        int nombreHeures = nombreSecondesEcoulees / 3600;
        int nombreMinutes = (nombreSecondesEcoulees % 3600) / 60;
        texte.text = $"{nombreHeures:00}:{nombreMinutes:00}";
        texteJour.text = $"Jour {_gameManager.NumeroJour}";
    }
}