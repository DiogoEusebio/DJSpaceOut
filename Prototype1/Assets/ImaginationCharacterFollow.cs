using UnityEngine;
using System.Collections;
using System;

public class ImaginationCharacterFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    public Camera Camera;
    public float ResizeIncreaseSpeed;

    private Vector3 cameraVelocity;

    public float rotationXoffset;
    public float rotationYoffset;
    public float rotationZoffset;
    public float ShakeMagnitude;

    public float offsetHorizontal;
    public float offsetVertical;
    public float offsetOtherHorizontal;

    // score variables
    private float nextTime;
    private float score;
    private float period;
    private void Awake()
    {
        offsetHorizontal = 0f;
        offsetVertical = 0f;
        offsetOtherHorizontal = 0f;

        transform.position = target.position + offsetPosition;

        transform.LookAt(new Vector3(target.position.x + rotationXoffset,
                                    target.position.y + rotationYoffset, target.position.z + rotationZoffset));

        nextTime = 0f;
        period = 1f;
        score = 0f;
    }
    private void Update()
    {
        Resize(ResizeIncreaseSpeed);

        if (Time.time > nextTime)
        {
            nextTime += period;
            score += Camera.rect.y;
            //Debug.Log("time: " + Time.time + "current score: " + score);
        }

        Refresh();
    }

    public void Refresh()
    {
        // Define a target position
        Vector3 targetPosition = new Vector3(target.position.x + offsetPosition.x,
                                            transform.position.y,
                                            transform.position.z);
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraVelocity, 0.1f);
    }

    public IEnumerator Shake(float duration)
    {
        Vector3 originalCameraPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float zOffset = UnityEngine.Random.Range(-1f, 1f) * ShakeMagnitude;
            float yOffset = UnityEngine.Random.Range(-1f, 1f) * ShakeMagnitude;
            transform.position = new Vector3(originalCameraPos.x, originalCameraPos.y + yOffset, originalCameraPos.z + zOffset);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalCameraPos;
    }


    public void Resize(float heightChange)
    {
        if (Camera.rect.y > 0 && Camera.rect.y < 0.9 || heightChange > 0)
        {
            // change height
            var Y = Camera.rect.y + heightChange;

            // update width to maintain aspect ration
            var W = 1 - Y;

            // center X
            var X = 0.5f - W / 2;

            Camera.rect = new Rect(X, Y, W, Camera.rect.height);
        }
        
        // 0.1 maybe better
        if (Camera.rect.y > 0.9)
        {
            //Debug.Log("gg easy knoob kidd");
            Camera.rect = new Rect(0, 1, 1, Camera.rect.height);
            ResizeIncreaseSpeed = 0f;
            // add bored/loose animation and retry/main menu buttons
        }
    }
}