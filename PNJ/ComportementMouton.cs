using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementMouton : MonoBehaviour
{
    Cr�ationIle ile;

    GameObject ennemie;
    Joueur ComportementEnemmie;

    ManagerPnj GestionDesPnj;

    string nomObjet = "Mouton";

    int rayonDeD�placement = 2;
    float tempsD�placement;
    int nbDepasFait = 0;
    float distanceDeVue = 5;

    int NiveauDePuissance = 1;

    Vector3 d�placement;
    float vitesse;


    delegate void Action�tat();
    Action�tat[] Actions�tat { get; set; }
    public enum �tatPnj { cr�ation, D�placement, Attaque, Fuite, Mort, Nb�tatsPnj }
    enum TransitionPnj { Activation, EnMarche, Termin� };
    bool EstVivant { get; set; }


    �tatPnj �tatActuel;
    TransitionPnj Transition�tat;

    public readonly int probbilit�Apparition = 4;
    private int pointVie = 50;
    int pointD�gat = 2;
    int distanceAttaque = 8;
    (int, int) positionActuel;
    (int, int) Destination;

    // Start is called before the first frame update
    private void Start()
    {
        ile = GetComponentInParent<Cr�ationIle>();
        GestionDesPnj = GetComponentInParent<ManagerPnj>();

        EstVivant = false;
        InitialiserActions�tat();

        �tatActuel = �tatPnj.cr�ation;
        Transition�tat = TransitionPnj.Activation;
    }
    private void InitialiserActions�tat()
    {
        Actions�tat = new Action�tat[(int)�tatPnj.Nb�tatsPnj];
        Actions�tat[(int)�tatPnj.cr�ation] = G�rercr�ation;
        Actions�tat[(int)�tatPnj.D�placement] = G�rer�tatD�placement;
        Actions�tat[(int)�tatPnj.Attaque] = G�rerAttaque;
        Actions�tat[(int)�tatPnj.Fuite] = G�rerFuite;
        Actions�tat[(int)�tatPnj.Mort] = G�rerMort;
    }

    // Update is called once per frame
    void Update() => Actions�tat[(int)�tatActuel]();

    (int, int) D�terminerPositionActuel()
    {
        int x = (int)(transform.position.x / ile.tailleCase);
        int y = (int)(transform.position.z / ile.tailleCase);
        return (x, y);
    }


    private void G�rercr�ation()
    {

        switch (Transition�tat)
        {
            case TransitionPnj.Activation:

                Transition�tat = TransitionPnj.EnMarche;
                break;
            case TransitionPnj.EnMarche:
                positionActuel = D�terminerPositionActuel();

                Transition�tat = TransitionPnj.Termin�;
                break;
            case TransitionPnj.Termin�:
                EstVivant = true;
                Debug.Log("on a cr�er le mouton");
                �tatActuel = �tatPnj.D�placement;
                Transition�tat = TransitionPnj.Activation;

                break;
        }
    }

    public float ObtenirVitesse()
    {
        if (�tatActuel == �tatPnj.cr�ation || �tatActuel == �tatPnj.Mort)
            return 0;
        else
            return vitesse;
    }

    bool Arriv��Destination()
    {
        (int, int) Actuel = D�terminerPositionActuel();
        int positionenX = Actuel.Item1;
        int positionenY = Actuel.Item2;

        if (positionenX - Destination.Item1 < 1 && positionenY - Destination.Item2 < 1)
            return true;

        else
            return false;
    }

    private void G�rer�tatD�placement()
    {
        switch (Transition�tat)
        {
            case TransitionPnj.Activation:
                tempsD�placement = 0;
                positionActuel = D�terminerPositionActuel();


                G�n�rerDestination();

                Transition�tat = TransitionPnj.EnMarche;
                tempsD�placement = Random.Range(3, 4.5f);

                d�placement = new Vector3(Destination.Item1 - positionActuel.Item1, 0, Destination.Item2 - positionActuel.Item2);
                vitesse = d�placement.magnitude / tempsD�placement;
                break;

            case TransitionPnj.EnMarche:

                if (tempsD�placement > 10)
                    Transition�tat = TransitionPnj.Activation;

                tempsD�placement += Time.deltaTime;

                if (Physics.Raycast(transform.position, GetComponentsInChildren<Transform>()[1].TransformDirection(Vector3.forward), distanceDeVue))
                    Transition�tat = TransitionPnj.Activation;

                if (!Arriv��Destination())
                {

                    transform.Translate(d�placement * vitesse * Time.deltaTime);

                    GetComponentsInChildren<Transform>()[1].LookAt(transform.position + d�placement);
                    break;
                }

                else
                {
                    Transition�tat = TransitionPnj.Activation;
                    break;
                }
        }
    }

    private object GetComponentsInChildrens<T>()
    {
        throw new System.NotImplementedException();
    }

    void G�rerAttaque()
    {
        switch (Transition�tat)
        {
            case TransitionPnj.Activation:

                positionActuel = D�terminerPositionActuel();
                Destination = ((int)ennemie.transform.position.x, (int)ennemie.transform.position.z);

                d�placement = new Vector3(Destination.Item1 - positionActuel.Item1, 0, Destination.Item2 - positionActuel.Item2);

                Transition�tat = TransitionPnj.EnMarche;
                Debug.Log("on vas attaquer l� l�");
                break;

            case TransitionPnj.EnMarche:

                if (tempsD�placement > 10)
                    Transition�tat = TransitionPnj.Activation;

                tempsD�placement += Time.deltaTime;
                int vitesse = 5;

                if (!Arriv��Destination())
                {
                    transform.Translate(d�placement * vitesse * Time.deltaTime);

                    GetComponentsInChildren<Transform>()[1].LookAt(transform.position + d�placement);

                    Debug.Log(" je bouje vers ma prois");
                    break;
                }

                else
                {
                    positionActuel = D�terminerPositionActuel();

                    if (EstEnnemieAttaquable())
                    {
                        if (ComportementEnemmie.infoJoueur.vie > 0)
                        {

                            Debug.Log("tien toi mon sale");
                            ComportementEnemmie.GetComponent<GestionFacteursVitaux>().AjouterSurBarre("BarreVie", -5);

                            Transition�tat = TransitionPnj.Activation;
                        }
                        else
                        {
                            // Transition�tat = TransitionPnj.Termin�;

                        }
                    }

                    break;
                }

            case TransitionPnj.Termin�:
                Debug.Log("finito le gros");
                �tatActuel = �tatPnj.D�placement;
                Transition�tat = TransitionPnj.Activation;
                break;
        }
    }

    bool EstEnnemieAttaquable()
    {
        Vector3 distance = new Vector3(this.transform.position.x - ennemie.transform.position.x, 0, this.transform.position.z - ennemie.transform.position.z);
        return (distance.magnitude < 15);


    }
    void G�n�rerDestination()
    {
        int compteurIt�ration = 0;
        do
        {
            if (compteurIt�ration > 40)
            {
                Destroy(gameObject);
                break;
            }

            compteurIt�ration++;
            nbDepasFait = 0;
            Destination = (positionActuel.Item1 + Random.Range(-rayonDeD�placement, rayonDeD�placement + 1), positionActuel.Item2 + Random.Range(-rayonDeD�placement, rayonDeD�placement + 1));
        }
        while (ile.carte[Destination.Item1, Destination.Item2] == 'O');
    }

    void D�terminerPointDeFuite()
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
    private void G�rerFuite()
    {
        switch (Transition�tat)
        {
            case TransitionPnj.Activation:
                tempsD�placement = 1;

                positionActuel = D�terminerPositionActuel();
                D�terminerPointDeFuite();

                d�placement = new Vector3(Destination.Item1 - positionActuel.Item1, 0, Destination.Item2 - positionActuel.Item2);
                vitesse = d�placement.magnitude / tempsD�placement;
                Debug.Log("on Fuit");
                Transition�tat = TransitionPnj.EnMarche;
                break;
            case TransitionPnj.EnMarche:

                if (tempsD�placement > 10)
                    Transition�tat = TransitionPnj.Activation;

                tempsD�placement += Time.deltaTime;

                if (Physics.Raycast(transform.position, GetComponentsInChildren<Transform>()[1].TransformDirection(Vector3.forward), distanceDeVue))
                    Transition�tat = TransitionPnj.Activation;

                if (!Arriv��Destination())
                {
                    transform.Translate(d�placement * vitesse * Time.deltaTime);

                    GetComponentsInChildren<Transform>()[1].LookAt(transform.position + d�placement);
                    break;
                }

                else
                {
                    if ((Mathf.Abs(new Vector3(Destination.Item1 - ennemie.transform.position.x, 0, Destination.Item2 - ennemie.transform.position.z).magnitude) > 10))
                    {
                        �tatActuel = �tatPnj.D�placement;

                    }
                    Transition�tat = TransitionPnj.Activation;
                    break;
                }
        }
    }
    private void G�rerMort()
    {

        switch (Transition�tat)
        {
            case TransitionPnj.Activation:

                Transition�tat = TransitionPnj.EnMarche;
                break;
            case TransitionPnj.EnMarche:

                Debug.Log("Au non je meur");

                GetComponent<Ressource>().LacherRessource();

                Destroy(gameObject);
                Debug.Log("fini du mouton");
                break;
        }
    }
    public void R�ceptionAttaque(GameObject assayant, int puissanceAssayant, int pointDeD�gat)
    {
        ennemie = assayant;

        Debug.Log("ouch j'ai mal ");
        Debug.Log(pointVie.ToString());
        Transition�tat = TransitionPnj.Activation;


        G�rerVie(-pointDeD�gat);

        ComportementEnemmie = ennemie.GetComponent<Joueur>();

        if (�tatActuel != �tatPnj.Mort)
        {
            if (NiveauDePuissance > puissanceAssayant)
            {
                Debug.Log("on attaque");
                �tatActuel = �tatPnj.Attaque;
            }
            else
            {
                Debug.Log("on fuit");
                �tatActuel = �tatPnj.Fuite;
            }
        }
    }

    void G�rerVie(int modificationVie)
    {
        pointVie += modificationVie;
        if (pointVie <= 0)
            �tatActuel = �tatPnj.Mort;
    }
}
