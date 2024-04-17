using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCell : MonoBehaviour
{
    public Vector3 originPosition;
    public Transform center;

    public Rigidbody rb;

    void Start()
    {
        originPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        StartMove();
    }

    public void StartMove()
    {
        Vector3 vector = transform.position - center.position;

        rb.AddForce(vector * 2, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(1,2),Random.Range(1,2),Random.Range(1,2)), ForceMode.Impulse);
    }

    public void StartGoBack()
    {
        StartCoroutine(startMoveBack());

        IEnumerator startMoveBack()
        {
            rb.useGravity = false;

            while (Vector3.Distance(transform.position, originPosition) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, originPosition, 0.1f);
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
