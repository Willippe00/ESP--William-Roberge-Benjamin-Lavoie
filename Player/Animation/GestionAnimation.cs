using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionAnimation : MonoBehaviour
{
    [SerializeField] Animator animateur;
    [SerializeField] Joueur joueur;

    [SerializeField] Transform rep�reRotation;

    float quantit�Mouvement = 0;
    Vector2 direction = Vector2.one;

    public void Animation()
    {
        CalculerMouvementAnimation();
        CalculerDirectionCorps();
    }

    #region CALCUL_ANIMATION

    private void CalculerMouvementAnimation() //calcul la quantit� de mouvement qui sera transmise dans l'animation du personnage
    {
        quantit�Mouvement = Mathf.Max(Mathf.Abs(joueur.v.x),Mathf.Abs(joueur.v.z)) / joueur.scriptMouvJoueur.vitesseMax;
        animateur.SetFloat("Quantit�Mouvement", quantit�Mouvement);
    }

    private void CalculerDirectionCorps()
    {
        direction = joueur.infoJoueur.entr�es;
        if (direction.magnitude > 0.1)
        {
            direction = direction.normalized;
            rep�reRotation.LookAt(joueur.transform.position + new Vector3(direction.x, 0, direction.y));
        }
    }

    #endregion

    #region INTERACTION

    public void Prendre(bool valeur)
    {
        animateur.SetBool("Pickup", valeur);
    }
    #endregion
}
