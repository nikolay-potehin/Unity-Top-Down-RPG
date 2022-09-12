using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject owner;
    public GameObject target;

    void Start()
    {
        owner = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAim();
    }

    public void UpdateAim()
    {
        if (target == null)
            return;

        Vector2 difference = (target.transform.position - transform.position).normalized;
        Debug.Log(difference);
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        Debug.Log(rotZ);
    }
}
