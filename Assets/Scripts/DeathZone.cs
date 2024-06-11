using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public GameObject Player;
    private void FixedUpdate()
    {
        transform.localPosition = new Vector3(Player.transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       SceneManager.LoadScene(0);
    }
}
