using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class NonStaticiQuestItem : QuestItems
{
    private Rigidbody rb;
    private BoxCollider bCollider;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        bCollider = GetComponent<BoxCollider>();
    }

    protected override void BringObjectToCam()
    {
        rb.useGravity = false;
        bCollider.enabled = false;
        base.BringObjectToCam();
    }
}
