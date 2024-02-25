using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public List<GameObject> rooms;

    public GameObject closedRoom;
    public GameObject singleDoorLeftRoom;
    public GameObject singleDoorRightRoom;
    public GameObject singleDoorTopRoom;
    public GameObject singleDoorBottomRoom;

    public int spawnCount;
    public int spawnStopCount;
    public int globalSeed;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;

    public System.Random globalRandInt;

    private void Awake()
    {
        spawnCount = 0;
        globalSeed = Random.Range(0, 99999);
        //globalSeed = 57993;
        globalRandInt = new System.Random(globalSeed);
    }

    private void Update()
    {
        if(waitTime <= 0 && spawnedBoss == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                }
            }
        } else
        {
            if (waitTime >= -2)
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
