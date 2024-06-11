using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.GraphicsBuffer;

public class LockCameraToRoom : MonoBehaviour
{
    public Transform camPosition;
    public Transform mainCamera;
    public float transitionDuration;
    public bool cameraHasFinished;
    public PlayerMovement2D player;

    private void Start()
    {
        mainCamera = FindFirstObjectByType<Camera>().transform;
        player = FindObjectOfType<PlayerMovement2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TransitionToTarget(camPosition));
            StartCoroutine(WakeEnemies());
        }
    }
    IEnumerator TransitionToTarget(Transform target)
    {
        if (target.position == mainCamera.position)
        {
            yield break;
        }
        player.LockMovement();
        float t = 0.0f;
        Vector3 startingPos = mainCamera.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            mainCamera.position = Vector3.Lerp(startingPos, target.position, t);
            yield return 0;
        }
        cameraHasFinished = true;
        player.UnlockMovement();
    }
    IEnumerator WakeEnemies()
    {
        yield return new WaitUntil(() => cameraHasFinished == true);
        foreach (EWalker walker in transform.parent.GetComponentsInChildren<EWalker>())
        {
            if (!walker.isSleeping)
            {
                break;
            }
            walker.isSleeping = false;
        }
        foreach (EShooter shooter in transform.parent.GetComponentsInChildren<EShooter>())
        {
            if (!shooter.isSleeping)
            {
                break;
            }
            shooter.isSleeping = false;
        }
    }
}
