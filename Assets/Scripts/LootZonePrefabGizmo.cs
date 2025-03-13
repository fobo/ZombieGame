using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class LootZonePrefabGizmo : MonoBehaviour
{
    public enum LootZoneShape
    {
        L_Shape,
        L_Shape_Mirrored,
        L_Shape_Flipped,
        L_Shape_FlippedMirrored,
        Square,
        Long,
        Circle,
        Hollow_Square
    }

    public LootZoneShape selectedShape = LootZoneShape.Square; // Default shape
    public Vector2 prefabSize = new Vector2(5, 5); // Manual override size
    public Color gizmoColor = new Color(1, 0, 1, 0.5f); // Purple

    private void OnDrawGizmos()
    {
        DrawPrefabGizmo(gizmoColor);
    }

    private void OnDrawGizmosSelected()
    {
        DrawPrefabGizmo(new Color(1, 1, 0, 1)); // Yellow when selected
    }

    private void DrawPrefabGizmo(Color color)
    {
        Gizmos.color = color;

        switch (selectedShape)
        {
            case LootZoneShape.L_Shape:
                DrawLShape(false, false);
                break;
            case LootZoneShape.L_Shape_Mirrored:
                DrawLShape(true, false);
                break;
            case LootZoneShape.L_Shape_Flipped:
                DrawLShape(false, true);
                break;
            case LootZoneShape.L_Shape_FlippedMirrored:
                DrawLShape(true, true);
                break;
            case LootZoneShape.Square:
                Gizmos.DrawWireCube(transform.position, new Vector3(prefabSize.x, prefabSize.y, 0));
                break;
            case LootZoneShape.Long:
                Gizmos.DrawWireCube(transform.position, new Vector3(prefabSize.x * 2, prefabSize.y / 2, 0));
                break;
            case LootZoneShape.Circle:
                DrawCircle();
                break;
            case LootZoneShape.Hollow_Square:
                DrawHollowSquare();
                break;

        }

#if UNITY_EDITOR
        Handles.Label(transform.position, selectedShape.ToString());
#endif
    }

private void DrawLShape(bool mirror, bool flip)
{
    Vector3 center = transform.position;
    float width = prefabSize.x;
    float height = prefabSize.y;

    // Define corner positions relative to center
    Vector3 horizontalCenter = center + new Vector3(0, -height / 4, 0); // Slightly below center
    Vector3 verticalCenter = center + new Vector3(-width / 4, 0, 0);   // Slightly left of center

    Vector3 horizontalSize = new Vector3(width, height / 2, 0); // Wide but shorter
    Vector3 verticalSize = new Vector3(width / 2, height, 0);   // Tall but narrower

    // Mirroring (Flip horizontally)
    if (mirror)
    {
        verticalCenter.x = -verticalCenter.x;
    }

    // Flipping (Flip vertically)
    if (flip)
    {
        horizontalCenter.y = -horizontalCenter.y;
    }

    // Draw two overlapping rectangles to form an L
    Gizmos.DrawWireCube(horizontalCenter, horizontalSize);
    Gizmos.DrawWireCube(verticalCenter, verticalSize);
}




    private void DrawCircle()
    {
        Vector3 center = transform.position;
        float radius = prefabSize.x / 2;
        int segments = 20;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = (i / (float)segments) * Mathf.PI * 2;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }

    private void DrawHollowSquare()
    {
        Vector3 center = transform.position;
        float halfX = prefabSize.x / 2;
        float halfY = prefabSize.y / 2;

        Gizmos.DrawWireCube(center, new Vector3(prefabSize.x, prefabSize.y, 0));
        Gizmos.DrawWireCube(center, new Vector3(prefabSize.x - 2, prefabSize.y - 2, 0));
    }


}
