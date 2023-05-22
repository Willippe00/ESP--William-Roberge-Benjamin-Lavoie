using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flèche : MonoBehaviour
{
    [SerializeField] LayerMask layers;
    [HideInInspector] public GestionInventaire scriptGestInv;

    float vitesse = 15;
    Vector3 gravité = Physics.gravity / 3;
    Vector3 v;

    public float dégats = 5;

    private void Awake()
    {
        v = transform.forward * vitesse;
    }

    private void Update()
    {
        v += gravité * Time.deltaTime;
        if (v != Vector3.zero) transform.LookAt(transform.position - v);
        Collision(v * Time.deltaTime);
    }

    private void Collision(Vector3 v)
    {
        GameObject collision = FonctionRayCast.DoubleRayCast(transform, layers, v.magnitude, 0.1f);
        transform.position += v;

        if (collision || transform.position.y < 0)
        {
            scriptGestInv.ApparaitreObjet(12, new Vector3(transform.position.x, 0, transform.position.z), false, 1);
            Destroy(gameObject);
        }

    }
}
