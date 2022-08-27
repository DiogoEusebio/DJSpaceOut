using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    public float speed;
    public float StrafeSpeed;
    public float rotationSpeed;
    public float maxAngle;

    public CharacterFollow Camera;
    public ImaginationCharacterFollow ImaginationCamera;
    public CarController Car;
    public CarController2 Car2;

    public Collider HeadCollider;
    public Collider BodyCollider;

    private Animator Animator;
    private bool walking;
    private float maxLeftAngle;
    private float maxRightAngle;

    private bool UsingPower;
    private float StartTime;
    private float PowerPeriod;

    public List<GameObject> PickedUpPowers;

    public Timer timer;

    public AudioSource ouch;
    public AudioSource victory;
    public AudioSource lose;

    int score = 0;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        walking = true;
        maxLeftAngle = transform.eulerAngles.y - maxAngle;
        maxRightAngle = transform.eulerAngles.y + maxAngle;

        PickedUpPowers = new List<GameObject>();
        UsingPower = false;
        BodyCollider.enabled = true;
        HeadCollider.enabled = true;

        StartTime = 0f;
        PowerPeriod = 10f;
    }

    void Update()
    {
        if (UsingPower)
        {
            if (StartTime == 0f)
                StartTime = Time.time;
            // Debug.Log("start time: " + StartTime + "current TIme: " + Time.time);
            if (Time.time > StartTime + PowerPeriod)
            {
                this.TurnOffPower();

                //delete powerup and remove from list
                for (var i = 0; i < PickedUpPowers.Count; i++)
                {
                    GameObject obj2Remove = this.PickedUpPowers[i];
                    this.PickedUpPowers.RemoveAt(i);
                    Destroy(obj2Remove);
                }
            }
        }
        if (Time.timeScale != 0f)
            this.Move();

        checkLoss();     
    }

    void Move()
    {
        //strafe
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Animator.SetBool("Walking", true);
            //transform.GetChild(0).Rotate(0, 1f, 0, Space.World);
            transform.Translate(-this.transform.forward * StrafeSpeed * Time.deltaTime, Space.Self);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Animator.SetBool("Walking", true);
            //transform.GetChild(0).Rotate(0, -1f, 0, Space.World);
            transform.Translate(this.transform.forward * StrafeSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
        	walking = false;
        	Animator.SetBool("Idle", true);
            Animator.SetBool("Walking", false);
            ImaginationCamera.ResizeIncreaseSpeed = 0.0002f;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            walking = true;
            Animator.SetBool("Idle", false);
            Animator.SetBool("Walking", true);
        }
        
        if (walking)
        {
            ImaginationCamera.ResizeIncreaseSpeed = -0.0002f;

            Animator.SetBool("Walking", true);
            transform.Translate(this.transform.right * speed * Time.deltaTime, Space.Self);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.gameObject.CompareTag("CrossWalk") && this.PickedUpPowers.Count == 0)
        {
            // if character collides stop walking and loose points, hit animaiton, camera shake
            walking = false;
            Animator.SetBool("Walking", false);
            Animator.SetBool("Idle", true);
            ImaginationCamera.Resize(0.05f);
            ouch.Play();
        }

        if(collision.collider.gameObject.CompareTag("Car"))
        {            // loose
            walking = false;
           	Animator.SetBool("Walking", false);
           	Animator.SetBool("Idle", false);
            Animator.SetBool("RunOver", true);          
            Car.Moving = false;
            FindObjectOfType<GameM>().EndGame();
            lose.Play();
        }

        if(this.PickedUpPowers.Count != 0)
        {
            // dont loose points
            walking = false;
            Animator.SetBool("Walking", false);

            // loose power
            TurnOffPower();

            //delete powerup and remove from list
            GameObject obj2Remove = this.PickedUpPowers[PickedUpPowers.Count - 1];
            this.PickedUpPowers.RemoveAt(PickedUpPowers.Count - 1);
            Destroy(obj2Remove);
        }

        // small stop cause game feel
        StartCoroutine(ChangeTime(0f, 0.1f));
        StartCoroutine(Camera.Shake(0.1f));
    }
    IEnumerator ChangeTime(float slowTime, float duration)
    {
        Time.timeScale = slowTime;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("CrossWalk"))
        {
            Car.Moving = true;
        }

        if (collider.gameObject.CompareTag("CrossWalk2"))
        {
            Car2.Moving = true;
        }

        if (collider.gameObject.CompareTag("School"))
        {
            walking = false;
            Animator.SetBool("Walking", false);
            Animator.SetBool("Win", true);           
			Car.Moving = false;           
            FindObjectOfType<GameM>().GameWin();
            victory.Play();            
        }
    }

    public void PowerUpPickUp(GameObject power)
    {
        this.PickedUpPowers.Add(power);
        if (this.PickedUpPowers.Count == 3)
        {
            TurnOnPower();
        }
    }

    public void TurnOnPower()
    {
        // use power aka toggle colliders

        UsingPower = true;
        BodyCollider.enabled = false;
        HeadCollider.enabled = false;
    }
    public void TurnOffPower()
    {
        // use power aka toggle colliders

        UsingPower = false;
        BodyCollider.enabled = true;
        HeadCollider.enabled = true;
    }

    void checkLoss() {
   		if((int)timer.gameTimer == 0) {
            timer.gameTimer = 0f;
            walking = false;
            Animator.SetBool("End", true);
            lose.Play();
            FindObjectOfType<GameM>().EndGame();
        }

        if(ImaginationCamera.Camera.rect.y == 1) {
        	walking = false;       	
            Animator.SetBool("End", true);
            lose.Play();
            FindObjectOfType<GameM>().EndGame();
        }  
    }

    public int getScore() {
    	return score;
    }
}
