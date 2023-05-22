using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressource : MonoBehaviour
{
    #region Initialisation

    AnimationRessource scriptAgite;
    G�n�rationRessource scriptGenRessource;

    public string nom = "";
    public List<int> idOutil;
    public float vie = 30;
    public float tauxReg�n = 5;
    public float tempsAvantReg�n = 0;
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
            scriptGenRessource = GetComponentInParent<G�n�rationRessource>();
        }
    }

    public void Interagis(int outil, float d�gats, float chance, out bool estEau) //#1 V�rifie l'outil, applique les d�gats et appelle le tremblement ou appelle v�rifier vie si la ressource ne tremble pas
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
                vie -= d�gats;
                this.chance = chance;

                if (gameObject.tag == "Roche")
                {
                    ActiverTremblement(0.6f, 30f, 0.02f);
                }
                else if (gameObject.tag == "Arbre") ActiverTremblement(0.6f, 30f, 0.02f);
                else if (gameObject.tag == "Station") ActiverTremblement(0.6f, 30f, 0.02f);
                else V�rifierVie();
            }
        }
    }

    public void Interagis(float d�gats) //#1.1 Interaction avec la main
    {
        if (gameObject.tag == "Buisson") ActiverTremblement(0.3f, 30f, 0.015f);
        else if(idOutil.Count == 0)
        {
            vie -= d�gats;
            chance = 0;
            ActiverTremblement(0.6f, 30f, 0.02f);
        }

    }

        void ActiverTremblement(float dur�e, float vitesseAgitation, float forceAgitation) //#2 Active le tremblement qui lui appelera V�rifierVie lorsque le tremblement est fini
    {
        scriptAgite.ActiveAgitation(gameObject, dur�e, vitesseAgitation, forceAgitation);
    }

    public void V�rifierVie() //#3 V�rifie la vie et active l'animation de mort, D�truit l'objet s'il ne poss�de pas d'animation de mort
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

    void ActiverArbreTombe(float dur�e, float vitesse) //#4 Active l'animation de mort qui elle active la destruction lorsque termin�
    {
        scriptAgite.ActiverArbreTombe(gameObject, dur�e, vitesse);
    }


    public void Destruction() //#5 Drop les objets et d�truit le gameobject
    {
        for (int i = 0; i < objetObtenues.Length; i++) //Parcours les objets � lacher et si le nombre al�atoire est plus petit que le facteur chance fois le multiplicateur de chance, l'objet est lach�
        {
            if (Random.Range(0f, 1f) <= chanceObtenir[i] * (1 + chance)) L�cherObjet(objetObtenues[i]);
        }
        if(transform.tag != "Station") scriptGenRessource.ressourceD�truite.Enqueue(transform.localPosition);
        Destroy(gameObject);
    }

    private void DestructionBuisson()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            for (int i = 0; i < objetObtenues.Length; i++) //Parcours les objets � lacher et si le nombre al�atoire est plus petit que le facteur chance fois le multiplicateur de chance, l'objet est lach�
            {
                if (Random.Range(0f, 1f) <= chanceObtenir[i] * (1 + chance)) L�cherObjet(objetObtenues[i]);
            }
        }
    }


    void L�cherObjet(int idObjet) //#6 Lache l'objet indiqu� qui lui va effectuer un petite trajectoire sur un point autours de la ressource
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
        for (int i = 0; i < objetObtenues.Length; i++) //Parcours les objets � lacher et si le nombre al�atoire est plus petit que le facteur chance fois le multiplicateur de chance, l'objet est lach�
        {
            if (Random.Range(0f, 1f) <= chanceObtenir[i] * (1 + chance)) L�cherObjet(objetObtenues[i]);
        }
    }
    #endregion


}

