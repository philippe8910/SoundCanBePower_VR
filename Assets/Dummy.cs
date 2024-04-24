using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private Animator animator;

    public GameObject effect; // 要移动的物体

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Sword")
        {
            animator.Play("pushed");

            var effectInstance = Instantiate(effect, transform.position + Vector3.up * 2, Quaternion.identity);
            Destroy(effectInstance, 1.5f);
        }
    }
}
