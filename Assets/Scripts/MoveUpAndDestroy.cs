using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveUpAndDestroy : MonoBehaviour
{
    [SerializeField] int lifeSpan;
    public Color32 alpha;
    public TextMeshProUGUI tmp;
    public float speedY;
    public float reduceAlpha;
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeSpan--;
        transform.position += new Vector3(0, speedY, 0);
        tmp.alpha -= reduceAlpha;
        if (lifeSpan < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
