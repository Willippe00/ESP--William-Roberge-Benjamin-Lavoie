using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouvementJoueur : MonoBehaviour
{
    [SerializeField] Joueur joueur;
    [SerializeField] G�rerEntr�es scriptG�rEntr�es;

    [SerializeField] public float vitesseMax = 10;
    [SerializeField] float acc�l�ration = 1;
    [SerializeField] float d�c�l�ration = 2;

    public Vector3 Mouvement(Vector3 v)
    {
        Vector2 entr�eMovement = scriptG�rEntr�es.entr�eMovement;
        joueur.infoJoueur.entr�es = entr�eMovement;
        Acc�l�rationJoueur(ref v, entr�eMovement);
        if (v.x != 0 && entr�eMovement.x == 0) v.x = D�c�l�ration(v.x);
        if (v.z != 0 && entr�eMovement.y == 0) v.z = D�c�l�ration(v.z);

        if (v.magnitude > vitesseMax) return v.normalized * vitesseMax;
        else return v;
    }

    #region CALCULS_MOUVEMENT

    public void Acc�l�rationJoueur(ref Vector3 v, Vector2 entr�eMovement)
    {
        v += new Vector3(entr�eMovement.x * acc�l�ration, 0, entr�eMovement.y * acc�l�ration) * Time.deltaTime;

        if (Mathf.Abs(v.x) > vitesseMax) v.x = Mathf.Sign(v.x) * vitesseMax; //on s'assure que la velocit� ne d�passe pas le max peut-importe la direction
        if (Mathf.Abs(v.z) > vitesseMax) v.z = Mathf.Sign(v.z) * vitesseMax;
    }

    public float D�c�l�ration(float velocity)
    {
        if (Mathf.Abs(velocity) > 0.05)
        {
            float direction = Mathf.Sign(velocity);
            velocity -= direction * d�c�l�ration * Time.deltaTime; //Time.deltaTime car c'est une acc�l�ration
            if (direction * velocity < 0) velocity = 0; //test si le signe du vecteur modifier a changer de signe (donc que la deceleration la fait changer de direction), si cest le cas le v.x devient 0
        }
        else velocity = 0;

        return velocity;
    }

    #endregion
}
