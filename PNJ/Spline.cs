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

    public static void Cr�erCentre(ref List<Vector2> points, Vector2 centre)
    {
        points = new List<Vector2>
        {
            centre+Vector2.left,
            centre+(Vector2.left+Vector2.up)*.5f,
            centre + (Vector2.right + Vector2.down) * .5f,
            centre + Vector2.right
        };
    }

    public static void Cr�erD�but(ref List<Vector2> points, Vector2 d�but)
    {
        points = new List<Vector2>
        {
            d�but,
            d�but+Vector2.up*.5f,
            d�but + (Vector2.right*2 + Vector2.down) * .5f,
            d�but + Vector2.right*2
        };


    }


    public static void AjouterSegment(ref List<Vector2> points, Vector2 ancre)
    {
        points.Add(points[points.Count-1] *2 - points[points.Count - 2]);
        points.Add((points[points.Count-1] + ancre)/2);
        points.Add(ancre);
    }

    public static Vector2[] Acc�derPoints(List<Vector2> pts, int i)
    {
        return new Vector2[] { pts[i*3], pts[i*3+1], pts[i*3+2], pts[i*3+3] };
    }

    public static Vector2[] CalculerPtsDist�gales(List<Vector2> pts, float espace, float qualit�) //sert � r�partir des pts �galement le long du segment, pour pouvoir calculer une vitesse r�guli�re
    {
        List<Vector2> ptsDist�gales = new();
        ptsDist�gales.Add(pts[0]);
        Vector2 ptsPr�c�dent = pts[0];
        float distDuDernierPts = 0;

        Vector2[] p = Acc�derPoints(pts, 0);
        float distanceControle = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]); //calcule la distance entre les ancres et les points de contr�le
        float longueurCourbe = Vector2.Distance(p[0], p[3]) + distanceControle / 2f; //estime la longeur de la courbe avec la distance entre les deux points + la moiti� de la distance entre les pts de controle
        int division = Mathf.CeilToInt(longueurCourbe * qualit� * 10); //plus la qualit� est haute, plus la distance entre les pts de la courbes seront rapproch�s
        float t = 0;
        while (t <= 1) 
        {
            t += 1f / division;
            Vector2 ptsSurCourbe = CubicLerp(p[0], p[1], p[2], p[3], t);
            distDuDernierPts += Vector2.Distance(ptsPr�c�dent, ptsSurCourbe);

            while (distDuDernierPts >= espace) //On accepte que le point puisse �tre l�g�rement plus proche que l'espace voulu
            {
                float surplusDistance = distDuDernierPts - espace; 
                Vector2 nouveauPtsDist�gale = ptsSurCourbe + (ptsPr�c�dent - ptsSurCourbe).normalized * surplusDistance; //On recule le pts autant que le surplus de distance
                ptsDist�gales.Add(nouveauPtsDist�gale);
                distDuDernierPts = surplusDistance;
                ptsPr�c�dent = nouveauPtsDist�gale;  
            }

            ptsPr�c�dent = ptsSurCourbe;
        }

        return ptsDist�gales.ToArray();
    }


}
