using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllEnums;

public class EWalker : MonoBehaviour
{
    //walker is a simple enemy that just walks towards the player slowly 
    public Sprite sprite;
    public Transform targetLocation;
    private Rigidbody2D rb;
    public int maxHealth;
    public int currentHealth;
    public int moveSpeed;
    public int attackDamage;
    public int attackHitboxFrames;
    public DamageType attackDamageType;
    public int contactDamage;
    public DamageType contactDamageType;
    private GameObject player;
    public GameObject attackHitbox;
    private bool isAttacking;
    public List<int> resistances = new List<int>(); //when damage of type comes in, compare index of DamageType enum to index in resistances  


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().gameObject;
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
        rb.AddForce(transform.forward * moveSpeed);
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
    }

    private void ReduceHealth(int amount)
    {
        currentHealth -= amount;
    }
    public void Kill()
    {
        //roll loot
        //Instantiate loot
        //Destroy this object
        //Add statistic
    }
}