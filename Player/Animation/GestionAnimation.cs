using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionAnimation : MonoBehaviour
{
    [SerializeField] Animator animateur;
    [SerializeField] Joueur joueur;

    [SerializeField] Transform repèreRotation;

    float quantitéMouvement = 0;
    Vector2 direction = Vector2.one;

    public void Animation()
    {
        CalculerMouvementAnimation();
        CalculerDirectionCorps();
    }

    #region CALCUL_ANIMATION

    private void CalculerMouvementAnimation() //calcul la quantité de mouvement qui sera transmise dans l'animation du personnage
    {
        quantitéMouvement = Mathf.Max(Mathf.Abs(joueur.v.x),Mathf.Abs(joueur.v.z)) / joueur.scriptMouvJoueur.vitesseMax;
        animateur.SetFloat("QuantitéMouvement", quantitéMouvement);
    }

    private void CalculerDirectionCorps()
    {
        direction = joueur.infoJoueur.entrées;
        if (direction.magnitude > 0.1)
        {
            direction = direction.normalized;
            repèreRotation.LookAt(joueur.transform.position + new Vector3(direction.x, 0, direction.y));
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
