using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioSource SFXObject;
    public AudioClip defaultHoverSound;
    public AudioClip defaultClickSound;

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



    public void temp(AudioClip clip, Transform spawnPos, float volume = 1f)
    {

        AudioSource audioSource = Instantiate(SFXObject, spawnPos.position, Quaternion.identity);

        audioSource.clip = clip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);


    }
    /// <summary>
    /// Plays a sound effect clip at a specific position.
    /// </summary>
    public void PlaySFXClip(AudioClip clip, Transform clipPos, float volume = 1f)
    {
        if (clip == null || clipPos == null)
        {
            Debug.LogWarning("Missing AudioClip or position in PlaySFXClip.");
            return;
        }

        AudioSource source = Instantiate(SFXObject, clipPos.position, Quaternion.identity);

        source.clip = clip;
        source.volume = volume;

        source.spatialBlend = 1f;
        source.minDistance = 1f;
        source.maxDistance = 50f;
        source.rolloffMode = AudioRolloffMode.Linear;

        source.Play();
        Destroy(source.gameObject, clip.length);
    }



    public void PlaySFXClip2D(AudioClip clip, Transform clipPos, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Missing AudioClip in PlaySFXClip2D.");
            return;
        }

        AudioSource source = Instantiate(SFXObject, Camera.main.transform);
        source.transform.localPosition = Vector3.zero;

        source.clip = clip;
        source.volume = volume;

        source.spatialBlend = 0f;

        source.Play();
        Destroy(source.gameObject, clip.length);
    }



    /// <summary>
    /// Plays a random sound effect from an array at a specific position (3D audio).
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

    /// <summary>
    /// Plays a random sound effect from an array for the player to hear at any location..
    /// </summary>
    public void PlayRandomSFXClip2D(AudioClip[] clips, Transform position, float volume = 1f)
    {
        if (clips == null || clips.Length == 0 || position == null)
        {
            Debug.LogWarning("Missing or empty clips array or position in PlayRandomSFXClip.");
            return;
        }

        int randomIndex = Random.Range(0, clips.Length);
        AudioClip selectedClip = clips[randomIndex];

        PlaySFXClip2D(selectedClip, position, volume);
    }
}
