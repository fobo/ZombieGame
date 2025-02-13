using UnityEngine;

public class YSorter : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y);
    }
}
