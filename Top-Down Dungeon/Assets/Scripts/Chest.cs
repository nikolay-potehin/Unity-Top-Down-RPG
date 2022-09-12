using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int moneyAmount = 5;

    protected override void OnCollect()
    {
        if (!collceted)
        {
            collceted = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.coins += moneyAmount;
            GameManager.instance.ShowText($"+{moneyAmount} рублей!", 
                                          6,
                                          Color.yellow,
                                          transform.position,
                                          Vector3.up,
                                          1.5f);
        }
    }
}
