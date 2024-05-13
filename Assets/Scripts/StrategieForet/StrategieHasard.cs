using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategieHasard : StrategieForet
{


    public override void GenererForet(float delimitionArbres, GameObject ArbrePrefab, GameObject[] arbres)
    {
        float tailleTableauFloat = 120 / delimitionArbres;      // Pourquoi 120? Afin de simplifier les divisions, les delimitation des arbres sont à 60 et -60 (x et z).
                                                                // Si par exemple chaque arbre occupe un espace de 4x4, il y aurait donc un total de 30 par 30 arbres
        int tailleTableau = (int)Math.Round(tailleTableauFloat);// Il y a donc 120 unités possible.
        bool[,] TableauForet = new bool[tailleTableau, tailleTableau];
        InitHasard(TableauForet);
        BannirCase(TableauForet);
        GenererArbresForet(arbres, TableauForet, delimitionArbres, ArbrePrefab);
    }
    public override void GenererArbresForet(GameObject[] arbres, bool[,] TableauArbreBool, float delimitionArbres, GameObject ArbrePrefab)
    {
        foreach (GameObject arbre in arbres)
        {
            GameObject.Destroy(arbre);
        }
        for (int i = 0; i < TableauArbreBool.GetLength(0); i++)
        {
            for (int j = 0; j < TableauArbreBool.GetLength(1); j++)
            {
                if (TableauArbreBool[i, j]) // Vérifie si un arbre doit être placée ici.
                {
                    float randX = UnityEngine.Random.Range(-1.25f, 1.26f);  // Pour que l'emplacement des arbres ressemble moins à une grille que ceux de la grille.
                    float randZ = UnityEngine.Random.Range(-1.25f, 1.26f);
                    Vector3 position = new Vector3(PremiereCaseX + (delimitionArbres * i) +randX, 0, PremiereCaseZ + (delimitionArbres * j)+randZ);
                    // Transforme la position de l'arbre à partir de la position de la case dans le tableau original.
                    GameObject.Instantiate(ArbrePrefab, position, Quaternion.identity);
                }
            }
        }
    }
    private void InitHasard(bool[,] tab)
    {
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            for (int j = 0; j < tab.GetLength(1); j++)
            {
                int randInt = UnityEngine.Random.Range(0, 2); // 50/50 chance
                if (randInt == 0)
                {
                    tab[i, j] = true;
                }
                else
                {
                    tab[i, j] = false;
                }
            }
        }
    }
}
