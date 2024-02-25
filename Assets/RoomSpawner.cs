using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    private RoomTemplates templates;
    public bool spawned = false;
    private System.Random randInt;
    public float waitTime = 4f;
    public bool hasEncounter;
    public int encounterSeed;
    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.05f);
    }

    private void Spawn()
    {
        //When we reach a certain number of rooms spawned force single door room spawns to stop spawning new rooms
        

        if (spawned == false)
        {
            if (templates.spawnCount >= templates.spawnStopCount)
            {
                switch (openingDirection)
                {
                    case 1:
                        Instantiate(templates.singleDoorBottomRoom, transform.position, Quaternion.identity);
                        spawned = true;
                        templates.spawnCount++;
                        encounterSeed = templates.globalRandInt.Next(0, 99999);
                        break;
                    case 2:
                        Instantiate(templates.singleDoorTopRoom, transform.position, Quaternion.identity);
                        spawned = true;
                        templates.spawnCount++;
                        encounterSeed = templates.globalRandInt.Next(0, 99999);
                        break;
                    case 3:
                        Instantiate(templates.singleDoorLeftRoom, transform.position, Quaternion.identity);
                        spawned = true;
                        templates.spawnCount++;
                        encounterSeed = templates.globalRandInt.Next(0, 99999);
                        break;
                    case 4:
                        Instantiate(templates.singleDoorRightRoom, transform.position, Quaternion.identity);
                        spawned = true;
                        templates.spawnCount++;
                        encounterSeed = templates.globalRandInt.Next(0, 99999);
                        break;
                    default:

                        break;
                }
                templates.spawnCount++;
                templates.waitTime += 0.5f;
                return;
            }
            switch (openingDirection)
            {
                case 1:
                    int rand1 = templates.globalRandInt.Next(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand1], transform.position, Quaternion.identity);
                    spawned = true;
                    templates.spawnCount++;
                    encounterSeed = templates.globalRandInt.Next(0, 99999);
                    break;
                case 2:
                    int rand2 = templates.globalRandInt.Next(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand2], transform.position, Quaternion.identity);
                    spawned = true;
                    templates.spawnCount++;
                    encounterSeed = templates.globalRandInt.Next(0, 99999);
                    break;
                case 3:
                    int rand3 = templates.globalRandInt.Next(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand3], transform.position, Quaternion.identity);
                    spawned = true;
                    templates.spawnCount++;
                    encounterSeed = templates.globalRandInt.Next(0, 99999);
                    break;
                case 4:
                    int rand4 = templates.globalRandInt.Next(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand4], transform.position, Quaternion.identity);
                    spawned = true;
                    templates.spawnCount++;
                    encounterSeed = templates.globalRandInt.Next(0, 99999);
                    break;
                default:

                    break;
            }
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
