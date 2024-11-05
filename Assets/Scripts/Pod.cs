using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod : MonoBehaviour
{
    // The joint will be attached to this object
    private FixedJoint fixedJoint;

    [SerializeField] Key podKey;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with another object (you can specify a tag or other conditions here)
        if (collision.gameObject.CompareTag("PodItem"))
        {
            // If this object doesn't already have a joint, attach one
            if (fixedJoint == null)
            {
                fixedJoint = gameObject.AddComponent<FixedJoint>();

                // Attach the other object to the joint
                fixedJoint.connectedBody = collision.rigidbody;
                podKey.gameObject.SetActive(true);

                /* Optionally, you can set additional joint properties here, like the break force
                fixedJoint.breakForce = 10000f; // Max force to break the joint
                */
            }
        }
    }

    /* Optionally, you can add a way to manually release the joint (e.g., when the player presses a button)
    public void Release()
    {
        if (fixedJoint != null)
        {
            Destroy(fixedJoint);
        }
    }
    */
}
