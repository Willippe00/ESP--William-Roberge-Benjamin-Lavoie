using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementMouton : MonoBehaviour
{
    CrÈationIle ile;

    GameObject ennemie;
    Joueur ComportementEnemmie;

    ManagerPnj GestionDesPnj;

    string nomObjet = "Mouton";

    int rayonDeDÈplacement = 2;
    float tempsDÈplacement;
    int nbDepasFait = 0;
    float distanceDeVue = 5;

    int NiveauDePuissance = 1;

    Vector3 dÈplacement;
    float vitesse;


    delegate void Action…tat();
    Action…tat[] Actions…tat { get; set; }
    public enum …tatPnj { crÈation, DÈplacement, Attaque, Fuite, Mort, Nb…tatsPnj }
    enum TransitionPnj { Activation, EnMarche, TerminÈ };
    bool EstVivant { get; set; }


    …tatPnj …tatActuel;
    TransitionPnj Transition…tat;

    public readonly int probbilitÈApparition = 4;
    private int pointVie = 50;
    int pointDÈgat = 2;
    int distanceAttaque = 8;
    (int, int) positionActuel;
    (int, int) Destination;

    // Start is called before the first frame update
    private void Start()
    {
        ile = GetComponentInParent<CrÈationIle>();
        GestionDesPnj = GetComponentInParent<ManagerPnj>();

        EstVivant = false;
        InitialiserActions…tat();

        …tatActuel = …tatPnj.crÈation;
        Transition…tat = TransitionPnj.Activation;
    }
    private void InitialiserActions…tat()
    {
        Actions…tat = new Action…tat[(int)…tatPnj.Nb…tatsPnj];
        Actions…tat[(int)…tatPnj.crÈation] = GÈrercrÈation;
        Actions…tat[(int)…tatPnj.DÈplacement] = GÈrer…tatDÈplacement;
        Actions…tat[(int)…tatPnj.Attaque] = GÈrerAttaque;
        Actions…tat[(int)…tatPnj.Fuite] = GÈrerFuite;
        Actions…tat[(int)…tatPnj.Mort] = GÈrerMort;
    }

    // Update is called once per frame
    void Update() => Actions…tat[(int)…tatActuel]();

    (int, int) DÈterminerPositionActuel()
    {
        int x = (int)(transform.position.x / ile.tailleCase);
        int y = (int)(transform.position.z / ile.tailleCase);
        return (x, y);
    }


    private void GÈrercrÈation()
    {

        switch (Transition…tat)
        {
            case TransitionPnj.Activation:

                Transition…tat = TransitionPnj.EnMarche;
                break;
            case TransitionPnj.EnMarche:
                positionActuel = DÈterminerPositionActuel();

                Transition…tat = TransitionPnj.TerminÈ;
                break;
            case TransitionPnj.TerminÈ:
                EstVivant = true;
                Debug.Log("on a crÈer le mouton");
                …tatActuel = …tatPnj.DÈplacement;
                Transition…tat = TransitionPnj.Activation;

                break;
        }
    }

    public float ObtenirVitesse()
    {
        if (…tatActuel == …tatPnj.crÈation || …tatActuel == …tatPnj.Mort)
            return 0;
        else
            return vitesse;
    }

    bool ArrivÈ¿Destination()
    {
        (int, int) Actuel = DÈterminerPositionActuel();
        int positionenX = Actuel.Item1;
        int positionenY = Actuel.Item2;

        if (positionenX - Destination.Item1 < 1 && positionenY - Destination.Item2 < 1)
            return true;

        else
            return false;
    }

    private void GÈrer…tatDÈplacement()
    {
        switch (Transition…tat)
        {
            case TransitionPnj.Activation:
                tempsDÈplacement = 0;
                positionActuel = DÈterminerPositionActuel();


                GÈnÈrerDestination();

                Transition…tat = TransitionPnj.EnMarche;
                tempsDÈplacement = Random.Range(3, 4.5f);

                dÈplacement = new Vector3(Destination.Item1 - positionActuel.Item1, 0, Destination.Item2 - positionActuel.Item2);
                vitesse = dÈplacement.magnitude / tempsDÈplacement;
                break;

            case TransitionPnj.EnMarche:

                if (tempsDÈplacement > 10)
                    Transition…tat = TransitionPnj.Activation;

                tempsDÈplacement += Time.deltaTime;

                if (Physics.Raycast(transform.position, GetComponentsInChildren<Transform>()[1].TransformDirection(Vector3.forward), distanceDeVue))
                    Transition…tat = TransitionPnj.Activation;

                if (!ArrivÈ¿Destination())
                {

                    transform.Translate(dÈplacement * vitesse * Time.deltaTime);

                    GetComponentsInChildren<Transform>()[1].LookAt(transform.position + dÈplacement);
                    break;
                }

                else
                {
                    Transition…tat = TransitionPnj.Activation;
                    break;
                }
        }
    }

    private object GetComponentsInChildrens<T>()
    {
        throw new System.NotImplementedException();
    }

    void GÈrerAttaque()
    {
        switch (Transition…tat)
        {
            case TransitionPnj.Activation:

                positionActuel = DÈterminerPositionActuel();
                Destination = ((int)ennemie.transform.position.x, (int)ennemie.transform.position.z);

                dÈplacement = new Vector3(Destination.Item1 - positionActuel.Item1, 0, Destination.Item2 - positionActuel.Item2);

                Transition…tat = TransitionPnj.EnMarche;
                Debug.Log("on vas attaquer l‡ l‡");
                break;

            case TransitionPnj.EnMarche:

                if (tempsDÈplacement > 10)
                    Transition…tat = TransitionPnj.Activation;

                tempsDÈplacement += Time.deltaTime;
                int vitesse = 5;

                if (!ArrivÈ¿Destination())
                {
                    transform.Translate(dÈplacement * vitesse * Time.deltaTime);

                    GetComponentsInChildren<Transform>()[1].LookAt(transform.position + dÈplacement);

                    Debug.Log(" je bouje vers ma prois");
                    break;
                }

                else
                {
                    positionActuel = DÈterminerPositionActuel();

                    if (EstEnnemieAttaquable())
                    {
                        if (ComportementEnemmie.infoJoueur.vie > 0)
                        {

                            Debug.Log("tien toi mon sale");
                            ComportementEnemmie.GetComponent<GestionFacteursVitaux>().AjouterSurBarre("BarreVie", -5);

                            Transition…tat = TransitionPnj.Activation;
                        }
                        else
                        {
                            // Transition…tat = TransitionPnj.TerminÈ;

                        }
                    }

                    break;
                }

            case TransitionPnj.TerminÈ:
                Debug.Log("finito le gros");
                …tatActuel = …tatPnj.DÈplacement;
                Transition…tat = TransitionPnj.Activation;
                break;
        }
    }

    bool EstEnnemieAttaquable()
    {
        Vector3 distance = new Vector3(this.transform.position.x - ennemie.transform.position.x, 0, this.transform.position.z - ennemie.transform.position.z);
        return (distance.magnitude < 15);


    }
    void GÈnÈrerDestination()
    {
        int compteurItÈration = 0;
        do
        {
            if (compteurItÈration > 40)
            {
                Destroy(gameObject);
                break;
            }

            compteurItÈration++;
            nbDepasFait = 0;
            Destination = (positionActuel.Item1 + Random.Range(-rayonDeDÈplacement, rayonDeDÈplacement + 1), positionActuel.Item2 + Random.Range(-rayonDeDÈplacement, rayonDeDÈplacement + 1));
        }
        while (ile.carte[Destination.Item1, Destination.Item2] == 'O');
    }

    void DÈterminerPointDeFuite()
    {
        do
        {
            (int, int) positioEnemmie = ((int)(ennemie.transform.position.x / ile.tailleCase), (int)(ennemie.transform.position.z / ile.tailleCase));

            int sensDeplacementHorizontale = 1;
            int sensDeplacementVertical = 1;

            if (positioEnemmie.Item1 > positionActuel.Item1)
                sensDeplacementHorizontale = -1;

            if (positioEnemmie.Item1 > positionActuel.Item1)
                sensDeplacementVertical = -1;


            Destination.Item1 = positionActuel.Item1 + Random.Range(1, 2) * sensDeplacementHorizontale;
            Destination.Item2 = positionActuel.Item2 + Random.Range(1, 2) * sensDeplacementVertical;

        }
        while (ile.carte[Destination.Item1, Destination.Item2] == 'O');
    }
    private void GÈrerFuite()
    {
        switch (Transition…tat)
        {
            case TransitionPnj.Activation:
                tempsDÈplacement = 1;

                positionActuel = DÈterminerPositionActuel();
                DÈterminerPointDeFuite();

                dÈplacement = new Vector3(Destination.Item1 - positionActuel.Item1, 0, Destination.Item2 - positionActuel.Item2);
                vitesse = dÈplacement.magnitude / tempsDÈplacement;
                Debug.Log("on Fuit");
                Transition…tat = TransitionPnj.EnMarche;
                break;
            case TransitionPnj.EnMarche:

                if (tempsDÈplacement > 10)
                    Transition…tat = TransitionPnj.Activation;

                tempsDÈplacement += Time.deltaTime;

                if (Physics.Raycast(transform.position, GetComponentsInChildren<Transform>()[1].TransformDirection(Vector3.forward), distanceDeVue))
                    Transition…tat = TransitionPnj.Activation;

                if (!ArrivÈ¿Destination())
                {
                    transform.Translate(dÈplacement * vitesse * Time.deltaTime);

                    GetComponentsInChildren<Transform>()[1].LookAt(transform.position + dÈplacement);
                    break;
                }

                else
                {
                    if ((Mathf.Abs(new Vector3(Destination.Item1 - ennemie.transform.position.x, 0, Destination.Item2 - ennemie.transform.position.z).magnitude) > 10))
                    {
                        …tatActuel = …tatPnj.DÈplacement;

                    }
                    Transition…tat = TransitionPnj.Activation;
                    break;
                }
        }
    }
    private void GÈrerMort()
    {

        switch (Transition…tat)
        {
            case TransitionPnj.Activation:

                Transition…tat = TransitionPnj.EnMarche;
                break;
            case TransitionPnj.EnMarche:

                Debug.Log("Au non je meur");

                GetComponent<Ressource>().LacherRessource();

                Destroy(gameObject);
                Debug.Log("fini du mouton");
                break;
        }
    }
    public void RÈceptionAttaque(GameObject assayant, int puissanceAssayant, int pointDeDÈgat)
    {
        ennemie = assayant;

        Debug.Log("ouch j'ai mal ");
        Debug.Log(pointVie.ToString());
        Transition…tat = TransitionPnj.Activation;


        GÈrerVie(-pointDeDÈgat);

        ComportementEnemmie = ennemie.GetComponent<Joueur>();

        if (…tatActuel != …tatPnj.Mort)
        {
            if (NiveauDePuissance > puissanceAssayant)
            {
                Debug.Log("on attaque");
                …tatActuel = …tatPnj.Attaque;
            }
            else
            {
                Debug.Log("on fuit");
                …tatActuel = …tatPnj.Fuite;
            }
        }
    }

    void GÈrerVie(int modificationVie)
    {
        pointVie += modificationVie;
        if (pointVie <= 0)
            …tatActuel = …tatPnj.Mort;
    }
}
