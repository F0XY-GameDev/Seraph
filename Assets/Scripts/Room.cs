using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isBossRoom;
    public bool isCleared;
    public GameObject encounter;
    public GameObject stairPrefab;
    private RoomTemplates templates;
    [SerializeField] private int openingDirection;

    public void Awake()
    {
        templates = FindObjectOfType<RoomTemplates>();
    }
    public void Start()
    {
        StartCoroutine(SpawnEncounter());
    }
    public void SpawnBoss()
    {
        if (isBossRoom)
        {
            //spawn Boss
        }
    }
    public void SetOpeningDirection(int value)
    {
        openingDirection = value;
    }
    public void SpawnStairs()
    {
        if (isBossRoom && isCleared)
        {
            Instantiate(stairPrefab, this.transform);
        }
    }

    public IEnumerator SpawnEncounter()
    {
        yield return new WaitUntil(() => templates.spawnedBoss == true);
        if (isBossRoom)
        {
            yield break;
        }
        switch(openingDirection)
        {
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            case 10:

                break;
            case 11:

                break;
            case 12:

                break;
            case 13:

                break;
            case 14:

                break;
            case 15:

                break;
            default:
                break;
        }
        //spawn the encounter
    }
}
