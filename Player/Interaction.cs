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
    [SerializeField] GameObject prefabÉpée;
    [SerializeField] GameObject prefabHache;
    [SerializeField] GameObject prefabPioche;
    [SerializeField] GameObject prefabLance;

    [SerializeField] GameObject prefabFlèche;

    [SerializeField] float dégatsMain = 5;

    public void ToucheIntéragir()
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
        if (objetEnMain.effet != "Autres") scriptGestFacteursVitaux.AjouterSurBarre($"Barre{objetEnMain.effet}", objetEnMain.quantitéEffet);
        scriptGestInv.RetirerObjetCaseBarreInv(scriptGestInv.caseBarreSélectionner - 1);
        if(objetEnMain.effet != "Faim") scriptGestInv.AjouterObjetMain(banqueObjet.banqueObjet[14]);
    }

    void UtiliserOutil(Objet objetEnMain)
    {
        GameObject collision = FonctionRayCast.DoubleRayCast(joueur.repèreRotation.transform, layerOutils, objetEnMain.portée, 0.1f);
        AnimObjetEnMain(objetEnMain);

        if (collision != null)
        {
            Ressource ressource = collision.GetComponent<Ressource>();

            bool estEau = false;
            if (ressource != null) ressource.Interagis(objetEnMain.id, objetEnMain.dégats, objetEnMain.chance, out estEau);
            if (estEau) scriptGestInv.AjouterObjetMain(banqueObjet.banqueObjet[33]);
        }
    }

    void UtiliserMain(float dégat) //utile pour l'anim de la main et l'interaction avec certaines ressources
    {
        scriptGestStation.RetirerStationEnMain();
        GameObject collision = FonctionRayCast.DoubleRayCast(joueur.repèreRotation.transform, layerOutils, 1, 0.1f);
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
                ressource.Interagis(dégat);
                scriptGestAnim.Prendre(true);
            }

        }
    }

    void UtiliserArme(Objet objetEnMain)
    {
        GameObject cible = FonctionRayCast.VisionRayCast(joueur.repèreRotation.transform, layerArmes, objetEnMain.portée, objetEnMain.étendue);
        AnimObjetEnMain(objetEnMain);

        if (cible != null)
        {
            cible.GetComponent<ComportementMouton>().RéceptionAttaque(gameObject, 5, (int)objetEnMain.dégats);
        }
    }

    void UtiliserArc(Objet objetEnMain)
    {
        AnimObjetEnMain(objetEnMain);

        if (scriptGestInv.RetirerObjet(banqueObjet.banqueObjet[12]))
        {

            Transform transformRepère = joueur.repèreRotation.transform;
            Flèche flèche = Instantiate(prefabFlèche, transformRepère.position + Vector3.up * transform.localScale.y / 2, transformRepère.rotation).GetComponent<Flèche>();
            flèche.transform.parent = transform.parent;
            flèche.dégats = objetEnMain.dégats;
            flèche.scriptGestInv = scriptGestInv;

        }
    }

    void AnimObjetEnMain(Objet objetEnMain)
    {
        Transform parent = joueur.repèreRotation.transform;
        if (objetEnMain.nom == "Arc_1") { Instantiate(prefabArc, parent); return; }
        if (objetEnMain.nom == "Pioche_1" || objetEnMain.nom == "Pioche_2" || objetEnMain.nom == "Pioche_3") { Instantiate(prefabPioche, parent); return; }
        if (objetEnMain.nom == "Hache_1" || objetEnMain.nom == "Hache_2" || objetEnMain.nom == "Hache_3") { Instantiate(prefabHache, parent); return; }
        if (objetEnMain.nom == "Épée_1" || objetEnMain.nom == "Épée_2" || objetEnMain.nom == "Épée_3") { Instantiate(prefabÉpée, parent); return; }
        if (objetEnMain.nom == "Lance_1" || objetEnMain.nom == "Lance_2" || objetEnMain.nom == "Lance_3") { Instantiate(prefabLance, parent); return; }

    }

    void PlacerObjet(Objet objetEnMain)
    {
        scriptGestStation.ControleStation(objetEnMain);
    }
}
