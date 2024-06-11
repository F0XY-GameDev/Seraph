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
    public bool persistant;

    private void Start()
    {
        if (FindObjectOfType<LevelManager>().debugMode)
        {
            //spriteRenderer.enabled = true;
        } else
        {
            //spriteRenderer.enabled = false;
        }
        if (GetComponent<Bullet>() != null)
        {
            Bullet bullet = GetComponent<Bullet>();
            damage = bullet.damage;
            damageType = bullet.damageType;
        }
    }

    private void FixedUpdate()
    {
        if (lifeSpan < 0)
        {
            Destroy(this.gameObject);
        }
        if (persistant)
        {
            return;
        }
        lifeSpan--;
    }
}
