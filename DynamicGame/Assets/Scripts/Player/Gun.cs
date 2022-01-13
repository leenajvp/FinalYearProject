using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : WeaponsBase
{
    private float zDist = 10.0f;
    public GameObject holdPos;
    [SerializeField] 
    private float mSensitivity = 2;
    [SerializeField] 
    private float maxDown = -60F;
    [SerializeField] 
    private float maxUp = 60F;

    private float rotationY = 0;

    public override void Start()
    {
        base.Start();
        inUse = true;
        idNum = 0;
    }

    public override void Update()
    {
        base.Update();

        Aim();
    }

    private void Aim()
    {
        //RaycastHit hit = new RaycastHit();
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float distZ = 20;

        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * mSensitivity;
        rotationY = Mathf.Clamp(rotationY, maxDown, maxUp);

        holdPos.transform.localEulerAngles = new Vector3(0, rotationX, 0);
        holdPos.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
    }


}
