using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPnj : MonoBehaviour
{
    [SerializeField] Transform parentAnimaux;
    [SerializeField] Cr�ationIle ile;
    [SerializeField] GestionInventaire gstInv;


    List<GameObject> ListPnjActif = new List<GameObject>();
    Queue<GameObject> oiseaux = new();
    List<ConteneurApparition> ApparitonJourn�e = new List<ConteneurApparition>();
    public Dictionary<string, int> nombrePnj = new Dictionary<string, int>();

    List<(int, int, GameObject)> TypesDePnj = new List<(int, int, GameObject)>();

    int maximumParTypeDEPnj = 80;

    ManagerHeure gestionnaireHeure;


    float heureJeu;

    #region pnj�Apparaitre

    static int nbMoutonMin = 3;
    static int nbMoutonMax = 5;

    [SerializeField] GameObject Mouton;

    [SerializeField] int maxOiseau = 20;
    float fr�quencePertePlume = 300;
    float t = 0;

    [SerializeField] GameObject prefabOiseau;


    #endregion

    void Start()
    {
        gestionnaireHeure = GetComponentInParent<ManagerHeure>();
        

        initialiseListeTypePnj();

        ApparitionJournaliere();

        fr�quencePertePlume /= maxOiseau;
        ApparaitreOiseaux();
    }

    private void Update()
    {
        G�rerRessource�chapp�();
    }



    #region Oiseau
    void ApparaitreOiseaux()
    {
        for (int i = 0; i < maxOiseau / 5; i++)
        {
            Vector2 pos = ile.suivitBiomes[2][Random.Range(0, ile.suivitBiomes[2].Count)];
            for (int n = 0; n < 5; n++) oiseaux.Enqueue(Instantiate(prefabOiseau, new Vector3(pos.x, prefabOiseau.transform.position.y, pos.y), prefabOiseau.transform.rotation, parentAnimaux));
        }
    }

    void G�rerRessource�chapp�()
    {
        t += Time.deltaTime;
        if (t >= fr�quencePertePlume)
        {
            t = 0;
            gstInv.ApparaitreObjet(73, oiseaux.Peek().transform.position, false, 1);
            oiseaux.Enqueue(oiseaux.Dequeue());
        }
    }

    #endregion

    void initialiseListeTypePnj()
    {
        TypesDePnj.Add((nbMoutonMin, nbMoutonMax, Mouton));


        nombrePnj.Add(Mouton.name, 0);
    }


    public void V�rifierApparition(float heureJeu)
    {

        foreach (ConteneurApparition apparition in ApparitonJourn�e)
        {
            if (apparition.heureApparition == heureJeu && nombrePnj[apparition.objectApparition.name] < 20)
            {
                faireApparaitreMouton(apparition);
            }
        }
    }

    void faireApparaitreMouton(ConteneurApparition pnjPotentiel)
    {
        Debug.Log("on fait apparaitre LALA");
        Instantiate(pnjPotentiel.objectApparition, pnjPotentiel.coodon�Appariton, Quaternion.identity, parentAnimaux);
        nombrePnj[pnjPotentiel.objectApparition.name]++;
    }



    public void ApparitionJournaliere()
    {
        ApparitonJourn�e.Clear();

        foreach (var Type in TypesDePnj)
        {
            int nbDePnj = Random.Range(Type.Item1, Type.Item2);

            for (int i = 0; i <= nbDePnj; ++i)
            {
                ApparitonJourn�e.Add(new ConteneurApparition(Type.Item3, D�terminerPointApparition(), 4, ile.tailleCase));
            }
        }
    }

    (int, int) D�terminerPointApparition() // peut �tre mieux �crit
    {
        Vector2 coodon�e = ile.suivitBiomes[1][Random.Range(0, ile.suivitBiomes[1].Count)];
        int X = (int)coodon�e.x;
        int Y = (int)coodon�e.y;

        return (X, Y);
    }

   public void ApparitonD�butJeu()
    {
        for(int i = 0; i< 15; ++i)
        {
            (int, int) pointApparition = D�terminerPointApparition();
            Instantiate(Mouton,new Vector3(pointApparition.Item1*ile.tailleCase,0,pointApparition.Item2*ile.tailleCase) , Quaternion.identity, parentAnimaux);
        }

    }

}

public class ConteneurApparition // modifier les crit�re de protection
{
    (int, int) pointApparition;
    public Vector3 coodon�Appariton;
    public GameObject objectApparition;
    public int heureApparition;
    bool NonApparue;

    public ConteneurApparition(GameObject ObjectApparition, (int, int) PointApparition, int HeureApparition, float tailleCase)
    {
        objectApparition = ObjectApparition;

        pointApparition = PointApparition;

        coodon�Appariton = D�terminerCoordonn�(PointApparition, tailleCase);

        heureApparition = HeureApparition;


    }

    Vector3 D�terminerCoordonn�((int, int) PointApparition, float tailleCase) => new Vector3(pointApparition.Item1 * tailleCase, 0, pointApparition.Item2 * tailleCase);

}
