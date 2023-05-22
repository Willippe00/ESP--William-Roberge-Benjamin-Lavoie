using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GénérationRessource : MonoBehaviour
{

    [SerializeField] CréationIle ile;
    [SerializeField] Transform parentRessource;

    public Queue<Vector2> ressourceDétruite = new Queue<Vector2>();
    List<Vector2> constructionsEtRessources = new List<Vector2>(); //Au moment de la création contient uniquement les ressources, après uniquement les constructions

    [SerializeField] List<GameObject> ressourcePlaine;
    [SerializeField] List<GameObject> ressourceGlace;
    [SerializeField] List<GameObject> ressourceDésert;
    [SerializeField] List<GameObject> ressourceForêt;
    [SerializeField] List<GameObject> ressourcePlage;

    [SerializeField] float tempsRégén = 30;
    float tempsRestant = 0;

    [SerializeField] float quantitéRessource = 0.05f;

    private void Start()
    {
        GénérerRessources();
    }

    public void GénérerRessources()
    {
        for (int i = 0; i < Mathf.Ceil(ile.tailleCarte.x * ile.tailleCarte.y * quantitéRessource * ile.tailleCase); i++)
        {
            CréationRessource(new Vector2(Random.Range(0f, ile.tailleCarte.x), Random.Range(0f, ile.tailleCarte.y)), true);
        }
        constructionsEtRessources = new List<Vector2>();
    }



    #region GestionRessources

    public void RessourceDétruite(Vector2 pos)
    {
        ressourceDétruite.Enqueue(pos);
    }


    void CréationRessource(Vector2 pos, bool premièreCréation)
    {
        if (!premièreCréation) pos = new Vector2((pos.x + ile.tailleCase / 2) / ile.tailleCase, (pos.y + ile.tailleCase / 2) / ile.tailleCase);

        GameObject prefab = null;
        switch (RécupérerBiome(pos))
        {
            case 'O': //Commun donc placé pour éviter de passer par chacun pour rien
                break;

            case 'R':
                prefab = ressourcePlaine[Random.Range(0, ressourcePlaine.Count-1)];
                break;

            case 'G':
                prefab = ressourceGlace[Random.Range(0, ressourceGlace.Count - 1)];
                break;

            case 'D':
                prefab = ressourceDésert[Random.Range(0, ressourceDésert.Count - 1)];
                break;

            case 'F':
                prefab = ressourceForêt[Random.Range(0, ressourceForêt.Count - 1)];
                break;

            case 'P':
                prefab = ressourcePlage[Random.Range(0, ressourcePlage.Count - 1)];
                break;
        }
        if(prefab != null)
        {
            GameObject objet = Instantiate(prefab, new Vector3(pos.x * ile.tailleCase - ile.tailleCase / 2, 0, pos.y * ile.tailleCase - ile.tailleCase / 2), Quaternion.Euler(prefab.transform.rotation.eulerAngles.x, Random.Range(0,360), prefab.transform.rotation.eulerAngles.z), parentRessource);
        }
    }


    bool VérifierAjoutRessource(Vector2 posNouvelleRessource, Vector3 taille) //Regarde si la ressource créer superpose une autre, ou une construction
    {
        taille /= 2;
        foreach (Vector2 pos in constructionsEtRessources)
        {
            if (Mathf.Abs(pos.x - posNouvelleRessource.x) < taille.x +1 || Mathf.Abs(pos.y - posNouvelleRessource.y) < taille.z +1) return false;
        }
        return true;
    }

    char RécupérerBiome(Vector2 pos)
    {
        return ile.carte[Mathf.CeilToInt(pos.x)-1, Mathf.CeilToInt(pos.y)-1];
    }

    #endregion


    #region RegénérationRessource
    void RegénérationRessource()
    {
        tempsRestant += Time.fixedDeltaTime;
        if (tempsRestant > tempsRégén)
        {
            if (ressourceDétruite.Count > 0) CréationRessource(ressourceDétruite.Dequeue(), false);
            tempsRestant = 0;
        }
    }

    #endregion


    private void FixedUpdate()
    {
        RegénérationRessource();
    }

}
