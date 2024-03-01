using AllEnums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] int lifeSpan;
    public int damage;
    public DamageType damageType;
    private Rigidbody2D rb;
    private Player player;
    private float scale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = FindAnyObjectByType<GameCursor>().GetComponent<GameCursor>().DirectionToCursor(this.transform);
        player = FindAnyObjectByType<Player>();
        speed = player.shotSpeed;
        scale = player.shotSize;
        lifeSpan = player.shotLifeTime;
    }
    private void Start()
    {
        transform.localScale = transform.localScale * scale;
        rb.velocity = direction * speed;


        Vector3 tempDirection = new Vector3(Mathf.Clamp(direction.x, -1f, 1f), Mathf.Clamp(direction.y, -1f, 1f), 0);
        //rb.AddForce(tempDirection * speed);
        Debug.Log(rb.velocity);
        Debug.Log("direction:" + direction);
        Debug.Log("tempDirection" + tempDirection);
        if (direction.normalized.x <= 0.1 && direction.normalized.x >= -0.1)
        {
            Debug.Log("Slow Shot");
        }        
    }
    private void FixedUpdate()
    {
        
        if (lifeSpan >= -2) { lifeSpan--; }
        if (lifeSpan < 0) 
        {
            lifeSpan = -2;
            Destroy(this.gameObject); 
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //this is doing nothing, debug it later, for now you've put in a timer and it delets after the time
        if (collision.collider.CompareTag("Player"))
        {
            return;
        } 
        else if (collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
