using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class SpawnPointGizmo : MonoBehaviour
{
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Set Gizmo color
        Gizmos.DrawSphere(transform.position, 0.5f); // Draw a sphere at the spawn point

        // Draw text label above the spawn point
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;
        style.alignment = TextAnchor.MiddleCenter;

        Handles.Label(transform.position + Vector3.up * 0.75f, "Spawn Point", style);
    }
    #endif
}
