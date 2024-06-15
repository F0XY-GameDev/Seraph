using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GameCursor : MonoBehaviour
{
    [SerializeField] private Transform player;
    private LineRenderer lineRenderer;
    private Camera cam;
    Vector3 point = new Vector3();
    public GameObject bullet;
    public bool canShoot;
    public int shootCooldown;
    public int maxShootCooldown;
    CustomInput cursorInput;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        cursorInput = new CustomInput();
    }
    void Start()
    {
        Player p = player.GetComponent<Player>();
        maxShootCooldown = p.attackCooldown;
        cam = Camera.main;
    }
    private void OnEnable()
    {
        cursorInput.Enable();
        cursorInput.Player.Attack.performed += OnFire;
    }
    private void OnDisable()
    {
        cursorInput.Disable();
        cursorInput.Player.Attack.performed -= OnFire;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, player.localPosition);
        lineRenderer.SetPosition(1, transform.localPosition);
        transform.position = cam.ScreenToWorldPoint(new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, cam.nearClipPlane));
    }
    private void FixedUpdate()
    {
        shootCooldown--;
        if (shootCooldown <= 0 )
        {
            shootCooldown = 0;
            canShoot = true;
        }
    }
    public void EnableCursorLine()
    {
        lineRenderer.enabled = true;
    }
    public void DisableCursorLine()
    {
        lineRenderer.enabled = false;
    }
    private void OnFire(InputAction.CallbackContext value)
    {
        if (!canShoot)
        {
            return;
        }
        Instantiate(bullet, player.transform.position, Quaternion.identity);
        shootCooldown = maxShootCooldown;
        canShoot = false;
    }
    public Vector3 DirectionToCursor(Transform other)
    {
        //return Vector3 with direction from player to cursor
        Vector3 value = this.transform.position - other.position;
        Debug.Log("CursorPosition" + this.transform.position);
        return value;
    }
    public void UpdatePlayerStats()
    {
        Player p = player.GetComponent<Player>();
        maxShootCooldown = p.attackCooldown;
    }
}
