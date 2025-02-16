using UnityEngine;
using Cinemachine;

public class AssignCameraTarget : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        AssignFollowTarget();
    }

    private void OnEnable()
    {
        AssignFollowTarget();
    }

    private void AssignFollowTarget()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera component is missing!");
            return;
        }

        if (virtualCamera.Follow == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                virtualCamera.Follow = player.transform;
                Debug.Log("Cinemachine camera follow target assigned to Player.");
            }
            else
            {
                Debug.LogWarning("No GameObject with tag 'Player' found! Camera follow target not set.");
            }
        }
    }
}
