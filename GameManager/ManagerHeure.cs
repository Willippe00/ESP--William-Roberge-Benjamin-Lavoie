using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ManagerHeure : MonoBehaviour
{
    [SerializeField] Transform soleil;
    [SerializeField] Light lumi�reSoleil;
    [SerializeField] Transform lune;
    [SerializeField] Light lumi�reLune;
    [SerializeField] Text cadran;

    [SerializeField] float intensit�Soleil = 1;
    [SerializeField] float intensit�Lune = 0.5f;

    ManagerPnj gestionnairePnj;

    public float heure = 12;
    [SerializeField] float facteurTemps = 60;


    void Start()
    {
        facteurTemps = 1/facteurTemps;
        gestionnairePnj = GetComponent<ManagerPnj>();
        gestionnairePnj.ApparitionJournaliere();
        gestionnairePnj.ApparitonD�butJeu();
    }

    void Update()
    {
        G�rerHeure();
    }

    public void G�rerHeure()
    {
        heure %= 24;

        float heureSuivante = heure;
        if (heure > 7 && heure < 19) heureSuivante += Time.deltaTime * facteurTemps;
        else heureSuivante += Time.deltaTime * facteurTemps *3;

        if (Mathf.Floor(heure) < Mathf.Floor(heureSuivante)) gestionnairePnj.V�rifierApparition(Mathf.Floor(heure));

        heure = heureSuivante;
        G�rerPositionSoleil(heure);
        ChangerAffichageHeure(heure);
    }


    void G�rerPositionSoleil(float heure) // pour am�lior� https://docs.unity3d.com/ScriptReference/Transform-rotation.html
    {
        lumi�reSoleil.intensity = intensit�Soleil * Intensit�Astre();
        lumi�reLune.intensity = intensit�Lune * (1 - lumi�reSoleil.intensity/intensit�Soleil);

        float angleRotation = (360 * (heure / 24) - 100);
        soleil.rotation = Quaternion.Euler(angleRotation, 0, 0);
        lune.rotation = Quaternion.Euler(angleRotation, 0, 0);
    }

    float Intensit�Astre()
    {
        if(heure > 6 && heure < 19) return Mathf.Max(0, -1f / 120f * (heure - 12f) * (heure - 12f) + 1f);
        else return 0;
    }

    void ChangerAffichageHeure(float heure)
    {
        int minute = Mathf.FloorToInt((heure - Mathf.FloorToInt(heure)) * 60);
        if(minute<10) cadran.text = $"{Mathf.FloorToInt(heure)}:0{minute}";
        else cadran.text = $"{Mathf.FloorToInt(heure)}:{minute}";
    }


}
