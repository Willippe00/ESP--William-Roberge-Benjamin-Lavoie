using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Spline
{

    public static Vector2 CubicLerp(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        return 
            Mathf.Pow((1 - t), 3) * a + 
            3 * Mathf.Pow((1 - t), 2) * t * b + 
            3 * (1 - t) * (t * t) * c + 
            (t * t * t) * d;
    }

    public static void CréerCentre(ref List<Vector2> points, Vector2 centre)
    {
        points = new List<Vector2>
        {
            centre+Vector2.left,
            centre+(Vector2.left+Vector2.up)*.5f,
            centre + (Vector2.right + Vector2.down) * .5f,
            centre + Vector2.right
        };
    }

    public static void CréerDébut(ref List<Vector2> points, Vector2 début)
    {
        points = new List<Vector2>
        {
            début,
            début+Vector2.up*.5f,
            début + (Vector2.right*2 + Vector2.down) * .5f,
            début + Vector2.right*2
        };


    }


    public static void AjouterSegment(ref List<Vector2> points, Vector2 ancre)
    {
        points.Add(points[points.Count-1] *2 - points[points.Count - 2]);
        points.Add((points[points.Count-1] + ancre)/2);
        points.Add(ancre);
    }

    public static Vector2[] AccéderPoints(List<Vector2> pts, int i)
    {
        return new Vector2[] { pts[i*3], pts[i*3+1], pts[i*3+2], pts[i*3+3] };
    }

    public static Vector2[] CalculerPtsDistÉgales(List<Vector2> pts, float espace, float qualité) //sert à répartir des pts également le long du segment, pour pouvoir calculer une vitesse régulière
    {
        List<Vector2> ptsDistÉgales = new();
        ptsDistÉgales.Add(pts[0]);
        Vector2 ptsPrécédent = pts[0];
        float distDuDernierPts = 0;

        Vector2[] p = AccéderPoints(pts, 0);
        float distanceControle = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]); //calcule la distance entre les ancres et les points de contrôle
        float longueurCourbe = Vector2.Distance(p[0], p[3]) + distanceControle / 2f; //estime la longeur de la courbe avec la distance entre les deux points + la moitié de la distance entre les pts de controle
        int division = Mathf.CeilToInt(longueurCourbe * qualité * 10); //plus la qualité est haute, plus la distance entre les pts de la courbes seront rapprochés
        float t = 0;
        while (t <= 1) 
        {
            t += 1f / division;
            Vector2 ptsSurCourbe = CubicLerp(p[0], p[1], p[2], p[3], t);
            distDuDernierPts += Vector2.Distance(ptsPrécédent, ptsSurCourbe);

            while (distDuDernierPts >= espace) //On accepte que le point puisse être légèrement plus proche que l'espace voulu
            {
                float surplusDistance = distDuDernierPts - espace; 
                Vector2 nouveauPtsDistÉgale = ptsSurCourbe + (ptsPrécédent - ptsSurCourbe).normalized * surplusDistance; //On recule le pts autant que le surplus de distance
                ptsDistÉgales.Add(nouveauPtsDistÉgale);
                distDuDernierPts = surplusDistance;
                ptsPrécédent = nouveauPtsDistÉgale;  
            }

            ptsPrécédent = ptsSurCourbe;
        }

        return ptsDistÉgales.ToArray();
    }


}
