using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    public Vector3 pointA; // Ponto inicial no espaço 3D
    public Vector3 pointB; // Ponto final no espaço 3D
    public float duration = 2.0f; // Tempo para a plataforma ir de A para B (em segundos)
    public bool loop = true; // Define se a plataforma deve ir e voltar entre os pontos
    public float delayAfterExit = 0.5f; // Tempo que a plataforma continua se movendo após o jogador sair

    private float timer;
    private bool goingToB = true; // Determina a direção atual da plataforma
    private bool isMoving = false; // Flag para controlar se a plataforma está se movendo
    private float exitTimer = 0f; // Temporizador para controlar o movimento após a saída

    private Vector3 initialPosition; // Posição inicial da plataforma
    private Quaternion initialRotation; // Rotação inicial da plataforma

    void Start()
    {
        // Inicializa a posição da plataforma no ponto A
        transform.position = pointA;
        SaveInitialState(); // Salva o estado inicial da plataforma
    }

    void Update()
    {
        // Mover a plataforma enquanto isMoving for verdadeiro
        if (isMoving)
        {
            Move();
        }

        // Se o temporizador de saída estiver ativo, continue movendo até o tempo acabar
        if (exitTimer > 0f)
        {
            exitTimer -= Time.deltaTime;
            if (exitTimer <= 0f)
            {
                isMoving = false; // Para o movimento após o atraso
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isMoving = true; // Começa a mover a plataforma quando o jogador entra em contato
            exitTimer = 0f; // Reseta o temporizador de saída
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isMoving = true; // Continua movendo enquanto o jogador estiver em contato
            exitTimer = 0f; // Reseta o temporizador de saída
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            exitTimer = delayAfterExit; // Inicia o temporizador de saída
        }
    }

    public void SaveInitialState()
    {
        initialPosition = transform.position; // Salva a posição inicial
        initialRotation = transform.rotation; // Salva a rotação inicial
    }

    public void Reset()
    {
        transform.position = initialPosition; // Restaura a posição inicial
        transform.rotation = initialRotation; // Restaura a rotação inicial
        timer = 0f; // Reseta o temporizador de movimento
        isMoving = false; // Reseta o estado de movimento
    }

    void Move()
    {
        timer += Time.deltaTime / duration;

        if (goingToB)
        {
            // Move a plataforma de A para B
            transform.position = Vector3.Lerp(pointA, pointB, timer);
        }
        else
        {
            // Move a plataforma de B para A
            transform.position = Vector3.Lerp(pointB, pointA, timer);
        }

        // Quando a plataforma chega ao destino, inverte a direção
        if (timer >= 1.0f)
        {
            timer = 0f;
            goingToB = !goingToB;
            if (!loop)
            {
                enabled = false; // Desativa o script se o loop estiver desativado
            }
        }
    }
}
