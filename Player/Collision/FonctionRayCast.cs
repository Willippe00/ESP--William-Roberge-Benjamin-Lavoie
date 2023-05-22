using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FonctionRayCast : MonoBehaviour
{

    public static GameObject DoubleRayCast(Transform transformJoueur, LayerMask layer, float portée, float distanceCentre)
    {
        RaycastHit hit;

        Vector3 distanceRayon = transformJoueur.right * (transformJoueur.localScale.x * distanceCentre); //distance entre chaque rayon calculer avec la taille du gameobject
        Vector3 origine = transformJoueur.position - distanceRayon; //calculer en fonction du nombre de rayon (impaire rayon au centre)

        Debug.DrawRay(origine, transformJoueur.forward * portée, Color.yellow);
        Debug.DrawRay(origine + distanceRayon * 2, transformJoueur.forward * portée, Color.yellow);

        if (!Physics.Raycast(origine, transformJoueur.forward, out hit, portée, layer))
        {
            Physics.Raycast(origine + distanceRayon * 2, transformJoueur.forward, out hit, portée, layer);
        }
        return hit.collider ? hit.collider.gameObject: null;
    }
    public static GameObject DoubleRayCast(Transform transformJoueur, LayerMask layer, float portée, float distanceCentre, out float distance)
    {
        RaycastHit hit;

        Vector3 distanceRayon = transformJoueur.right * (transformJoueur.localScale.x / distanceCentre); //distance entre chaque rayon calculer avec la taille du gameobject
        Vector3 origine = new Vector3(transformJoueur.position.x, 0, transformJoueur.position.z) - distanceRayon; //calculer en fonction du nombre de rayon (impaire rayon au centre)

        Debug.DrawRay(origine, transformJoueur.forward * portée, Color.yellow);
        Debug.DrawRay(origine + distanceRayon * 2, transformJoueur.forward * portée, Color.yellow);

        if (!Physics.Raycast(origine, transformJoueur.forward, out hit, portée, layer))
        {
            Physics.Raycast(origine + distanceRayon * 2, transformJoueur.forward, out hit, portée, layer);
        }
        distance = hit.distance;
        return hit.collider ? hit.collider.gameObject : null;
    }
    public static GameObject VisionRayCast(Transform transformJoueur, LayerMask layer, float portée, float étendue)
    {
        Collider collision = null;
        RaycastHit hit;

        float fréquence = 0.1f; // rayon par degré
        float départ = 90 - étendue / 2; //Début de la distribution des rayons
        float nbRayon = Mathf.Ceil(étendue * fréquence) + 1;
        float écart = étendue / nbRayon;

        for (int i = 0; i < nbRayon; i++)
        {
            float rotation = 90 - (départ + écart * i);
            Vector3 direction = Vector3.RotateTowards(transformJoueur.forward, Mathf.Sign(rotation) * transformJoueur.right, Mathf.Deg2Rad * Mathf.Abs(rotation), 100).normalized * portée / (1 + Mathf.Abs(rotation / (10 * nbRayon))); //Créer le vecteur de direction du rayon, il se base sur le vecteur foward et tourne vers les cotés selon la différence de l'angle avec le vecteur foward. Réduit sa longeur lorsque l'angle approche des extrémités
            if (Physics.Raycast(new Vector3(transformJoueur.position.x, 0, transformJoueur.position.z), direction, out hit, portée, layer))
            {
                portée = hit.distance;
                collision = hit.collider;
            }
            Debug.DrawRay(new Vector3(transformJoueur.position.x, 0, transformJoueur.position.z), direction, Color.blue);
        }

        return collision ? collision.gameObject : null;
    }



}
