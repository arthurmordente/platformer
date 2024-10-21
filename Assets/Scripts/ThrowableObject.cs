using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Vector3 startPosition; // Posição inicial do objeto
    private Rigidbody rb; // Referência ao Rigidbody do objeto
    private bool isThrown; // Indica se o objeto foi arremessado

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o componente Rigidbody
        SaveInitialState(); // Salva o estado inicial do objeto
    }

    // Salva a posição inicial do objeto
    public void SaveInitialState()
    {
        startPosition = transform.position; // Salva a posição inicial
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

        // Opcionalmente, reativa o objeto caso tenha sido desativado
        gameObject.SetActive(true); // Ativa o objeto, se necessário
    }

    // Método chamado quando o objeto é arremessado
    public void Throw()
    {
        isThrown = true; // Marca o objeto como arremessado
        // Implementar qualquer lógica adicional de arremesso aqui
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
