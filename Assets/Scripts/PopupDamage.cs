using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupDamage : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textMeshProUGUI;
    public float lifetime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }


}
