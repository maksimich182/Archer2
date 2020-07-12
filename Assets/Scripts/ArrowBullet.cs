using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBullet : MonoBehaviour
{
    public Collider arrow;
    public Collider arrowHead;
    public float timeToDestroy = 5;

    IEnumerator OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == 9)
        {
            Destroy(coll.gameObject);
        }
        if (coll.gameObject.layer == 10)
        {
            arrowHead.GetComponent<Collider>().isTrigger = false;
            arrow.GetComponent<Rigidbody>().Sleep();
            yield return wait(timeToDestroy);
            arrowHead.GetComponent<Collider>().isTrigger = true;
            arrow.GetComponent<Collider>().isTrigger = true;
        }
    }

    IEnumerator wait(float waitTime)
    {
        float counter = 0;

        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            yield return null;
        }
    }

}
