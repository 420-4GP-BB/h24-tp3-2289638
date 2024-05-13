using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

public class StrategieGOL : StrategieForet
{
    public override void GenererForet(float delimitionArbres, GameObject ArbrePrefab, GameObject[] arbres)
    {
        float tailleTableauFloat = 120 / delimitionArbres;      // Pourquoi 120? Afin de simplifier les divisions, les delimitation des arbres sont à 60 et -60 (x et z).
                                                                // Si par exemple chaque arbre occupe un espace de 4x4, il y aurait donc un total de 30 par 30 arbres
        int tailleTableau = (int)Math.Round(tailleTableauFloat);// Il y a donc 120 unités possible.
        bool[,] TableauForet = new bool[tailleTableau, tailleTableau];
        InitHasard(TableauForet);
        for (int i=0;i<10;i++)
        {
            TableauForet = GameOfLifeRound(TableauForet);
        }
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
                    Vector3 position = new Vector3(PremiereCaseX + (delimitionArbres * i) + randX, 0, PremiereCaseZ + (delimitionArbres * j) + randZ);
                    // Transforme la position de l'arbre à partir de la position de la case dans le tableau original.
                    GameObject.Instantiate(ArbrePrefab, position, Quaternion.identity);
                }
            }
        }
    }
    private bool[,] GameOfLifeRound(bool[,] tab)
    {
        bool[,] newTab = new bool[tab.GetLength(0),tab.GetLength(1)];
        for (int i = 0;i < tab.GetLength(0); i++)
        {
            for (int j = 0;j < tab.GetLength(1); j++)
            {
                int voisins = CalculerNombreVoisin(tab, i, j);
                bool isAlive = tab[i, j];

                if (isAlive && (voisins == 3 || voisins == 4 || voisins == 6 || voisins == 7 || voisins == 8))
                {
                    newTab[i, j] = true;
                }
                else if (!isAlive && (voisins == 3 || voisins == 6 || voisins == 7 || voisins == 8))
                {
                    newTab[i, j] = true;
                }
                else
                {
                    newTab[i, j] = false;
                }
            }
        }
        return newTab;
    }
    private int CalculerNombreVoisin(bool[,] tab,int i, int j)
    {
        int voisins = 0;
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0)   // La case n'est pas voisin à elle même.
                    continue;

                int checkX = i + x;     
                int checkY = j + y;
                // Vérifier qu'on ne cherche pas des voisins à des cases inexistante.
                if (checkX >= 0 && checkX < tab.GetLength(0) && checkY >= 0 && checkY < tab.GetLength(1))
                {
                    if (tab[checkX, checkY])
                        voisins++;
                }
            }
        }
        return voisins;
    }
}
