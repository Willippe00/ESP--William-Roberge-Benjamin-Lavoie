using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GestionInventaire : MonoBehaviour
{

    #region Variables et init

    [SerializeField] Joueur joueur;
    [SerializeField] GameManager gameManager;
    [SerializeField] GestionStations scriptGestStations;
    [SerializeField] GameObject miniMapCam;

    bool open = false;

    List<Objet> banqueObjet;
    Dictionary<string, Objet> banqueCraft;

    [SerializeField] GameObject prefabObjet;
    [SerializeField] GameObject parentCasesInv;
    [SerializeField] GameObject parentCasesBarreInv;
    [SerializeField] GameObject parentCasesCraft1;
    [SerializeField] GameObject parentCasesCraft2;
    [SerializeField] GameObject parentCasesCraft3;
    [SerializeField] Text textFamille;
    [SerializeField] GameObject inventaire;
    public int fen�treCraft = 1;

    ObjetCurseur objSelec;
    [SerializeField] GameObject curseur;

    List<Objet> objetsInv;
    Objet[] objetsBarreInv;
    Objet[] objetsCraft;
    Objet[] objetsFamilleInv; //Objet qui vont �tre affich� dans l'inventaire selon la famille
    List<Image> casesInv;
    List<Image> casesBarreInv;
    List<Image> casesCraft1;
    List<Image> casesCraft2;
    List<Image> casesCraft3;


    public int caseBarreS�lectionner = 1;

    string[] familles = { "Inventaire", "Consommables", "Outils & Armes", "Ressources", "Stations" };
    public int familleAffich� = 0;

    public bool InventairePlein
    {
        get
        {
            gameManager.inventairePlein = objetsInv.Count >= casesInv.Count;
            return gameManager.inventairePlein;
        }
    }


    private void Start() //Seulement pour test ou objet initiaux
    {

        InitialiserCases();

        objSelec.Init(curseur);

        objetsInv = new List<Objet>();
        objetsBarreInv = new Objet[casesBarreInv.Count];
        objetsFamilleInv = new Objet[casesInv.Count + casesBarreInv.Count];

        banqueObjet = GetComponent<BanqueObjet>().banqueObjet;
        banqueCraft = GetComponent<BanqueObjet>().banqueCraft;

        S�lectionnerCaseInventaire(1);

        //outils
        R�cupererObjet(1);
        R�cupererObjet(4);
        R�cupererObjet(7);
        R�cupererObjet(10);
        R�cupererObjet(12);
        R�cupererObjet(12);
        R�cupererObjet(14);
        R�cupererObjet(89);

        //structure
        R�cupererObjet(79);
        R�cupererObjet(80);
        R�cupererObjet(81);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(82);
        R�cupererObjet(83);
        R�cupererObjet(83);
        R�cupererObjet(83);

        //plus
        R�cupererObjet(28);
        R�cupererObjet(76);
        R�cupererObjet(77);
        //R�cupererObjet(14);



    }

    void InitialiserCases() //Rassemble les cases de l'inventaire pour changer leurs images
    {
        casesInv = new List<Image>();
        casesBarreInv = new List<Image>();
        casesCraft1 = new List<Image>();
        casesCraft2 = new List<Image>();
        casesCraft3 = new List<Image>();

        JoindreParentImage(casesInv, parentCasesInv);
        JoindreParentImage(casesBarreInv, parentCasesBarreInv);
        JoindreParentImage(casesCraft1, parentCasesCraft1);
        JoindreParentImage(casesCraft2, parentCasesCraft2);
        JoindreParentImage(casesCraft3, parentCasesCraft3);

    }

    static void JoindreParentImage(List<Image> cases, GameObject parent) //Remplis un tableau d'image a partir du parent contenant les cases
    {
        foreach (Image image in parent.GetComponentsInChildren<Image>())
        {
            if (image.gameObject.name == "ImageObjet") cases.Add(image);
        }
    }

    #endregion

    #region Contr�le
    public void ActiverInventaire(int numCraft)
    {
        if (!open) OuvrirInventaire(numCraft);
        else FermerInventaire();
    }

    void OuvrirInventaire(int numCraft)
    {
        fen�treCraft = numCraft;
        scriptGestStations.RetirerStationEnMain();
        OuvrirCraft();
        ChangerFamilleAffich�(0);
        G�n�rerAffichageInv();
        inventaire.SetActive(true);
        miniMapCam.SetActive(true);
        open = true;

        foreach (Button bouton in parentCasesBarreInv.GetComponentsInChildren<Button>()) bouton.interactable = true;
    }

    void FermerInventaire()
    {
        parentCasesCraft1.SetActive(false);
        parentCasesCraft2.SetActive(false);
        parentCasesCraft3.SetActive(false);
        inventaire.SetActive(false);
        miniMapCam.SetActive(false);
        ViderFenetreCraft();
        open = false;

        foreach (Button bouton in parentCasesBarreInv.GetComponentsInChildren<Button>()) bouton.interactable = false;
    }

    public void S�lectionnerCaseInventaire(int numCase)
    {
        if (numCase <= 7)
        {
            scriptGestStations.RetirerStationEnMain();
            casesBarreInv[caseBarreS�lectionner - 1].GetComponentsInParent<Image>()[1].color = new Color(0.877f, 0.877f, 0.877f, 0.862f);
            casesBarreInv[numCase - 1].GetComponentsInParent<Image>()[1].color = new Color(1, 1, 1, 1);
            caseBarreS�lectionner = numCase;

            joueur.infoJoueur.objetEnMain = objetsBarreInv[numCase - 1] != null ? objetsBarreInv[numCase - 1].id : -1; //-1 = aucun objet

        }
    }

    #endregion

    #region Affichage
    public void ChangerFamilleAffich�(int direction)
    {
        if (direction == 0) familleAffich� = 0; //Si la direction envoyer est 0 on veut remettre la famille a zero
        else
        {
            familleAffich� = (familleAffich� + direction) % familles.Length;
            if (familleAffich� < 0) familleAffich� += familles.Length;
        }

        textFamille.text = familles[familleAffich�];

        G�n�rerAffichageInv();
    }

    void G�n�rerAffichageInv()
    {
        string famille = familles[familleAffich�];


        objetsInv = objetsInv.OrderBy(objet => objet.nom).ToList();

        int i = 0;
        foreach (Objet objet in objetsInv)
        {
            if (familleAffich� == 0 || objet.famille == famille)
            {
                objetsFamilleInv[i] = objet;

                casesInv[i].enabled = true;
                casesInv[i].sprite = objet.image;

                AjouterObjetCase(objetsFamilleInv, casesInv, objet, i);

                i++;
            }
        }
        for (; i < casesInv.Count; i++)
        {
            RetirerObjetCase(objetsFamilleInv, casesInv, i);
        }

    }

    void OuvrirCraft()
    {
        if (fen�treCraft == 1)
        {
            parentCasesCraft1.SetActive(true);
            objetsCraft = new Objet[casesCraft1.Count];
        }
        else if (fen�treCraft == 2)
        {
            parentCasesCraft2.SetActive(true);
            objetsCraft = new Objet[casesCraft2.Count];
        }
        else if (fen�treCraft == 3)
        {
            parentCasesCraft3.SetActive(true);
            objetsCraft = new Objet[casesCraft3.Count];
        }

    }

    static void AjouterObjetCase(Objet[] tableauObjet, List<Image> imagesCases, Objet objetAjout�, int i)
    {
        tableauObjet[i] = objetAjout�;
        imagesCases[i].enabled = true;
        imagesCases[i].sprite = objetAjout�.image;
    }

    static void RetirerObjetCase(Objet[] tableauObjet, List<Image> imagesCases, int i)
    {
        tableauObjet[i] = null;
        imagesCases[i].enabled = false;
    }

    public void AjouterObjetMain(Objet objetAjout�)
    {
        objetsBarreInv[caseBarreS�lectionner - 1] = objetAjout�;
        casesBarreInv[caseBarreS�lectionner - 1].enabled = true;
        casesBarreInv[caseBarreS�lectionner - 1].sprite = objetAjout�.image;
        S�lectionnerCaseInventaire(caseBarreS�lectionner);
        G�n�rerAffichageInv();

    }

    public void RetirerObjetCaseBarreInv(int i)
    {
        scriptGestStations.RetirerStationEnMain();
        objetsBarreInv[i] = null;
        casesBarreInv[i].enabled = false;
        S�lectionnerCaseInventaire(caseBarreS�lectionner);
        G�n�rerAffichageInv();
    }

    void AjouterObjetCraft(Objet objetAjout�, int i)
    {
        objetsCraft[i] = objetAjout�;
        if (fen�treCraft == 1)
        {
            casesCraft1[i].enabled = true;
            casesCraft1[i].sprite = objetAjout�.image;
        }
        else if (fen�treCraft == 2)
        {
            casesCraft2[i].enabled = true;
            casesCraft2[i].sprite = objetAjout�.image;
        }
        else if (fen�treCraft == 3)
        {
            casesCraft3[i].enabled = true;
            casesCraft3[i].sprite = objetAjout�.image;
        }
    }

    void RetirerObjetCraft(int i)
    {
        objetsCraft[i] = null;
        if (fen�treCraft == 1)
        {
            casesCraft1[i].enabled = false;

        }
        else if (fen�treCraft == 2)
        {
            casesCraft2[i].enabled = false;
        }
        else if (fen�treCraft == 3)
        {
            casesCraft3[i].enabled = false;
        }
    }
    #endregion

    #region Gestion inventaire

    public bool RetirerObjet(Objet objet)
    {
        for (int i = 0; i < objetsInv.Count; i++)
        {
            if (objetsInv[i] == objet)
            {
                objetsInv.RemoveAt(i);
                G�n�rerAffichageInv();
                return true;
            }
        }
        for (int i = 0; i < objetsBarreInv.Length; i++)
        {
            if (objetsBarreInv[i] == objet)
            {
                RetirerObjetCaseBarreInv(i);
                return true;
            }
        }
        return false;

    }

    public void R�cupererObjet(GameObject objet)
    {
        if (!InventairePlein)
        {
            R�cupererObjet(objet.GetComponent<ComportementObjet>().id);
            G�n�rerAffichageInv();
            Destroy(objet);
        }
        else foreach (GameObject objetSol in gameManager.listeObjetSol) objetSol.GetComponent<ComportementObjet>().vitesseR�cup�ration = 0;
    }

    public void R�cupererObjet(int idObjet)
    {
        objetsInv.Add(banqueObjet[idObjet]);
        G�n�rerAffichageInv();
    }

    public void OnClick_Case(GameObject imageCliquer)
    {
        if (!objSelec.suitCurseur && imageCliquer.GetComponent<Image>().enabled)  //Click sans objet sur la souris
        {
            RectTransform[] parents = imageCliquer.GetComponentsInParent<RectTransform>();
            if (parents[3].name == "PanneauInventaire")
            {
                objSelec.image = objSelec.objet.GetComponent<Image>();
                Image imageCliquerImage = imageCliquer.GetComponent<Image>();
                objSelec.image.sprite = imageCliquerImage.sprite;
                objSelec.objet.transform.position = imageCliquer.transform.position;
                objSelec.image.rectTransform.sizeDelta = imageCliquerImage.rectTransform.sizeDelta;
                objSelec.info = objetsFamilleInv[parents[1].GetSiblingIndex()];

                objSelec.suitCurseur = true;
                objSelec.image.enabled = true;
                SuivreD�placementCurseur();
            }
            else if (parents[3].name == "BarreInventaire")
            {
                objetsInv.Add(objetsBarreInv[parents[1].GetSiblingIndex()]);
                RetirerObjetCase(objetsBarreInv, casesBarreInv, parents[1].GetSiblingIndex());
                S�lectionnerCaseInventaire(caseBarreS�lectionner);
                G�n�rerAffichageInv();
            }
            else
            {
                objetsInv.Add(objetsCraft[parents[1].GetSiblingIndex()]);

                if (parents[1].GetSiblingIndex() != objetsCraft.Length - 1)
                {
                    RetirerObjetCraft(parents[1].GetSiblingIndex());
                }
                else
                {
                    for (int i = 0; i < objetsCraft.Length; i++)
                    {
                        RetirerObjetCraft(i);
                    }
                }
                CalculCraft();
                G�n�rerAffichageInv();
            }
        }
        else if (objSelec.suitCurseur && !imageCliquer.GetComponent<Image>().enabled)  //Click avec objet sur la souris
        {
            RectTransform[] parents = imageCliquer.GetComponentsInParent<RectTransform>();
            if (parents[2].name == "FenetreCraft1" || parents[2].name == "FenetreCraft2" || parents[2].name == "FenetreCraft3")
            {
                if (parents[1].GetSiblingIndex() != objetsCraft.Length - 1)
                {
                    AjouterObjetCraft(objSelec.info, parents[1].GetSiblingIndex());
                    objetsInv.Remove(objSelec.info);
                    LacherObjetDuCurseur();
                    CalculCraft();
                    G�n�rerAffichageInv();
                }
            }
            else if (parents[3].name == "BarreInventaire")
            {
                AjouterObjetCase(objetsBarreInv, casesBarreInv, objSelec.info, parents[1].GetSiblingIndex());
                objetsInv.Remove(objSelec.info);
                LacherObjetDuCurseur();
                G�n�rerAffichageInv();
                S�lectionnerCaseInventaire(caseBarreS�lectionner);
            }
            else LacherObjetDuCurseur();
        }
    }


    void LacherObjetDuCurseur()
    {
        objSelec.image.enabled = false;
        objSelec.suitCurseur = false;
        G�n�rerAffichageInv();
    }

    void SuivreD�placementCurseur()
    {
        if (objSelec.suitCurseur) objSelec.objet.transform.position = Input.mousePosition;
    }

    public void ViderInventaire()
    {
        if (open) FermerInventaire();
        for (int i = 0; i < objetsInv.Count; i++)
        {
            ApparaitreObjet(objetsInv[i].id, 1);
            objetsInv.RemoveAt(i);
        }
        G�n�rerAffichageInv();
    }

    void ApparaitreObjet(int idObjet, float vitesseR�cup)
    {
        GameObject objet = Instantiate(prefabObjet, joueur.transform.position, prefabObjet.transform.rotation, transform);
        ComportementObjet scriptObjet = objet.GetComponent<ComportementObjet>();
        scriptObjet.LoadSprite(banqueObjet[idObjet].image);
        scriptObjet.id = idObjet;
        scriptObjet.vitesseR�cup�ration *= vitesseR�cup;
        Sprite spriteObj = objet.GetComponent<SpriteRenderer>().sprite;
        objet.transform.localScale = new Vector3(2.5f * 24 / spriteObj.rect.width, 2.5f * 32 / spriteObj.rect.height, objet.transform.localScale.z);

        scriptObjet.Apparaitre();
    }



    public void ApparaitreObjet(int id, Vector3 emplacement, bool immobile, float vitesseR�cup)
    {
        ComportementObjet scriptObjet = Instantiate(prefabObjet, emplacement, prefabObjet.transform.rotation).GetComponent<ComportementObjet>();

        scriptObjet.transform.parent = gameObject.transform;
        scriptObjet.immobile = immobile;
        scriptObjet.LoadSprite(banqueObjet[id].image);
        scriptObjet.id = id;
        scriptObjet.vitesseR�cup�ration *= vitesseR�cup;

        scriptObjet.Apparaitre();
    }
    public void LacherObjetEnMain()
    {
        if (objetsBarreInv[caseBarreS�lectionner - 1] != null)
        {
            Vector3 pos = joueur.rep�reRotation.transform.forward + joueur.transform.position;
            ApparaitreObjet(objetsBarreInv[caseBarreS�lectionner - 1].id, new Vector3(pos.x, 0, pos.z), true, 0);
            RetirerObjetCaseBarreInv(caseBarreS�lectionner - 1);
        }
    }

    #endregion

    #region Fabrication

    void CalculCraft()
    {
        string craft = "";
        for (int i = 0; i < objetsCraft.Length - 1; i++)
        {
            if (objetsCraft[i] != null)
            {
                craft += objetsCraft[i].id;
                craft += "_";
            }
        }

        if (banqueCraft.ContainsKey(craft))
        {
            AjouterObjetCraft(banqueCraft[craft], objetsCraft.Length - 1);
        }
        else if (objetsCraft[objetsCraft.Length - 1] != null)
        {
            RetirerObjetCraft(objetsCraft.Length - 1);
        }
    }

    void ViderFenetreCraft()
    {
        for (int i = 0; i < objetsCraft.Length - 1; i++)
        {
            if (objetsCraft[i] != null)
            {
                objetsInv.Add(objetsCraft[i]);
                RetirerObjetCraft(i);
            }
        }
        RetirerObjetCraft(objetsCraft.Length - 1);
    }
    #endregion


    private void Update()
    {
        SuivreD�placementCurseur();
    }

    public struct ObjetCurseur
    {
        public GameObject objet;
        public Image image;
        public Objet info;
        public bool suitCurseur;
        public int origineI;


        public void Init(GameObject objet)
        {
            suitCurseur = false;
            this.objet = objet;

        }

    }
}
