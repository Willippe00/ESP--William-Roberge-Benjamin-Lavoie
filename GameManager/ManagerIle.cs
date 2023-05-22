using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerIle : MonoBehaviour
{

    [SerializeField] GameObject TitreJeu;
    [SerializeField] GameObject Soleil;
    [SerializeField] GameObject Lune;
    [SerializeField] G�n�rationRessource scriptG�nRess;

    [SerializeField] GameObject Mouton;




    public const int HauteurIle = 120; // niveau de s�curit� a corriger


    public const int LargeurIle = 120; // niveau de s�curit� a corriger


    // Vector2 vecteurGrandeurG�n� = new Vector2(2, 23);

    Vector2[] DimmensionIle;


    //new Vector2[HauteurIle+1] { new Vector2(2, LargeurIle-2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2) };  // tous des valeur fictive

    public CaseCarte[,] carteIle = new CaseCarte[LargeurIle, HauteurIle];

    public int nbcase = HauteurIle * LargeurIle;


    public int Nbanimaux;

    public float NiveauIle = 0; //D�termine le niveau on est g�n�r� l'�le





    float HeureJeu = 14;

    #region composentdujeu


    int RayonMinimumPetitBiome = 25;
    int RayonMaximumPetitBiome = 40;  // a modifier

    int RayonMinimumMoyenBiome = 35;
    int RayonMaximumMoyenBiome = 55;  // a modifier

    int RayonMinimumGrandBiome = 47;
    int RayonMaximumGrandBiome = 65;  // a modifier

    public float longeurUnityCase = 5;
    #endregion

    #region Spawner
    [SerializeField] private GameObject SeedOiseau;
    [SerializeField] private GameObject SeedLoup;

    private List<CaseCarte> ListeSpwaner = new List<CaseCarte>();
    private List<GameObject> ListePnjActif = new List<GameObject>();
    #endregion

    #region Pr�fabBiome
    [SerializeField] private GameObject BiomePlage;
    [SerializeField] private GameObject BiomeOc�an;
    [SerializeField] private GameObject BiomeD�sertG�n�rique;
    [SerializeField] private GameObject BiomeForetG�n�rique;
    [SerializeField] private GameObject BiomeGlacierG�n�rique;
    [SerializeField] private GameObject BiomeRemplissage;

    #endregion


    #region Composente du d�sert


    public List<CaseCarte> casesD�sert;

    #endregion

    #region Composant For�t

    public List<CaseCarte> casesFor�t;
    #endregion

    #region Composant Plage

    public List<CaseCarte> casesPlage = new List<CaseCarte>();
    #endregion

    #region Composant Glacier

    public List<CaseCarte> casesGlacier;
    #endregion

    #region Composant Remplissage

    public List<CaseCarte> casesRemplissage = new List<CaseCarte>();
    #endregion

    enum P�riodeJourn� { Matin, ApresMidi, Nuit }
    float Heure = 14;


    private void Awake()
    {
        G�n�rerIle();  // test de d�mo
    }
    // Update is called once per frame
    void Update()
    {
        //g�r�Heure();

        // g�r�Pnj();
        //G�rerPnj();
    }

    public void g�r�Heure()
    {
        // HeureJeu += Time.deltaTime*100;      // voir si facteur est bon

        if (HeureJeu <= 24)
        {
            if (HeureJeu > 6 && HeureJeu < 20)
            {
                Debug.Log("on est le jour");
                HeureJeu += Time.deltaTime;
                // on vas g�r�r la position du soleil
                //on vas g�r�r le temps de la journ�
                // le soleil d�crit une rotation au dessu du plan 180 degr� r�parte 3/4 ensoleiller 1/4 nuit    1jour jeu = 15min irl
            }
            else
            {
                Debug.Log("on est le soir");
                HeureJeu += Time.deltaTime;
            }

            Debug.Log(HeureJeu.ToString());
            G�rerPositionSoleil(HeureJeu);
        }
        else
        {
            //Instantiate(Mouton, new Vector3(150, 0, 200), Quaternion.identity);
            HeureJeu = 0;
            // G�rerSpawner(); // on fait spanner les pnj 1 fois par jour
        }
    }

    public void G�rerPositionSoleil(float heure) // pour am�lior� https://docs.unity3d.com/ScriptReference/Transform-rotation.html
    {
        //ligne pour �viter un erreur si le soleil est manquant
        float angleRotation = (360 * (heure / 24)) + 270;
        // position du soleil

        Quaternion CibleSoleil = Quaternion.Euler(angleRotation, 0, 0);
        Quaternion CibleLune = Quaternion.Euler(-angleRotation, 0, 0);

        // Soleil.transform.Rotate(Vector3.right, angleRotation);

        //Soleil.transform.rotation = new Quaternion(angleRotation, 0, 0, 0);
        Soleil.transform.rotation = CibleSoleil;

        // position de la  lune antagoniste

        //Lune.transform.Rotate(new Vector3(1, 0, 0), angleRotation);
        Lune.transform.rotation = CibleLune;
    }

    public static Vector2[] G�n�r�BordureIle()
    {
        Vector2[] bordure = new Vector2[HauteurIle + 1];

        int s�curit� = 0;
        do
        {
            s�curit�++;

            bordure[0] = new Vector2((int)Random.Range(10, LargeurIle / 2), (int)Random.Range(LargeurIle / 2, LargeurIle - 10));

            for (int i = 1; i < bordure.Length; ++i)
            {
                int BorneGauche;
                int BorneDroit;



                BorneGauche = Random.Range(-1, 2);
                BorneDroit = Random.Range(-1, 2);

                if ((BorneGauche + bordure[i - 1].x) < 6 || (BorneGauche + bordure[i - 1].x) > LargeurIle / 2)
                {

                    BorneGauche = 0;
                }

                if ((BorneDroit + bordure[i - 1].y) > LargeurIle - 6 || (BorneDroit + bordure[i - 1].y) < LargeurIle / 2)
                {
                    BorneDroit = 0;
                }
                //while(((BorneGauche + bordure[i - 1].x) < 6) || (BorneDroit + bordure[i - 1].y > LargeurIle - 6) || BorneDroit < LargeurIle / 2); // cette ligne la marche tbnk
                //while (((BorneGauche + bordure[i - 1].x) < 0) && ((BorneGauche + bordure[i - 1].x) > LargeurIle / 2) && (BorneDroit + bordure[i - 1].y) < LargeurIle / 2 && ((BorneDroit + bordure[i - 1].y) > LargeurIle));
                // while ((BorneGauche > LargeurIle / 3 || BorneDroit < LargeurIle / 2 || (BorneGauche + bordure[i - 1].x) < 6 || BorneDroit+ bordure[i - 1].y > LargeurIle-6));

                bordure[i] = new Vector2(BorneGauche + bordure[i - 1].x, BorneDroit + bordure[i - 1].y);
            }
        } while (v�rifierGrosseurIle(bordure) && s�curit� < 20);

        return bordure;
    }

    static bool v�rifierGrosseurIle(Vector2[] bordure)
    {
        int grosseur = 0;
        for (int i = 0; i < bordure.Length; ++i)
        {
            grosseur += (int)bordure[i].y - (int)bordure[i].x;
        }
        return grosseur > ((HauteurIle * LargeurIle) / 1.2);
    }

    public void G�n�rerIle()
    {
        /*
         a int�grer :

        - zone de blage poc�dural 1 ou deux cube en bordure de l'oc�ant
        - un point d'origine de la jungle pour ensuite l'�tendre
        - un point d'arigine du d�sert
        - combler le reste avec environment fourtout


         */

        G�n�rerCarteVierge();



        DimmensionIle = G�n�r�BordureIle();

        G�n�rerCaseEau();

        G�n�rerPlage();

        G�n�rerCasesBiome(ref casesD�sert, BiomeD�sertG�n�rique);
        G�n�rerCasesBiome(ref casesFor�t, BiomeForetG�n�rique);
        G�n�rerCasesBiome(ref casesGlacier, BiomeGlacierG�n�rique);

        G�n�rerRemplissage();

        ConstruireIle();

        scriptG�nRess.G�n�rerRessources();

        Instantiate(Mouton, new Vector3(150, 0, 200), Quaternion.identity).transform.parent = transform;


        //Instantiate(Mouton, new Vector3(170, 0, 300), Quaternion.identity);
        //Instantiate(Mouton, new Vector3(152, 0, 200), Quaternion.identity);
        //Instantiate(Mouton, new Vector3(153, 0, 200), Quaternion.identity);
        //Debug.Log("monton cr�er");

    }


    void G�n�rerCarteVierge()
    {
        int Indice = 0;
        for (int z = 0; z <= HauteurIle - 1; z++)
        {
            for (int x = 0; x <= LargeurIle - 1; x++)
            {
                carteIle[x, z] = new CaseCarte(x, z, Indice++);  // voir si d'autre attribut a indiquer

            }

        }
    }
    public char[,] G�n�r�CarteSimplifie()
    {
        char[,] carteSimplifier = new char[LargeurIle + 1, HauteurIle + 1];

        for (int Y = HauteurIle; Y > 0; --Y)// v�rifier si plus un a ajout�        ces un explorateur invers�
        {
            for (int X = 0; X <= HauteurIle; ++X)// doit prendre du d�but a la fin 
            {
                carteSimplifier[X, Y] = carteIle[X, Y].Symbole;
            }
        }
        return carteSimplifier;
    }



    public void ConstruireIle()
    {
        casesRemplissage = null;
        casesD�sert = null;
        casesGlacier = null;
        casesFor�t = null;

        for (int i = 0; i <= nbcase - 1; ++i)
        {
            CaseCarte case�Construire = ExplorateurDeCarte(i);
            if (case�Construire != null && case�Construire.EstVierge == false)
            {
                GameObject objet = Instantiate(case�Construire.PrefabeCase, new Vector3(case�Construire.Coordonn�X * longeurUnityCase, case�Construire.PrefabeCase.transform.position.y, case�Construire.Coordonn�Z * longeurUnityCase), Quaternion.identity);
                objet.transform.localScale = new Vector3((longeurUnityCase / 10), (longeurUnityCase / 10), (longeurUnityCase / 10));

                case�Construire.PrefabeCase.SetActive(true);
            }
        }
    }



    void G�n�rerCaseEau()
    {
        for (int i = 0; i < nbcase - 1; i++)
        {
            CaseCarte casePotentiel = ExplorateurDeCarte(i);
            if ((casePotentiel.Coordonn�X < DimmensionIle[casePotentiel.Coordonn�Z].x) || (casePotentiel.Coordonn�X > DimmensionIle[casePotentiel.Coordonn�Z].y))
            {
                casePotentiel.ModifierResouce(BiomeOc�an);
            }
        }
    }

    void G�n�rerPlage()
    {
        for (int i = 0; i < nbcase; ++i)
        {
            CaseCarte casePotentiel = ExplorateurDeCarte(i);
            if (casePotentiel.EstVierge == true && ((casePotentiel.Coordonn�X == DimmensionIle[casePotentiel.Coordonn�Z].x) || (casePotentiel.Coordonn�X == DimmensionIle[casePotentiel.Coordonn�Z].y) || (casePotentiel.Coordonn�Z == 0) || (casePotentiel.Coordonn�Z == HauteurIle - 1)))
            {
                casePotentiel.ModifierResouce(BiomePlage);
                casesPlage.Add(casePotentiel);
            }
        }
    }


    void G�n�rerCasesBiome(ref List<CaseCarte> casesBiome, GameObject prefabBiome)
    {
        casesBiome = G�n�Biome(nbcase / 9);
        foreach (CaseCarte caseBiome in casesBiome) caseBiome.ModifierResouce(prefabBiome);
    }

    void G�n�rerRemplissage()
    {
        for (int i = 0; i < nbcase; ++i)
        {
            CaseCarte CasePotentiel = ExplorateurDeCarte(i);
            if (CasePotentiel.EstVierge)
            {
                // ex�cuter un remplissage al�atoire avec une banque fournie
                CasePotentiel.ModifierResouce(BiomeRemplissage);
                casesRemplissage.Add(CasePotentiel);
            }
        }
    }

    List<CaseCarte> G�n�Biome(int NbCaseMin)
    {
        List<CaseCarte> CasesBiome = new List<CaseCarte>();

        int s�curit� = 0;
        while (CasesBiome.Count <= NbCaseMin && s�curit� < nbcase / 3)
        {
            s�curit�++;
            Vector2 OrigineBiomePrimaire = G�n�rateurDOrigine();


            CasesBiome.Add(carteIle[(int)OrigineBiomePrimaire.x, (int)OrigineBiomePrimaire.y]);
            // ajout� origine dans list avant -----------------------------a faire
            CasesBiome = IdentifierCaseDansZone(CasesBiome, OrigineBiomePrimaire, RayonMinimumPetitBiome, RayonMaximumGrandBiome);

            //choisir origine secondaire
            CaseCarte CaseOrigineBiomeSecondaire = CasesBiome[Random.Range(0, CasesBiome.Count)];
            Vector2 OrigineBiomeSecondaire = new Vector2(CaseOrigineBiomeSecondaire.Coordonn�X, CaseOrigineBiomeSecondaire.Coordonn�Z);

            // construire la liste du desert complet
            CasesBiome = IdentifierCaseDansZone(CasesBiome, OrigineBiomeSecondaire, RayonMinimumMoyenBiome, RayonMaximumGrandBiome);


        }


        return CasesBiome;
    }


    CaseCarte ExplorateurDeCarte(int indice) // peut �tre optimiser            
    {

        for (int z = 0; z <= HauteurIle - 1; z++)
        {
            for (int x = 0; x <= LargeurIle - 1; x++)
            {

                if (carteIle[x, z].indiceCase == indice)
                {
                    return carteIle[x, z];  // pense a ajouter un break

                }
            }

        }
        return null;       // lever une erreur si arrive
    }

    //Random.Range(min, max);

    Vector2 G�n�rateurDOrigine()  // peut �tre optimiser
    {
        int Coordonn�X;
        int Coordonn�Y;
        bool estDisponible = true;
        Vector2 ValeurRetour = Vector2.zero;

        int s�curit� = 0;
        while (estDisponible && s�curit� < 300)
        {
            s�curit�++;
            Coordonn�Y = Random.Range(2, HauteurIle - 2);    // valeur a valider

            Coordonn�X = Random.Range((int)DimmensionIle[Coordonn�Y].x, (int)DimmensionIle[Coordonn�Y].y - 10);    // valeur a valider

            if (carteIle[Coordonn�X, Coordonn�Y].EstVierge)
                ValeurRetour = new Vector2(Coordonn�X, Coordonn�Y);

            estDisponible = !carteIle[Coordonn�X, Coordonn�Y].EstVierge;
        }

        return ValeurRetour;
    }
    List<CaseCarte> IdentifierCaseDansZone(List<CaseCarte> CaseDejaIdentifier, Vector2 OrigineDeRecher, int rayonMin, int rayonMax)
    {
        // on choisie un rayon 
        // on trouve tout les case dans le rayon choisie
        // on valide si chaque case est vierge
        // pour chaque case on valide si on l'a d�j�
        // on retourne la liste

        int rayonZone = Random.Range(rayonMin, rayonMax) / 3;


        // on vas r�crire l'algoritme selon �a : https://lexique.netmath.ca/cercle-dans-un-plan-cartesien/

        int h = 0; //(int)OrigineDeRecher.x;
        int k = 0;//(int)OrigineDeRecher.y;

        int r = rayonZone;



        for (int x = -rayonZone; x <= rayonZone; ++x)
        {
            int borneBas = (int)-(Mathf.Sqrt(Mathf.Abs((r * r) - ((x - h) * (x - h))))) + k;
            int borneHaut = (int)(Mathf.Sqrt(Mathf.Abs((r * r) - ((x - h) * (x - h))))) + k;

            for (int z = borneBas; z <= borneHaut; ++z)
            {
                if ((x + (int)OrigineDeRecher.x > 0) && (x + (int)OrigineDeRecher.x < LargeurIle - 1) && ((int)OrigineDeRecher.y + z > 0) && ((int)OrigineDeRecher.y + z < HauteurIle - 1))
                {
                    if ((carteIle[x + (int)OrigineDeRecher.x, (int)OrigineDeRecher.y + z].EstVierge == true) && !CaseDejaIdentifier.Contains(carteIle[x + (int)OrigineDeRecher.x, (int)OrigineDeRecher.y + z]))
                    {
                        CaseDejaIdentifier.Add(carteIle[x + (int)OrigineDeRecher.x, (int)OrigineDeRecher.y + z]);                                                   // fonction a finir
                    }
                }
            }
        }

        return CaseDejaIdentifier;
    }

    void G�n�rerSpwaner(CaseCarte Case�Modifier, GameObject NouveauSeed)
    {
        ListeSpwaner.Add(Case�Modifier);
        Case�Modifier.ModifierSeedDeSpawn(NouveauSeed);
    }

    void G�rerSpawner() // fini
    {
        foreach (CaseCarte Case�Spwan in ListeSpwaner)
        {

            if (Case�Spwan.probabilit�DeSpawn != 0 && (1 == Random.Range(0, Case�Spwan.probabilit�DeSpawn)))  //  a finir  g�rer le pourcentage d'apparition
            {
                // g�r�er le tot d'apparition
                //
                GameObject NouveauPnj = Case�Spwan.SeedDeSpawn;
                Instantiate(NouveauPnj, new Vector3(Case�Spwan.Coordonn�X * longeurUnityCase, 0, Case�Spwan.Coordonn�Z * longeurUnityCase), Quaternion.identity);
                ListePnjActif.Add(NouveauPnj);
            }
        }
    }

    void G�rerPnj()
    {
        foreach (GameObject Pnj in ListePnjActif)
        {
            // trois �tat possible 

            // attaque

            // d�fense

            // fuite

            // paterne al�atoire

        }
        // simplement les suprimer une fois instencier
    }

    public Vector2 G�n�rerSpawnJoueur()
    {
        CaseCarte caseSpawn = casesPlage[Random.Range(0, casesPlage.Count)];
        return new Vector2(caseSpawn.Coordonn�X * longeurUnityCase, caseSpawn.Coordonn�Z * longeurUnityCase);
    }
}

