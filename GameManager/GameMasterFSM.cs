using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// William ROberge 2022-02-03 

// base on GameMasterFSM de Vincent Echelard pfi session A2021         ------------ v�rifier pour plagia

public enum �tatsSc�ne { Sc�neTitre, Sc�neJeu, Sc�neFinale, Nb�tatsSc�ne }
public class GameMasterFSM : MonoBehaviour
{
    readonly string[] NomsSc�nes = { "Sc�neTitre", "Sc�neJeu", "Sc�neFinale" };

    public string MessageFinal { get; private set; }
    private static bool IsCreated;
    private static float D�laiTouche { get; set; }

    private static �tatsSc�ne SceneState;

    private static �tatsSc�ne nextSceneState;
    public static �tatsSc�ne NextSceneState
    {
        get { return nextSceneState; }
        set
        {
            if (value >= �tatsSc�ne.Sc�neTitre && value <= �tatsSc�ne.Sc�neFinale)
            {
                nextSceneState = value;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))// 02-03 Choisir bouton sortie d'urgence
        {
            Quitter();
        }


    }
    public void Quitter()
    {
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
       #else
            Application.Quit();
           #endif

    }

    private void EffectuerTransition()
    {
        SceneManager.LoadScene(NomsSc�nes[(int)NextSceneState], LoadSceneMode.Single);
        SceneState = NextSceneState;
    }

    public void ProchaineSc�ne()
    {
        NextSceneState = (�tatsSc�ne)(((int)SceneState + 1) % (int)�tatsSc�ne.Nb�tatsSc�ne);
    }

    public void Sc�nePr�c�dente()
    {
        NextSceneState = SceneState != �tatsSc�ne.Sc�neTitre ? SceneState - 1 : �tatsSc�ne.Sc�neFinale;
    }

}
