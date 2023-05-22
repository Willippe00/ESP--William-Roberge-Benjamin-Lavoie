using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objet
{
    public string nom = "";
    public string famille = "Aucune"; // familles: "Inventaire", "Consommables", "Outils & Armes", "Ressources", "Stations"
    public string usage = "Aucun"; // usages: "Aucun", "Consommable", "Outil","Arme", "Arc", "Placer"
    public int id = -1; //num�ro identificateur de l'objet
    public float poid = 1;
    public float d�gats = 0;
    public float chance = 0;
    public float port�e = 1;
    public float �tendue = 50;
    public string effet; //"faim", "soif", "vie", "autre"
    public float quantit�Effet = 0;
    public float puissance = 0;

    public int emplacement;

    public Sprite image;

    #region Contructeurs


    //Main
    public Objet(string nom)
    {
        this.nom = nom;
        id = -1;
        d�gats = 5;
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
    public Objet(string nom, string famille, string usage, float poid, float d�gats, float chance)
    {
        this.nom = nom;
        this.poid = poid;
        this.famille = famille;
        this.usage = usage;
        this.d�gats = d�gats;
        this.chance = chance;
    }

    //Armes
    public Objet(string nom, string famille, string usage, float poid, float d�gats, float chance, float port�e, float �tendue)
    {
        this.nom = nom;
        this.poid = poid;
        this.famille = famille;
        this.usage = usage;
        this.d�gats = d�gats;
        this.chance = chance;
        this.port�e = port�e;
        this.�tendue = �tendue;
    }

    //Arc
    public Objet(string nom, string famille, string usage, float poid, float d�gats)
    {
        this.nom = nom;
        this.poid = poid;
        this.famille = famille;
        this.usage = usage;
        this.d�gats = d�gats;
    }

    //Consommable
    public Objet(string nom, string famille, string usage, float poid, string effet, float quantit�Effet)
    {
        this.nom = nom;
        this.poid = poid;
        this.famille = famille;
        this.usage = usage;
        this.effet = effet;
        this.quantit�Effet = quantit�Effet;
    }

    public Objet(Objet objet) //Pour copier l'objet
    {
        nom = objet.nom;
        id = objet.id;
        poid = objet.poid;
        famille = objet.famille;
        usage = objet.usage;
        d�gats = objet.d�gats;
        effet = objet.effet;
        quantit�Effet = objet.quantit�Effet;

        image = objet.image;
    }
    #endregion


}
