using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Joystick joystick;
    private GameObject owner;

    private void Awake()
    {
        owner = transform.parent.gameObject;
    }

    private void Update()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;

        transform.position = new Vector3(x, y, 0) + owner.transform.position;
    }
}
