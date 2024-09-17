using UnityEngine;

public class Key : MonoBehaviour
{
    private Vector3 startPosition; // Posição inicial da chave
    private Rigidbody rb; // Referência ao Rigidbody da chave
    private bool isCollected; // Indica se a chave foi coletada

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o componente Rigidbody
        SaveInitialState(); // Salva o estado inicial da chave
    }

    public void SaveInitialState()
    {
        startPosition = transform.position; // Salva a posição inicial
        isCollected = false; // Define como não coletada no início
    }

    public void Reset()
    {
        // Reseta a chave ao estado inicial
        transform.position = startPosition; // Restaura a posição inicial
        isCollected = false; // Restaura o estado de não coletada

        // Reseta a velocidade linear e angular da chave
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Zera a velocidade
            rb.angularVelocity = Vector3.zero; // Zera a rotação
        }

        gameObject.SetActive(true); // Ativa a chave
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isCollected)
        {
            FindObjectOfType<Goal>().CollectKey();
            isCollected = true; // Marca a chave como coletada
            gameObject.SetActive(false); // Desativa a chave
        }
    }
}
