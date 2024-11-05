using UnityEngine;

public class HingePlatform : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Salva a posição e rotação iniciais da plataforma
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Método para resetar a posição e rotação para os valores iniciais
    public void ResetPlatform()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Zera a velocidade para garantir que a plataforma pare completamente
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
