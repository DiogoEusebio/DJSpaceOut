using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImaginationCharacter : MonoBehaviour
{
    public float ForwardMaxSpeed;
    public float StrafeMaxSpeed;

    public ImaginationCharacterFollow Camera;
    public CharacterMovementController RealWorldCharacter;

    private Animator Animator;

    private bool MovingForward;
    private float InterpolateForward;

    private bool MovingRight;
    private bool MovingLeft;
    private float InterpolateStrafe;
    void Awake()
    {
        Animator = GetComponent<Animator>();
        
        MovingForward = true;
        InterpolateForward = 0f;

        MovingRight = false;
        MovingLeft = false;
        InterpolateStrafe = 0f;
    }

    void Update()
    {
        if (Time.timeScale != 0f)
            this.Move();
    }

    void Move()
    {
        //strafe
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.Translate(this.transform.right * StrafeSpeed * Time.deltaTime, Space.World);
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    transform.Translate(-this.transform.right * StrafeSpeed * Time.deltaTime, Space.World);
        //}
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("MovingRight " + MovingRight + "MovingLeft " + MovingLeft + "Inter " + InterpolateStrafe);
            MovingRight = true;
            if (MovingLeft)
            {
                MovingLeft = false;
                InterpolateStrafe = 0f;
            }

            if (MovingRight && InterpolateStrafe < 1f)
            {
                transform.Translate(this.transform.right * Mathf.Lerp(0f, StrafeMaxSpeed, InterpolateStrafe) * Time.deltaTime, Space.World);
                InterpolateStrafe += 1f * Time.deltaTime;
            } 
            else if(MovingRight)
            {
                transform.Translate(this.transform.forward * StrafeMaxSpeed * Time.deltaTime, Space.World);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MovingLeft = true;
            if (MovingRight)
            {
                MovingRight = false;
                InterpolateStrafe = 0f;
            }

            if (MovingLeft && InterpolateStrafe < 1f)
            {
                transform.Translate(-this.transform.right * Mathf.Lerp(0f, StrafeMaxSpeed, InterpolateStrafe) * Time.deltaTime, Space.World);
                InterpolateStrafe += 1f * Time.deltaTime;
            }
            else if (MovingLeft)
            {
                transform.Translate(-this.transform.forward * StrafeMaxSpeed * Time.deltaTime, Space.World);
            }
        }

        // fo debug only
        if (Input.GetKey(KeyCode.S))
        {
            MovingForward = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            MovingForward = true;
        }

        if(MovingForward && InterpolateForward < 1f)
        {
            transform.Translate(this.transform.forward * Mathf.SmoothStep(0f, ForwardMaxSpeed, InterpolateForward) * Time.deltaTime, Space.World);
            InterpolateForward += 0.5f * Time.deltaTime;
        }
        else if(MovingForward)
        {
            transform.Translate(this.transform.forward * ForwardMaxSpeed * Time.deltaTime, Space.World);
        }
        else if(!MovingForward)
        {
            InterpolateForward = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("InvisibleWall"))
        {
            // disable obstacle after colliding with it
            collision.gameObject.GetComponent<Collider>().enabled = false;


            // Debug.Log("pickedup: " + RealWorldCharacter.PickedUpPowers.Count);
            if (RealWorldCharacter.PickedUpPowers.Count != 0)
            {
                //delete powerup and remove from list
                // Debug.Log("oioiioiioii");
                GameObject obj2Remove = RealWorldCharacter.PickedUpPowers[RealWorldCharacter.PickedUpPowers.Count - 1];
                RealWorldCharacter.PickedUpPowers.RemoveAt(RealWorldCharacter.PickedUpPowers.Count - 1);
                Destroy(obj2Remove);
                RealWorldCharacter.TurnOffPower();
            }
            else
            {
                Camera.Resize(0.05f);
            }

            // small stop cause game feel
            StartCoroutine(ChangeTime(0f, 0.3f));
            StartCoroutine(Camera.Shake(0.1f));
        }
    }
    IEnumerator ChangeTime(float slowTime, float duration)
    {
        // stop camera resize
        var oldResizeSpeed = Camera.ResizeIncreaseSpeed;
        Camera.ResizeIncreaseSpeed = 0f;
        // stop character movement
        MovingForward = false;

        yield return new WaitForSecondsRealtime(duration);

        //reset
        Camera.ResizeIncreaseSpeed = oldResizeSpeed;
        MovingForward = true;
    }
}
