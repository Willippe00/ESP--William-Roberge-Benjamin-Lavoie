using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ManagerHeure : MonoBehaviour
{
    [SerializeField] Transform soleil;
    [SerializeField] Light lumièreSoleil;
    [SerializeField] Transform lune;
    [SerializeField] Light lumièreLune;
    [SerializeField] Text cadran;

    [SerializeField] float intensitéSoleil = 1;
    [SerializeField] float intensitéLune = 0.5f;

    ManagerPnj gestionnairePnj;

    public float heure = 12;
    [SerializeField] float facteurTemps = 60;


    void Start()
    {
        facteurTemps = 1/facteurTemps;
        gestionnairePnj = GetComponent<ManagerPnj>();
        gestionnairePnj.ApparitionJournaliere();
        gestionnairePnj.ApparitonDébutJeu();
    }

    void Update()
    {
        GérerHeure();
    }

    public void GérerHeure()
    {
        heure %= 24;

        float heureSuivante = heure;
        if (heure > 7 && heure < 19) heureSuivante += Time.deltaTime * facteurTemps;
        else heureSuivante += Time.deltaTime * facteurTemps *3;

        if (Mathf.Floor(heure) < Mathf.Floor(heureSuivante)) gestionnairePnj.VérifierApparition(Mathf.Floor(heure));

        heure = heureSuivante;
        GérerPositionSoleil(heure);
        ChangerAffichageHeure(heure);
    }


    void GérerPositionSoleil(float heure) // pour amélioré https://docs.unity3d.com/ScriptReference/Transform-rotation.html
    {
        lumièreSoleil.intensity = intensitéSoleil * IntensitéAstre();
        lumièreLune.intensity = intensitéLune * (1 - lumièreSoleil.intensity/intensitéSoleil);

        float angleRotation = (360 * (heure / 24) - 100);
        soleil.rotation = Quaternion.Euler(angleRotation, 0, 0);
        lune.rotation = Quaternion.Euler(angleRotation, 0, 0);
    }

    float IntensitéAstre()
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
