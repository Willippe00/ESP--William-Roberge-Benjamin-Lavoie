using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] string menuPrincipale;
    [SerializeField] string chargement;
    [SerializeField] GameObject interfaces;


    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void RetournerMenu()
    {
        SceneManager.LoadScene(menuPrincipale, LoadSceneMode.Single);
    }

    public void ReprendreJeu()
    {
        gameObject.SetActive(false);
        interfaces.SetActive(true);
        Time.timeScale = 1;
    }


}

