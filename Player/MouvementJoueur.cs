using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouvementJoueur : MonoBehaviour
{
    [SerializeField] Joueur joueur;
    [SerializeField] GérerEntrées scriptGérEntrées;

    [SerializeField] public float vitesseMax = 10;
    [SerializeField] float accélération = 1;
    [SerializeField] float décélération = 2;

    public Vector3 Mouvement(Vector3 v)
    {
        Vector2 entréeMovement = scriptGérEntrées.entréeMovement;
        joueur.infoJoueur.entrées = entréeMovement;
        AccélérationJoueur(ref v, entréeMovement);
        if (v.x != 0 && entréeMovement.x == 0) v.x = Décélération(v.x);
        if (v.z != 0 && entréeMovement.y == 0) v.z = Décélération(v.z);

        if (v.magnitude > vitesseMax) return v.normalized * vitesseMax;
        else return v;
    }

    #region CALCULS_MOUVEMENT

    public void AccélérationJoueur(ref Vector3 v, Vector2 entréeMovement)
    {
        v += new Vector3(entréeMovement.x * accélération, 0, entréeMovement.y * accélération) * Time.deltaTime;

        if (Mathf.Abs(v.x) > vitesseMax) v.x = Mathf.Sign(v.x) * vitesseMax; //on s'assure que la velocité ne dépasse pas le max peut-importe la direction
        if (Mathf.Abs(v.z) > vitesseMax) v.z = Mathf.Sign(v.z) * vitesseMax;
    }

    public float Décélération(float velocity)
    {
        if (Mathf.Abs(velocity) > 0.05)
        {
            float direction = Mathf.Sign(velocity);
            velocity -= direction * décélération * Time.deltaTime; //Time.deltaTime car c'est une accélération
            if (direction * velocity < 0) velocity = 0; //test si le signe du vecteur modifier a changer de signe (donc que la deceleration la fait changer de direction), si cest le cas le v.x devient 0
        }
        else velocity = 0;

        return velocity;
    }

    #endregion
}
