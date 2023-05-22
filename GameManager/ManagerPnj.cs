using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPnj : MonoBehaviour
{
    [SerializeField] Transform parentAnimaux;
    [SerializeField] CréationIle ile;
    [SerializeField] GestionInventaire gstInv;


    List<GameObject> ListPnjActif = new List<GameObject>();
    Queue<GameObject> oiseaux = new();
    List<ConteneurApparition> ApparitonJournée = new List<ConteneurApparition>();
    public Dictionary<string, int> nombrePnj = new Dictionary<string, int>();

    List<(int, int, GameObject)> TypesDePnj = new List<(int, int, GameObject)>();

    int maximumParTypeDEPnj = 80;

    ManagerHeure gestionnaireHeure;


    float heureJeu;

    #region pnjÀApparaitre

    static int nbMoutonMin = 3;
    static int nbMoutonMax = 5;

    [SerializeField] GameObject Mouton;

    [SerializeField] int maxOiseau = 20;
    float fréquencePertePlume = 300;
    float t = 0;

    [SerializeField] GameObject prefabOiseau;


    #endregion

    void Start()
    {
        gestionnaireHeure = GetComponentInParent<ManagerHeure>();
        

        initialiseListeTypePnj();

        ApparitionJournaliere();

        fréquencePertePlume /= maxOiseau;
        ApparaitreOiseaux();
    }

    private void Update()
    {
        GérerRessourceÉchappé();
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

    void GérerRessourceÉchappé()
    {
        t += Time.deltaTime;
        if (t >= fréquencePertePlume)
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


    public void VérifierApparition(float heureJeu)
    {

        foreach (ConteneurApparition apparition in ApparitonJournée)
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
        Instantiate(pnjPotentiel.objectApparition, pnjPotentiel.coodonéAppariton, Quaternion.identity, parentAnimaux);
        nombrePnj[pnjPotentiel.objectApparition.name]++;
    }



    public void ApparitionJournaliere()
    {
        ApparitonJournée.Clear();

        foreach (var Type in TypesDePnj)
        {
            int nbDePnj = Random.Range(Type.Item1, Type.Item2);

            for (int i = 0; i <= nbDePnj; ++i)
            {
                ApparitonJournée.Add(new ConteneurApparition(Type.Item3, DéterminerPointApparition(), 4, ile.tailleCase));
            }
        }
    }

    (int, int) DéterminerPointApparition() // peut être mieux écrit
    {
        Vector2 coodonée = ile.suivitBiomes[1][Random.Range(0, ile.suivitBiomes[1].Count)];
        int X = (int)coodonée.x;
        int Y = (int)coodonée.y;

        return (X, Y);
    }

   public void ApparitonDébutJeu()
    {
        for(int i = 0; i< 15; ++i)
        {
            (int, int) pointApparition = DéterminerPointApparition();
            Instantiate(Mouton,new Vector3(pointApparition.Item1*ile.tailleCase,0,pointApparition.Item2*ile.tailleCase) , Quaternion.identity, parentAnimaux);
        }

    }

}

public class ConteneurApparition // modifier les critère de protection
{
    (int, int) pointApparition;
    public Vector3 coodonéAppariton;
    public GameObject objectApparition;
    public int heureApparition;
    bool NonApparue;

    public ConteneurApparition(GameObject ObjectApparition, (int, int) PointApparition, int HeureApparition, float tailleCase)
    {
        objectApparition = ObjectApparition;

        pointApparition = PointApparition;

        coodonéAppariton = DéterminerCoordonné(PointApparition, tailleCase);

        heureApparition = HeureApparition;


    }

    Vector3 DéterminerCoordonné((int, int) PointApparition, float tailleCase) => new Vector3(pointApparition.Item1 * tailleCase, 0, pointApparition.Item2 * tailleCase);

}
