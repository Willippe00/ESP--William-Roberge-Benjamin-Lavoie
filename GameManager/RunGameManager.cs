using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// William Roberge 2022-02-03

// base on GameScreenManager de Vincent Echelard pfi session A2021         ------------ v�rifier pour plagia
public class RunGameManager : MonoBehaviour
{
    enum �tatJeu { TitreJeu,G�n�rationIle, Cin�matiqueIntro,exploration ,Crafting, FastFowardNuit,Pause, FinJeu, Nb�tatsJeu };
    enum TransitionJeu { Activation, EnMarche, Termin� };
    delegate void Action�tat();

    [SerializeField]
    GameObject TitreJeu;

    [SerializeField]
    float TempsApparitionMessage;

    Action�tat[] Actions�tat { get; set; }
    �tatJeu �tatActuel { get; set; }
    TransitionJeu Transition�tat { get; set; }
    

    float Temps�coul� { get; set; }

    char[,] Carte { get; set; }
    

    readonly Vector3 SpawnPoint = new Vector3(0, 0, 0);

    [SerializeField] ManagerIle gestionIle;
    
    
    bool CommandePersonnageEnable { get; set; }

    float HeurePourCin�matique;

    void Awake()
    {
        
        InitialiserActions�tat();
        
        �tatActuel = �tatJeu.TitreJeu;
        Transition�tat = TransitionJeu.Activation;
        //TitreJeu.SetActive(false);
        TitreJeu.SetActive(false);
        CommandePersonnageEnable = false;
    }

    private void InitialiserActions�tat()
    {
        Actions�tat = new Action�tat[(int)�tatJeu.Nb�tatsJeu];
        Actions�tat[(int)�tatJeu.TitreJeu] = G�rer�tatTitreJeu;
        Actions�tat[(int)�tatJeu.G�n�rationIle] = G�rerG�n�rationIle;
        Actions�tat[(int)�tatJeu.Cin�matiqueIntro] = G�rerCin�matiqueIntro;
        Actions�tat[(int)�tatJeu.exploration] = G�rerExploration;
        Actions�tat[(int)�tatJeu.Crafting] = G�rerCrafting;
        Actions�tat[(int)�tatJeu.FastFowardNuit] = G�rerFastFowardNuit;
        Actions�tat[(int)�tatJeu.Pause] = G�rerMenuPause;
        Actions�tat[(int)�tatJeu.FinJeu]= G�rerFinjeu;

    }

    void Update() => Actions�tat[(int)�tatActuel]();

    private void G�rer�tatTitreJeu()
    {
        switch (Transition�tat)
        {
            case TransitionJeu.Activation:
                Temps�coul� = 0;
                TitreJeu.SetActive(true);
                Transition�tat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                Temps�coul� += Time.deltaTime;
                Transition�tat = Temps�coul� < TempsApparitionMessage ? TransitionJeu.EnMarche : TransitionJeu.Termin�;
                break;
            case TransitionJeu.Termin�:
                TitreJeu.SetActive(false);
                �tatActuel = �tatJeu.G�n�rationIle;
                Transition�tat = TransitionJeu.Activation;
                break;
        }
    }

    private void G�rerG�n�rationIle()
    {
        switch (Transition�tat)
        {
            case TransitionJeu.Activation:
                Temps�coul� = 0;
                  // A coder
                Transition�tat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                Carte = new char[/* a ajuster avec les vrai grandeur de la carde*/8,8];//-------------------------------------------------------------------------------------
                // ManagerIle gestionIle = GameObject.Find("GlobalGameManager").GetComponent<ManagerIle>();     d�placer dans attribut
                //gestionIle.G�n�rerIle();
                Carte = gestionIle.G�n�r�CarteSimplifie();
                //gestionIle.ConstruireIle();

                Transition�tat = TransitionJeu.Termin�;

                /*Temps�coul� += Time.deltaTime;
                Transition�tat = Temps�coul� < TempsApparitionMessage ? TransitionJeu.EnMarche : TransitionJeu.Termin�;*/
                break;
            case TransitionJeu.Termin�:
                �tatActuel = �tatJeu.Cin�matiqueIntro;
                Transition�tat = TransitionJeu.Activation;
                break;
        }
    }

    private void G�rerCin�matiqueIntro()
    {
        switch (Transition�tat)
        {
            case TransitionJeu.Activation:
                HeurePourCin�matique = 14;
               // gestionIle.G�rerPositionSoleil(HeurePourCin�matique);
                // initialiser postion cam�ra aussi 
                Transition�tat = TransitionJeu.EnMarche;
                break;

            case TransitionJeu.EnMarche:
                
                // coder un 360 de l'ile en meintant rotation vers le centre et z constent r�partie sur 30 seconde
                Transition�tat = TransitionJeu.Termin�;
                break;

            case TransitionJeu.Termin�:
                �tatActuel = �tatJeu.exploration;
                Transition�tat = TransitionJeu.Activation;
                break;
        }
    }

    private void G�rerExploration()                                           // le gros Jambon-----------------------------------------------------------------------------------
    {
        switch (Transition�tat)
        {
            case TransitionJeu.Activation:
                HeurePourCin�matique = 14;
               // gestionIle.G�rerPositionSoleil(HeurePourCin�matique);
                // initialiser postion cam�ra aussi 
                Transition�tat = TransitionJeu.EnMarche;
                break;

            case TransitionJeu.EnMarche:
               // gestionIle.G�rerPositionSoleil(HeurePourCin�matique);
                Temps�coul� += Time.deltaTime;
                // doit int�gr� systh�me pour update d'heure
                /*
                if (EscadrilleEnnemi.Count < NbEnnemis && Temps�coul� > DeltaCr�ationEnnemi)                                    
                {
                    Cr�erEnnemi();
                    Temps�coul� = 0;
                }
                Transition�tat = NbEnnemisEnPosition < NbEnnemis ? TransitionJeu.EnMarche : TransitionJeu.Termin�;             // a modifier doit faire un switch case pour le crafting, le sommeille fast foward pause et fin de niveau
                */

                // ajout� systh�me de spown pr�iodique des png
                // ajout� reg�n�rment des ressource exploit�
                break;

            case TransitionJeu.Termin�:
                �tatActuel = �tatJeu.FinJeu;
                Transition�tat = TransitionJeu.Activation;
                break;


        }
    }

    private void G�rerCrafting()   // frame construit mais a d�velloper grandement
    {

        switch (Transition�tat)
        {
            case TransitionJeu.Activation:
                CommandePersonnageEnable = false;
                Transition�tat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                // a construire
                break;
            case TransitionJeu.Termin�:
                CommandePersonnageEnable = true;
                �tatActuel = �tatJeu.exploration;
                Transition�tat = TransitionJeu.Activation;
                break;

        }
    }

    private void G�rerFastFowardNuit()                          // frame construit mais a d�velloper grandement
    {
        switch (Transition�tat)
        {
            case TransitionJeu.Activation:
                CommandePersonnageEnable = false;
                Transition�tat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                // a construire
                break;


            case TransitionJeu.Termin�:
                CommandePersonnageEnable = true;
                �tatActuel = �tatJeu.exploration;
                Transition�tat = TransitionJeu.Activation;
                break;
        }
    }

    private void G�rerMenuPause()// un peut plus complex Stay stand by 
    {
        switch (Transition�tat)
        {
            case TransitionJeu.Activation:
                CommandePersonnageEnable = false;
                Transition�tat = TransitionJeu.EnMarche;
                break;

        }
    }

    private void G�rerFinjeu()
    {

        // compl�tment a cot�
    }
}
