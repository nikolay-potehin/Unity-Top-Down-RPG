using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] topRooms;
    public GameObject[] rightRooms;
    public GameObject[] roomsArrangement;

    public List<GameObject> rooms;
    public GameObject portal;

    private void Start()
    {
        Invoke(nameof(SetRoomsArrangement), 1f);
    }

    private void SetRoomsArrangement()
    {
        portal.transform.position = rooms[^1].transform.position;

        for (int i = 0; i < rooms.Count - 1; i++)
        {
            var rand = Random.Range(0, roomsArrangement.Length);
            Instantiate(roomsArrangement[rand], rooms[i].transform.position, Quaternion.identity);
        }
    }
}
