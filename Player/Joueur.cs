using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joueur : MonoBehaviour
{
    public InfoJoueur infoJoueur;

    [SerializeField] CréationIle ile;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject caméra;
    [SerializeField] public MouvementJoueur scriptMouvJoueur;
    [SerializeField] Interaction scriptInteract;
    [SerializeField] CollisionJoueur scriptCollJoueur;
    [SerializeField] GérerObjets scriptGérerObjets;
    [SerializeField] GestionFacteursVitaux scriptGstFactVitaux;
    [SerializeField] GestionInventaire scriptGstInv;
    [SerializeField] Animator animateur;
    [SerializeField] GestionAnimation scriptGestionAnimation;
    [SerializeField] public GameObject repèreRotation;
    

    public Vector3 v = Vector3.zero;

    private void Start()
    {
        infoJoueur.Init();
        Initialiser();
    }

    private void Update()
    {
        infoJoueur.Rafraichir();
        v = scriptMouvJoueur.Mouvement(v);
        scriptGérerObjets.DétecterObjetSol();
        AppliquerVelocité(scriptCollJoueur.CalculCollision(v));
        scriptGestionAnimation.Animation();
    }

    private void AppliquerVelocité(Vector3 v)
    {
        gameObject.transform.Translate(v);
    }

    private void Initialiser()
    {
        Vector2 spawn;
        spawn = GénérerSpawnJoueur();

        transform.position = new Vector3(spawn.x, transform.position.y, spawn.y);
        caméra.transform.position = new Vector3(spawn.x, caméra.transform.position.y, spawn.y);
        caméra.GetComponent<ComportementCaméra>().CalculerDéplacement();
    }

    Vector2 GénérerSpawnJoueur()
    {
        Vector2 pos = ile.suivitBiomes[2][Random.Range(0, ile.suivitBiomes[2].Count)];
        return pos* ile.tailleCase;
    }

    public void ToucherParEnnemie(float dégats, float recul)
    {

    }

    public void TuerJoueur()
    {
        scriptGstInv.ViderInventaire();
        Initialiser();
        scriptGstFactVitaux.RemplirVitaux();
    }



    public struct InfoJoueur //Struct utiliser pour garder un suivit de l'état du joueur
    {
        public bool estEnMouvement;
        public Vector2 entrées;

        public int objetEnMain;

        public float faim;
        public float soif;
        public float vie;

        public void Init()
        {
            objetEnMain = -1;
        }

        public void Rafraichir()
        {
            estEnMouvement = false;
        }
    }

}
