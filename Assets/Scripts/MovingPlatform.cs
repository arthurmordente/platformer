using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 pointA; // Ponto inicial no espaço 3D
    public Vector3 pointB; // Ponto final no espaço 3D
    public float duration = 2.0f; // Tempo para a plataforma ir de A para B (em segundos)
    public bool loop = true; // Define se a plataforma deve ir e voltar entre os pontos

    private float timer;
    private bool goingToB = true; // Determina a direção atual da plataforma

    void Start()
    {
        // Inicializa a posição da plataforma no ponto A
        transform.position = pointA;
    }

    void Update()
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
