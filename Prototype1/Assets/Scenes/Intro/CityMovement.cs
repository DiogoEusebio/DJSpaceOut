using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityMovement : MonoBehaviour
{
    [Range(-0.2f, 0.2f)]
    public float xSpeed = -0.01f;

    private Transform position;
    // Start is called before the first frame update
    void Start()
    {
        position = GetComponent<Transform>();
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        position.transform.localPosition += new Vector3(xSpeed, 0, 0);
        
    }

}
