using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionJoueur : MonoBehaviour
{
    RayCastInfo raycastInfo;
    CollisionInfo collisionInfo;
    [SerializeField] BoxCollider boiteColl;
    [SerializeField] Joueur joueur;

    [SerializeField] LayerMask layerCollision;

    const float épaisseurMarge = 0.05f;
    float nbRayParMètre = 5;

    #region CALCULS_INITIAUX

    public void Start()
    {
        CalculNbRay();
        CalculEspaceRay();
    }

    void CalculNbRay() //calcul le nombre de rayon (ray) à distribuer le long de la surface de la base du joueur
    {
        raycastInfo.nbRay.x = Mathf.Max(boiteColl.bounds.size.z * nbRayParMètre, 2);
        raycastInfo.nbRay.y = 0;
        raycastInfo.nbRay.z = Mathf.Max(boiteColl.bounds.size.x * nbRayParMètre, 2);
    }

    void CalculEspaceRay()//Calcul l'espace exacte entre chaque rayon distribuer sur la base
    {
        Bounds bordure = boiteColl.bounds;
        bordure.Expand(épaisseurMarge * -2); //On part les rayons un peu à l'intérieur du personnage pour détecter les collisions lorsque l'objet rapide rentre dans le personnage légèrement dans une frame

        raycastInfo.espaceRay.x = bordure.size.z / (raycastInfo.nbRay.x - 1);
        raycastInfo.espaceRay.z = bordure.size.x / (raycastInfo.nbRay.z - 1);

    }

    #endregion

    #region CALCULS_UPDATE
    void CalculOrigineRay() //Sert à mettre à jour à chaque frame les coins de la base de la boite de collision
    {
        Bounds bordure = boiteColl.bounds;
        bordure.Expand(épaisseurMarge * -2); //On part les rayons un peu à l'intérieur du personnage pour détecter les collisions lorsque l'objet rapide rentre dans le personnage légèrement dans une frame

        raycastInfo.basGauche = new Vector3(bordure.min.x, bordure.min.y, bordure.max.z);
        raycastInfo.basDroit = new Vector3(bordure.max.x, bordure.min.y, bordure.max.z);
        raycastInfo.hautGauche = new Vector3(bordure.min.x, bordure.min.y, bordure.min.z);
        raycastInfo.hautDroite = new Vector3(bordure.max.x, bordure.min.y, bordure.min.z);
    }

    public Vector3 CalculCollision(Vector3 v)
    {
        v *= Time.deltaTime;
        collisionInfo.Rafraichir();
        CalculOrigineRay();

        if (v.x != 0)
        {
            CalculCollisionX(ref v);
        }
        if (v.z != 0)
        {
            CalculCollisionZ(ref v);
        }
        return v;
    }

    void CalculCollisionX(ref Vector3 v) //Collisions gauche et droite
    {
        float sens = Mathf.Sign(v.x);
        float longueurRay = Mathf.Abs(v.x) + épaisseurMarge;

        for (int i = 0; i < raycastInfo.nbRay.x; i++)
        {
            Vector3 origineRay = (sens == -1) ? raycastInfo.basGauche : raycastInfo.basDroit; //détermine le début des rayons selon le sens
            origineRay += Vector3.back * (raycastInfo.espaceRay.x * i);
            RaycastHit hit;
            
            if (Physics.Raycast(origineRay, Vector3.right * sens, out hit, longueurRay, layerCollision))
            {
                v.x = (hit.distance - épaisseurMarge) * sens;
                longueurRay = hit.distance; //raccourcis la longueur pour éviter d'appliquer des collisions à des objets plus et loin.
                if (sens == -1) collisionInfo.gauche = true; else  collisionInfo.droite = true;
                joueur.v.x = 0;
            }

            Debug.DrawRay(origineRay, Vector3.right * sens * longueurRay, Color.red); //montre visuellement dans l'éditeur les rayons
        }

    }

    void CalculCollisionZ(ref Vector3 v) //Collisions avant et arrière
    {
        float sens = Mathf.Sign(v.z);
        float longueurRay = Mathf.Abs(v.z) + épaisseurMarge;

        for (int i = 0; i < raycastInfo.nbRay.z; i++)
        {
            Vector3 origineRay = (sens == -1) ? raycastInfo.hautGauche : raycastInfo.basGauche; //détermine le début des rayons selon le sens
            origineRay += Vector3.right * (raycastInfo.espaceRay.z * i + v.x); // " + v.x " pour prendre en compte le coin lors que v.x est plus grand que la distance entre le collider et le coin
            RaycastHit hit;

            if (Physics.Raycast(origineRay, Vector3.forward * sens, out hit, longueurRay, layerCollision))
            {
                v.z = (hit.distance - épaisseurMarge) * sens;
                longueurRay = hit.distance; //raccourcis la longueur pour éviter d'appliquer des collisions à des objets plus et loin.
                if (sens == -1) collisionInfo.arrière = true; else collisionInfo.avant = true;
                joueur.v.z = 0;
            }

            Debug.DrawRay(origineRay, Vector3.forward * sens * longueurRay, Color.red); //montre visuellement dans l'éditeur les rayons
        }
    }

    #endregion

    public struct RayCastInfo
    {
        public Vector3 hautGauche, hautDroite, basGauche, basDroit; //Ces variables représente les coins de la base de la boite de collision, vue de haut (bas -> face / haut -> arrière)

        public Vector3 nbRay;
        public Vector3 espaceRay;
    }

    public struct CollisionInfo
    {
        public bool droite, gauche, arrière, avant;

        public void Rafraichir()
        {
            droite =  gauche = arrière = avant = false;
        }
    }

}
