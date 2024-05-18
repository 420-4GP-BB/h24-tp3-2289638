using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GestionnaireInterface : MonoBehaviour
{
    [SerializeField] private Button _boutonDemarrer;
    [SerializeField] private Button _boutonContinuer;

    enum Difficulte
    {
        Facile,
        Moyen,
        Difficile
    }

    private Difficulte difficulte;

    [SerializeField] private TMP_InputField nomJoueur;
    [SerializeField] private TMP_Text presentation;

    [SerializeField] private int[] valeursFacile;
    [SerializeField] private int[] valeursMoyen;
    [SerializeField] private int[] valeursDifficile;

    [SerializeField] private TMP_Text[] valeursDepart;
    [SerializeField] private TMP_Dropdown difficulteDropdown;
    [SerializeField] private TMP_Dropdown personnageDropdown;
    [SerializeField] private TMP_Dropdown ForetDropdown;
    [SerializeField] private TMP_Dropdown DistanceDropdown;

    [SerializeField] private GameObject[] personnages;
    [SerializeField] private GameObject[] personnagesPrefab;
    private GameObject personnageChoisi;
    private StrategieForet strategieChoisi;
    private float distanceChoisi;
    // Start is called before the first frame update
    void Start()
    {
        nomJoueur.text = "Mathurin";
        ChangerNomJoueur();

        difficulte = Difficulte.Facile;
        MettreAJour(valeursFacile);

        personnageChoisi = personnagesPrefab[0];
    }

    void Update()
    {
        _boutonDemarrer.interactable = nomJoueur.text != string.Empty;
        _boutonContinuer.interactable = PlayerPrefs.GetInt("PrefsSaved") == 1;
    }

    public void ChangerDifficulte()
    {
        difficulte = (Difficulte)difficulteDropdown.value;

        switch (difficulte)
        {
            case Difficulte.Facile:
                MettreAJour(valeursFacile);
                break;
            case Difficulte.Moyen:
                MettreAJour(valeursMoyen);
                break;
            case Difficulte.Difficile:
                MettreAJour(valeursDifficile);
                break;
        }
    }

    public void ChangerPersonnage()
    {
        personnages[0].SetActive(!personnages[0].activeSelf);
        personnages[1].SetActive(!personnages[1].activeSelf);
        ChoisirPersonnage(personnageDropdown.value);
    }
    private void ChoisirPersonnage(int choix)
    {
        personnageChoisi = personnagesPrefab[choix];
    }
    public void ChangerStrategie()
    {
        ChoisirStrategie(ForetDropdown.value);
    }
    private void ChoisirStrategie(int choix)
    {
        switch (choix)
        {
            case 0:
                strategieChoisi = new StrategieGrille();
                break;
            case 1:
                strategieChoisi = new StrategieHasard();
                break;
            case 2:
                strategieChoisi = new StrategieGOL();
                break;
        }
    }
    public void ChangerDistance()
    {
        distanceChoisi = DistanceDropdown.value + 3;
    }
    public void DemarrerPartie()
    {
        int[] valeursActuelles = null;
        switch (difficulte)
        {
            case Difficulte.Facile:
                valeursActuelles = valeursFacile;
                break;
            case Difficulte.Moyen:
                valeursActuelles = valeursMoyen;
                break;
            case Difficulte.Difficile:
                valeursActuelles = valeursDifficile;
                break;
        }

        ParametresParties.Instance.NomJoueur = nomJoueur.text;
        ParametresParties.Instance.OrDepart = valeursActuelles[0];
        ParametresParties.Instance.OeufsDepart = valeursActuelles[1];
        ParametresParties.Instance.SemencesDepart = valeursActuelles[2];
        ParametresParties.Instance.TempsCroissance = valeursActuelles[3];
        ParametresParties.Instance.DelaiCueillete = valeursActuelles[4];
        ParametresParties.Instance.ModelJoueur = personnageChoisi;
        ParametresParties.Instance.strategieForet = strategieChoisi;
        ParametresParties.Instance.distanceArbre = distanceChoisi;
        Debug.Log(personnageChoisi.name);
        Debug.Log(ParametresParties.Instance.ModelJoueur.name);

        // Pour la sauvegarde
        ParametresParties.Instance.ChoixModel = personnageDropdown.value;
        ParametresParties.Instance.ChoixStrategie = ForetDropdown.value;

        if (nomJoueur.text != string.Empty)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ferme");
        }
    }
    public void ContinuerPartie()
    {
        ParametresParties.Instance.NomJoueur = PlayerPrefs.GetString("NomJoueur");

        ParametresParties.Instance.OrDepart = PlayerPrefs.GetInt("OrDepart");
        ParametresParties.Instance.OeufsDepart = PlayerPrefs.GetInt("OeufsDepart");
        ParametresParties.Instance.SemencesDepart = PlayerPrefs.GetInt("SemencesDepart");
        ParametresParties.Instance.TempsCroissance = PlayerPrefs.GetInt("TempsCroissance");
        ParametresParties.Instance.DelaiCueillete = PlayerPrefs.GetInt("DelaiCueillete");

        ChoisirStrategie(PlayerPrefs.GetInt("ChoixStrategie"));
        ChoisirPersonnage(PlayerPrefs.GetInt("ChoixModel"));

        ParametresParties.Instance.ModelJoueur = personnageChoisi;
        ParametresParties.Instance.strategieForet = strategieChoisi;
        ParametresParties.Instance.distanceArbre = PlayerPrefs.GetFloat("DistanceArbre");

        UnityEngine.SceneManagement.SceneManager.LoadScene("Ferme");
    }
    public void QuitterJeu()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void MettreAJour(int[] valeurs)
    {
        for (int i = 0; i < valeursDepart.Length; i++)
        {
            valeursDepart[i].text = valeurs[i].ToString();
        }
    }

    public void ChangerNomJoueur()
    {
        presentation.text = $"\u266A \u266B Dans la ferme \u00e0  {nomJoueur.text} \u266B \u266A";
    }
}