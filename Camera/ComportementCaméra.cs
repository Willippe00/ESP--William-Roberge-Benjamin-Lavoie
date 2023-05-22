using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementCaméra : MonoBehaviour
{
    [SerializeField] Joueur joueur;
    [SerializeField] Camera caméra;

    [SerializeField] Vector2 tailleZone = new Vector2(3, 5);
    Vector3 distanceCaméra;

    Vector3 v = Vector3.zero;
    [SerializeField] float speed = 3;

    InfoZone infoZone;

    private void Start()
    {
        joueur = GameObject.FindGameObjectWithTag("Player").GetComponent<Joueur>();

        infoZone.taille = tailleZone;
        infoZone.centre = joueur.transform.transform.position;

        distanceCaméra = transform.position - infoZone.centre;

    }

    void Update()
    {
        if (joueur.v.magnitude > 0 || v.magnitude > 0.05f)
        {
            CalculerDéplacement();
        }
    }

    public void CalculerDéplacement()
    {
        v = Vector3.zero;
        infoZone.CalculBordure();
        CalculCentreZone();
        DéplacerCaméra();
    }

    void CalculCentreZone() //Décale le centre de la zone si le joueur en sort
    {
        Vector3 pos = joueur.transform.position;

        if (pos.z > infoZone.avant) v.z += pos.z - infoZone.avant;
        else if (pos.z < infoZone.arrière) v.z -= infoZone.arrière - pos.z;

        if (pos.x > infoZone.droite) v.x += pos.x - infoZone.droite;
        else if (pos.x < infoZone.gauche) v.x -= infoZone.gauche - pos.x;

        infoZone.centre += v * speed * Time.deltaTime;
    }

    void DéplacerCaméra()
    {
        gameObject.transform.position = infoZone.centre + distanceCaméra;
    }

    struct InfoZone
    {
        public float rotationX;
        public Vector3 centre;
        public float avant, arrière, droite, gauche;
        public Vector2 taille;


        public void CalculBordure()
        {
            avant = centre.z + taille.y/2;
            arrière = centre.z - taille.y/2;
            droite = centre.x + taille.x/2;
            gauche = centre.x - taille.x/2;
        }

        public float FonctionDroite(float z, float angle) //À compléter, prendre en compte le champ de vision et la hauteur de la caméra (mais dabord cherche si ca existe)
        {
            return Mathf.Tan(angle) * (z - avant) + gauche;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(infoZone.centre, new Vector3(infoZone.taille.x, 0, infoZone.taille.y));
        //Gizmos.DrawLine(new Vector3(infoZone.gauche, joueur.transform.position.y, infoZone.avant), new Vector3(infoZone.FonctionDroite(infoZone.arrière,caméra.transform.localRotation.x), joueur.transform.position.y, infoZone.arrière));
    }
}
