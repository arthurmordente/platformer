using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyFollow_Move : MonoBehaviour
{
    private PlayerController player;
    private Rigidbody rb;
    [SerializeField] private float maxSpeed;
    private bool aggroActive = false;
    private float moveSpeed = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (aggroActive)
        {
            Vector3 direction = (transform.position - player.transform.position) * -1;
            rb.AddForce(direction * maxSpeed * Time.deltaTime, ForceMode.Acceleration);
        }

        if(!player.GetStageManager().GetCurrentStage().IsWithinBounds(transform.position))
        {
            Destroy(gameObject);
        }
    }

    public void VerifyAggro(bool active)
    {
        aggroActive = active;
    }
}
