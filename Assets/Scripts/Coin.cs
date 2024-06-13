using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value, lifeSpan, slowRotationSpeed, mediumRotationSpeed, fastRotationSpeed, spriteTimer;
    [SerializeField] private float minForce, maxForce, rotationSpeed;
    [SerializeField] private bool isDying;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Coin(int _value, int _lifeSpan)
    {
        value = _value;
        lifeSpan = _lifeSpan;        
    }
    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float xForce, yForce;
        xForce = Random.Range(minForce, maxForce);
        yForce = Random.Range(minForce, maxForce);
        rb.AddForce(new Vector2(xForce, yForce));
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        lifeSpan--;
        if (lifeSpan < 0 )
        {
            Destroy(this.gameObject);
        }
        if (lifeSpan >= 300)
        {
            rotationSpeed = mediumRotationSpeed;
        } else if (lifeSpan <= 300 && lifeSpan >= 200)
        {
            rotationSpeed = mediumRotationSpeed;
        } else if (lifeSpan < 200 && lifeSpan > 100)
        {
            spriteTimer--;
            rotationSpeed = mediumRotationSpeed;
            if (spriteTimer <= 0)
            {
                spriteTimer = 25;
                ToggleSpriteRenderer();
            }
        } else if (lifeSpan <= 100)
        {
            spriteTimer--;
            rotationSpeed = mediumRotationSpeed;
            if (spriteTimer <= 0)
            {
                spriteTimer = 10;
                ToggleSpriteRenderer();
            }
        }
        transform.Rotate(new Vector3(0, rotationSpeed, 0));
    }
    private void ToggleSpriteRenderer()
    {
        if (spriteRenderer.enabled == true)
        {
            spriteRenderer.enabled = false;
        } else
        {
            spriteRenderer.enabled = true;
        }
    }
}
