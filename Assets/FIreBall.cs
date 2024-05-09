using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FIreBall : MonoBehaviour
{
    public GameObject destoryEffect;

    public float speed = 10;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Hit");
        }

        Instantiate(destoryEffect, transform.position, Quaternion.identity);
        Destroy(destoryEffect , 2);
        Destroy(gameObject);
    }
}
