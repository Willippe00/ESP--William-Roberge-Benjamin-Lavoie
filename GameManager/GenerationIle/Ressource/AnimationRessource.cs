using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRessource : MonoBehaviour
{
    Vector3 posIniObjetAgiter;
    GameObject objetAgiter;
    float vitesseAgitation = 1;
    float forceAgitation = .1f;
    float tempsAgitation = 0;
    float dur�eAgitation = .5f;

    GameObject arbreTombe;
    float tempsArbreTombe = 0;
    float dur�eArbreTombe = 1;
    float vitesseChuteArbreTombe = 0.5f;
    Vector3 axeChute;


    private void Update()
    {
        if (arbreTombe != null) ArbreTombe();
        else if (objetAgiter != null) Agiter();
    }

    #region AGITATION_RESSOURCE

    public void ActiveAgitation(GameObject objetAgiter, float dur�eAgitation, float vitesseAgitation, float forceAgitation)
    {
        posIniObjetAgiter = objetAgiter.transform.position;
        this.forceAgitation = forceAgitation;
        this.objetAgiter = objetAgiter;
        this.dur�eAgitation = dur�eAgitation;
        this.vitesseAgitation = vitesseAgitation;
    }


    void Agiter()
    {
        Transform ressource = objetAgiter.transform;
        ressource.position = new Vector3(
            posIniObjetAgiter.x + (Mathf.Sin(Time.time * vitesseAgitation) * forceAgitation) / (1f + tempsAgitation *5),
            ressource.position.y,
            posIniObjetAgiter.z + (Mathf.Sin(Time.time * vitesseAgitation) * forceAgitation) / (1f + tempsAgitation *5)
            );

        if (tempsAgitation > dur�eAgitation)
        {
            ressource.position = posIniObjetAgiter;
            tempsAgitation = 0;
            objetAgiter.GetComponent<Ressource>().V�rifierVie();
            objetAgiter = null;
        }
        else tempsAgitation += Time.deltaTime;
    }

    #endregion

    #region ANIMATION_CHUTEARBRE

    public void ActiverArbreTombe(GameObject arbreTombe, float dur�eArbreTombe, float vitesseChuteArbreTombe)
    {
        this.arbreTombe = arbreTombe;
        this.dur�eArbreTombe = dur�eArbreTombe;
        this.vitesseChuteArbreTombe = vitesseChuteArbreTombe;

        do { axeChute = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)); }
        while (axeChute.magnitude == 0f);
    }

    void ArbreTombe()
    {
        Transform arbre = arbreTombe.transform;

        if (tempsArbreTombe < dur�eArbreTombe - 0.4f) arbre.RotateAround(new Vector3(arbre.position.x, 0, arbre.position.z), axeChute, Time.deltaTime * Mathf.Pow(tempsArbreTombe, 2f) * vitesseChuteArbreTombe);


        if (tempsArbreTombe > dur�eArbreTombe)
        {
            tempsArbreTombe = 0;
            arbreTombe.GetComponent<Ressource>().Destruction();
            arbreTombe = null;
        }
        else tempsArbreTombe += Time.deltaTime;
    }


    #endregion

    #region PARTICULES_ARBRE

    #endregion
}
