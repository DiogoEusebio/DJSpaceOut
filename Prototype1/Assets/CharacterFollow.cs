using UnityEngine;
using System.Collections;

public class CharacterFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    private Vector3 cameraVelocity;
    
    public float rotationXoffset;
    public float rotationYoffset;
    public float rotationZoffset;
    
    public float ShakeMagnitude;

    public float offsetHorizontal;
    public float offsetVertical;
    public float offsetOtherHorizontal;
    private void Awake()
    {
        offsetHorizontal = 0f;
        offsetVertical = 0f;
        offsetOtherHorizontal = 0f;

        transform.position = target.TransformPoint(new Vector3(offsetPosition.x + (offsetHorizontal),
                                                                   offsetPosition.y + (offsetVertical),
                                                                   offsetPosition.z + (offsetOtherHorizontal)));
        
        transform.LookAt(new Vector3(target.position.x + rotationXoffset,
                                    target.position.y + rotationYoffset, target.position.z + rotationZoffset));
    }
    private void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        // why tho??
        if (offsetPositionSpace == Space.Self)
        {
            // Define a target position
            Vector3 targetPosition = target.TransformPoint(new Vector3(offsetPosition.x + (offsetHorizontal),
                                                                   offsetPosition.y + (offsetVertical),
                                                                   offsetPosition.z + (offsetOtherHorizontal)));
            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraVelocity, 0.1f);
        }
    }

    public IEnumerator Shake(float duration)
    {
        Vector3 originalCameraPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float zOffset = Random.Range(-1f, 1f) * ShakeMagnitude;
            float yOffset = Random.Range(-1f, 1f) * ShakeMagnitude;
            transform.position = new Vector3(originalCameraPos.x, originalCameraPos.y + yOffset, originalCameraPos.z + zOffset);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalCameraPos;
    }
}