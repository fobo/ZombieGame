using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] private GameObject animationPrefab;

    public void SpawnRandomAnimation2D()
    {
        if (animationPrefab == null)
        {
            Debug.LogWarning("Animation prefab is not assigned.");
            return;
        }

        // Get the Collider2D component to define the bounds
        CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
        if (col == null)
        {
            Debug.LogWarning("Enemy is missing a Collider2D!");
            return;
        }

        Bounds bounds = col.bounds;

        // Get a random point inside the bounds
        Vector2 randomPos = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        // Spawn the animation at that point
        GameObject animInstance = Instantiate(animationPrefab, randomPos, Quaternion.identity);

        float animLength = GetAnimationLength(animInstance);
        Destroy(animInstance, animLength);
    }

    private float GetAnimationLength(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            if (clips.Length > 0)
            {
                return clips[0].length;
            }
        }

        Animation animation = obj.GetComponent<Animation>();
        if (animation != null && animation.clip != null)
        {
            return animation.clip.length;
        }

        Debug.LogWarning("No animation clip found on prefab.");
        return 1f; // fallback duration
    }
}
