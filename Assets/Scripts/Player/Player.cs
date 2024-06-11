using AllEnums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currentHealth;
    [SerializeField] private int money;
    [Header("Starting Stats")]
    public int[] stats; //0 is Body, 1 is Mind, 2 is Luck, 3 is Demonity
    public float shotSpeed;
    public int maxHealth;
    public float moveSpeed;
    public float shotSize;
    public int damage;
    public int iFrames;
    public DamageType damageType;
    public int shotLifeTime;
    public List<int> resistances = new List<int>(); //when damage of type comes in, compare index of DamageType enum to index in resistances  
    public GameObject damageNumber;

    private void Start()
    {
        maxHealth = (stats[0] * 100) / 4;
        currentHealth = maxHealth;
    }
    private void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
        iFrames--;
        if (iFrames < 0)
        {
            iFrames = 0;
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col);
        if (col.CompareTag("Coin"))
        {
            AddMoney(col.GetComponent<Coin>().value);
            Destroy(col.gameObject);
            return;
        }
        if (col == null)
        {
            Debug.Log("Collided with non-hitbox");
            return;
        }
        if (col.GetComponent<Hitbox>() != null) 
        { 
            int incomingDamage = col.GetComponent<Hitbox>().damage;
            int incomingDamageType = (int)col.GetComponent<Hitbox>().damageType;
            int resistance = resistances[incomingDamageType];
            if (resistance > 0)
            {
                resistance = 0;
            }        
            float finalDamage = incomingDamage - (int)Mathf.Clamp((incomingDamage * ((1+ resistance) / 100)), 0 , (float)incomingDamage * 0.8f);
            ReduceHealth(Mathf.RoundToInt(finalDamage));
        }
    }
    private void AddMoney(int amount)
    {
        money += amount;
    }
    private void ReduceHealth(int amount)
    {
        if (iFrames > 1)
        {
            return;
        }
        currentHealth -= amount;
        GameObject go = damageNumber;
        go.GetComponent<TextMeshProUGUI>().text = ("-" + amount.ToString());
        Instantiate(go, GetComponentInChildren<Canvas>().transform);
    }
    private void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    private void Die()
    {

        Debug.Log("Player Has Died");
    }
}
