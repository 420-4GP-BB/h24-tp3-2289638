using UnityEngine;

public class Utilitaires
{
    public static Vector3 color2vec(Color color)
    {
        return new Vector3(color.r, color.g, color.b);
    }

    public static Color vec2color(Vector3 vec, float alpha = 1.0f)
    {
        return new Color(vec.x, vec.y, vec.z, alpha);
    }

    public static GameObject ObjetSousSouris()
    {
        Vector3 positionSouris = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(positionSouris);

        foreach (var hit in Physics.RaycastAll(ray))
        {
            var actionnable = hit.collider.gameObject.GetComponent<IActionnable>();
            if (actionnable != null)
                return hit.collider.gameObject;
        }

        return null;
    }
}