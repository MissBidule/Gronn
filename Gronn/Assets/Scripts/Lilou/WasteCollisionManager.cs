using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteCollisionManager : MonoBehaviour
{

    public float force;
    public int trash = -1;
    public bool hasScored = false;

   //Detect collisions between the waste and the plank
    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "up")
        {
            //Debug.Log("touché");
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(collision.gameObject.transform.up*force, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        hasScored = true;
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "TrashbinBrown")
        {
            trash = 0;
        }
        if (collision.gameObject.name == "TrashbinGreen")
        {
            trash = 1;
        }
        if (collision.gameObject.name == "TrashbinYellow")
        {
            trash = 2;
        }
        if (collision.gameObject.name == "DeadZone")
        {
            trash = 3;
        }
    }
}
