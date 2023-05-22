using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public Objet objet;
    public bool interactible = true;
    public bool placer = false;


    public void Interaction(GestionInventaire scriptGestInv)
    {
        if (interactible && placer)
        {
            switch (objet.nom)
            {
                case "CraftTable_1":
                    scriptGestInv.ActiverInventaire(2);
                    break;
                case "CraftTable_2":
                    scriptGestInv.ActiverInventaire(3);
                    break;
            }
        }
    }

    public void DétruireStation()
    {
        transform.parent.GetComponent<GestionInventaire>().ApparaitreObjet(objet.id, new Vector3(transform.position.x, 0, transform.position.z), false, 1);
        transform.parent.GetComponent<GameManager>().listeStationPlacer.Remove(gameObject);
    }
}
