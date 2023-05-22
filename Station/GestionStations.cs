using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionStations : MonoBehaviour
{
    [SerializeField] GestionInventaire scriptGestInv;
    [SerializeField] Transform parentStations;

    [SerializeField] Joueur joueur;


    [SerializeField] Transform repèreRotation;
    [SerializeField] int layerCollision = 12;

    private GameObject stationEnMain;
    private Objet station = null;

    public List<string> nomsPrefabStations;
    public List<GameObject> prefabStation;

    bool emplacementValide = true;

    public void ControleStation(Objet station)
    {
        if (this.station == null || this.station.nom != station.nom) MettreStationEnMain(station);
        else PlacerStation();
    }

    public void MettreStationEnMain(Objet station)
    {

        int i = nomsPrefabStations.FindIndex(x => x == station.nom);
        if (i != -1)
        {
            this.station = station;
            GameObject prefab = prefabStation[i];

            stationEnMain = Instantiate(prefab, repèreRotation.position, repèreRotation.rotation);
            stationEnMain.transform.parent = repèreRotation.transform;
            stationEnMain.GetComponent<Station>().objet = station;
            stationEnMain.transform.position += repèreRotation.forward * Mathf.Max(0.7f,(prefab.transform.localScale.x * 1.1f));
            stationEnMain.transform.position += repèreRotation.up * (prefab.transform.localScale.x * 0.5f);

        }
    }

    private void PlacerStation()
    {
        if (emplacementValide)
        {
            stationEnMain.GetComponent<Station>().placer = true;
            stationEnMain.transform.parent = parentStations;
            stationEnMain.layer = layerCollision;
            stationEnMain = null;
            station = null;
            scriptGestInv.RetirerObjetCaseBarreInv(scriptGestInv.caseBarreSélectionner - 1);

        }
    }

    public void RetirerStationEnMain()
    {
        if(stationEnMain != null)
        {
            Destroy(stationEnMain);
            stationEnMain = null;
            station = null;
        }
    }


}
