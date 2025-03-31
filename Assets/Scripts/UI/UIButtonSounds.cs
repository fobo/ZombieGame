using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound;
    public AudioClip clickSound;
    [Range(0f, 1f)] public float volume = 1f;


    void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioClip clipToPlay = hoverSound != null ? hoverSound : SFXManager.Instance.defaultHoverSound;

        if (clipToPlay != null)
        {
            SFXManager.Instance.PlaySFXClip2D(clipToPlay, gameObject.transform, volume);
        }
    }

    private void PlayClickSound()
    {
        AudioClip clipToPlay = clickSound != null ? clickSound : SFXManager.Instance.defaultClickSound;

        if (clipToPlay != null)
        {
            SFXManager.Instance.PlaySFXClip2D(clipToPlay, gameObject.transform, volume);
        }
    }
}
