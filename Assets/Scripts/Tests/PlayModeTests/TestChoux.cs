using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class TestChoux
{
    private GameObject chou, soleil;
    private Inventaire inventaire;

    [SetUp]
    public void CreerObjets()
    {
        soleil = new GameObject("Directional Light");
        soleil.AddComponent<Light>();
        soleil.AddComponent<Soleil>();
        chou = GameObject.Instantiate(PrefabUtility.LoadPrefabContents("Assets/Prefabs/Chou.prefab"));

        var joueur = new GameObject("Joueur");
        inventaire = joueur.AddComponent<Inventaire>();
    }

    [TearDown]
    public void DetruireObjets()
    {
        GameObject.Destroy(soleil);
        GameObject.Destroy(chou);
        GameObject.Destroy(inventaire.gameObject);
    }

    [UnityTest]
    public IEnumerator TestChouCueillir()
    {
        // ====== EXEMPLE DE TEST DÉJÀ FONCTIONNEL ======
        // Valide ce qui se passe quand on plante un chou, qu'on attend 3 jours, puis qu'on le cueille.
        // On vérifie que le nombre de choux


        // ARRANGE: dans le SetUp + ici
        var emplacement = chou.GetComponent<EmplacementChouVide>();

        // ACT
        inventaire.Graines = 1;
        inventaire.Choux = 0;
        emplacement.Planter(inventaire);
        yield return null;

        var chouCroissant = chou.GetComponent<ChouCroissant>();
        yield return null;

        // Trois jours pour pousser :
        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        var chouPret = chou.GetComponent<ChouPret>();

        chouPret.Ramasser(inventaire);
        yield return null;

        // ASSERT
        Assert.AreEqual(inventaire.Choux, 1);
    }

    [UnityTest]
    public IEnumerator TestChouPerdGraine()
    {
        // TODO: Tester que quand on vient de planter un chou, l'inventaire a une graine en moins
        //
        // Faites un :         yield return null;
        // après avoir planté le chou, question de simuler qu'au moins 1 frame s'est écoulée avant que
        // vous fassiez votre test
        // ARRANGE: dans le SetUp + ici
        var emplacement = chou.GetComponent<EmplacementChouVide>();

        // ACT
        inventaire.Graines = 1;         // Le joueur commence avec une seule graine, et il devrait donc en finir avec aucune.
        inventaire.Choux = 0;
        emplacement.Planter(inventaire);
        yield return null;
        Assert.IsTrue(inventaire.Graines==0);
    }

    [UnityTest]
    public IEnumerator TestChouJourneesPassees()
    {
        // TODO: Tester qu'au bout de 3 jours, le chou est prêt �  se faire cueillir
        //
        // Faites un :         yield return null;
        // après chaque appel de la méthode JourneePassee(); du composant ChouCroissant, question de simuler
        // qu'au moins 1 frame s'écoule entre chaque appel

        // ARRANGE: dans le SetUp + ici
        var emplacement = chou.GetComponent<EmplacementChouVide>();
        ComportementJoueur comportementJoueur = new ComportementJoueur();
        // ACT
        inventaire.Graines = 1;
        inventaire.Choux = 0;
        emplacement.Planter(inventaire);
        yield return null;

        var chouCroissant = chou.GetComponent<ChouCroissant>();
        yield return null;

        // Trois jours pour pousser :
        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        ChouPret chouPret = chou.GetComponent<ChouPret>();

        if (chouPret != null)
        {
            Assert.IsTrue(typeof(IRamassable).IsAssignableFrom(chouPret.GetType()) && chouPret.Permis(comportementJoueur));    
                                            // On s'assure que la composante ChouPret fait ce qu'elle est suppose faire.
                                            // Que c'est un ramassable, et qu'on est permis de le ramasser
        }
        else                                // Si le chou n'a pas de composant chouPret, c'est qu'il ne passe pas ce teste.
        {
            Assert.Fail();
        }
    }

    [UnityTest]
    public IEnumerator TestChouReplanter()
    {
        // TODO: Vérifier qu'on peut replanter un deuxième chou sur le même emplacement
        // après l'avoir cueilli

        // ARRANGE: dans le SetUp + ici
        var emplacement = chou.GetComponent<EmplacementChouVide>();

        // ACT
        inventaire.Graines = 2;
        inventaire.Choux = 0;
        emplacement.Planter(inventaire);
        yield return null;

        var chouCroissant = chou.GetComponent<ChouCroissant>();
        yield return null;

        // Trois jours pour pousser :
        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        var chouPret = chou.GetComponent<ChouPret>();

        chouPret.Ramasser(inventaire);
        yield return null;

        emplacement = chou.GetComponent<EmplacementChouVide>();                     // On reprend l'emplacement vide du chou
        emplacement.Planter(inventaire);
        
        var chouCroissantDeuxiemePlantation = chou.GetComponent<ChouCroissant>();
        Assert.IsTrue(inventaire.Graines==0 && chouCroissantDeuxiemePlantation != null);
    }
}