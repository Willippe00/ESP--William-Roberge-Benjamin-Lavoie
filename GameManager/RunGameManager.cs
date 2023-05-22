using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// William Roberge 2022-02-03

// base on GameScreenManager de Vincent Echelard pfi session A2021         ------------ vérifier pour plagia
public class RunGameManager : MonoBehaviour
{
    enum ÉtatJeu { TitreJeu,GénérationIle, CinématiqueIntro,exploration ,Crafting, FastFowardNuit,Pause, FinJeu, NbÉtatsJeu };
    enum TransitionJeu { Activation, EnMarche, Terminé };
    delegate void ActionÉtat();

    [SerializeField]
    GameObject TitreJeu;

    [SerializeField]
    float TempsApparitionMessage;

    ActionÉtat[] ActionsÉtat { get; set; }
    ÉtatJeu ÉtatActuel { get; set; }
    TransitionJeu TransitionÉtat { get; set; }
    

    float TempsÉcoulé { get; set; }

    char[,] Carte { get; set; }
    

    readonly Vector3 SpawnPoint = new Vector3(0, 0, 0);

    [SerializeField] ManagerIle gestionIle;
    
    
    bool CommandePersonnageEnable { get; set; }

    float HeurePourCinématique;

    void Awake()
    {
        
        InitialiserActionsÉtat();
        
        ÉtatActuel = ÉtatJeu.TitreJeu;
        TransitionÉtat = TransitionJeu.Activation;
        //TitreJeu.SetActive(false);
        TitreJeu.SetActive(false);
        CommandePersonnageEnable = false;
    }

    private void InitialiserActionsÉtat()
    {
        ActionsÉtat = new ActionÉtat[(int)ÉtatJeu.NbÉtatsJeu];
        ActionsÉtat[(int)ÉtatJeu.TitreJeu] = GérerÉtatTitreJeu;
        ActionsÉtat[(int)ÉtatJeu.GénérationIle] = GérerGénérationIle;
        ActionsÉtat[(int)ÉtatJeu.CinématiqueIntro] = GérerCinématiqueIntro;
        ActionsÉtat[(int)ÉtatJeu.exploration] = GérerExploration;
        ActionsÉtat[(int)ÉtatJeu.Crafting] = GérerCrafting;
        ActionsÉtat[(int)ÉtatJeu.FastFowardNuit] = GérerFastFowardNuit;
        ActionsÉtat[(int)ÉtatJeu.Pause] = GérerMenuPause;
        ActionsÉtat[(int)ÉtatJeu.FinJeu]= GérerFinjeu;

    }

    void Update() => ActionsÉtat[(int)ÉtatActuel]();

    private void GérerÉtatTitreJeu()
    {
        switch (TransitionÉtat)
        {
            case TransitionJeu.Activation:
                TempsÉcoulé = 0;
                TitreJeu.SetActive(true);
                TransitionÉtat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                TempsÉcoulé += Time.deltaTime;
                TransitionÉtat = TempsÉcoulé < TempsApparitionMessage ? TransitionJeu.EnMarche : TransitionJeu.Terminé;
                break;
            case TransitionJeu.Terminé:
                TitreJeu.SetActive(false);
                ÉtatActuel = ÉtatJeu.GénérationIle;
                TransitionÉtat = TransitionJeu.Activation;
                break;
        }
    }

    private void GérerGénérationIle()
    {
        switch (TransitionÉtat)
        {
            case TransitionJeu.Activation:
                TempsÉcoulé = 0;
                  // A coder
                TransitionÉtat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                Carte = new char[/* a ajuster avec les vrai grandeur de la carde*/8,8];//-------------------------------------------------------------------------------------
                // ManagerIle gestionIle = GameObject.Find("GlobalGameManager").GetComponent<ManagerIle>();     déplacer dans attribut
                //gestionIle.GénérerIle();
                Carte = gestionIle.GénéréCarteSimplifie();
                //gestionIle.ConstruireIle();

                TransitionÉtat = TransitionJeu.Terminé;

                /*TempsÉcoulé += Time.deltaTime;
                TransitionÉtat = TempsÉcoulé < TempsApparitionMessage ? TransitionJeu.EnMarche : TransitionJeu.Terminé;*/
                break;
            case TransitionJeu.Terminé:
                ÉtatActuel = ÉtatJeu.CinématiqueIntro;
                TransitionÉtat = TransitionJeu.Activation;
                break;
        }
    }

