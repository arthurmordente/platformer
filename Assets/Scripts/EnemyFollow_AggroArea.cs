using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow_AggroArea : MonoBehaviour
{
    [SerializeField] private EnemyFollow_Move enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.VerifyAggro(true);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            enemy.VerifyAggro(false);
        }
    }
}
