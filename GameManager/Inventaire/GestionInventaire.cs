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
    public int fenêtreCraft = 1;

    ObjetCurseur objSelec;
    [SerializeField] GameObject curseur;

    List<Objet> objetsInv;
    Objet[] objetsBarreInv;
    Objet[] objetsCraft;
    Objet[] objetsFamilleInv; //Objet qui vont être affiché dans l'inventaire selon la famille
    List<Image> casesInv;
    List<Image> casesBarreInv;
    List<Image> casesCraft1;
    List<Image> casesCraft2;
    List<Image> casesCraft3;


    public int caseBarreSélectionner = 1;

    string[] familles = { "Inventaire", "Consommables", "Outils & Armes", "Ressources", "Stations" };
    public int familleAffiché = 0;

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

        SélectionnerCaseInventaire(1);

        //outils
        RécupererObjet(1);
        RécupererObjet(4);
        RécupererObjet(7);
        RécupererObjet(10);
        RécupererObjet(12);
        RécupererObjet(12);
        RécupererObjet(14);
        RécupererObjet(89);

        //structure
        RécupererObjet(79);
        RécupererObjet(80);
        RécupererObjet(81);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(82);
        RécupererObjet(83);
        RécupererObjet(83);
        RécupererObjet(83);

        //plus
        RécupererObjet(28);
        RécupererObjet(76);
        RécupererObjet(77);
        //RécupererObjet(14);



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

    #region Contrôle
    public void ActiverInventaire(int numCraft)
    {
        if (!open) OuvrirInventaire(numCraft);
        else FermerInventaire();
    }

    void OuvrirInventaire(int numCraft)
    {
        fenêtreCraft = numCraft;
        scriptGestStations.RetirerStationEnMain();
        OuvrirCraft();
        ChangerFamilleAffiché(0);
        GénérerAffichageInv();
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

    public void SélectionnerCaseInventaire(int numCase)
    {
        if (numCase <= 7)
        {
            scriptGestStations.RetirerStationEnMain();
            casesBarreInv[caseBarreSélectionner - 1].GetComponentsInParent<Image>()[1].color = new Color(0.877f, 0.877f, 0.877f, 0.862f);
            casesBarreInv[numCase - 1].GetComponentsInParent<Image>()[1].color = new Color(1, 1, 1, 1);
            caseBarreSélectionner = numCase;

            joueur.infoJoueur.objetEnMain = objetsBarreInv[numCase - 1] != null ? objetsBarreInv[numCase - 1].id : -1; //-1 = aucun objet

        }
    }

    #endregion

    #region Affichage
    public void ChangerFamilleAffiché(int direction)
    {
        if (direction == 0) familleAffiché = 0; //Si la direction envoyer est 0 on veut remettre la famille a zero
        else
        {
            familleAffiché = (familleAffiché + direction) % familles.Length;
            if (familleAffiché < 0) familleAffiché += familles.Length;
        }

        textFamille.text = familles[familleAffiché];

        GénérerAffichageInv();
    }

    void GénérerAffichageInv()
    {
        string famille = familles[familleAffiché];


        objetsInv = objetsInv.OrderBy(objet => objet.nom).ToList();

        int i = 0;
        foreach (Objet objet in objetsInv)
        {
            if (familleAffiché == 0 || objet.famille == famille)
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
        if (fenêtreCraft == 1)
        {
            parentCasesCraft1.SetActive(true);
            objetsCraft = new Objet[casesCraft1.Count];
        }
        else if (fenêtreCraft == 2)
        {
            parentCasesCraft2.SetActive(true);
            objetsCraft = new Objet[casesCraft2.Count];
        }
        else if (fenêtreCraft == 3)
        {
            parentCasesCraft3.SetActive(true);
            objetsCraft = new Objet[casesCraft3.Count];
        }

    }

    static void AjouterObjetCase(Objet[] tableauObjet, List<Image> imagesCases, Objet objetAjouté, int i)
    {
        tableauObjet[i] = objetAjouté;
        imagesCases[i].enabled = true;
        imagesCases[i].sprite = objetAjouté.image;
    }

    static void RetirerObjetCase(Objet[] tableauObjet, List<Image> imagesCases, int i)
    {
        tableauObjet[i] = null;
        imagesCases[i].enabled = false;
    }

    public void AjouterObjetMain(Objet objetAjouté)
    {
        objetsBarreInv[caseBarreSélectionner - 1] = objetAjouté;
        casesBarreInv[caseBarreSélectionner - 1].enabled = true;
        casesBarreInv[caseBarreSélectionner - 1].sprite = objetAjouté.image;
        SélectionnerCaseInventaire(caseBarreSélectionner);
        GénérerAffichageInv();

    }

    public void RetirerObjetCaseBarreInv(int i)
    {
        scriptGestStations.RetirerStationEnMain();
        objetsBarreInv[i] = null;
        casesBarreInv[i].enabled = false;
        SélectionnerCaseInventaire(caseBarreSélectionner);
        GénérerAffichageInv();
    }

    void AjouterObjetCraft(Objet objetAjouté, int i)
    {
        objetsCraft[i] = objetAjouté;
        if (fenêtreCraft == 1)
        {
            casesCraft1[i].enabled = true;
            casesCraft1[i].sprite = objetAjouté.image;
        }
        else if (fenêtreCraft == 2)
        {
            casesCraft2[i].enabled = true;
            casesCraft2[i].sprite = objetAjouté.image;
        }
        else if (fenêtreCraft == 3)
        {
            casesCraft3[i].enabled = true;
            casesCraft3[i].sprite = objetAjouté.image;
        }
    }

    void RetirerObjetCraft(int i)
    {
        objetsCraft[i] = null;
        if (fenêtreCraft == 1)
        {
            casesCraft1[i].enabled = false;

        }
        else if (fenêtreCraft == 2)
        {
            casesCraft2[i].enabled = false;
        }
        else if (fenêtreCraft == 3)
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
                GénérerAffichageInv();
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

    public void RécupererObjet(GameObject objet)
    {
        if (!InventairePlein)
        {
            RécupererObjet(objet.GetComponent<ComportementObjet>().id);
            GénérerAffichageInv();
            Destroy(objet);
        }
        else foreach (GameObject objetSol in gameManager.listeObjetSol) objetSol.GetComponent<ComportementObjet>().vitesseRécupération = 0;
    }

    public void RécupererObjet(int idObjet)
    {
        objetsInv.Add(banqueObjet[idObjet]);
        GénérerAffichageInv();
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
                SuivreDéplacementCurseur();
            }
            else if (parents[3].name == "BarreInventaire")
            {
                objetsInv.Add(objetsBarreInv[parents[1].GetSiblingIndex()]);
                RetirerObjetCase(objetsBarreInv, casesBarreInv, parents[1].GetSiblingIndex());
                SélectionnerCaseInventaire(caseBarreSélectionner);
                GénérerAffichageInv();
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
                GénérerAffichageInv();
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
                    GénérerAffichageInv();
                }
            }
            else if (parents[3].name == "BarreInventaire")
            {
                AjouterObjetCase(objetsBarreInv, casesBarreInv, objSelec.info, parents[1].GetSiblingIndex());
                objetsInv.Remove(objSelec.info);
                LacherObjetDuCurseur();
                GénérerAffichageInv();
                SélectionnerCaseInventaire(caseBarreSélectionner);
            }
            else LacherObjetDuCurseur();
        }
    }


    void LacherObjetDuCurseur()
    {
        objSelec.image.enabled = false;
        objSelec.suitCurseur = false;
        GénérerAffichageInv();
    }

    void SuivreDéplacementCurseur()
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
        GénérerAffichageInv();
    }

    void ApparaitreObjet(int idObjet, float vitesseRécup)
    {
        GameObject objet = Instantiate(prefabObjet, joueur.transform.position, prefabObjet.transform.rotation, transform);
        ComportementObjet scriptObjet = objet.GetComponent<ComportementObjet>();
        scriptObjet.LoadSprite(banqueObjet[idObjet].image);
        scriptObjet.id = idObjet;
        scriptObjet.vitesseRécupération *= vitesseRécup;
        Sprite spriteObj = objet.GetComponent<SpriteRenderer>().sprite;
        objet.transform.localScale = new Vector3(2.5f * 24 / spriteObj.rect.width, 2.5f * 32 / spriteObj.rect.height, objet.transform.localScale.z);

        scriptObjet.Apparaitre();
    }



    public void ApparaitreObjet(int id, Vector3 emplacement, bool immobile, float vitesseRécup)
    {
        ComportementObjet scriptObjet = Instantiate(prefabObjet, emplacement, prefabObjet.transform.rotation).GetComponent<ComportementObjet>();

        scriptObjet.transform.parent = gameObject.transform;
        scriptObjet.immobile = immobile;
        scriptObjet.LoadSprite(banqueObjet[id].image);
        scriptObjet.id = id;
        scriptObjet.vitesseRécupération *= vitesseRécup;

        scriptObjet.Apparaitre();
    }
    public void LacherObjetEnMain()
    {
        if (objetsBarreInv[caseBarreSélectionner - 1] != null)
        {
            Vector3 pos = joueur.repèreRotation.transform.forward + joueur.transform.position;
            ApparaitreObjet(objetsBarreInv[caseBarreSélectionner - 1].id, new Vector3(pos.x, 0, pos.z), true, 0);
            RetirerObjetCaseBarreInv(caseBarreSélectionner - 1);
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
        SuivreDéplacementCurseur();
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
