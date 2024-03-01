using AllEnums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Collider2D col;
    public int lifeSpan;
    public int damage;
    public DamageType damageType;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (FindObjectOfType<LevelManager>().debugMode)
        {
            spriteRenderer.enabled = true;
        } else
        {
            spriteRenderer.enabled = false;
        }
        
    }

    private void FixedUpdate()
    {
        lifeSpan--;
        if (lifeSpan < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
