using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveUpAndDestroy : MonoBehaviour
{
    [SerializeField] int lifeSpan;
    private TextMeshProUGUI tmp;
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeSpan--;
        transform.position += new Vector3(transform.position.x, lifeSpan, transform.position.z);
        if (lifeSpan < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
