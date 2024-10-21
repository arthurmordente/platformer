using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Gate gate;  // Referência ao objeto Gate que será controlado
    private bool isActive = false; // Estado da PressurePlate (ativa ou não)

    private void OnTriggerStay(Collider other)
    {
        // Verifica se o objeto que está na placa é o player ou um objeto movível
        if (other.CompareTag("Player") || other.CompareTag("PushableObject"))
        {
            Activate();  // Mantém a PressurePlate ativada enquanto o objeto está em contato
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Desativa a PressurePlate quando o objeto sai de cima dela
        if (other.CompareTag("Player") || other.CompareTag("PushableObject"))
        {
            Deactivate();  // Fecha o portão quando o objeto sai
        }
    }

    // Método para ativar a PressurePlate (pelo Raycast ou pelo contato físico)
    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            gate.ActivateGate(true);  // Ativa o portão quando a PressurePlate é ativada
            Debug.Log("PressurePlate ativada!");
        }
    }

    // Método para desativar a PressurePlate (se necessário)
    public void Deactivate()
    {
        if (isActive)
        {
            isActive = false;
            gate.ActivateGate(false);  // Desativa o portão quando a PressurePlate é desativada
            Debug.Log("PressurePlate desativada!");
        }
    }
}
