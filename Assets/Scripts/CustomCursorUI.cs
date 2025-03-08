using UnityEngine;
using UnityEngine.UI;

public class CustomCursorUI : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Image reloadRing;

    private Vector2 cursorHotspot;
    private Gun playerGun;
    private float reloadTimer;
    private float reloadDuration;
    private bool isReloading = false; // Track reloading state

    void Start()
    {
        cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

        playerGun = FindObjectOfType<Gun>();

        if (reloadRing)
        {
            reloadRing.gameObject.SetActive(false); // Hide it at start
        }

        if (playerGun != null)
        {
            playerGun.OnReloadStart += StartReloadAnimation;
        }
    }


    void Update()
    {
        // Update UI to follow cursor
        if (reloadRing)
        {
            reloadRing.transform.position = Input.mousePosition;
        }

        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            reloadRing.fillAmount = Mathf.Clamp01(reloadTimer / reloadDuration);

            // Stop animation when complete
            if (reloadTimer >= reloadDuration)
            {
                isReloading = false;
                reloadRing.fillAmount = 0; // Reset UI
            }
        }
    }

    private void StartReloadAnimation(float reloadTime)
    {
        if (reloadRing)
        {
            reloadRing.gameObject.SetActive(true); // Show when reloading
            reloadRing.fillAmount = 0f;
        }

        reloadDuration = reloadTime;
        reloadTimer = 0f;
        isReloading = true;
    }

}
