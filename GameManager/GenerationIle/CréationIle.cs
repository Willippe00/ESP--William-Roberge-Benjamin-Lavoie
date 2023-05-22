using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CréationIle : MonoBehaviour
{
    
    [HideInInspector] public char[,] carte;

    [SerializeField] GameObject parentCarte;

    [SerializeField] public float tailleCase;

    [SerializeField] public Vector2Int tailleCarte;
    [SerializeField] int rayonPrimaire;
    [SerializeField] int rayonSecondaire;
    public int limiteIle;
    [SerializeField] int nbOrigine;

    [SerializeField] public List<char> typesBiomes = new(); //0 = Océan, 1 = Remplissage
    [SerializeField] List<float> quantitéBiomes = new(); //0 = Océan, 1 = Remplissage
    [SerializeField] List<GameObject> prefabBiomes = new(); //0 = Océan, 1 = Remplissage

    [HideInInspector] public List<Vector2Int>[] suivitBiomes;

    private void Awake()
    {
        Init();
        CarteInitiale();
        IleBrut();
        Plages();
        for (int i = 3; i < typesBiomes.Count; i++) Biomes(i, typesBiomes[1].ToString());
        Générer();
        Nettoyer();

        Time.timeScale = 1;
    }


    #region Étape0 - InitierVariables

    private void Init()
    {
        suivitBiomes = new List<Vector2Int>[typesBiomes.Count];
        for (int i = 0; i < suivitBiomes.Length; i++) suivitBiomes[i] = new List<Vector2Int>();

        quantitéBiomes[1] = (int) ((tailleCarte.x * tailleCarte.y) * 1100 / 14400 * quantitéBiomes[1]);

        limiteIle = rayonPrimaire + rayonSecondaire + 1;
    }

    #endregion


    #region Étape1 - Carte Initiale

    void CarteInitiale()
    {
        carte = new char[tailleCarte.x, tailleCarte.y];
        for (int y = 0; y < carte.GetLength(1); y++)
        {
            for (int x = 0; x < carte.GetLength(0); x++)
            {
                carte[x, y] = typesBiomes[0];
                suivitBiomes[0].Add(new Vector2Int(x, y));
            }
        }
    }
    #endregion


    #region Étape2 - IleBrut

    private void IleBrut()
    {
        for (int i = 0; i < nbOrigine; i++)
        {
            CréerOrigine(1, 0);
        }
        Vector2Int pos = CréerPosValideSurBiome(1);
        for (int i = 0; i < quantitéBiomes[1]; i++)
        {
            GénérerCercle(pos, rayonPrimaire + Random.Range(-2, 1), 1, typesBiomes[0].ToString());
            pos = CréerPosValideSurBiome(1);
        }
    }
    private bool CréerOrigine(int biome, int biomeSurface)
    {
        Vector2Int pos = new Vector2Int(Random.Range(limiteIle *2, tailleCarte.x - limiteIle*2), Random.Range(limiteIle*2, tailleCarte.y - limiteIle*2));
        if (carte[pos.x, pos.y] != typesBiomes[biomeSurface]) return CréerOrigine(biome, biomeSurface);
        GénérerCercle(pos, rayonPrimaire + Random.Range(-2, 1), biome, typesBiomes[biomeSurface].ToString());
        return true;
    }

    private Vector2Int CréerPosValideSurBiome(int biome)
    {
        Vector2Int pos = suivitBiomes[biome][Random.Range(0, suivitBiomes[biome].Count)];
        if (pos.x > tailleCarte.x - limiteIle || pos.x < limiteIle || pos.y > tailleCarte.y - limiteIle || pos.y < limiteIle) return CréerPosValideSurBiome(biome);
        return pos;
    }

    private void GénérerCercle(Vector2Int centre, int r, int biome, string surfaceValide)
    {
        for (int x = -r + 1; x <= r - 1; x++)
        {
            int borneBas = (int)-Mathf.Sqrt(Mathf.Abs((r * r) - (x * x)));
            int borneHaut = (int)Mathf.Sqrt(Mathf.Abs((r * r) - (x * x)));

            for (int y = borneBas; y <= borneHaut; y++)
            {
                Vector2Int pos = new Vector2Int(centre.x + x, centre.y + y);
                if (EstCaseValide(surfaceValide, pos.x, pos.y) && y != -r && y != r)
                {
                    carte[pos.x, pos.y] = typesBiomes[biome];
                    suivitBiomes[biome].Add(pos);
                }
            }
        }
    }

    private bool EstCaseValide(string surfaceValide, int x, int y)
    {
        if (x < 0 || x >= tailleCarte.x || y < 0 || y >= tailleCarte.y) return false;
        else return surfaceValide.Contains(carte[x, y]);
    }

    private void CompterCasesPartout(int biome)
    {
        for (int y = limiteIle/2; y < carte.GetLength(1) - limiteIle / 2; y++)
        {
            for (int x = limiteIle / 2; x < carte.GetLength(0) - limiteIle / 2; x++)
            {
                if (carte[x, y] == typesBiomes[biome]) suivitBiomes[biome].Add(new Vector2Int(x, y));

            }
        }
    }
    #endregion


    #region Étape3 - Plages

    void Plages()
    {
        suivitBiomes[0] = new();
        CompterCasesPartout(0);
        
        for (int i = 0; i < suivitBiomes[0].Count; i++)
        {
            int r = new List<int> { 2, 2, 2, 2, Mathf.Max(2, tailleCarte.x /35)}[Random.Range(0, 5)];
            GénérerCercle(suivitBiomes[0][i], r, 2, typesBiomes[1].ToString());
        }
    }


    #endregion


    #region Étape4 - Biomes

    void Biomes(int biome, string biomeSurface)
    {
        CréerOrigine(biome, 1);
        for (int i = 0; i < quantitéBiomes[biome]; i++)
        {
            Vector2Int pos = CréerPosValideSurBiome(biome);
            GénérerCercle(pos, rayonPrimaire + Random.Range(-2, 1), biome, biomeSurface);
        }
    }

    #endregion


    #region Étape6 - Générer

    void GénérerVersionConsole()
    {
        string affichage = "";
        for (int y = 0; y < carte.GetLength(1); y++)
        {
            for (int x = 0; x < carte.GetLength(0); x++)
            {
                affichage += carte[x, y];
            }
            affichage += "\n";
        }

        Debug.Log(affichage);

    }

    void Générer()
    {
        for (int i = 0; i < suivitBiomes.Length; i++) suivitBiomes[i] = new();

        for (int y = 0; y < carte.GetLength(1); y++)
        {
            for (int x = 0; x < carte.GetLength(0); x++)
            {
                CompterCase(carte[x, y], new Vector2Int(x,y));

                GameObject parcelle = prefabBiomes[TesterBiome(carte[x,y])];
                parcelle = Instantiate(parcelle, new Vector3(x * tailleCase, 0, y * tailleCase), parcelle.transform.rotation, parentCarte.transform);
                parcelle.transform.localScale *= tailleCase/10;
            }
        }
    }

    void CompterCase(char typeBiome, Vector2Int pos)
    {
        suivitBiomes[TesterBiome(typeBiome)].Add(pos);
    }

    int TesterBiome(char biome)
    {
        for (int i = 0; i < typesBiomes.Count; i++)
        {
            if (biome == typesBiomes[i]) return i;
        }
        return 0;
    }
    #endregion


    #region Étape7 - Nettoyer
    void Nettoyer()
    {
        prefabBiomes = null;
    }
    #endregion

}
