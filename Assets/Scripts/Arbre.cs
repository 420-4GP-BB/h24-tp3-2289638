using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbre : MonoBehaviour , IPoussable
{
    private float TempsDescente = 2.0f;             // Bien que les consignes demandent que l'arbre descent sur une période de 0.5s, je trouve cette dernière trop courte
    private float TempsEcoulee;
    private Vector3 ArbreDescente;
    private bool ArbreDisponible = true;
    [SerializeField] public GameObject LogPrefab;
    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        return new EtatPousser(sujet,this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return ArbreDisponible;             // voir L.28
    }
    public void Pousser(float v, Vector3 PlayerRight)
    {
        transform.Rotate(PlayerRight, Time.deltaTime * v, Space.World);
    }

    public void Tomber()
    {
        ArbreDisponible = false;            // Pour éviter que l'arbre ne puisse être cliqué lorsqu'elle est dans son animation de descente, ce qui serait catastrophique pour le jeu.
        TempsEcoulee = 0.0f;
        float NewY = transform.position.y;
        NewY -= 2.0f;
        ArbreDescente = transform.position; // Attribution de la nouvelle position à laquelle devrait se trouver l'arbre avant d'être deleted. En bas du sol pour être invisible.
        ArbreDescente.y = NewY;
        StartCoroutine(TomberEnum());
    }
    private IEnumerator TomberEnum()        // Une enum pour que ce soit une animation.
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
        double elevation = (Random.value * 0.12) + 0.1;     // Les valeurs possibles ici (0.1 jusqu'à 0.22) finissent tous avec la buche qui est un peu dans le sol.
                                                            // Ils sont choisi au hasard afin de faire en sorte que les buches ne sont pas tous les même.
        double rotationY = (Random.value * 180) - 90;       // idem
        double scale = (Random.value * 0.2) + 0.4;          // idem
        position.y = (float)elevation;
        Quaternion rotation = Quaternion.Euler(90f,(float)rotationY, 0);    // Application des valeurs obtenus au hasard.
        GameObject newLog = Instantiate(LogPrefab, position, rotation);
        newLog.transform.localScale = new Vector3((float)scale, (float)scale, (float)scale);
    }
}
