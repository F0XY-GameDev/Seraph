using AllEnums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    private CustomInput input = null;
    public int currentHealth;
    [SerializeField] private int money;
    private SpriteRenderer spriteRenderer;
    [Header("Player Stats")]
    public int[] stats; //0 is Body, 1 is Mind, 2 is Luck, 3 is Demonity
    public float shotSpeed;
    public int maxHealth;
    public float moveSpeed;
    public float chargeMoveSpeed;
    public float regularMoveSpeed;
    public float shotSize;
    public int attackCooldown;
    public int damage;
    public DamageType damageType;
    public int shotLifeTime;
    public List<int> resistances = new List<int>(); //when damage of type comes in, compare index of DamageType enum to index in resistances

    [Header("Ability Stats")]
    public float ability2ChargeTime;
    public int ability2ChannelTimer, ability2MaxChannel, ability2MinChannel;
    public bool isChannelingAbility2;
    public int ability2ChannelSpriteTimer;

    [Header("Debug Data")]
    public int iFrames;
    public int iFramesFromTakingDamage;
    public GameObject damageNumber;

    [Header("Ability2 Stats")]
    public Vector2[] ability2Directions;
    public float ability2Speed;
    public float ability2ShotSize;
    public int ability2Damage;
    public int ability2LifeTime;
    public DamageType ability2DamageType;

    [Header("Prefabs")]
    public GameObject abilityBulletPrefab;


    private void Awake()
    {
        input = new CustomInput();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        if (stats[0] <= 0)
        {
            stats[0] = 1;
        }
        maxHealth = (stats[0] * 100 / 4) + 15;
        currentHealth = maxHealth;
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Ability1.performed += OnAbility1Performed;
        input.Player.Ability1.canceled += OnAbility1Canceled;
        input.Player.Ability2.started += OnAbility2Started;
        input.Player.Ability2.performed += OnAbility2Performed;
        input.Player.Ability2.canceled += OnAbility2Canceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Ability1.performed -= OnAbility1Performed;
        input.Player.Ability1.canceled -= OnAbility1Canceled;
        input.Player.Ability2.started -= OnAbility2Started;
        input.Player.Ability2.performed -= OnAbility2Performed;
        input.Player.Ability2.canceled -= OnAbility2Canceled;
    }
    private void FixedUpdate()
    {
        if (isChannelingAbility2)
        {
            ability2ChannelSpriteTimer--;
            if (ability2ChannelSpriteTimer <= 0)
            {
                ToggleSpriteColor();
            }
        }
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
            if (col.CompareTag("Bullet"))
            {
                Destroy(col.gameObject);
            }
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
        iFrames += iFramesFromTakingDamage;
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
    private void OnAbility1Performed(InputAction.CallbackContext value)
    {
        //instant ability
    }
    private void OnAbility1Canceled(InputAction.CallbackContext value)
    {

    }
    private void OnAbility2Started(InputAction.CallbackContext value)
    {
        moveSpeed = chargeMoveSpeed;
        var holdInteraction = value.interaction as HoldInteraction;
        holdInteraction.duration = ability2ChargeTime;
        //start channeling
        isChannelingAbility2 = true;
    }
    private void OnAbility2Performed(InputAction.CallbackContext value)
    {
        //channeled ability
        //release channeled ability
        moveSpeed = regularMoveSpeed;
        foreach (Vector2 direction in ability2Directions)
        {
            GameObject temporaryBullet = abilityBulletPrefab;
            Bullet tempBullet = temporaryBullet.GetComponent<Bullet>();
            tempBullet.SetDirection(direction);
            //tempBullet.isAbilityBullet = true;
            Instantiate(temporaryBullet, this.transform.position, Quaternion.identity).GetComponent<Bullet>();
        }
        isChannelingAbility2 = false;
        ResetSpriteColor();
    }
    private void OnAbility2Canceled(InputAction.CallbackContext value)
    {
        moveSpeed = regularMoveSpeed;
        //release channeled ability
        isChannelingAbility2 = false;
        ResetSpriteColor();
    }
    private void ToggleSpriteColor()
    {
        ability2ChannelSpriteTimer = 25;
        if (spriteRenderer.color ==  Color.white)
        {
            spriteRenderer.color = Color.red;
        }else
        {
            spriteRenderer.color = Color.white;
        }
    }
    private void ResetSpriteColor()
    {
        spriteRenderer.color = Color.white;
    }
    private void Die()
    {

        Debug.Log("Player Has Died");
    }
}
