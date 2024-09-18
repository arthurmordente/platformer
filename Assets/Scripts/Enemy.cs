using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 startPosition; // Posição inicial do inimigo
    private Rigidbody rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void Reset()
    {        
        // Reseta o inimigo ao estado inicial
        transform.position = startPosition;
        // Se o inimigo tiver outras variáveis de estado, você pode resetá-las aqui

        if (rb = null)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
