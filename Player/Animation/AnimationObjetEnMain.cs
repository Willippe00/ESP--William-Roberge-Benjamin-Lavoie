using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObjetEnMain : MonoBehaviour
{
    [SerializeField] Vector3 axe = new Vector3(1, 0, 0);
    [SerializeField] float étendue = 45;
    [SerializeField] float temps = 2;
    [SerializeField] string type = "Outil";

    float marqueurTemps = 0;



    private void Update()
    {
        marqueurTemps += Time.deltaTime;

        if (marqueurTemps < temps)
        {
            float vitesse = (étendue / temps) * Time.deltaTime;
            switch (type)
            {
                case "Lance":
                    AnimationLance(vitesse);
                    break;

                case "Épée":
                    AnimationÉpée(vitesse);
                    break;

                case "Arc":
                    break;

                default:
                    AnimationBase(vitesse);
                    break;
            }
        }
        else Destroy(gameObject);
    }

    void AnimationBase(float vitesse)
    {
        transform.Rotate(axe, vitesse);
    }

    void AnimationÉpée(float vitesse)
    {
        if (marqueurTemps < temps * 0.7f) transform.Rotate(axe, vitesse);
        else transform.Rotate(axe, vitesse * 0.2f);
    }

    void AnimationLance(float vitesse)
    {
        if(marqueurTemps < temps * 0.7f) transform.Translate(Vector3.forward * vitesse);
        else transform.Translate(-Vector3.forward * vitesse * 0.6f);
    }
}
