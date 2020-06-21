using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float maxSpeed;
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().transform.forward * -Random.Range(1, maxSpeed);
    }
}
