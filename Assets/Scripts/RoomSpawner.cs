using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    public bool needBottomDoor = false;
    public bool needTopDoor = false;
    public bool needRightDoor = false;
    public bool needLeftDoor = false;
    public bool needBottomWall = false;
    public bool needTopWall = false;
    public bool needRightWall = false;
    public bool needLeftWall = false;
    private RoomTemplates templates;
    public bool spawned = false;
    private System.Random randInt;
    public float waitTime;
    public bool hasEncounter;
    public int encounterSeed;
    public int priority;
    public bool checkedForWalls = false;

    private void Awake()
    {
        priority = Random.Range(0, 1000);
    }
    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = FindObjectOfType<RoomTemplates>();
        Debug.Log(templates);
        //int randomDelay = templates.globalRandInt.Next(15, 25);
        float delay = Random.Range(0.5f, 0.75f);
        StartCoroutine(CheckForWalls(delay));

        StartCoroutine(Spawn());
    }
    private IEnumerator CheckForWalls(float delay)
    {
        yield return new WaitForSeconds(delay);
        RaycastHit2D topHit = Physics2D.CircleCast(this.transform.position, 0.1f, Vector2.up, 13.1f);
        RaycastHit2D bottomHit = Physics2D.CircleCast(this.transform.position, 0.1f, Vector2.down, 13.1f);
        RaycastHit2D leftHit = Physics2D.CircleCast(this.transform.position, 0.1f, Vector2.left, 8.1f);
        RaycastHit2D rightHit = Physics2D.CircleCast(this.transform.position, 0.1f, Vector2.right, 8.1f);

        if (topHit.collider != null)
        {
            if (topHit.collider.CompareTag("CameraMover"))
            {
                needTopDoor = true;
                needTopWall = false;
            }
            if (topHit.collider.CompareTag("Wall"))
            {
                needTopWall = true;
                needTopDoor = false;
            }
        }
        if (bottomHit.collider != null)
        {
            if (bottomHit.collider.CompareTag("CameraMover"))
            {
                needBottomDoor = true;
                needBottomWall = false;
            }
            if (bottomHit.collider.CompareTag("Wall"))
            {
                needBottomWall = true;
                needBottomDoor = false;
            }
        }
        if (leftHit.collider != null)
        {
            if (leftHit.collider.CompareTag("CameraMover"))
            {
                needLeftDoor = true;
                needLeftWall = false;
            }
            if (leftHit.collider.CompareTag("Wall"))
            {
                needLeftWall = true;
                needLeftDoor = false;
            }
        }
        if (rightHit.collider != null)
        {
            if (rightHit.collider.CompareTag("CameraMover"))
            {
                needRightDoor = true;
                needRightWall = false;
            }
            if (rightHit.collider.CompareTag("Wall"))
            {
                needRightWall = true;
                needRightDoor = false;
            }
        }
        checkedForWalls = true;
        yield break;
    }
    private IEnumerator Spawn()
    {
        var col = Physics2D.OverlapCircle(this.transform.position, 2f);
        Debug.Log(col);
        if (col != null)
        {
            if (col.CompareTag("SpawnPoint"))
            {
                Destroy(gameObject);
            }
        }
        yield return new WaitUntil(() => checkedForWalls == true);
        //yield return new WaitForSeconds(0.75f);
        Room room;
        if (!spawned)
        {
            //When we reach a certain number of rooms spawned force lowest number door room spawns to stop spawning new rooms
            if (templates.spawnCount >= templates.spawnStopCount)
            {
                //MOVE INTO SECOND PART OF SCRIPT WHEN IT'S WORKING AS INTENDED
                switch (openingDirection)
                {
                    case 1:
                        room = Instantiate(templates.singleDoorBottomRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    case 2:
                        room = Instantiate(templates.singleDoorTopRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    case 3:
                        room = Instantiate(templates.singleDoorLeftRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    case 4:
                        room = Instantiate(templates.singleDoorRightRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    default:
                        break;
                }
                if (spawned) { yield break; }
            }
                        /*if at spawnpoint.position + 12 (in x direction) is GObj tagged CameraMover, also need opening in Right direction
                          if at spawnpoint.position - 12 (in x direction) is GObj tagged CameraMover, also need opening in Left direction
                          if at spawnpoint.position + 7 (in y direction) is GObj tagged CameraMover, also need opening in Top direction
                          if at spawnpoint.position - 7 (in y direction) is GObj tagged CameraMover, also need opening in Bottom direction 
                          also do the same thing for wall

                         */

                        // for openingDirection
                        // 1 --> need bottom door
                        // 2 --> need top door
                        // 3 --> need left door
                        // 4 --> need right door 
                        // 5 --> need bottom top door 
                        // 6 --> need bottom left door
                        // 7 --> need bottom right door
                        // 8 --> need top left door
                        // 9 --> need top right door
                        // 10 --> need left right door
                        // 11 --> need bottom top left door
                        // 12 --> need bottom top right door
                        // 13 --> need bottom left right door
                        // 14 --> need top left right door
                        // 15 --> need all door

            if (needTopDoor == true && needBottomDoor == true && needLeftDoor == true && needRightDoor == true)
            {
                openingDirection = 15;
            }
            else if (needTopDoor == true && needBottomDoor == false && needLeftDoor == true && needRightDoor == true)
            {
                openingDirection = 14;
            }
            else if (needTopDoor == false && needBottomDoor == true && needLeftDoor == true && needRightDoor == true)
            {
                openingDirection = 13;
            }
            else if (needTopDoor == true && needBottomDoor == true && needLeftDoor == false && needRightDoor == true)
            {
                openingDirection = 12;
            }
            else if (needTopDoor == true && needBottomDoor == true && needLeftDoor == true && needRightDoor == false)
            {
                openingDirection = 11;
            }
            else if (needTopDoor == false && needBottomDoor == false && needLeftDoor == true && needRightDoor == true)
            {
                openingDirection = 10;
            }
            else if (needTopDoor == true && needBottomDoor == false && needLeftDoor == false && needRightDoor == true)
            {
                openingDirection = 9;
            }
            else if (needTopDoor == true && needBottomDoor == false && needLeftDoor == true && needRightDoor == false)
            {
                openingDirection = 8;
            }
            else if (needTopDoor == false && needBottomDoor == true && needLeftDoor == false && needRightDoor == true)
            {
                openingDirection = 7;
            }
            else if (needTopDoor == false && needBottomDoor == true && needLeftDoor == true && needRightDoor == false)
            {
                openingDirection = 6;
            }
            else if (needTopDoor == true && needBottomDoor == true && needLeftDoor == false && needRightDoor == false)
            {
                openingDirection = 5;
            }
            else if (needTopDoor == false && needBottomDoor == false && needLeftDoor == false && needRightDoor == true)
            {
                openingDirection = 4;
            }
            else if (needTopDoor == false && needBottomDoor == false && needLeftDoor == true && needRightDoor == false)
            {
                openingDirection = 3;
            }
            else if (needTopDoor == true && needBottomDoor == false && needLeftDoor == false && needRightDoor == false)
            {
                openingDirection = 2;
            }
            else if (needTopDoor == false && needBottomDoor == true && needLeftDoor == false && needRightDoor == false)
            {
                openingDirection = 1;
            }
            Debug.Log("After checking for doors OpeningDir is " +  openingDirection);

            switch (openingDirection)
            {
                case 1: //bottom room cases
                    if (needTopWall && needLeftWall && needRightWall)
                    {
                        //spawn only bottom door room
                        room = Instantiate(templates.BRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needTopWall && needLeftWall && needRightWall)
                    {
                        //spawn bottom/top door room
                        room = Instantiate(templates.BTRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needTopWall && !needLeftWall && needRightWall)
                    {
                        //spawn bottom/left door room
                        room = Instantiate(templates.BLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needTopWall && needLeftWall && !needRightWall)
                    {
                        //spawn bottom/right door room
                        room = Instantiate(templates.BRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needTopWall && !needLeftWall && needRightWall)
                    {
                        //spawn bottom/top/left door room
                        room = Instantiate(templates.BTLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needTopWall && needLeftWall && !needRightWall)
                    {
                        //spawn bottom/top/right door room
                        room = Instantiate(templates.BTRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needTopWall && !needLeftWall && !needRightWall)
                    {
                        //spawn bottom/left/right door room
                        room = Instantiate(templates.BLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand1 = templates.globalRandInt.Next(0, templates.BRooms.Length - 1);
                    room = Instantiate(templates.BRooms[rand1], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);
                    //int encRand1 = templates.globalRandInt.Next(0, templates.bottomEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room1.transform, templates.bottomEncounters[encRand1]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 2: //top room cases
                    if (needBottomWall && needLeftWall && needRightWall)
                    {
                        //spawn only top door room
                        room = Instantiate(templates.TRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && needLeftWall && needRightWall)
                    {
                        //spawn bottom/top door room
                        room = Instantiate(templates.BTRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && !needLeftWall && needRightWall)
                    {
                        //spawn top/left door room
                        room = Instantiate(templates.TLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && needLeftWall && !needRightWall)
                    {
                        //spawn top/right door room
                        room = Instantiate(templates.TRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && !needLeftWall && needRightWall)
                    {
                        //spawn bottom/top/left door room
                        room = Instantiate(templates.BTLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && needLeftWall && !needRightWall)
                    {
                        //spawn bottom/top/right door room
                        room = Instantiate(templates.BTRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && !needLeftWall && !needRightWall)
                    {
                        //spawn top/left/right door room
                        room = Instantiate(templates.TLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand2 = templates.globalRandInt.Next(0, templates.TRooms.Length - 1);
                    room = Instantiate(templates.TRooms[rand2], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);
                                        
                    //int encRand2 = templates.globalRandInt.Next(0, templates.topEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room2.transform, templates.topEncounters[encRand2]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 3: //left room cases
                    if (needBottomWall && needTopWall && needRightWall)
                    {
                        //spawn only left door room
                        room = Instantiate(templates.LRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && needTopWall && needRightWall)
                    {
                        //spawn bottom/left door room
                        room = Instantiate(templates.BTRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && !needTopWall && needRightWall)
                    {
                        //spawn top/left door room
                        room = Instantiate(templates.TLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && needTopWall && !needRightWall)
                    {
                        //spawn left/right door room
                        room = Instantiate(templates.LRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && !needTopWall && needRightWall)
                    {
                        //spawn bottom/top/left door room
                        room = Instantiate(templates.BTLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && needTopWall && !needRightWall)
                    {
                        //spawn bottom/left/right door room
                        room = Instantiate(templates.BLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && !needTopWall && !needRightWall)
                    {
                        //spawn top/left/right door room
                        room = Instantiate(templates.TLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand3 = templates.globalRandInt.Next(0, templates.LRooms.Length - 1);
                    room = Instantiate(templates.LRooms[rand3], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand3 = templates.globalRandInt.Next(0, templates.leftEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room3.transform, templates.leftEncounters[encRand3]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 4: //right room cases
                    if (needBottomWall && needTopWall && needLeftWall)
                    {
                        //spawn only right door room
                        room = Instantiate(templates.RRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && needTopWall && needLeftWall)
                    {
                        //spawn bottom/right door room
                        room = Instantiate(templates.BRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && !needTopWall && needLeftWall)
                    {
                        //spawn top/right door room
                        room = Instantiate(templates.TRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && needTopWall && !needLeftWall)
                    {
                        //spawn left/right door room
                        room = Instantiate(templates.LRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && !needTopWall && needLeftWall)
                    {
                        //spawn bottom/top/right door room
                        room = Instantiate(templates.BTRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && needTopWall && !needLeftWall)
                    {
                        //spawn bottom/left/right door room
                        room = Instantiate(templates.BLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && !needTopWall && !needLeftWall)
                    {
                        //spawn top/left/right door room
                        room = Instantiate(templates.TLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand4 = templates.globalRandInt.Next(0, templates.RRooms.Length - 1);
                    room = Instantiate(templates.RRooms[rand4], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand4 = templates.globalRandInt.Next(0, templates.rightEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room4.transform, templates.rightEncounters[encRand4]));
                    
                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 5: //bottom/top room cases
                    if (needLeftWall && needRightWall)
                    {
                        //spawn bottom/top room
                        room = Instantiate(templates.BTRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needLeftWall && needRightWall)
                    {
                        //spawn bottom/top/left room
                        room = Instantiate(templates.BTLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needLeftWall && !needRightWall)
                    {
                        //spawn bottom/top/right room
                        room = Instantiate(templates.BTRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand5 = templates.globalRandInt.Next(0, templates.BTRooms.Length - 1);
                    room = Instantiate(templates.BTRooms[rand5], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand5 = templates.globalRandInt.Next(0, templates.bottomTopEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room5.transform, templates.bottomTopEncounters[encRand5]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 6: //bottom/left room cases
                    if (needTopWall && needRightWall)
                    {
                        //spawn bottom/left room
                        room = Instantiate(templates.BLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needTopWall && needRightWall)
                    {
                        //spawn bottom/top/left room
                        room = Instantiate(templates.BTLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needTopWall && !needRightWall)
                    {
                        //spawn bottom/left/right room
                        room = Instantiate(templates.BLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand6 = templates.globalRandInt.Next(0, templates.BLRooms.Length - 1);
                    room = Instantiate(templates.BLRooms[rand6], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand6 = templates.globalRandInt.Next(0, templates.bottomLeftEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room2.transform, templates.topEncounters[encRand2]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 7: //bottom/right room cases
                    if (needTopWall && needLeftWall)
                    {
                        //spawn bottom/right room
                        room = Instantiate(templates.BRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needTopWall && needLeftWall)
                    {
                        //spawn bottom/top/right room
                        room = Instantiate(templates.BTRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needTopWall && !needLeftWall)
                    {
                        //spawn bottom/left/right room
                        room = Instantiate(templates.BLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand7 = templates.globalRandInt.Next(0, templates.BRRooms.Length - 1);
                    room = Instantiate(templates.BRRooms[rand7], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand7 = templates.globalRandInt.Next(0, templates.bottomRightEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room3.transform, templates.leftEncounters[encRand3]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 8: //top/left room cases
                    if (needBottomWall && needRightWall)
                    {
                        //spawn top/left room
                        room = Instantiate(templates.TLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && needRightWall)
                    {
                        //spawn bottom/top/left room
                        room = Instantiate(templates.BTLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && !needRightWall)
                    {
                        //spawn top/left/right room
                        room = Instantiate(templates.TLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand8 = templates.globalRandInt.Next(0, templates.TLRooms.Length - 1);
                    room = Instantiate(templates.TLRooms[rand8], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand8 = templates.globalRandInt.Next(0, templates.topLeftEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room4.transform, templates.rightEncounters[encRand4]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 9: //top/right room cases
                    if (needBottomWall && needLeftWall)
                    {
                        //spawn top/right room
                        room = Instantiate(templates.TRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && needLeftWall)
                    {
                        //spawn bottom/top/right room
                        room = Instantiate(templates.BTRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && !needLeftWall)
                    {
                        //spawn top/left/right room
                        room = Instantiate(templates.TLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand9 = templates.globalRandInt.Next(0, templates.TRRooms.Length - 1);
                    room = Instantiate(templates.TRRooms[rand9], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand9 = templates.globalRandInt.Next(0, templates.topRightEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room1.transform, templates.bottomEncounters[encRand1]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 10: //left/right room cases
                    if (needBottomWall && needTopWall)
                    {
                        //spawn left/right room
                        room = Instantiate(templates.LRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (!needBottomWall && needTopWall)
                    {
                        //spawn bottom/left/right room
                        room = Instantiate(templates.BLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    else if (needBottomWall && !needTopWall)
                    {
                        //spawn top/left/right room
                        room = Instantiate(templates.TLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand10 = templates.globalRandInt.Next(0, templates.LRRooms.Length - 1);
                    room = Instantiate(templates.LRRooms[rand10], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand10 = templates.globalRandInt.Next(0, templates.leftRightEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room2.transform, templates.topEncounters[encRand2]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 11: //bottom/top/left room cases
                    if (needRightWall)
                    {
                        //spawn bottom/top/left room
                        room = Instantiate(templates.BTLRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand11 = templates.globalRandInt.Next(0, templates.BTLRooms.Length - 1);
                    room = Instantiate(templates.BTLRooms[rand11], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand11 = templates.globalRandInt.Next(0, templates.bottomTopLeftEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room3.transform, templates.leftEncounters[encRand3]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 12: //bottom/top/right room cases
                    if (needLeftWall)
                    {
                        //spawn bottom/top/right room
                        room = Instantiate(templates.BTRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand12 = templates.globalRandInt.Next(0, templates.BTRRooms.Length - 1);
                    room = Instantiate(templates.BTRRooms[rand12], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand12 = templates.globalRandInt.Next(0, templates.bottomTopLeftEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room4.transform, templates.rightEncounters[encRand4]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 13: //bottom/left/right room cases
                    if (needTopWall)
                    {
                        //spawn bottom/left/right room
                        room = Instantiate(templates.BLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand13 = templates.globalRandInt.Next(0, templates.BLRRooms.Length - 1);
                    room = Instantiate(templates.BLRRooms[rand13], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand13 = templates.globalRandInt.Next(0, templates.bottomLeftRightEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room1.transform, templates.bottomEncounters[encRand1]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 14: //top/left/right room cases
                    if (needBottomWall)
                    {
                        //spawn top/left/right room
                        room = Instantiate(templates.TLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                        room.SetOpeningDirection(openingDirection);
                        spawned = true;
                        templates.IncreaseSpawnCount(1);
                        break;
                    }
                    int rand14 = templates.globalRandInt.Next(0, templates.TLRRooms.Length - 1);
                    room = Instantiate(templates.TLRRooms[rand14], transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);

                    //int encRand14 = templates.globalRandInt.Next(0, templates.topLeftRightEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room2.transform, templates.topEncounters[encRand2]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                case 15:
                    //int rand15 = templates.globalRandInt.Next(0, templates.BTLRRooms.Length - 1);
                    room = Instantiate(templates.BTLRRoom, transform.position, Quaternion.identity).GetComponent<Room>();
                    room.SetOpeningDirection(openingDirection);
                    //int encRand15 = templates.globalRandInt.Next(0, templates.bottomTopLeftRightEncounters.Count - 1);
                    //StartCoroutine(SpawnEncounterAfterBossSpawned(room3.transform, templates.leftEncounters[encRand3]));

                    spawned = true;
                    templates.IncreaseSpawnCount(1);
                    break;
                default:
                    if (openingDirection <= 0)
                    {
                        Debug.Log("Opening Direction too low");                        
                    }else
                    {
                        Debug.Log("Opening Direction too high");
                    }
                    break;
            }
            // for openingDirection
            // 1 --> need bottom door
            // 2 --> need top door
            // 3 --> need left door
            // 4 --> need right door 
            // 5 --> need bottom top door 
            // 6 --> need bottom left door
            // 7 --> need bottom right door
            // 8 --> need top left door
            // 9 --> need top right door
            // 10 --> need left right door
            // 11 --> need bottom top left door
            // 12 --> need bottom top right door
            // 13 --> need bottom left right door
            // 14 --> need top left right door
            // 15 --> need all door
            if (templates.waitTime <= 0.5f) { templates.waitTime += 0.5f; }

        }
        yield break;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnCollider"))
        {
            Destroy(gameObject);
        }
        if (other.CompareTag("SpawnPoint"))
        {
            Debug.Log("Spawner " + this.gameObject.name + " Collided with " + other.gameObject.name);
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                //do nothing
            } 
            else if (other.GetComponent<RoomSpawner>().spawned == true && spawned == false)
            {
                Destroy(gameObject);
            } 
            else if (other.GetComponent<RoomSpawner>().spawned == false && spawned == true)
            {
                Destroy(other.gameObject);
            }
            //spawned = true;
        }
    }
    private IEnumerator SpawnEncounterAfterBossSpawned(Transform parentRoom, GameObject encounter)
    {
        yield return new WaitUntil(() => templates.spawnedBoss == true);
        if (parentRoom.GetComponent<Room>().isBossRoom)
        {
            yield break;
        }
        Instantiate(encounter, parentRoom);
    }
}
