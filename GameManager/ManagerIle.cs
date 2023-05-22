using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerIle : MonoBehaviour
{

    [SerializeField] GameObject TitreJeu;
    [SerializeField] GameObject Soleil;
    [SerializeField] GameObject Lune;
    [SerializeField] GénérationRessource scriptGénRess;

    [SerializeField] GameObject Mouton;




    public const int HauteurIle = 120; // niveau de sécurité a corriger


    public const int LargeurIle = 120; // niveau de sécurité a corriger


    // Vector2 vecteurGrandeurGéné = new Vector2(2, 23);

    Vector2[] DimmensionIle;


    //new Vector2[HauteurIle+1] { new Vector2(2, LargeurIle-2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2), new Vector2(2, LargeurIle - 2) };  // tous des valeur fictive

    public CaseCarte[,] carteIle = new CaseCarte[LargeurIle, HauteurIle];

    public int nbcase = HauteurIle * LargeurIle;


    public int Nbanimaux;

    public float NiveauIle = 0; //Détermine le niveau on est généré l'île





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

    #region PréfabBiome
    [SerializeField] private GameObject BiomePlage;
    [SerializeField] private GameObject BiomeOcéan;
    [SerializeField] private GameObject BiomeDésertGénérique;
    [SerializeField] private GameObject BiomeForetGénérique;
    [SerializeField] private GameObject BiomeGlacierGénérique;
    [SerializeField] private GameObject BiomeRemplissage;

    #endregion


    #region Composente du désert


    public List<CaseCarte> casesDésert;

    #endregion

    #region Composant Forêt

    public List<CaseCarte> casesForêt;
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

    enum PériodeJourné { Matin, ApresMidi, Nuit }
    float Heure = 14;


    private void Awake()
    {
        GénérerIle();  // test de démo
    }
    // Update is called once per frame
    void Update()
    {
        //géréHeure();

        // géréPnj();
        //GérerPnj();
    }

    public void géréHeure()
    {
        // HeureJeu += Time.deltaTime*100;      // voir si facteur est bon

        if (HeureJeu <= 24)
        {
            if (HeureJeu > 6 && HeureJeu < 20)
            {
                Debug.Log("on est le jour");
                HeureJeu += Time.deltaTime;
                // on vas gérér la position du soleil
                //on vas gérér le temps de la journé
                // le soleil décrit une rotation au dessu du plan 180 degré réparte 3/4 ensoleiller 1/4 nuit    1jour jeu = 15min irl
            }
            else
            {
                Debug.Log("on est le soir");
                HeureJeu += Time.deltaTime;
            }

            Debug.Log(HeureJeu.ToString());
            GérerPositionSoleil(HeureJeu);
        }
        else
        {
            //Instantiate(Mouton, new Vector3(150, 0, 200), Quaternion.identity);
            HeureJeu = 0;
            // GérerSpawner(); // on fait spanner les pnj 1 fois par jour
        }
    }

    public void GérerPositionSoleil(float heure) // pour amélioré https://docs.unity3d.com/ScriptReference/Transform-rotation.html
    {
        //ligne pour éviter un erreur si le soleil est manquant
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

    public static Vector2[] GénéréBordureIle()
    {
        Vector2[] bordure = new Vector2[HauteurIle + 1];

        int sécurité = 0;
        do
        {
            sécurité++;

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
        } while (vérifierGrosseurIle(bordure) && sécurité < 20);

        return bordure;
    }

    static bool vérifierGrosseurIle(Vector2[] bordure)
    {
        int grosseur = 0;
        for (int i = 0; i < bordure.Length; ++i)
        {
            grosseur += (int)bordure[i].y - (int)bordure[i].x;
        }
        return grosseur > ((HauteurIle * LargeurIle) / 1.2);
    }

    public void GénérerIle()
    {
        /*
         a intégrer :

        - zone de blage pocédural 1 ou deux cube en bordure de l'océant
        - un point d'origine de la jungle pour ensuite l'étendre
        - un point d'arigine du désert
        - combler le reste avec environment fourtout


         */

        GénérerCarteVierge();



        DimmensionIle = GénéréBordureIle();

        GénérerCaseEau();

        GénérerPlage();

        GénérerCasesBiome(ref casesDésert, BiomeDésertGénérique);
        GénérerCasesBiome(ref casesForêt, BiomeForetGénérique);
        GénérerCasesBiome(ref casesGlacier, BiomeGlacierGénérique);

        GénérerRemplissage();

        ConstruireIle();

        scriptGénRess.GénérerRessources();

        Instantiate(Mouton, new Vector3(150, 0, 200), Quaternion.identity).transform.parent = transform;


        //Instantiate(Mouton, new Vector3(170, 0, 300), Quaternion.identity);
        //Instantiate(Mouton, new Vector3(152, 0, 200), Quaternion.identity);
        //Instantiate(Mouton, new Vector3(153, 0, 200), Quaternion.identity);
        //Debug.Log("monton créer");

    }


    void GénérerCarteVierge()
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
    public char[,] GénéréCarteSimplifie()
    {
        char[,] carteSimplifier = new char[LargeurIle + 1, HauteurIle + 1];

        for (int Y = HauteurIle; Y > 0; --Y)// vérifier si plus un a ajouté        ces un explorateur inversé
        {
            for (int X = 0; X <= HauteurIle; ++X)// doit prendre du début a la fin 
            {
                carteSimplifier[X, Y] = carteIle[X, Y].Symbole;
            }
        }
        return carteSimplifier;
    }



    public void ConstruireIle()
    {
        casesRemplissage = null;
        casesDésert = null;
        casesGlacier = null;
        casesForêt = null;

        for (int i = 0; i <= nbcase - 1; ++i)
        {
            CaseCarte caseÀConstruire = ExplorateurDeCarte(i);
            if (caseÀConstruire != null && caseÀConstruire.EstVierge == false)
            {
                GameObject objet = Instantiate(caseÀConstruire.PrefabeCase, new Vector3(caseÀConstruire.CoordonnéX * longeurUnityCase, caseÀConstruire.PrefabeCase.transform.position.y, caseÀConstruire.CoordonnéZ * longeurUnityCase), Quaternion.identity);
                objet.transform.localScale = new Vector3((longeurUnityCase / 10), (longeurUnityCase / 10), (longeurUnityCase / 10));

                caseÀConstruire.PrefabeCase.SetActive(true);
            }
        }
    }



    void GénérerCaseEau()
    {
        for (int i = 0; i < nbcase - 1; i++)
        {
            CaseCarte casePotentiel = ExplorateurDeCarte(i);
            if ((casePotentiel.CoordonnéX < DimmensionIle[casePotentiel.CoordonnéZ].x) || (casePotentiel.CoordonnéX > DimmensionIle[casePotentiel.CoordonnéZ].y))
            {
                casePotentiel.ModifierResouce(BiomeOcéan);
            }
        }
    }

    void GénérerPlage()
    {
        for (int i = 0; i < nbcase; ++i)
        {
            CaseCarte casePotentiel = ExplorateurDeCarte(i);
            if (casePotentiel.EstVierge == true && ((casePotentiel.CoordonnéX == DimmensionIle[casePotentiel.CoordonnéZ].x) || (casePotentiel.CoordonnéX == DimmensionIle[casePotentiel.CoordonnéZ].y) || (casePotentiel.CoordonnéZ == 0) || (casePotentiel.CoordonnéZ == HauteurIle - 1)))
            {
                casePotentiel.ModifierResouce(BiomePlage);
                casesPlage.Add(casePotentiel);
            }
        }
    }


    void GénérerCasesBiome(ref List<CaseCarte> casesBiome, GameObject prefabBiome)
    {
        casesBiome = GénéBiome(nbcase / 9);
        foreach (CaseCarte caseBiome in casesBiome) caseBiome.ModifierResouce(prefabBiome);
    }

    void GénérerRemplissage()
    {
        for (int i = 0; i < nbcase; ++i)
        {
            CaseCarte CasePotentiel = ExplorateurDeCarte(i);
            if (CasePotentiel.EstVierge)
            {
                // exécuter un remplissage aléatoire avec une banque fournie
                CasePotentiel.ModifierResouce(BiomeRemplissage);
                casesRemplissage.Add(CasePotentiel);
            }
        }
    }

    List<CaseCarte> GénéBiome(int NbCaseMin)
    {
        List<CaseCarte> CasesBiome = new List<CaseCarte>();

        int sécurité = 0;
        while (CasesBiome.Count <= NbCaseMin && sécurité < nbcase / 3)
        {
            sécurité++;
            Vector2 OrigineBiomePrimaire = GénérateurDOrigine();


            CasesBiome.Add(carteIle[(int)OrigineBiomePrimaire.x, (int)OrigineBiomePrimaire.y]);
            // ajouté origine dans list avant -----------------------------a faire
            CasesBiome = IdentifierCaseDansZone(CasesBiome, OrigineBiomePrimaire, RayonMinimumPetitBiome, RayonMaximumGrandBiome);

            //choisir origine secondaire
            CaseCarte CaseOrigineBiomeSecondaire = CasesBiome[Random.Range(0, CasesBiome.Count)];
            Vector2 OrigineBiomeSecondaire = new Vector2(CaseOrigineBiomeSecondaire.CoordonnéX, CaseOrigineBiomeSecondaire.CoordonnéZ);

            // construire la liste du desert complet
            CasesBiome = IdentifierCaseDansZone(CasesBiome, OrigineBiomeSecondaire, RayonMinimumMoyenBiome, RayonMaximumGrandBiome);


        }


        return CasesBiome;
    }


    CaseCarte ExplorateurDeCarte(int indice) // peut être optimiser            
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

    Vector2 GénérateurDOrigine()  // peut être optimiser
    {
        int CoordonnéX;
        int CoordonnéY;
        bool estDisponible = true;
        Vector2 ValeurRetour = Vector2.zero;

        int sécurité = 0;
        while (estDisponible && sécurité < 300)
        {
            sécurité++;
            CoordonnéY = Random.Range(2, HauteurIle - 2);    // valeur a valider

            CoordonnéX = Random.Range((int)DimmensionIle[CoordonnéY].x, (int)DimmensionIle[CoordonnéY].y - 10);    // valeur a valider

            if (carteIle[CoordonnéX, CoordonnéY].EstVierge)
                ValeurRetour = new Vector2(CoordonnéX, CoordonnéY);

            estDisponible = !carteIle[CoordonnéX, CoordonnéY].EstVierge;
        }

        return ValeurRetour;
    }
    List<CaseCarte> IdentifierCaseDansZone(List<CaseCarte> CaseDejaIdentifier, Vector2 OrigineDeRecher, int rayonMin, int rayonMax)
    {
        // on choisie un rayon 
        // on trouve tout les case dans le rayon choisie
        // on valide si chaque case est vierge
        // pour chaque case on valide si on l'a déjà
        // on retourne la liste

        int rayonZone = Random.Range(rayonMin, rayonMax) / 3;


        // on vas récrire l'algoritme selon ça : https://lexique.netmath.ca/cercle-dans-un-plan-cartesien/

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

    void GénérerSpwaner(CaseCarte CaseÀModifier, GameObject NouveauSeed)
    {
        ListeSpwaner.Add(CaseÀModifier);
        CaseÀModifier.ModifierSeedDeSpawn(NouveauSeed);
    }

    void GérerSpawner() // fini
    {
        foreach (CaseCarte CaseÀSpwan in ListeSpwaner)
        {

            if (CaseÀSpwan.probabilitéDeSpawn != 0 && (1 == Random.Range(0, CaseÀSpwan.probabilitéDeSpawn)))  //  a finir  gérer le pourcentage d'apparition
            {
                // géréer le tot d'apparition
                //
                GameObject NouveauPnj = CaseÀSpwan.SeedDeSpawn;
                Instantiate(NouveauPnj, new Vector3(CaseÀSpwan.CoordonnéX * longeurUnityCase, 0, CaseÀSpwan.CoordonnéZ * longeurUnityCase), Quaternion.identity);
                ListePnjActif.Add(NouveauPnj);
            }
        }
    }

    void GérerPnj()
    {
        foreach (GameObject Pnj in ListePnjActif)
        {
            // trois état possible 

            // attaque

            // défense

            // fuite

            // paterne aléatoire

        }
        // simplement les suprimer une fois instencier
    }

    public Vector2 GénérerSpawnJoueur()
    {
        CaseCarte caseSpawn = casesPlage[Random.Range(0, casesPlage.Count)];
        return new Vector2(caseSpawn.CoordonnéX * longeurUnityCase, caseSpawn.CoordonnéZ * longeurUnityCase);
    }
}

public class CaseCarte
{
    float longeurUnityCase = 30;


    public Vector2 positionUnityMilieuCase { get; set; }   // a modifier pour init
    public int CoordonnéX;  // a modifier pour init
    public int CoordonnéZ { get; set; }   // a modifier pour init
    public int CoordonnéY { get; set; }   // a modifier pour init
    bool Estaccessible { get; set; }   // a modifier pour init
    public bool EstVierge { get; set; }   // a modifier pour init
    public int indiceCase { get; set; }   // a modifier pour init
    public char Symbole { get; set; }   // a modifier pour init

    public GameObject PrefabeCase { get; set; }



    public GameObject SeedDeSpawn = null;
    public int probabilitéDeSpawn = 0;


    public CaseCarte(int X, int Z, int Indice)
    {
        CoordonnéX = X;
        CoordonnéZ = Z;
        CoordonnéY = 0;
        EstVierge = true;
        indiceCase = Indice;
        Symbole = 'V';

        positionUnityMilieuCase = new Vector3(CoordonnéX, CoordonnéY, CoordonnéZ);


    }

    public void ModifierResouce(GameObject NouveauPrefabRessource)
    {

        RessourceTerrain NouvelleRessource = NouveauPrefabRessource.GetComponent<RessourceTerrain>();
        Symbole = NouvelleRessource.symbole;
        PrefabeCase = NouveauPrefabRessource;

        PrefabeCase.transform.position = new Vector3(CoordonnéX * longeurUnityCase, 0, CoordonnéZ * longeurUnityCase);
        //  PrefabeCase.SetActive(true);
        EstVierge = false;

    }

    public void ModifierSeedDeSpawn(GameObject NouveauSeed)
    {
        PNJGénérique ScriptNouveauPnj = NouveauSeed.GetComponentInChildren<PNJGénérique>();
        probabilitéDeSpawn = ScriptNouveauPnj.probbilitéApparition;
        SeedDeSpawn = NouveauSeed;
    }


}
