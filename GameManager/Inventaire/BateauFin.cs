using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BateauFin : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Camera cam�ra;

    [SerializeField] ManagerIle Ile;

    ComportementCam�ra gestionCam�ra;

    GameMasterFSM Manager;

    Vector3 directionSortie;

    float vitesse = 8;

    float tempD�placement;

    Vector3 distanceCam�ra = new Vector3(0, 11,0);

    void Start()
    {
        gestionCam�ra = cam�ra.GetComponent<ComportementCam�ra>();
        gestionCam�ra.enabled = false;

        Manager = FindObjectOfType<GameMasterFSM>();

        directionSortie = D�terminerPointSortie();

    }

    // Update is called once per frame
    void Update()
    {
        if(tempD�placement > 15)
        {
            Manager.ProchaineSc�ne();
        }

        tempD�placement += Time.deltaTime;
        transform.Translate(directionSortie * vitesse * Time.deltaTime);
        D�placerCam�ra();
    }

    Vector3 D�terminerPointSortie()
    {
        (int, int) coordon� = D�terminerPositionActuel();

        Vector3 direction;

        if (estDeLEau(coordon�.Item1 + 1, coordon�.Item2))
            {
            direction = Vector3.right;
        }


        else if (estDeLEau(coordon�.Item1 - 1, coordon�.Item2))
        {
            direction = Vector3.left;
        }

        else if(estDeLEau(coordon�.Item1, coordon�.Item2-1))
        {
            direction = Vector3.back;
        }

        else if (estDeLEau(coordon�.Item1, coordon�.Item2 + 1))
        {
            direction = Vector3.forward;
        }

        else
        {
            direction = Vector3.zero;
        }

        return direction;
    }

    (int, int) D�terminerPositionActuel()
    {
       int x = (int)(transform.position.x / Ile.longeurUnityCase);
       int y = (int)(transform.position.z / Ile.longeurUnityCase);
        return (0, 0);
    }

    bool estDeLEau(int x, int y)
    {
        return Ile.carteIle[x, y].Symbole == 'O'; // vlider le char
                                                                            //  Debug.Log(" on a trouver une sortie ");
    }

    void D�placerCam�ra()
    {
        cam�ra.transform.position = this.transform.position + distanceCam�ra;
    }
}
