using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// William ROberge 2022-02-03 

// base on GameMasterFSM de Vincent Echelard pfi session A2021         ------------ vérifier pour plagia

public enum ÉtatsScène { ScèneTitre, ScèneJeu, ScèneFinale, NbÉtatsScène }
public class GameMasterFSM : MonoBehaviour
{
    readonly string[] NomsScènes = { "ScèneTitre", "ScèneJeu", "ScèneFinale" };

    public string MessageFinal { get; private set; }
    private static bool IsCreated;
    private static float DélaiTouche { get; set; }

    private static ÉtatsScène SceneState;

    private static ÉtatsScène nextSceneState;
    public static ÉtatsScène NextSceneState
    {
        get { return nextSceneState; }
        set
        {
            if (value >= ÉtatsScène.ScèneTitre && value <= ÉtatsScène.ScèneFinale)
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
        SceneManager.LoadScene(NomsScènes[(int)NextSceneState], LoadSceneMode.Single);
        SceneState = NextSceneState;
    }

    public void ProchaineScène()
    {
        NextSceneState = (ÉtatsScène)(((int)SceneState + 1) % (int)ÉtatsScène.NbÉtatsScène);
    }

    public void ScènePrécédente()
    {
        NextSceneState = SceneState != ÉtatsScène.ScèneTitre ? SceneState - 1 : ÉtatsScène.ScèneFinale;
    }

}
