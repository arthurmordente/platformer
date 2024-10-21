using UnityEngine;

public class Gate : MonoBehaviour
{
    public Vector3 openPosition;  // Posição aberta do portão
    public Vector3 closedPosition;  // Posição fechada do portão
    public float speed = 2f;  // Velocidade de abertura e fechamento

    private Vector3 targetPosition;  // Posição alvo para o movimento
    public bool isActivated = false;

    void Start()
    {
        closedPosition = transform.position;  // Define a posição inicial como fechada
        targetPosition = closedPosition;
    }

    void Update()
    {
        // Move o portão lentamente até a posição alvo
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void ActivateGate(bool isActive)
    {
        isActivated = isActive;

        if (isActivated)
        {
            targetPosition = openPosition;  // Move para a posição aberta
        }
        else
        {
            targetPosition = closedPosition;  // Move de volta para a posição fechada
        }
    }
}
