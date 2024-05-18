using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauvegarderJoueur : SauvegardeBase
{
    public override void LoadFromData(JsonData data)
    {
        GetComponent<CharacterController>().enabled = false;
        LoadTransformFromData(data);
        GetComponent<CharacterController>().enabled = true;
        Inventaire inventaire = GetComponent<Inventaire>();
        EnergieJoueur energie = GetComponent<EnergieJoueur>();
        inventaire.Or = (int)data["orActuel"];
        inventaire.Oeuf = (int)data["oeufsActuel"];
        inventaire.Choux = (int)data["chouxActuel"];
        inventaire.Buches = (int)data["buchesActuel"];
        inventaire.Graines = (int)data["grainesActuel"];
        Debug.Log("Energie recuperee: " + float.Parse(data["energieRestante"].ToString()));
        energie.Energie = float.Parse(data["energieRestante"].ToString());
    }

    public override JsonData SavedData()
    {
        Inventaire inventaire = GetComponent<Inventaire>();
        EnergieJoueur energie = GetComponent<EnergieJoueur>();
        JsonData data = SavedTransform;
        data["orActuel"] = inventaire.Or;
        data["oeufsActuel"] = inventaire.Oeuf;
        data["chouxActuel"] = inventaire.Choux;
        data["buchesActuel"] = inventaire.Buches;
        data["grainesActuel"] = inventaire.Graines;
        data["energieRestante"] = energie.Energie;
        Debug.Log("Energie sauvegardee: "+energie.Energie);
        return data;
    }
    public override void OnBeforeSerialize()
    {
        SaveID = "Joueur";
    }
}
