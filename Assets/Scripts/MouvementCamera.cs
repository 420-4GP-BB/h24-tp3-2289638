using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementCamera : MonoBehaviour
{

    private UnityEngine.GameObject _joueur;
    [SerializeField] private Vector3 _offsetJoueur;

    // Start is called before the first frame update
    void Start()
    {
        _joueur = UnityEngine.GameObject.Find("Joueur");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 translation = _joueur.transform.TransformDirection(_offsetJoueur); 
        Vector3 nouvellePosition = _joueur.transform.position + translation;
        transform.position = nouvellePosition;
        transform.LookAt(_joueur.transform.position + _joueur.transform.forward * 2);
    }
}
