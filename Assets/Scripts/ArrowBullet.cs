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
            yield return Assets.Scripts.SupportFunction.wait(timeToDestroy);
            arrowHead.GetComponent<Collider>().isTrigger = true;
            arrow.GetComponent<Collider>().isTrigger = true;
        }
    }
}
