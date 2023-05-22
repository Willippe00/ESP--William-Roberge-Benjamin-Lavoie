using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementObjet : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] float tempsVie = 60; //nombre de seconde avant que lobjet disparaisse
    float heureMort;
    float heureImobile = -1;

    [SerializeField] float vitesseD�placement = 4;
    public float vitesseR�cup�ration = 1; //est diff�rente si l'objet est lach� par le joueur

    Vector3 posFinale;
    public bool immobile = false;

    float forceOscillation = .2f;
    float vitesseOscillation = 3;

    public int id = 0;


    public void LoadSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }


    public void Apparaitre()
    {
        gameManager = gameObject.GetComponentInParent<GameManager>();
        gameManager.listeObjetSol.Add(gameObject);

        if (gameManager.inventairePlein) vitesseR�cup�ration = 0;

        heureMort = Time.time + tempsVie;

        Vector2 pos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        posFinale = new Vector3(pos.x, 0, pos.y);
        posFinale = posFinale + new Vector3(transform.position.x, 0, transform.position.z);
    }

    void Disparaitre()
    {
        gameManager.listeObjetSol.Remove(gameObject);
        Destroy(gameObject);
    }

    void OscillationObjet()
    {
        if (heureImobile == -1) heureImobile = Time.time;
        transform.position = new Vector3(transform.position.x, 1 * forceOscillation + posFinale.y + (Mathf.Cos(Mathf.PI + (Time.time) * vitesseOscillation - heureImobile) * forceOscillation), transform.position.z);
    }

    void D�placement()
    {
        transform.position = Vector3.MoveTowards(transform.position, posFinale, Time.deltaTime * vitesseD�placement);
        if (transform.position == posFinale) immobile = true;
    }

    public void Attirer(Transform joueur, float distance)
    {
        transform.position = Vector3.MoveTowards(transform.position, joueur.position, Time.deltaTime * vitesseR�cup�ration / distance);
    }

    public void Update()
    {
        if (heureMort < Time.time) Disparaitre();

        if (!immobile) D�placement();
        else OscillationObjet();
    }

}
