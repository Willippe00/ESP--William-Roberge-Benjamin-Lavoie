using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementOiseau : MonoBehaviour
{
    CréationIle ile;

    List<Vector2> pts;
    Vector2[] segment;
    float tailleTerritoire = 10;
    Rect territoire;

    Vector2 v = new();
    float t = 0;
    int iInSegment = 0;
    [SerializeField] float qualité = 1f;
    [SerializeField] float espace = 1f;
    [SerializeField] float vitesse = 0.9f;


    private void Start()
    {
        ile = GetComponentInParent<CréationIle>();
        territoire = DéfinirTerritoire();

        Spline.CréerDébut(ref pts, new Vector2(transform.position.x, transform.position.z));
        segment = Spline.CalculerPtsDistÉgales(pts, espace, qualité);

    }

    private void Update()
    {
        CalculerDéplacement();

        Vector3 pos = new Vector3(v.x * ile.tailleCase, transform.position.y, v.y * ile.tailleCase);
        transform.LookAt(pos);
        transform.position = pos ;
    }

    private Rect DéfinirTerritoire()
    {
        Vector2 nid = new Vector2(transform.position.x, transform.position.z);
        return new Rect(nid.x-tailleTerritoire/2, nid.y - tailleTerritoire / 2, Mathf.Min(tailleTerritoire, (ile.tailleCarte.x - ile.limiteIle)-(nid.x + tailleTerritoire)), Mathf.Min(tailleTerritoire, (ile.tailleCarte.y - ile.limiteIle) - (nid.y + tailleTerritoire)));
    }

    private void CalculerTrajectoire()
    {
        Vector2[] segment = Spline.AccéderPoints(pts, 0);
        float vitesseSegment = Mathf.Min(0.4f,vitesse / Vector2.Distance(segment[0], segment[3]));
        t += Time.deltaTime * vitesseSegment;

        if(t >=1)
        {
            segment = AjouterSegment();
        }
        v = Spline.CubicLerp(segment[0], segment[1], segment[2], segment[3], t);
    }

    private void CalculerDéplacement()
    {
        t += Time.deltaTime * vitesse;
        if (t >= 1)
        {
            t--;
            iInSegment++;
            if (iInSegment >= segment.Length-1)
            {

                iInSegment = 0;
                AjouterSegment();
                segment = Spline.CalculerPtsDistÉgales(pts, espace, qualité);
            }
        }
        v = Vector2.Lerp(segment[iInSegment], segment[iInSegment+1], t);
    }

    private Vector2[] AjouterSegment()
    {
        
        Vector2 pos = new Vector2(Random.Range(territoire.xMin, territoire.xMax), Random.Range(territoire.yMin, territoire.yMax));
        Spline.AjouterSegment(ref pts, pos);

        //PositionerPtsControl();
        pts.RemoveRange(0, 3);

        return Spline.AccéderPoints(pts, 0);
    }

    private void PositionerPtsControl()
    {
        Vector2 dist1 = (pts[3] - pts[0]).normalized;
        Vector2 dist2 = (pts[6] - pts[3]).normalized;

        pts[2] = pts[3] + (dist1 - dist2);
        pts[4] = pts[3] - (dist1 - dist2);
    }

}
