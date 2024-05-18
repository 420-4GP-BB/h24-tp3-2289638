using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategieGrille : StrategieForet
{
    public override void GenererForet(float delimitionArbres, GameObject ArbrePrefab, GameObject[] arbres)
    {
        float tailleTableauFloat = 120 / delimitionArbres;      // Pourquoi 120? Afin de simplifier les divisions, les delimitation des arbres sont à 60 et -60 (x et z).
                                                                // Si par exemple chaque arbre occupe un espace de 4x4, il y aurait donc un total de 30 par 30 arbres
        int tailleTableau = (int)Math.Round(tailleTableauFloat);// Il y a donc 120 unités possible.
        bool[,] TableauForet = new bool[tailleTableau,tailleTableau];
        InitAllTrue(TableauForet);
        BannirCase(TableauForet);                               // Met à false les cases ou il ne devrait pas avoir d'arbres.
        GenererArbresForet(arbres, TableauForet,delimitionArbres,ArbrePrefab);
    }
    private void InitAllTrue(bool[,] tab)
    {
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            for (int j = 0; j < tab.GetLength(1); j++)
            {
                tab[i,j] = true;
            }
        }
    }
    public override void GenererArbresForet(GameObject[] arbres, bool[,]TableauArbreBool, float delimitionArbres, GameObject ArbrePrefab)
    {
        GameObject Foret = GameObject.FindGameObjectWithTag("ForetGroupe");
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
                    Vector3 position = new Vector3(PremiereCaseX + (delimitionArbres * i), 0, (PremiereCaseZ + (delimitionArbres * j)));
                    // Transforme la position de l'arbre à partir de la position de la case dans le tableau original.
                    GameObject.Instantiate(ArbrePrefab, position, Quaternion.identity, Foret.transform);
                }
            }
        }
    }

}
