using System;
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
    public int openingDirection;

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
        int encounterRand;
        GameObject encounter;
        switch (openingDirection)
        {
            // for openingDirection
            // 1 --> bottom door
            // 2 --> top door
            // 3 --> left door
            // 4 --> right door 
            // 5 --> bottom top door 
            // 6 --> bottom left door
            // 7 --> bottom right door
            // 8 --> top left door
            // 9 --> top right door
            // 10 --> left right door
            // 11 --> bottom top left door
            // 12 --> bottom top right door
            // 13 --> bottom left right door
            // 14 --> top left right door
            // 15 --> all door
            case 1:
                encounterRand = templates.globalRandInt.Next(0, templates.bottomEncounters.Count - 1);
                encounter = Instantiate(templates.bottomEncounters[encounterRand], this.transform);
                break;
            case 2:
                encounterRand = templates.globalRandInt.Next(0, templates.topEncounters.Count - 1);
                encounter = Instantiate(templates.topEncounters[encounterRand], this.transform);
                break;
            case 3:
                encounterRand = templates.globalRandInt.Next(0, templates.leftEncounters.Count - 1);
                encounter = Instantiate(templates.leftEncounters[encounterRand], this.transform);
                break;
            case 4:
                encounterRand = templates.globalRandInt.Next(0, templates.rightEncounters.Count - 1);
                encounter = Instantiate(templates.rightEncounters[encounterRand], this.transform);
                break;
            case 5:
                encounterRand = templates.globalRandInt.Next(0, templates.bottomTopEncounters.Count - 1);
                encounter = Instantiate(templates.bottomTopEncounters[encounterRand], this.transform);
                break;
            case 6:
                encounterRand = templates.globalRandInt.Next(0, templates.bottomLeftEncounters.Count - 1);
                encounter = Instantiate(templates.bottomLeftEncounters[encounterRand], this.transform);
                break;
            case 7:
                encounterRand = templates.globalRandInt.Next(0, templates.bottomRightEncounters.Count - 1);
                encounter = Instantiate(templates.bottomRightEncounters[encounterRand], this.transform);
                break;
            case 8:
                encounterRand = templates.globalRandInt.Next(0, templates.topLeftEncounters.Count - 1);
                encounter = Instantiate(templates.topLeftEncounters[encounterRand], this.transform);
                break;
            case 9:
                encounterRand = templates.globalRandInt.Next(0, templates.topRightEncounters.Count - 1);
                encounter = Instantiate(templates.topRightEncounters[encounterRand], this.transform);
                break;
            case 10:
                encounterRand = templates.globalRandInt.Next(0, templates.leftRightEncounters.Count - 1);
                encounter = Instantiate(templates.leftRightEncounters[encounterRand], this.transform);
                break;
            case 11:
                encounterRand = templates.globalRandInt.Next(0, templates.bottomTopLeftEncounters.Count - 1);
                encounter = Instantiate(templates.bottomTopLeftEncounters[encounterRand], this.transform);
                break;
            case 12:
                encounterRand = templates.globalRandInt.Next(0, templates.bottomTopRightEncounters.Count - 1);
                encounter = Instantiate(templates.bottomTopRightEncounters[encounterRand], this.transform);
                break;
            case 13:
                encounterRand = templates.globalRandInt.Next(0, templates.bottomLeftRightEncounters.Count - 1);
                encounter = Instantiate(templates.bottomLeftRightEncounters[encounterRand], this.transform);
                break;
            case 14:
                encounterRand = templates.globalRandInt.Next(0, templates.topLeftRightEncounters.Count - 1);
                encounter = Instantiate(templates.topLeftRightEncounters[encounterRand], this.transform);
                break;
            case 15:
                encounterRand = templates.globalRandInt.Next(0, templates.bottomTopLeftRightEncounters.Count - 1);
                encounter = Instantiate(templates.bottomTopLeftRightEncounters[encounterRand], this.transform);
                break;
            default:
                if (openingDirection >= 16)
                {
                    Debug.Log("OpeningDirection too high");
                } else
                {
                    Debug.Log("OpeningDirection too low");
                }
                break;
        }
        yield break;
    }
}
