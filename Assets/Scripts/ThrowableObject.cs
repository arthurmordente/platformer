using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Vector3 startPosition; // Posição inicial do objeto
    private Rigidbody rb; // Referência ao Rigidbody do objeto
    private bool isThrown; // Indica se o objeto foi arremessado
    public Transform originalParent; // Pai original do objeto

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o componente Rigidbody
        SaveInitialState(); // Salva o estado inicial do objeto
    }

    // Salva a posição inicial e o pai original do objeto
    public void SaveInitialState()
    {
        startPosition = transform.position; // Salva a posição inicial
        originalParent = transform.parent; // Salva o pai original
        isThrown = false; // Define como não arremessado no início
    }

    // Método para resetar o objeto à sua posição inicial
    public void Reset()
    {
        // Reseta a posição e o estado do objeto
        transform.position = startPosition; // Restaura a posição inicial
        isThrown = false; // Restaura o estado de não arremessado

        // Reseta a velocidade linear e angular do Rigidbody
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Zera a velocidade
            rb.angularVelocity = Vector3.zero; // Zera a rotação
        }

        // Volta a ser filho do pai original
        transform.SetParent(originalParent);

        // Opcionalmente, reativa o objeto caso tenha sido desativado
        gameObject.SetActive(true); // Ativa o objeto, se necessário
    }

    // Método chamado quando o objeto é arremessado
    public void Throw()
    {
        isThrown = true; // Marca o objeto como arremessado
        // Implementar qualquer lógica adicional de arremesso aqui
    }

    // Método chamado para quando o jogador pegar o objeto (altera o pai para o jogador)
    public void PickUp(Transform newParent)
    {
        transform.SetParent(newParent); // Define o novo pai como o jogador
        rb.isKinematic = true; // Desabilita física enquanto o objeto está sendo segurado
    }

    // Método chamado quando o objeto é solto pelo jogador
    public void Drop()
    {
        transform.SetParent(null); // Solta o objeto (sem pai temporário)
        rb.isKinematic = false; // Volta a ser afetado pela física

        // Quando soltar o objeto, ele volta ao pai original
        Invoke("ReturnToOriginalParent", 0.1f); // Dá um pequeno delay para retornar ao pai original
    }

    // Volta a ser filho do pai original
    private void ReturnToOriginalParent()
    {
        if (!isThrown) // Apenas retorna ao pai se não estiver arremessado
        {
            transform.SetParent(originalParent); // Volta ao pai original
        }
    }

    // Exemplo de detecção de colisão
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && isThrown)
        {
            // Se o objeto foi arremessado e colidiu com o chão, pode ser que você queira ativar alguma lógica
            Debug.Log("Objeto colidiu com o chão após ser arremessado.");
        }
    }
}
