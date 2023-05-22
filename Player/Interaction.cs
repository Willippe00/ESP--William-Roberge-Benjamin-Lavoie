using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Interaction : MonoBehaviour
{
    [SerializeField] Joueur joueur;
    [SerializeField] LayerMask layerOutils;
    [SerializeField] LayerMask layerArmes;
    [SerializeField] BanqueObjet banqueObjet;
    [SerializeField] GestionAnimation scriptGestAnim;
    [SerializeField] GestionFacteursVitaux scriptGestFacteursVitaux;
    [SerializeField] GestionInventaire scriptGestInv;
    [SerializeField] GestionStations scriptGestStation;

    [SerializeField] GameObject prefabArc;
    [SerializeField] GameObject prefab�p�e;
    [SerializeField] GameObject prefabHache;
    [SerializeField] GameObject prefabPioche;
    [SerializeField] GameObject prefabLance;

    [SerializeField] GameObject prefabFl�che;

    [SerializeField] float d�gatsMain = 5;

    public void ToucheInt�ragir()
    {
        // usages: "Aucun", "Consommable", "Outil", "Arme", "Arc", "Placer"

        if (joueur.infoJoueur.objetEnMain != -1)
        {
            Objet objetEnMain = banqueObjet.banqueObjet[joueur.infoJoueur.objetEnMain];
            switch (objetEnMain.usage)
            {
                case "Consommable":
                    UtiliserConsommable(objetEnMain);
                    break;

                case "Outil":
                    UtiliserOutil(objetEnMain);
                    break;

                case "Arme":
                    UtiliserArme(objetEnMain);
                    break;

                case "Arc":
                    UtiliserArc(objetEnMain);
                    break;

                case "Placer":
                    PlacerObjet(objetEnMain);
                    break;

                default:
                    UtiliserMain(3);
                    break;
            }
        }
        else UtiliserMain(3);
    }


    void UtiliserConsommable(Objet objetEnMain)
    {
        if (objetEnMain.effet != "Autres") scriptGestFacteursVitaux.AjouterSurBarre($"Barre{objetEnMain.effet}", objetEnMain.quantit�Effet);
        scriptGestInv.RetirerObjetCaseBarreInv(scriptGestInv.caseBarreS�lectionner - 1);
        if(objetEnMain.effet != "Faim") scriptGestInv.AjouterObjetMain(banqueObjet.banqueObjet[14]);
    }

    void UtiliserOutil(Objet objetEnMain)
    {
        GameObject collision = FonctionRayCast.DoubleRayCast(joueur.rep�reRotation.transform, layerOutils, objetEnMain.port�e, 0.1f);
        AnimObjetEnMain(objetEnMain);

        if (collision != null)
        {
            Ressource ressource = collision.GetComponent<Ressource>();

            bool estEau = false;
            if (ressource != null) ressource.Interagis(objetEnMain.id, objetEnMain.d�gats, objetEnMain.chance, out estEau);
            if (estEau) scriptGestInv.AjouterObjetMain(banqueObjet.banqueObjet[33]);
        }
    }

    void UtiliserMain(float d�gat) //utile pour l'anim de la main et l'interaction avec certaines ressources
    {
        scriptGestStation.RetirerStationEnMain();
        GameObject collision = FonctionRayCast.DoubleRayCast(joueur.rep�reRotation.transform, layerOutils, 1, 0.1f);
        if (collision != null)
        {
            Ressource ressource = collision.GetComponent<Ressource>();
            Station station = collision.GetComponent<Station>();

            if (station != null)
            {
                station.Interaction(scriptGestInv);
            }
            else if (ressource != null)
            {
                ressource.Interagis(d�gat);
                scriptGestAnim.Prendre(true);
            }

        }
    }

    void UtiliserArme(Objet objetEnMain)
    {
        GameObject cible = FonctionRayCast.VisionRayCast(joueur.rep�reRotation.transform, layerArmes, objetEnMain.port�e, objetEnMain.�tendue);
        AnimObjetEnMain(objetEnMain);

        if (cible != null)
        {
            cible.GetComponent<ComportementMouton>().R�ceptionAttaque(gameObject, 5, (int)objetEnMain.d�gats);
        }
    }

    void UtiliserArc(Objet objetEnMain)
    {
        AnimObjetEnMain(objetEnMain);

        if (scriptGestInv.RetirerObjet(banqueObjet.banqueObjet[12]))
        {

            Transform transformRep�re = joueur.rep�reRotation.transform;
            Fl�che fl�che = Instantiate(prefabFl�che, transformRep�re.position + Vector3.up * transform.localScale.y / 2, transformRep�re.rotation).GetComponent<Fl�che>();
            fl�che.transform.parent = transform.parent;
            fl�che.d�gats = objetEnMain.d�gats;
            fl�che.scriptGestInv = scriptGestInv;

        }
    }

    void AnimObjetEnMain(Objet objetEnMain)
    {
        Transform parent = joueur.rep�reRotation.transform;
        if (objetEnMain.nom == "Arc_1") { Instantiate(prefabArc, parent); return; }
        if (objetEnMain.nom == "Pioche_1" || objetEnMain.nom == "Pioche_2" || objetEnMain.nom == "Pioche_3") { Instantiate(prefabPioche, parent); return; }
        if (objetEnMain.nom == "Hache_1" || objetEnMain.nom == "Hache_2" || objetEnMain.nom == "Hache_3") { Instantiate(prefabHache, parent); return; }
        if (objetEnMain.nom == "�p�e_1" || objetEnMain.nom == "�p�e_2" || objetEnMain.nom == "�p�e_3") { Instantiate(prefab�p�e, parent); return; }
        if (objetEnMain.nom == "Lance_1" || objetEnMain.nom == "Lance_2" || objetEnMain.nom == "Lance_3") { Instantiate(prefabLance, parent); return; }

    }

    void PlacerObjet(Objet objetEnMain)
    {
        scriptGestStation.ControleStation(objetEnMain);
    }
}
