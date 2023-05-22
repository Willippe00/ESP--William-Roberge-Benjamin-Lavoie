using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressource : MonoBehaviour
{
    #region Initialisation

    AnimationRessource scriptAgite;
    GénérationRessource scriptGenRessource;

    public string nom = "";
    public List<int> idOutil;
    public float vie = 30;
    public float tauxRegén = 5;
    public float tempsAvantRegén = 0;
    float chance;

    [SerializeField] int[] objetObtenues;
    [SerializeField] float[] chanceObtenir;

    [SerializeField] GameObject prefabObjet;
    BanqueObjet banqueObjet;
    #endregion


    #region Interaction

    private void Start()
    {
        if (gameObject.tag != "Eau")
        {
            banqueObjet = GetComponentInParent<BanqueObjet>();
            scriptAgite = GetComponentInParent<AnimationRessource>();
            scriptGenRessource = GetComponentInParent<GénérationRessource>();
        }
    }

    public void Interagis(int outil, float dégats, float chance, out bool estEau) //#1 Vérifie l'outil, applique les dégats et appelle le tremblement ou appelle vérifier vie si la ressource ne tremble pas
    {
        estEau = false;
        if (idOutil.Contains(outil))
        {
            if (gameObject.tag == "Eau")
            {
                estEau = true;
            }
            else
            {
                vie -= dégats;
                this.chance = chance;

                if (gameObject.tag == "Roche")
                {
                    ActiverTremblement(0.6f, 30f, 0.02f);
                }
                else if (gameObject.tag == "Arbre") ActiverTremblement(0.6f, 30f, 0.02f);
                else if (gameObject.tag == "Station") ActiverTremblement(0.6f, 30f, 0.02f);
                else VérifierVie();
            }
        }
    }

    public void Interagis(float dégats) //#1.1 Interaction avec la main
    {
        if (gameObject.tag == "Buisson") ActiverTremblement(0.3f, 30f, 0.015f);
        else if(idOutil.Count == 0)
        {
            vie -= dégats;
            chance = 0;
            ActiverTremblement(0.6f, 30f, 0.02f);
        }

    }

        void ActiverTremblement(float durée, float vitesseAgitation, float forceAgitation) //#2 Active le tremblement qui lui appelera VérifierVie lorsque le tremblement est fini
    {
        scriptAgite.ActiveAgitation(gameObject, durée, vitesseAgitation, forceAgitation);
    }

    public void VérifierVie() //#3 Vérifie la vie et active l'animation de mort, Détruit l'objet s'il ne possède pas d'animation de mort
    {
        if (vie < 0)
        {
            if (gameObject.tag == "Arbre") ActiverArbreTombe(2.5f, 25f);
            else Destruction();
        }
        if(gameObject.tag == "Buisson")
        {
            DestructionBuisson();
        }
    }

    void ActiverArbreTombe(float durée, float vitesse) //#4 Active l'animation de mort qui elle active la destruction lorsque terminé
    {
        scriptAgite.ActiverArbreTombe(gameObject, durée, vitesse);
    }


    public void Destruction() //#5 Drop les objets et détruit le gameobject
    {
        for (int i = 0; i < objetObtenues.Length; i++) //Parcours les objets à lacher et si le nombre aléatoire est plus petit que le facteur chance fois le multiplicateur de chance, l'objet est laché
        {
            if (Random.Range(0f, 1f) <= chanceObtenir[i] * (1 + chance)) LâcherObjet(objetObtenues[i]);
        }
        if(transform.tag != "Station") scriptGenRessource.ressourceDétruite.Enqueue(transform.localPosition);
        Destroy(gameObject);
    }

    private void DestructionBuisson()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            for (int i = 0; i < objetObtenues.Length; i++) //Parcours les objets à lacher et si le nombre aléatoire est plus petit que le facteur chance fois le multiplicateur de chance, l'objet est laché
            {
                if (Random.Range(0f, 1f) <= chanceObtenir[i] * (1 + chance)) LâcherObjet(objetObtenues[i]);
            }
        }
    }


    void LâcherObjet(int idObjet) //#6 Lache l'objet indiqué qui lui va effectuer un petite trajectoire sur un point autours de la ressource
    {
        GameObject objet = Instantiate(prefabObjet, transform.position, prefabObjet.transform.rotation, transform.parent.parent);
        ComportementObjet infoObjet = objet.GetComponent<ComportementObjet>();
        infoObjet.LoadSprite(banqueObjet.banqueObjet[idObjet].image);
        infoObjet.id = idObjet;
        Sprite spriteObj = objet.GetComponent<SpriteRenderer>().sprite;
        objet.transform.localScale = new Vector3(2.5f * 24 / spriteObj.rect.width, 2.5f * 32 / spriteObj.rect.height, objet.transform.localScale.z);

        infoObjet.Apparaitre();
    }

    public void LacherRessource()
    {
        for (int i = 0; i < objetObtenues.Length; i++) //Parcours les objets à lacher et si le nombre aléatoire est plus petit que le facteur chance fois le multiplicateur de chance, l'objet est laché
        {
            if (Random.Range(0f, 1f) <= chanceObtenir[i] * (1 + chance)) LâcherObjet(objetObtenues[i]);
        }
    }
    #endregion


}

