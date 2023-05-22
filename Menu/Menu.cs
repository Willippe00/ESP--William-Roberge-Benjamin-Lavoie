using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] string jeu;
    [SerializeField] string chargement;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void LancerJeu()
    {
        SceneManager.LoadScene(jeu);
    }

    public void Options()
    {
        

    }
}