public class CaseCarte
{
    float longeurUnityCase = 30;


    public Vector2 positionUnityMilieuCase { get; set; }   // a modifier pour init
    public int Coordonn�X;  // a modifier pour init
    public int Coordonn�Z { get; set; }   // a modifier pour init
    public int Coordonn�Y { get; set; }   // a modifier pour init
    bool Estaccessible { get; set; }   // a modifier pour init
    public bool EstVierge { get; set; }   // a modifier pour init
    public int indiceCase { get; set; }   // a modifier pour init
    public char Symbole { get; set; }   // a modifier pour init

    public GameObject PrefabeCase { get; set; }



    public GameObject SeedDeSpawn = null;
    public int probabilit�DeSpawn = 0;


    public CaseCarte(int X, int Z, int Indice)
    {
        Coordonn�X = X;
        Coordonn�Z = Z;
        Coordonn�Y = 0;
        EstVierge = true;
        indiceCase = Indice;
        Symbole = 'V';

        positionUnityMilieuCase = new Vector3(Coordonn�X, Coordonn�Y, Coordonn�Z);


    }

    public void ModifierResouce(GameObject NouveauPrefabRessource)
    {

        RessourceTerrain NouvelleRessource = NouveauPrefabRessource.GetComponent<RessourceTerrain>();
        Symbole = NouvelleRessource.symbole;
        PrefabeCase = NouveauPrefabRessource;

        PrefabeCase.transform.position = new Vector3(Coordonn�X * longeurUnityCase, 0, Coordonn�Z * longeurUnityCase);
        //  PrefabeCase.SetActive(true);
        EstVierge = false;

    }

    public void ModifierSeedDeSpawn(GameObject NouveauSeed)
    {
        PNJG�n�rique ScriptNouveauPnj = NouveauSeed.GetComponentInChildren<PNJG�n�rique>();
        probabilit�DeSpawn = ScriptNouveauPnj.probbilit�Apparition;
        SeedDeSpawn = NouveauSeed;
    }


}
