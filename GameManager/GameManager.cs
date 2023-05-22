using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> listeObjetSol;
    public List<GameObject> listeStationPlacer;
    [SerializeField] public List<GameObject> joueurs;

    [HideInInspector] public bool inventairePlein = false;

    void awake()
    {
        listeObjetSol = new List<GameObject>();
        listeStationPlacer = new List<GameObject>();
    }
}
