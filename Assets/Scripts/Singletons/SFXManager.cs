using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Plays a sound effect clip at a specific position (ignored in 2D but keeps code flexible).
    /// </summary>
    public void PlaySFXClip(AudioClip clip, Transform position, float volume = 1f)
    {
        if (clip == null || position == null)
        {
            Debug.LogWarning("Missing AudioClip or position in PlaySFXClip.");
            return;
        }

        GameObject sfxObject = new GameObject("SFX_" + clip.name);
        sfxObject.transform.position = position.position; // Still nice to track for debugging

        AudioSource source = sfxObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0f; // 2D sound
        source.Play();

        Destroy(sfxObject, clip.length);
    }

    /// <summary>
    /// Plays a random sound effect from an array at a specific position (2D audio).
    /// </summary>
    public void PlayRandomSFXClip(AudioClip[] clips, Transform position, float volume = 1f)
    {
        if (clips == null || clips.Length == 0 || position == null)
        {
            Debug.LogWarning("Missing or empty clips array or position in PlayRandomSFXClip.");
            return;
        }

        int randomIndex = Random.Range(0, clips.Length);
        AudioClip selectedClip = clips[randomIndex];

        PlaySFXClip(selectedClip, position, volume);
    }
}
