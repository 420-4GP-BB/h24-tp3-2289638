using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionnaireSauvegarde
{
    // Code inspirée fortement de celui du prof.
    private static GestionnaireSauvegarde _instance;
    Logger logger = new Logger();
    private bool isPlayerCreated = false;
    public bool isLoading;
    public static GestionnaireSauvegarde Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GestionnaireSauvegarde();

            return _instance;
        }
    }

    private const string NOM_FICHIER = "sauvegarde.json";
    private const string OBJECTS_KEY = "objects";
    private const string SAVEID_KEY = "$saveID";

    private string _cheminFichier; 
    private JsonData objects = null;
    private GestionnaireSauvegarde()
    {
        _cheminFichier = Path.Combine(Application.persistentDataPath, NOM_FICHIER);
    }
    public bool FichierExiste
    {
        get => !string.IsNullOrEmpty(_cheminFichier) && File.Exists(_cheminFichier);
    }
    public void SauvegarderPartie()
    {
        JsonData result = new JsonData();

        var allSaveables = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
        logger.Log("Number of saveables: " + allSaveables.ToArray().Length);
        JsonData savedObjects = new JsonData();
        foreach (var saveable in allSaveables)
        {
            logger.Log("Saveable ID: " + saveable.SaveID);
            JsonData data = saveable.SavedData();
            data[SAVEID_KEY] = saveable.SaveID;
            savedObjects.Add(data);
        }

        result[OBJECTS_KEY] = savedObjects;

        // On écrit le fichier avec une indentation pour le rendre lisible
        var writer = new JsonWriter();
        writer.PrettyPrint = true;
        result.ToJson(writer);
        System.IO.File.WriteAllText(_cheminFichier, writer.ToString());
    }

    public void ChargerPartie(string nomScene)
    {
        if (!FichierExiste)
        {
            return;
        }

        string text = File.ReadAllText(_cheminFichier);

        objects = JsonMapper.ToObject(text)[OBJECTS_KEY];
        if (objects != null)
        {
            SceneManager.sceneLoaded += LoadAfter; // La méthode sera appelée après le chargement de la scène
            SceneManager.LoadScene(nomScene, LoadSceneMode.Single);
        }
    }

    private void LoadAfter(Scene s, LoadSceneMode mode)
    {
        logger.Log("Pre-scene loading");
        var allLoadables = Object.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>()
            .ToDictionary(o => o.SaveID, o => o);

        int nombreObjets = objects.Count;

        // Il faut restaurer les objets qui sont dans la sauvegarde
        for (int i = 0; i < nombreObjets; i++)
        {
            JsonData data = objects[i];
            string saveID = data[SAVEID_KEY].ToString();
            logger.Log("Save ID Looking: " + saveID);

            if (allLoadables.ContainsKey(saveID))
            {
                logger.Log("Save ID Found");
                allLoadables[saveID].LoadFromData(data);
                allLoadables.Remove(saveID); // On enlève car on a déjà traité cet objet
                // Ceux qui restent seront détruits
            }
        }


        // Les objets qui ne sont pas dans la sauvegarde doivent être détruits
        foreach (var loadable in allLoadables.Values)
        {
            MonoBehaviour obj = loadable as MonoBehaviour;
            GameObject.Destroy(obj.gameObject); // L'objet ne doit plus être dans la scène
        }

        SceneManager.sceneLoaded -= LoadAfter;
    }

    // Le joueur est instantiée, il faut donc le load après qu'il a été instantié.
    public void OnPlayerCreated()
    {
        isPlayerCreated = true;
        TryLoadPlayerData();
    }

    private void TryLoadPlayerData()
    {
        if (isPlayerCreated && objects != null)
        {
            LoadAfter(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }
}
