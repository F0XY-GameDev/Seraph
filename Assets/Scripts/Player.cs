using AllEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int currentHealth { get { return currentHealth; } set { currentHealth = value; } }
    private int money { get { return money; } set { money = value; } }
    [Header("Starting Stats")]
    public int[] stats; //0 is Body, 1 is Mind, 2 is Luck, 3 is Demonity
    public float shotSpeed;
    public int maxHealth;
    public float moveSpeed;
    public float shotSize;
    public int shotLifeTime;
    public List<int> resistances = new List<int>(); //when damage of type comes in, compare index of DamageType enum to index in resistances  

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col == null)
        {
            Debug.Log("Collided with non-hitbox");
            return;
        }
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

    private void ReduceHealth(int amount)
    {
        currentHealth -= amount;
    }
}
