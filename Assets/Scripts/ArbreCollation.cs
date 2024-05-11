using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbreCollation : MonoBehaviour
{
    [SerializeField] GameObject[] Collations;
    public bool CollationPrete;
    private float TempsDepuisCollation;
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
            TempsDepuisCollation = 0f;
        } else
        {
            TempsDepuisCollation += Time.deltaTime;
        }
        if (TempsDepuisCollation > 30f)
        {
            CreerCollation();
        }
    }
    private void CreerCollation()
    {
        CollationPrete = true;
        GameObject collationPrefab = Collations[Random.Range(0, Collations.Length)];
        GameObject collationInstance = Instantiate(collationPrefab, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
        collationInstance.GetComponent<Collation>().ArbreParent = this;
    }
}
