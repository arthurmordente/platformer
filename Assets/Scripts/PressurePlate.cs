using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Gate gate;  // Referência ao objeto Gate que será controlado

    private void OnTriggerStay(Collider other)
    {
        // Verifica se o objeto que está na placa é o player ou um objeto movível
        if (other.CompareTag("Player") || other.CompareTag("PushableObject"))
        {
            gate.ActivateGate(true);  // Mantém o portão aberto enquanto o objeto está em contato
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PushableObject"))
        {
            gate.ActivateGate(false);  // Fecha o portão quando o objeto sai
        }
    }
}
