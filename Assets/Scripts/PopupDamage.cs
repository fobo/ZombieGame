using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupDamage : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textMeshProUGUI;
    public float xdir;
    public float ydir;
    public float lifetime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        float xdir = Random.Range(-1f, 1f);
        float ydir = Random.Range(-1f, 1f);
        Vector3 offsetDir = new Vector3(xdir, ydir, 0);
        textMeshProUGUI.transform.position += offsetDir;
        Destroy(gameObject, lifetime);
    }


}
