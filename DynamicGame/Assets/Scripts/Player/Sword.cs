using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponsBase
{
    public bool isCollected;

    public override void Start()
    {
        base.Start();
        isCollected = false;
        idNum = 1;
    }


    public override void Update()
    {
        base .Update();

        if (!isCollected)
        {
            if (transform.parent != null)
            {
                isCollected = true;
            }
        }

        //set active if collected and in use
    }
}
