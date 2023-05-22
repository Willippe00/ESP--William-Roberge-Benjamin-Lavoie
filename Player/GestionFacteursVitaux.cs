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
    public float d�laiSoif = 20; //donc chaque 10 secondes
    public float d�laiFaim = 40; //donc chaque 20 secondes
    float d�gatSoif = 10;
    float d�gatFaim = 10;


    private void FixedUpdate()
    {
        UpdateFacteursVitaux();


    }

    void UpdateFacteursVitaux()
    {

        compteurSoif += Time.fixedDeltaTime;
        compteurFaim += Time.fixedDeltaTime;

        if (compteurSoif > d�laiSoif)
        {
            AjouterSurBarre(barreSoif.name, -1);
            compteurSoif = 0;

            if (compteurFaim > d�laiFaim)
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
        if (barre == barreFaim) AjouterSurBarre(barreVie.name, -d�gatFaim);
        if (barre == barreSoif) AjouterSurBarre(barreVie.name, -d�gatSoif);
        else if (barre == barreVie) gameObject.GetComponent<Joueur>().TuerJoueur();
    }


}
