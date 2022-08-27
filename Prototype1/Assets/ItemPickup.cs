using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public CharacterMovementController RealWorldCharacter;
    public Vector3 EquipLocalPosition;
    public Transform EquipParentTransform;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        // disable obstacle after colliding with it
        this.gameObject.GetComponent<Collider>().enabled = false;

        this.transform.parent = EquipParentTransform;
        this.transform.localPosition = EquipLocalPosition;
        this.RealWorldCharacter.PowerUpPickUp(this.gameObject);
    }
}
