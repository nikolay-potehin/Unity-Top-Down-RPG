using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 -> need bottom door
    // 2 -> need left door
    // 3 -> need top door
    // 4 -> need right door
    // for the 2 rooms to be connected

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        templates.rooms.Add(gameObject);

        //Invoke(nameof(Spawn), 1.0f);
        //Debug.Log($"generating {templates.currentRoomsAmount} / {templates.maxRooms} room");
        Spawn();
    }

    private void Spawn()
    {
        if (spawned)
            return;

        if (openingDirection == 1)
        {
            // Need to spawn a room with a BOTTOM door
            CreateRoom(templates.bottomRooms);
        }
        else if (openingDirection == 2)
        {
            // Need to spawn a room with a LEFT door
            CreateRoom(templates.leftRooms);
        }
        else if (openingDirection == 3)
        {
            // Need to spawn a room with a TOP door
            CreateRoom(templates.topRooms);
        }
        else if (openingDirection == 4)
        {
            // Need to spawn a room with a RIGHT door
            CreateRoom(templates.rightRooms);
        }
        spawned = true;
    }

    private void CreateRoom(GameObject[] rooms)
    {
        rand = Random.Range(0, rooms.Length);

        Instantiate(rooms[rand], transform.position, Quaternion.identity);
    }
}
