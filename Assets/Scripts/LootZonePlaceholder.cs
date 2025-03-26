using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//[ExecuteAlways] // Allows Gizmos to be drawn in Edit Mode
public class LootZonePlaceholder : MonoBehaviour
{
    public enum LootZoneShape
    {
        L_Shape,
        L_Shape_Mirrored,
        L_Shape_Flipped,
        L_Shape_FlippedMirrored,
        Long,
        Circle,
        Hollow_Square,
        Square
    }

    public LootZoneShape selectedShape = LootZoneShape.L_Shape;
    public int tier = 1;
    public Vector2 prefabSize = new Vector2(5, 5); // Manual override size

    private void Start()
    {
        int upgradedTier = TryUpgradeTier(tier);
        GameObject zoneInstance = LootZoneManager.Instance.GetRandomPrefabForZone(selectedShape.ToString(), upgradedTier);

        if (zoneInstance != null)
        {
            GameObject spawnedZone = Instantiate(zoneInstance, transform.position, Quaternion.identity);
            //Debug.Log($"[LootZone] Spawned zone prefab '{spawnedZone.name}' at Tier {upgradedTier}");

            // Apply TC to all Chests and Structures within the zone
            Chest[] chests = spawnedZone.GetComponentsInChildren<Chest>(true);
            foreach (var chest in chests)
            {
                int tc = Util.GetTCForZoneTier(upgradedTier, true);
                chest.SetTreasureClass(tc);
                //Debug.Log($"[LootZone] Assigned TC {tc} to Chest '{chest.gameObject.name}'");
            }

            Structure[] structures = spawnedZone.GetComponentsInChildren<Structure>(true);
            foreach (var structure in structures)
            {
                int tc = Util.GetTCForZoneTier(upgradedTier, false);
                structure.SetTreasureClass(tc);
                //Debug.Log($"[LootZone] Assigned TC {tc} to Structure '{structure.gameObject.name}'");
            }
        }
        else
        {
            Debug.LogWarning($"[LootZone] No prefab found for shape '{selectedShape}' at Tier {upgradedTier}");
        }
    }




    private int TryUpgradeTier(int currentTier)
    {
        //implement this later
        return currentTier;
    }




    private void OnDrawGizmos()
    {
        DrawGizmoBoundary(new Color(0, 1, 0, 0.5f)); // Green for normal view
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmoBoundary(new Color(0, 1, 1, 1)); // Cyan when selected
    }

    private void DrawGizmoBoundary(Color color)
    {
        Gizmos.color = color;

        switch (selectedShape)
        {
            case LootZoneShape.L_Shape:
                DrawLShape();
                break;
            case LootZoneShape.L_Shape_Mirrored:
                DrawLShapeMirrored();
                break;
            case LootZoneShape.L_Shape_Flipped:
                DrawLShapeFlipped();
                break;
            case LootZoneShape.L_Shape_FlippedMirrored:
                DrawLShapeFlippedMirrored();
                break;
            case LootZoneShape.Long:
                Gizmos.DrawWireCube(transform.position, new Vector3(prefabSize.x * 2, prefabSize.y / 2, 0));
                break;
            case LootZoneShape.Square:
                Gizmos.DrawWireCube(transform.position, new Vector3(prefabSize.x, prefabSize.y, 0));
                break;
            case LootZoneShape.Circle:
                DrawCircle();
                break;
            case LootZoneShape.Hollow_Square:
                DrawHollowSquare();
                break;
        }

#if UNITY_EDITOR
        Handles.Label(transform.position, selectedShape.ToString() + " (Tier " + tier + ")");
#endif
    }



    private void DrawLShape()
    {
        DrawLShapeVariant(1, 1);
    }

    private void DrawLShapeMirrored()
    {
        DrawLShapeVariant(-1, 1);
    }

    private void DrawLShapeFlipped()
    {
        DrawLShapeVariant(1, -1);
    }

    private void DrawLShapeFlippedMirrored()
    {
        DrawLShapeVariant(-1, -1);
    }

    // Helper method to draw an L shape with proper mirroring/flipping
    private void DrawLShapeVariant(int mirrorX, int mirrorY)
    {
        Vector3 center = transform.position;
        float width = prefabSize.x;
        float height = prefabSize.y;

        // Define corner positions relative to center
        Vector3 horizontalCenter = center + new Vector3(0 * mirrorX, -height / 4 * mirrorY, 0); // Slightly below center
        Vector3 verticalCenter = center + new Vector3(-width / 4 * mirrorX, 0 * mirrorY, 0);   // Slightly left of center

        Vector3 horizontalSize = new Vector3(width, height / 2, 0); // Wide but shorter
        Vector3 verticalSize = new Vector3(width / 2, height, 0);   // Tall but narrower

        // Draw the two overlapping rectangles
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
