using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;
    public int lifeSpan;
    public Coin(int _value, int _lifeSpan)
    {
        value = _value;
        lifeSpan = _lifeSpan;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(0f, 2f), Random.Range(0f, 2f)));
    }

    private void FixedUpdate()
    {
        lifeSpan--;
        if (lifeSpan < 0 )
        {
            Destroy(this.gameObject);
        }
    }
}
