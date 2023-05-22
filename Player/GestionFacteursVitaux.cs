using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GestionFacteursVitaux : MonoBehaviour
{
    [SerializeField] Slider barreVie;
    [SerializeField] Slider barreFaim;
    [SerializeField] Slider barreSoif;

    float compteurSoif = 0;
    float compteurFaim = 0;
    public float délaiSoif = 20; //donc chaque 10 secondes
    public float délaiFaim = 40; //donc chaque 20 secondes
    float dégatSoif = 10;
    float dégatFaim = 10;


    private void FixedUpdate()
    {
        UpdateFacteursVitaux();


    }

    void UpdateFacteursVitaux()
    {

        compteurSoif += Time.fixedDeltaTime;
        compteurFaim += Time.fixedDeltaTime;

        if (compteurSoif > délaiSoif)
        {
            AjouterSurBarre(barreSoif.name, -1);
            compteurSoif = 0;

            if (compteurFaim > délaiFaim)
            {
                AjouterSurBarre(barreFaim.name, -1);
                compteurFaim = 0;
            }
        }
    }

    public void RemplirVitaux()
    {
        barreVie.value = barreVie.maxValue;
        barreFaim.value = barreFaim.maxValue;
        barreSoif.value = barreSoif.maxValue;
    }

    public void AjouterSurBarre(string nomBarre, float ajout) //Utiliser pour modifier la valeur d'un des facteurs vitaux (ex: diminuer la soif)
    {
        Slider barre = nomBarre != barreVie.name ? (nomBarre == barreFaim.name ? barreFaim : barreSoif) : barreVie;
        barre.value = Mathf.Max(0, Mathf.Min(barre.maxValue, barre.value + ajout));

        if (barre.value <= 0) EffetBarreVide(barre);
    }

    void EffetBarreVide(Slider barre)
    {
        if (barre == barreFaim) AjouterSurBarre(barreVie.name, -dégatFaim);
        if (barre == barreSoif) AjouterSurBarre(barreVie.name, -dégatSoif);
        else if (barre == barreVie) gameObject.GetComponent<Joueur>().TuerJoueur();
    }


}
