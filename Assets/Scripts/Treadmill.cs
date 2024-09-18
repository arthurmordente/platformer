using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    [SerializeField] private float speed;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerScript = other.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.WalkBost(transform.forward.normalized, speed * Time.deltaTime);
                //Debug.Log(transform.forward.normalized);
            }

        }
    }
}
