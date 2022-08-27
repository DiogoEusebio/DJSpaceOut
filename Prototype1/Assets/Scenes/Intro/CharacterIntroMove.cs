using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIntroMove : MonoBehaviour
{
    private Animator animator;
    public GameObject cityPrefab;

    void OnTriggerEnter(Collider other)
    {
        this.createCityClone();
    }

    void createCityClone()
    {
        Transform trf = GetComponent<Transform>();
        Vector3 delta = trf.position;
        delta.x += 20;
        Instantiate(cityPrefab, delta, Quaternion.identity);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("ConstantWalk", true);
    }
}
