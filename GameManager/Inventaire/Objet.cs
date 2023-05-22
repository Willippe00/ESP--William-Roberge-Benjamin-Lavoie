using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objet
{
    public string nom = "";
    public string famille = "Aucune"; // familles: "Inventaire", "Consommables", "Outils & Armes", "Ressources", "Stations"
    public string usage = "Aucun"; // usages: "Aucun", "Consommable", "Outil","Arme", "Arc", "Placer"
    public int id = -1; //numéro identificateur de l'objet
    public float poid = 1;
    public float dégats = 0;
    public float chance = 0;
    public float portée = 1;
    public float étendue = 50;
    public string effet; //"faim", "soif", "vie", "autre"
    public float quantitéEffet = 0;
    public float puissance = 0;

    public int emplacement;

    public Sprite image;

    #region Contructeurs


    //Main
    public Objet(string nom)
    {
        this.nom = nom;
        id = -1;
        dégats = 5;
    }

    //Ressource
    public Objet(string nom, string famille, string usage, float poid)
    {
        this.nom = nom;
        this.poid = poid;
        this.famille = famille;
        this.usage = usage;
    }

    //Outils
    public Objet(string nom, string famille, string usage, float poid, float dégats, float chance)
    {
        this.nom = nom;
        this.poid = poid;
        this.famille = famille;
        this.usage = usage;
        this.dégats = dégats;
        this.chance = chance;
    }

    //Armes
    public Objet(string nom, string famille, string usage, float poid, float dégats, float chance, float portée, float étendue)
    {
        this.nom = nom;
        this.poid = poid;
        this.famille = famille;
        this.usage = usage;
        this.dégats = dégats;
        this.chance = chance;
        this.portée = portée;
        this.étendue = étendue;
    }

    //Arc
    public Objet(string nom, string famille, string usage, float poid, float dégats)
    {
        this.nom = nom;
        this.poid = poid;
        this.famille = famille;
        this.usage = usage;
        this.dégats = dégats;
    }

    //Consommable
    public Objet(string nom, string famille, string usage, float poid, string effet, float quantitéEffet)
    {
        this.nom = nom;
        this.poid = poid;
        this.famille = famille;
        this.usage = usage;
        this.effet = effet;
        this.quantitéEffet = quantitéEffet;
    }

    public Objet(Objet objet) //Pour copier l'objet
    {
        nom = objet.nom;
        id = objet.id;
        poid = objet.poid;
        famille = objet.famille;
        usage = objet.usage;
        dégats = objet.dégats;
        effet = objet.effet;
        quantitéEffet = objet.quantitéEffet;

        image = objet.image;
    }
    #endregion


}
