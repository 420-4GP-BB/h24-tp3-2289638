using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbre : MonoBehaviour , IPoussable
{
    private float TempsDescente = 2.0f;
    private float TempsEcoulee;
    private Vector3 ArbreDescente;
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
        while (TempsEcoulee <TempsDescente)
        {
            TempsEcoulee += Time.deltaTime;
            float lerp = TempsEcoulee / TempsDescente;
            transform.position = Vector3.Lerp(transform.position, ArbreDescente, lerp);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
