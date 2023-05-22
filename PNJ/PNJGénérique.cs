using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PNJGénérique
{
    delegate void ActionÉtat();
    ActionÉtat[] ActionsÉtat { get; set; }
    public enum ÉtatPnj { création, Déplacement, Attaque, Fuite, Mort, NbÉtatsPnj }
    enum TransitionPnj { Activation, EnMarche, Terminé };
    bool EstVivant { get; set; }




    ÉtatPnj ÉtatActuel { get; set; }
    TransitionPnj TransitionÉtat { get; set; }

    int probbilitéApparition { get; set; }
    int pointVie { get; set; }
    int pointDégat { get; set; }
    (int, int) positionActuel { get; set; }
    (int, int) Destination { get; set; }

    void awake()
    {

        InitialiserActionsÉtat();

        ÉtatActuel = ÉtatPnj.création;
        TransitionÉtat = TransitionPnj.Activation;

    }

    private void InitialiserActionsÉtat()
    {
        ActionsÉtat = new ActionÉtat[(int)ÉtatPnj.NbÉtatsPnj];
        ActionsÉtat[(int)ÉtatPnj.création] = Gérercréation;
        ActionsÉtat[(int)ÉtatPnj.Déplacement] = GérerÉtatDéplacment;
        ActionsÉtat[(int)ÉtatPnj.Attaque] = GérerAttaque;
        ActionsÉtat[(int)ÉtatPnj.Fuite] = GérerFuite;
        ActionsÉtat[(int)ÉtatPnj.Mort] = GérerMort;


    }



    void Update() => ActionsÉtat[(int)ÉtatActuel]();


    private void Gérercréation()   // frame construit mais a dévelloper grandement
    {
/*
        switch (TransitionÉtat)
        {
            case TransitionPnj.Activation:
                CommandePersonnageEnable = false;
                TransitionÉtat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                EstVivant = true;
                // on set les attribut de point de vie point, point de dégat
                break;
            case TransitionJeu.Terminé:
                CommandePersonnageEnable = true;
                ÉtatActuel = ÉtatJeu.exploration;
                TransitionÉtat = TransitionJeu.Activation;
                break;

        }
*/
    }

    private void GérerÉtatDéplacment()   // frame construit mais a dévelloper grandement
    {
/*
        switch (TransitionÉtat)
        {
            case TransitionPnj.Activation:
                CommandePersonnageEnable = false;
                TransitionÉtat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                // on set les attribut de point de vie point, point de dégat
                break;
            case TransitionJeu.Terminé:
                CommandePersonnageEnable = true;
                ÉtatActuel = ÉtatJeu.exploration;
                TransitionÉtat = TransitionJeu.Activation;
                break;

        }
*/
    }

    private void GérerAttaque()   // frame construit mais a dévelloper grandement
    {
/*
        switch (TransitionÉtat)
        {
            case TransitionPnj.Activation:
                CommandePersonnageEnable = false;
                TransitionÉtat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                // on set les attribut de point de vie point, point de dégat
                break;
            case TransitionJeu.Terminé:
                CommandePersonnageEnable = true;
                ÉtatActuel = ÉtatJeu.exploration;
                TransitionÉtat = TransitionJeu.Activation;
                break;

        }
*/
    }

    private void GérerFuite()   // frame construit mais a dévelloper grandement
    {
        /*
        switch (TransitionÉtat)
        {
            case TransitionPnj.Activation:
                CommandePersonnageEnable = false;
                TransitionÉtat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                // on set les attribut de point de vie point, point de dégat
                break;
            case TransitionJeu.Terminé:
                CommandePersonnageEnable = true;
                ÉtatActuel = ÉtatJeu.exploration;
                TransitionÉtat = TransitionJeu.Activation;
                break;

        }
        */
    }

    private void GérerMort()   // frame construit mais a dévelloper grandement
    {
        /*
        switch (TransitionÉtat)
        {
            case TransitionPnj.Activation:
                CommandePersonnageEnable = false;
                TransitionÉtat = TransitionJeu.EnMarche;
                break;
            case TransitionJeu.EnMarche:
                // on set les attribut de point de vie point, point de dégat
                break;
            case TransitionJeu.Terminé:
                CommandePersonnageEnable = true;
                ÉtatActuel = ÉtatJeu.exploration;
                TransitionÉtat = TransitionJeu.Activation;
                break;

        }
        */
    }

    void détruirePnj()
    {

    }

    void GestionPnj()
    {


    }

    abstract (int, int) DéterminerCaseActuel();
    

}
