using UnityEngine;

public class Jetpack : MonoBehaviour
{
    private Vector3 startPosition; // Posição inicial do jetpack
    private Rigidbody rb; // Referência ao Rigidbody do jetpack

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o componente Rigidbody
        SaveInitialState(); // Salva o estado inicial do jetpack
    }

    public void SaveInitialState()
    {
        startPosition = transform.position; // Salva a posição inicial
    }

    public void Reset()
    {
        // Reseta o jetpack ao estado inicial
        transform.position = startPosition; // Restaura a posição inicial

        // Reseta a velocidade linear e angular do jetpack
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Zera a velocidade
            rb.angularVelocity = Vector3.zero; // Zera a rotação
        }

        gameObject.SetActive(true); // Ativa o jetpack
    }
}
