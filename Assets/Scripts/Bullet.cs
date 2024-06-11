using AllEnums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    public float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] int lifeSpan;
    public float bulletSpeedMinMax;
    public int damage;
    public DamageType damageType;
    private Rigidbody2D rb;
    private Player player;
    private float scale;
    public bool isPlayerBullet;
    public EShooter enemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isPlayerBullet)
        {
            direction = FindAnyObjectByType<GameCursor>().transform.position;
            player = FindAnyObjectByType<Player>();
            speed = player.shotSpeed;
            scale = player.shotSize;
            damage = player.damage;
            damageType = player.damageType;
            lifeSpan = player.shotLifeTime;
            return;
        }
        if (enemy == null)
        {
            enemy = FindAnyObjectByType<EShooter>();
        }
        direction = FindAnyObjectByType<Player>().GetComponent<Transform>().position;
        speed = enemy.shotSpeed;
        scale = enemy.shotSize;
        damage = enemy.attackDamage;
        damageType = enemy.attackDamageType;
        lifeSpan = enemy.shotLifeSpan;


    }
    private void Start()
    {
        
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        

        var heading = direction - this.transform.position;
        var rotation = transform.position - mousePos;
        rb.velocity = new Vector2(heading.x, heading.y).normalized * speed;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        transform.localScale = transform.localScale * scale;


        Vector2 tempVelocity = new Vector2(Mathf.Clamp(heading.x, -bulletSpeedMinMax, bulletSpeedMinMax) * speed, Mathf.Clamp(heading.y, -bulletSpeedMinMax, bulletSpeedMinMax) * speed);

        if (tempVelocity.x < 2 * tempVelocity.y)
        {
            tempVelocity.x = 0;
        } else if (tempVelocity.y < 2 * tempVelocity.x)
        {
            tempVelocity.y = 0;
        }

        //rb.velocity = tempVelocity;

        
        Vector3 tempDirection = new Vector3(Mathf.Clamp(direction.x, -1f, 1f), Mathf.Clamp(direction.y, -1f, 1f), 0);
        //rb.AddForce(tempDirection * speed);
        Debug.Log("heading:" + heading);
        Debug.Log("direction:" + direction);
        Debug.Log("tempVelocity" + tempVelocity);
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
        if (isPlayerBullet)
        {
            //this is doing nothing, debug it later, for now you've put in a timer and it deletes after the time
            if (collision.collider.CompareTag("Player"))
            {
                return;
            }
            else if (collision.collider.CompareTag("Wall"))
            {
                Destroy(this.gameObject);
                return;
            }
        } else
        {
            if (collision.collider.CompareTag("Player"))
            {
                Destroy(this.gameObject);
                return;
            }
        }
        Debug.Log(this.gameObject + " collided with" + collision.gameObject);
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(this.gameObject + " has collided with " +  collision.gameObject);
        int wallLayer = LayerMask.NameToLayer("Wall");
        if (collision.collider.gameObject.layer == wallLayer)
        {
            Debug.Log(this.gameObject + " Collided with Wall");
            Destroy(this.gameObject);
        }
    }
}
