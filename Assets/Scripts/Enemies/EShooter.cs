using AllEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EShooter : MonoBehaviour
{
    //shooter is a simple enemy that just shoots towards the player at semi-random intervals
    public Sprite sprite;
    public Transform targetLocation;
    private Rigidbody2D rb;
    private Player player;
    public int maxHealth;
    public int currentHealth;
    public int moveSpeed;
    public int coinValue;
    public int attackDamage;
    public int timeBetweenAttacks;
    public float shotSpeed;
    public float shotSize;
    public int shotLifeSpan;
    public DamageType attackDamageType;
    public int contactDamage;
    public DamageType contactDamageType;
    public GameObject bulletPrefab;
    private bool isAttacking;
    public List<int> resistances = new List<int>(); // when damage of type comes in, compare index of DamageType enum to index in resistances  
    private RoomTemplates templates;
    public GameObject coinPrefab;
    public bool isSleeping;
    public int timesAttacked;
    public bool isResting;
    private void Start()
    {
        isSleeping = true;
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        timeBetweenAttacks = Random.Range(200, 400);
    }
    private void FixedUpdate()
    {
        if (isSleeping)
        {
            return;
        }
        if (currentHealth <= 0)
        {
            Kill();
        }
        targetLocation = player.transform;
        if (timeBetweenAttacks <= 0)
        {
            Attack(targetLocation);
        }
        timeBetweenAttacks--;        
    }
    private void Attack(Transform target)
    {        
        timesAttacked++;
        if (timesAttacked >= 3)
        {
            timeBetweenAttacks = 250;
            isResting = true;
            timesAttacked = 0;
            StartCoroutine(RestingBetweenAttacks());
        }
        else
        {
            timeBetweenAttacks = Random.Range(50, 150);
        }
        var heading = target.position - this.transform.position;
        Bullet tempBullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity).GetComponent<Bullet>();
        tempBullet.enemy = this;
        tempBullet.isEnemyBullet = true;
        tempBullet.heading = heading;
        Debug.Log("Shot Fired");
    }
    private IEnumerator RestingBetweenAttacks()
    {
        Debug.Log("Shooter Resting");
        yield return new WaitUntil(() => timeBetweenAttacks <= 2);
        isResting = false;
        Debug.Log("Naptime Over");
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
