using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlageRessource : MonoBehaviour, RessourceTerrain
{
    [SerializeField] GameObject ressourceSortante;
    public GameObject pr�fabOjetR�sultant => ressourceSortante;

    public char symbole => 'P';

}
