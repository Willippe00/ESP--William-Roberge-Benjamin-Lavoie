using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joueur : MonoBehaviour
{
    public InfoJoueur infoJoueur;

    [SerializeField] Cr�ationIle ile;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject cam�ra;
    [SerializeField] public MouvementJoueur scriptMouvJoueur;
    [SerializeField] Interaction scriptInteract;
    [SerializeField] CollisionJoueur scriptCollJoueur;
    [SerializeField] G�rerObjets scriptG�rerObjets;
    [SerializeField] GestionFacteursVitaux scriptGstFactVitaux;
    [SerializeField] GestionInventaire scriptGstInv;
    [SerializeField] Animator animateur;
    [SerializeField] GestionAnimation scriptGestionAnimation;
    [SerializeField] public GameObject rep�reRotation;
    

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
        scriptG�rerObjets.D�tecterObjetSol();
        AppliquerVelocit�(scriptCollJoueur.CalculCollision(v));
        scriptGestionAnimation.Animation();
    }

    private void AppliquerVelocit�(Vector3 v)
    {
        gameObject.transform.Translate(v);
    }

    private void Initialiser()
    {
        Vector2 spawn;
        spawn = G�n�rerSpawnJoueur();

        transform.position = new Vector3(spawn.x, transform.position.y, spawn.y);
        cam�ra.transform.position = new Vector3(spawn.x, cam�ra.transform.position.y, spawn.y);
        cam�ra.GetComponent<ComportementCam�ra>().CalculerD�placement();
    }

    Vector2 G�n�rerSpawnJoueur()
    {
        Vector2 pos = ile.suivitBiomes[2][Random.Range(0, ile.suivitBiomes[2].Count)];
        return pos* ile.tailleCase;
    }

    public void ToucherParEnnemie(float d�gats, float recul)
    {

    }

    public void TuerJoueur()
    {
        scriptGstInv.ViderInventaire();
        Initialiser();
        scriptGstFactVitaux.RemplirVitaux();
    }



    public struct InfoJoueur //Struct utiliser pour garder un suivit de l'�tat du joueur
    {
        public bool estEnMouvement;
        public Vector2 entr�es;

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
