using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbreCollation : MonoBehaviour
{
    [SerializeField] GameObject[] Collations;   // À remplir dans l'éditeur. On peut modifier les collations possible pour les arbres.
    public bool CollationPrete;                 // Variable nécessaire pour savoir lorsqu'une collation est prête (qu'elle attend d'être ramassée), et donc de ne pas en spawn d'autres
    private float TempsDepuisCollation;         // Variable qui augmente pour faire tomber une collation à chaque trente secondes.
    void Start()
    {
        CollationPrete = false;
        TempsDepuisCollation = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (CollationPrete)
        {
            TempsDepuisCollation = 0f;          // En ce, CollationPrete = true, on n'aura jamais d'autre collations qui vont apparaitre
        } else
        {
            TempsDepuisCollation += Time.deltaTime;// Autrement, le compteur pourra augmenter librement jusqu'à qu'il atteigne le 'cut-off' (30s).
        }
        if (TempsDepuisCollation > 30f)
        {
            CreerCollation();                   // Pas besoin de recommencer le compteur; la prochaine boucle update s'occupera de ça.
        }
    }
    private void CreerCollation()
    {
        CollationPrete = true;
        GameObject collationPrefab = Collations[Random.Range(0, Collations.Length)];
        GameObject collationInstance = Instantiate(collationPrefab, transform.position + new Vector3(0, 4, 0), Quaternion.identity);    // Hauteur de 4; elle tombera grace à rigidbody
        collationInstance.GetComponent<Collation>().ArbreParent = this; // Pour que la collation puisse signaler (this) arbre lorsqu'elle est ramassée
    }
}
