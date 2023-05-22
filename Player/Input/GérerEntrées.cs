using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GérerEntrées : MonoBehaviour
{
    [SerializeField] MouvementJoueur scriptMouvJoueur;
    [SerializeField] GestionInventaire scriptGestInv;
    [SerializeField] GestionAnimation scriptGestAnim;
    [SerializeField] Interaction scriptInteraction;
    [SerializeField] GameObject menuPause;

    [SerializeField] float attenteInteraction = 0.8f;
    float baliseAttente = 0;

    public Vector2 entréeMovement;

    #region RECUP_TOUCHES_CLAVIER

    public void ToucheIntéragir(InputAction.CallbackContext value)
    {
        if (value.started && baliseAttente < Time.time && !menuPause.activeInHierarchy)
        {
            scriptInteraction.ToucheIntéragir();
            baliseAttente = Time.time + attenteInteraction;
        }
    }

    public void ToucheDéplacement(InputAction.CallbackContext value)
    {
        if (!menuPause.activeInHierarchy)
        {
            entréeMovement = value.ReadValue<Vector2>(); //Transformer l'entrée en Vector2 (les touches ou le joystick représente une direction en 2D)}
            scriptGestAnim.Prendre(false);
        }
    }

    public void ToucheInventaire(InputAction.CallbackContext value)
    {
        if (value.started && !menuPause.activeInHierarchy) scriptGestInv.ActiverInventaire(1);
    }

    public void ToucheBarreInventaire(InputAction.CallbackContext value)
    {
        if (!value.canceled && !menuPause.activeInHierarchy) scriptGestInv.SélectionnerCaseInventaire((int)value.ReadValue<float>());
    }

    public void TouchePause(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (menuPause.activeInHierarchy) menuPause.GetComponent<PauseMenu>().ReprendreJeu();
            else menuPause.SetActive(true);
        }
    }

    public void ToucheLacherObjet(InputAction.CallbackContext value)
    {
        if (value.started) scriptGestInv.LacherObjetEnMain();
    }

    #endregion

}
