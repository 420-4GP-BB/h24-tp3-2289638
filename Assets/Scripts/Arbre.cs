using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbre : MonoBehaviour , IPoussable
{
    private float TempsDescente = 2.0f;
    private float TempsEcoulee;
    private Vector3 ArbreDescente;
    [SerializeField] public GameObject LogPrefab;
    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        return new EtatPousser(sujet,this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;
    }
    public void Pousser(float v, Vector3 PlayerRight)
    {
        transform.Rotate(PlayerRight, Time.deltaTime * v, Space.World);
    }

    public void Tomber()
    {
        TempsEcoulee = 0.0f;
        float NewY = transform.position.y;
        NewY -= 2.0f;
        ArbreDescente = transform.position;
        ArbreDescente.y = NewY;
        StartCoroutine(TomberEnum());
    }
    private IEnumerator TomberEnum()
    {
        yield return new WaitForSeconds(1);
        InstantiateLog();
        while (TempsEcoulee <TempsDescente)
        {
            TempsEcoulee += Time.deltaTime;
            float lerp = TempsEcoulee / TempsDescente;
            transform.position = Vector3.Lerp(transform.position, ArbreDescente, lerp);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
    private void InstantiateLog()
    {
        Vector3 position = transform.position;
        double elevation = (Random.value * 0.12) + 0.1;
        double rotationY = (Random.value * 180) - 90;
        double scale = (Random.value * 0.2) + 0.4;
        position.y = (float)elevation;
        Quaternion rotation = Quaternion.Euler(90f,(float)rotationY, 0);
        GameObject newLog = Instantiate(LogPrefab, position, rotation);
        newLog.transform.localScale = new Vector3((float)scale, (float)scale, (float)scale);
    }
}
