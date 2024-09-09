using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 startPosition; // Posição inicial do inimigo

    void Start()
    {
        startPosition = transform.position;
    }

    public void Reset()
    {
        // Reseta o inimigo ao estado inicial
        transform.position = startPosition;
        // Se o inimigo tiver outras variáveis de estado, você pode resetá-las aqui
    }
}