    private void GérerCinématiqueIntro()
    {
        switch (TransitionÉtat)
        {
            case TransitionJeu.Activation:
                HeurePourCinématique = 14;
               // gestionIle.GérerPositionSoleil(HeurePourCinématique);
                // initialiser postion caméra aussi 
                TransitionÉtat = TransitionJeu.EnMarche;
                break;

            case TransitionJeu.EnMarche:
                
                // coder un 360 de l'ile en meintant rotation vers le centre et z constent répartie sur 30 seconde
                TransitionÉtat = TransitionJeu.Terminé;
                break;

            case TransitionJeu.Terminé:
                ÉtatActuel = ÉtatJeu.exploration;
                TransitionÉtat = TransitionJeu.Activation;
                break;
        }
    }

    private void GérerExploration()                                           // le gros Jambon-----------------------------------------------------------------------------------
    {
        switch (TransitionÉtat)
        {
            case TransitionJeu.Activation:
                HeurePourCinématique = 14;
               // gestionIle.GérerPositionSoleil(HeurePourCinématique);
                // initialiser postion caméra aussi 
                TransitionÉtat = TransitionJeu.EnMarche;
                break;

            case TransitionJeu.EnMarche:
               // gestionIle.GérerPositionSoleil(HeurePourCinématique);
                TempsÉcoulé += Time.deltaTime;
                // doit intégré systhème pour update d'heure
                /*
                if (EscadrilleEnnemi.Count < NbEnnemis && TempsÉcoulé > DeltaCréationEnnemi)                                    
                {
                    CréerEnnemi();
                    TempsÉcoulé = 0;
                }
                TransitionÉtat = NbEnnemisEnPosition < NbEnnemis ? TransitionJeu.EnMarche : TransitionJeu.Terminé;             // a modifier doit faire un switch case pour le crafting, le sommeille fast foward pause et fin de niveau
                */

                // ajouté systhème de spown préiodique des png
                // ajouté regénérment des ressource exploité
                break;

            case TransitionJeu.Terminé:
                ÉtatActuel = ÉtatJeu.FinJeu;
                TransitionÉtat = TransitionJeu.Activation;
                break;


        }
    }

    private void GérerCrafting()   // frame construit mais a dévelloper grandement
    {

        switch (TransitionÉtat)
        {
            case TransitionJeu.Activation:
                CommandePersonnageEnable = false;
                TransitionÉtat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                // a construire
                break;
            case TransitionJeu.Terminé:
                CommandePersonnageEnable = true;
                ÉtatActuel = ÉtatJeu.exploration;
                TransitionÉtat = TransitionJeu.Activation;
                break;

        }
    }

    private void GérerFastFowardNuit()                          // frame construit mais a dévelloper grandement
    {
        switch (TransitionÉtat)
        {
            case TransitionJeu.Activation:
                CommandePersonnageEnable = false;
                TransitionÉtat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                // a construire
                break;


            case TransitionJeu.Terminé:
                CommandePersonnageEnable = true;
                ÉtatActuel = ÉtatJeu.exploration;
                TransitionÉtat = TransitionJeu.Activation;
                break;
        }
    }

    private void GérerMenuPause()// un peut plus complex Stay stand by 
    {
        switch (TransitionÉtat)
        {
            case TransitionJeu.Activation:
                CommandePersonnageEnable = false;
                TransitionÉtat = TransitionJeu.EnMarche;
                break;

        }
    }

    private void GérerFinjeu()
    {

        // complètment a coté
    }
}
