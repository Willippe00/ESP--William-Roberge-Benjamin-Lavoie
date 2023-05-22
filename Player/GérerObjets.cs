using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GérerObjets : MonoBehaviour
{
    GameManager gameManager;
    GestionInventaire scriptGestInv;

    [SerializeField] float distanceDétection = 1;
    [SerializeField] float distanceRécupération = .1f;

    private void OnEnable()
    {
        gameManager = gameObject.GetComponentInParent<GameManager>();
        scriptGestInv = gameObject.GetComponentInParent<GestionInventaire>();
    }

    public void DétecterObjetSol()
    {
        if (gameManager.listeObjetSol.Count != 0)
        {
            for(int i = 0; i < gameManager.listeObjetSol.Count; i++)
            {
                GameObject objet = gameManager.listeObjetSol[i];

                float distance = Vector3.Distance(transform.position, objet.transform.position);
                if (distance <= distanceDétection && objet.transform.position.y < 1)
                {
                    if (distance <= distanceRécupération)
                    {
                        RécupererObjet(objet);
                    }
                    else objet.GetComponent<ComportementObjet>().Attirer(transform, distance);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanceDétection);
    }

    private void RécupererObjet(GameObject objet)
    {
        gameManager.listeObjetSol.Remove(objet);
        scriptGestInv.RécupererObjet(objet);
    }
}
