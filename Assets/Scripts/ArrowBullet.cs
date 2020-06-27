using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBullet : MonoBehaviour
{
    // Start is called before the first frame update

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == 9)
        {
            Destroy(coll.gameObject);
        }
        if (coll.gameObject.layer == 10)
        {
           // GetComponentInParent<Rigidbody>().freezeRotation = true;
           // GetComponentInParent<Rigidbody>().transform.position = transform.position;
            GetComponentInParent<Rigidbody>().Sleep();
        }
    }
    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Destroy(collision.gameObject);
        }
    } */
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
