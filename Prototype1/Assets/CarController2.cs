using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarController2 : MonoBehaviour
{
    public float EndPositionZ;
    public float Speed;

    public bool Moving;

    void Start()
    {
        EndPositionZ += Mathf.Abs(this.transform.position.z);
        Moving = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Moving && Mathf.Abs(this.transform.position.z) < EndPositionZ)
            transform.Translate(this.transform.forward * Speed * Time.deltaTime, Space.World);
    }
}
