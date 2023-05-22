using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FonctionRayCast : MonoBehaviour
{

    public static GameObject DoubleRayCast(Transform transformJoueur, LayerMask layer, float port�e, float distanceCentre)
    {
        RaycastHit hit;

        Vector3 distanceRayon = transformJoueur.right * (transformJoueur.localScale.x * distanceCentre); //distance entre chaque rayon calculer avec la taille du gameobject
        Vector3 origine = transformJoueur.position - distanceRayon; //calculer en fonction du nombre de rayon (impaire rayon au centre)

        Debug.DrawRay(origine, transformJoueur.forward * port�e, Color.yellow);
        Debug.DrawRay(origine + distanceRayon * 2, transformJoueur.forward * port�e, Color.yellow);

        if (!Physics.Raycast(origine, transformJoueur.forward, out hit, port�e, layer))
        {
            Physics.Raycast(origine + distanceRayon * 2, transformJoueur.forward, out hit, port�e, layer);
        }
        return hit.collider ? hit.collider.gameObject: null;
    }
    public static GameObject DoubleRayCast(Transform transformJoueur, LayerMask layer, float port�e, float distanceCentre, out float distance)
    {
        RaycastHit hit;

        Vector3 distanceRayon = transformJoueur.right * (transformJoueur.localScale.x / distanceCentre); //distance entre chaque rayon calculer avec la taille du gameobject
        Vector3 origine = new Vector3(transformJoueur.position.x, 0, transformJoueur.position.z) - distanceRayon; //calculer en fonction du nombre de rayon (impaire rayon au centre)

        Debug.DrawRay(origine, transformJoueur.forward * port�e, Color.yellow);
        Debug.DrawRay(origine + distanceRayon * 2, transformJoueur.forward * port�e, Color.yellow);

        if (!Physics.Raycast(origine, transformJoueur.forward, out hit, port�e, layer))
        {
            Physics.Raycast(origine + distanceRayon * 2, transformJoueur.forward, out hit, port�e, layer);
        }
        distance = hit.distance;
        return hit.collider ? hit.collider.gameObject : null;
    }
    public static GameObject VisionRayCast(Transform transformJoueur, LayerMask layer, float port�e, float �tendue)
    {
        Collider collision = null;
        RaycastHit hit;

        float fr�quence = 0.1f; // rayon par degr�
        float d�part = 90 - �tendue / 2; //D�but de la distribution des rayons
        float nbRayon = Mathf.Ceil(�tendue * fr�quence) + 1;
        float �cart = �tendue / nbRayon;

        for (int i = 0; i < nbRayon; i++)
        {
            float rotation = 90 - (d�part + �cart * i);
            Vector3 direction = Vector3.RotateTowards(transformJoueur.forward, Mathf.Sign(rotation) * transformJoueur.right, Mathf.Deg2Rad * Mathf.Abs(rotation), 100).normalized * port�e / (1 + Mathf.Abs(rotation / (10 * nbRayon))); //Cr�er le vecteur de direction du rayon, il se base sur le vecteur foward et tourne vers les cot�s selon la diff�rence de l'angle avec le vecteur foward. R�duit sa longeur lorsque l'angle approche des extr�mit�s
            if (Physics.Raycast(new Vector3(transformJoueur.position.x, 0, transformJoueur.position.z), direction, out hit, port�e, layer))
            {
                port�e = hit.distance;
                collision = hit.collider;
            }
            Debug.DrawRay(new Vector3(transformJoueur.position.x, 0, transformJoueur.position.z), direction, Color.blue);
        }

        return collision ? collision.gameObject : null;
    }



}
