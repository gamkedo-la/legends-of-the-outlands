using UnityEngine;
using System.Collections;

public class CarryableObject : MonoBehaviour
{
    public virtual void dropObject()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false; //Turn inertia and gravity back on
        transform.parent = transform.parent.parent;
    }

    public virtual void dropObject(RaycastHit hit)
    {
        dropObject();
    }
}