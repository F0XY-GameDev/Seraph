using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    [Header("Room Prefab Arrays"), Tooltip("Arrays of rooms that can be spawned at runtime")]
    public GameObject[] BRooms;
    public GameObject[] TRooms;
    public GameObject[] LRooms;
    public GameObject[] RRooms;
    public GameObject[] BTRooms;
    public GameObject[] BLRooms;
    public GameObject[] BRRooms;
    public GameObject[] TLRooms;
    public GameObject[] TRRooms;
    public GameObject[] LRRooms;
    public GameObject[] BTLRooms;
    public GameObject[] BTRRooms;
    public GameObject[] BLRRooms;
    public GameObject[] TLRRooms;
    public GameObject[] BTLRRooms;

    [Header("Singular Room Prefabs")]
    public GameObject BRoom;
    public GameObject TRoom;
    public GameObject LRoom;
    public GameObject RRoom;
    public GameObject BTRoom;
    public GameObject BLRoom;
    public GameObject BRRoom;
    public GameObject TLRoom;
    public GameObject TRRoom;
    public GameObject LRRoom;
    public GameObject BTLRoom;
    public GameObject BTRRoom;
    public GameObject BLRRoom;
    public GameObject TLRRoom;
    public GameObject BTLRRoom;


    [Header("Encounter Prefabs")]
    public List<GameObject> bottomEncounters = new List<GameObject>();
    public List<GameObject> topEncounters = new List<GameObject>();
    public List<GameObject> leftEncounters = new List<GameObject>();
    public List<GameObject> rightEncounters = new List<GameObject>();
    public List<GameObject> bottomTopEncounters = new List<GameObject>();
    public List<GameObject> bottomLeftEncounters = new List<GameObject>();
    public List<GameObject> bottomRightEncounters = new List<GameObject>();
    public List<GameObject> topLeftEncounters = new List<GameObject>();
    public List<GameObject> topRightEncounters = new List<GameObject>();
    public List<GameObject> leftRightEncounters = new List<GameObject>();
    public List<GameObject> bottomTopLeftEncounters = new List<GameObject>();
    public List<GameObject> bottomTopRightEncounters = new List<GameObject>();
    public List<GameObject> bottomLeftRightEncounters = new List<GameObject>();
    public List<GameObject> topLeftRightEncounters = new List<GameObject>();
    public List<GameObject> bottomTopLeftRightEncounters = new List<GameObject>();





    [Header("All Rooms Spawned"), Tooltip("List used during runtime to store all rooms spawned on the current floor")]
    public List<GameObject> rooms;

    [Header("Edge Case Rooms"), Tooltip("Rooms that spawn under special circumstances")]
    public GameObject closedRoom;
    public GameObject singleDoorLeftRoom;
    public GameObject singleDoorRightRoom;
    public GameObject singleDoorTopRoom;
    public GameObject singleDoorBottomRoom;

    [Header("Boss Encounters"), Tooltip("Possible Boss Encounters for the current floor")]
    public List<GameObject> bosses = new List<GameObject>();

    [Header("Config")]
    public int spawnCount;
    public int spawnStopCount;
    public int globalSeed;
    public System.Random globalRandInt;
    public float waitTime;
    public bool spawnedBoss;
    [Header("Boss Icon Prefab")]
    public GameObject boss;

    

    private void Awake()
    {
        spawnCount = 0;
        globalSeed = Random.Range(0, 99999);
        //globalSeed = 64364;
        globalRandInt = new System.Random(globalSeed);
        waitTime = 5f;
    }
    public void IncreaseSpawnCount(int value)
    {
        spawnCount += value;
    }

    private void Update()
    {
        if(waitTime <= 0 && spawnedBoss == false)
        {
            foreach(GameObject go in rooms)
            {
                //if there is a room under or over it, remove the room with lower openingDirection
                List<Collider2D> colliderList = new List<Collider2D>();
                List<Room> roomList = new List<Room>();
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(go.transform.position, 2f);
                foreach (Collider2D col in colliderArray) { if (col.CompareTag("SpawnCollider")) { colliderList.Add(col); } }
                foreach (Collider2D col in colliderList) { roomList.Add(col.GetComponentInParent<Room>()); }
                foreach (Room room in roomList)
                {
                    if (go.GetComponent<Room>().openingDirection > room.openingDirection)
                    {
                        Debug.Log("Destroying " + room);
                        Destroy(room.gameObject);
                    }
                }
                /*if (col.Length > 1)
                {
                    room1 = go.GetComponent<Room>();
                    room2 = col[1].GetComponent<Room>();
                    if (room1 != null && room2 != null)
                    {
                        if (room1.openingDirection >= room2.openingDirection)
                        {
                            Destroy(room2.gameObject);
                            Debug.Log("Destroyed " + room2);
                        }
                    }
                    else
                    {
                        Debug.Log("Room1 is " + room1 + " and Room2 is " + room2);
                    }
                }*/
            }
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                    rooms[i].GetComponent<Room>().isBossRoom = true;
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
