using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StrategieForet
{
    public const int PremiereCaseX = -60;   // Premiere case en x, (y=0),z en haut � gauche � laquelle il peut se trouver un arbre
    public const int PremiereCaseZ = -60;
    public abstract void GenererForet(float delimitionArbres, GameObject ArbrePrefab, GameObject[] arbres);
    public virtual void BannirCase(bool[,] tab)
    {
        /*
         * Comme on travaille avec des distances entre arbres variantes, on ne peut pas hard-code les delimitations des zones bannis.
         * Nous allons donc proc�der avec des ratios, et toujours arrondir vers le haut.
         * Prenons les d�limitations dans le cas ou la distance entre chaque arbre est de 4, et qu'il y a donc 30 par 30 arbres sur la carte.
         * 
         * Les g�n�rations commencent � partir de en haut � gauche et g�n�re row par row.
         * On commence donc avec l'origine -60,-60
         * 
         * D�limitations du village:
         *      x: [-60,-40] donc apr�s [0,20] cases a partir de l'origine (-60,-60)
         *      z: [-29, 0], donc apr�s [31,60] cases a partir de l'origine (-60,-60)
         *      La zone bannie est un rectangle x:0,  z:31
         *      jusqu'�                         x:20, z:60
         *      Donc, dans le cas qu'on aurait un tableau 120 par 120
         *      0,0 serait �quivalent aux coordonn�es -60, -60
         *      On ferait : if ( (i>= 0 && i<=20) && (j>=31 && j<=60) ) {tab[i,j] = false}
         *      Ca nous fait donc un ratio de 0/120, 20/120 pour les i et 31/120, 60/120 pour les j
         *      
         *      Si l'on convertit �a � notre example de 30 par 30 arbres:
         *      En faisant : int(Math.RoundUp(0*30/120))
         *      x: [0,5] ; z : [8,15]
         *
         * Notons donc les d�limitations de toute les zones bannis:
         * Village : x[-60,-40] ; z[-29,0].   Ajust�es : x[0,20]    ; z[31,60]
         * Maison  : x[45, 60]  ; z[-60,-45]. Ajust�es : x[105,120] ; z[0 ,15]
         * Chemin1 : x[55, 60]  ; z[-45, -15].Ajust�es : x[115,120] ; z[15,45]
         * Chemin2 : x[-40, 60] ; z[-18,-12]. Ajust�es : x[20, 120] ; z[42,48]
         */
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            for (int j=0; j < tab.GetLength(1); j++)
            {
                if (VerifierCaseBannie(i, j, tab.GetLength(0)))
                {
                    tab[i, j] = false;
                }
            }
        }
    }
    public virtual bool VerifierCaseBannie(int i, int j, int tailleTab)
    {
        bool banned = false;

        // Village : x[0,20] ; z[31,60]
        if ((i >= 0 && i <= Mathf.CeilToInt(20.0f / 120 * tailleTab)) &&
            (j >= Mathf.CeilToInt(31.0f / 120 * tailleTab) && j <= Mathf.CeilToInt(60.0f / 120 * tailleTab)))
        {
            banned = true;
        }

        // Maison : x[105,120] ; z[0,15]
        if ((i >= Mathf.CeilToInt(105.0f / 120 * tailleTab) && i <= Mathf.CeilToInt(120.0f / 120 * tailleTab)) &&
            (j >= 0 && j <= Mathf.CeilToInt(15.0f / 120 * tailleTab)))
        {
            banned = true;
        }

        // Chemin1 : x[115,120] ; z[15,45]
        if ((i >= Mathf.CeilToInt(115.0f / 120 * tailleTab) && i <= Mathf.CeilToInt(120.0f / 120 * tailleTab)) &&
            (j >= Mathf.CeilToInt(15.0f / 120 * tailleTab) && j <= Mathf.CeilToInt(45.0f / 120 * tailleTab)))
        {
            banned = true;
        }

        // Chemin2 : x[20,120] ; z[42,48]
        if ((i >= Mathf.CeilToInt(20.0f / 120 * tailleTab) && i <= Mathf.CeilToInt(120.0f / 120 * tailleTab)) &&
            (j >= Mathf.CeilToInt(42.0f / 120 * tailleTab) && j <= Mathf.CeilToInt(48.0f / 120 * tailleTab)))
        {
            banned = true;
        }

        return banned;
    }
    public abstract void GenererArbresForet(GameObject[] arbres, bool[,] TableauArbreBool, float delimitionArbres, GameObject ArbrePrefab);
    public virtual void InitHasard(bool[,] tab)
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
