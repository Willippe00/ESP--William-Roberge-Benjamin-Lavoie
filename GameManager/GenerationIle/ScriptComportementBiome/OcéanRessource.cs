using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcéanRessource : MonoBehaviour, RessourceTerrain
{
    [SerializeField] GameObject ressourceSortante;
    public GameObject préfabOjetRésultant => ressourceSortante;

    public char symbole => 'O';

    
}
