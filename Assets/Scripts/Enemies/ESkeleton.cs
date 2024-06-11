using AllEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESkeleton : MonoBehaviour
{
    public Sprite sprite;
    public Transform targetLocation;
    private Rigidbody2D rb;
    public int maxHealth;
    public int currentHealth;
    public int moveSpeed;
    public int slowDistance;
    public int coinValue;
    public int attackDamage;
    public int attackHitboxFrames;
    public DamageType attackDamageType;
    public int contactDamage;
    public DamageType contactDamageType;
    private GameObject player;
    public GameObject attackHitbox;
    private bool isAttacking;
    public List<int> resistances = new List<int>(); //when damage of type comes in, compare index of DamageType enum to index in resistances  
    private RoomTemplates templates;
    public GameObject coinPrefab;
    public bool isSleeping;


    private void Start()
    {
        isSleeping = true;
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().gameObject;
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            Kill();
        }
        targetLocation = player.transform;
        transform.LookAt(targetLocation);
        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (Vector3.Distance(transform.position, targetLocation.position) < 0.5)
        {
            isAttacking = true;
            StartCoroutine(Attack(targetLocation));
            return;
        }
        MoveToPlayer(targetLocation);
    }
    //we've made an orbital, need to slow down when we are closer to player.
    private void MoveToPlayer(Transform target)
    {
        if (isSleeping)
        {
            return;
        }
        if(Vector2.Distance(transform.position, target.position) < slowDistance)
        {
            rb.AddForce(transform.forward * moveSpeed / 2);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else
        {
            rb.AddForce(transform.forward * moveSpeed);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }        
    }
    public IEnumerator Attack(Transform target)
    {
        yield return new WaitForSeconds(1f);
        Hitbox hitBox = Instantiate(attackHitbox, target).GetComponent<Hitbox>();
        hitBox.lifeSpan = attackHitboxFrames;
        hitBox.damage = attackDamage;
        hitBox.damageType = attackDamageType;
        isAttacking = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
        {
            Debug.Log("Collided with non-hitbox");
            return;
        }
        int incomingDamage = collision.GetComponent<Hitbox>().damage;
        int incomingDamageType = (int)collision.GetComponent<Hitbox>().damageType;
        int resistance = 0;
        if (incomingDamageType > resistances.Count)
        {
            resistance = resistances[incomingDamageType];
        }
        if (resistance < 0)
        {
            resistance = 0;
        }
        float finalDamage = incomingDamage - (int)Mathf.Clamp(incomingDamage * ((1 + resistance) / 100), 0, (float)incomingDamage * 0.8f);
        ReduceHealth(Mathf.RoundToInt(finalDamage));
        if (!collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void ReduceHealth(int amount)
    {
        currentHealth -= amount;
    }
    public void Kill()
    {
        //roll loot
        int luck = player.GetComponent<Player>().stats[2];
        for (int i = 0; i < luck + 1; i++)
        {
            if (templates.globalRandInt.Next(0, 100) >= 50)
            {
                //Create a coin with a value and lifespan, then add the coin and a circle collider to a new gameobject and instantiate
                GameObject go = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                go.GetComponent<Coin>().lifeSpan = 500;
                go.GetComponent<Coin>().value = coinValue;
            }
        }
        //Instantiate loot
        //Add statistic
        //Destroy this object
        Destroy(this.gameObject);

    }
}
