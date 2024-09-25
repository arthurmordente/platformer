using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePlayerGravity : MonoBehaviour
{
    private Rigidbody rb;
    public float jumpForce = 100f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody não encontrado no PlayerController!");
        }        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("GravityButton"))
        {
            OnGravityButtonPressed();
        }
    }

    // This method is called when the button is pressed
    void OnGravityButtonPressed()
    {
        if (rb.useGravity == true)
        {
            rb.useGravity = false;
            // Apply upward force to the player
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
        }
        else
            rb.useGravity = true; // Disables gravity
    }
}
