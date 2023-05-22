using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BateauFin : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Camera caméra;

    [SerializeField] ManagerIle Ile;

    ComportementCaméra gestionCaméra;

    GameMasterFSM Manager;

    Vector3 directionSortie;

    float vitesse = 8;

    float tempDéplacement;

    Vector3 distanceCaméra = new Vector3(0, 11,0);

    void Start()
    {
        gestionCaméra = caméra.GetComponent<ComportementCaméra>();
        gestionCaméra.enabled = false;

        Manager = FindObjectOfType<GameMasterFSM>();

        directionSortie = DéterminerPointSortie();

    }

    // Update is called once per frame
    void Update()
    {
        if(tempDéplacement > 15)
        {
            Manager.ProchaineScène();
        }

        tempDéplacement += Time.deltaTime;
        transform.Translate(directionSortie * vitesse * Time.deltaTime);
        DéplacerCaméra();
    }

    Vector3 DéterminerPointSortie()
    {
        (int, int) coordoné = DéterminerPositionActuel();

        Vector3 direction;

        if (estDeLEau(coordoné.Item1 + 1, coordoné.Item2))
            {
            direction = Vector3.right;
        }


        else if (estDeLEau(coordoné.Item1 - 1, coordoné.Item2))
        {
            direction = Vector3.left;
        }

        else if(estDeLEau(coordoné.Item1, coordoné.Item2-1))
        {
            direction = Vector3.back;
        }

        else if (estDeLEau(coordoné.Item1, coordoné.Item2 + 1))
        {
            direction = Vector3.forward;
        }

        else
        {
            direction = Vector3.zero;
        }

        return direction;
    }

    (int, int) DéterminerPositionActuel()
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

    void DéplacerCaméra()
    {
        caméra.transform.position = this.transform.position + distanceCaméra;
    }
}
