using UnityEngine;
using TMPro;

public class Deposito : MonoBehaviour
{
    public float maxMass = 10f; // Máxima massa que o depósito pode armazenar
    [SerializeField]
    public float currentMass = 0f; // Massa atual armazenada no depósito, configurável no editor
    public float transferRate = 1f; // Taxa de transferência de massa por segundo
    public float interactionDistance = 5f; // Distância mínima para interagir com o depósito

    [SerializeField] private Vector3 initialPosition; // Posição inicial
    [SerializeField] private float initialMass = 0f; // Massa inicial configurável no editor
    private Renderer depositRenderer; // Referência ao Renderer do depósito
    private Collider depositCollider; // Referência ao Collider do depósito

    public TMP_Text popUpText; // Componente de texto para mostrar a massa armazenada
    private bool isPlayerNearby = false; // Flag para verificar se o jogador está próximo
    private PlayerController player; // Referência ao jogador

    void Start()
    {
        depositRenderer = GetComponent<Renderer>();
        depositCollider = GetComponent<Collider>();

        if (depositRenderer == null)
        {
            Debug.LogError("Renderer não encontrado no depósito!");
        }

        if (depositCollider == null)
        {
            Debug.LogError("Collider não encontrado no depósito!");
        }

        if (popUpText == null)
        {
            Debug.LogError("PopUpText não encontrado no depósito!");
        }

        // Inicializa a massa inicial e a posição
        SaveInitialState();
        Reset();
    }

    public void TransferMassToPlayer(PlayerController player)
    {
        if (currentMass > 0)
        {
            float massToTransfer = Mathf.Min(transferRate * Time.deltaTime, currentMass);
            player.AddMass(massToTransfer);
            currentMass -= massToTransfer;
            UpdatePopUpText();
            StaticPopUpHandler.ShowPopUp(popUpText, $"{currentMass*10:F0}/{maxMass*10}", this);
            UpdateColorAndCollisionState(); // Atualiza a cor após modificar a massa
        }
    }

    public void ReceiveMassFromPlayer(PlayerController player)
    {
        if (player.GetCurrentMass() > 0 && currentMass < maxMass)
        {
            float massToTransfer = Mathf.Min(transferRate * Time.deltaTime, maxMass - currentMass, player.GetCurrentMass());
            player.RemoveMass(massToTransfer);
            currentMass += massToTransfer;
            UpdatePopUpText();
            StaticPopUpHandler.ShowPopUp(popUpText, $"{currentMass*10:F0}/{maxMass*10}", this);
            UpdateColorAndCollisionState(); // Atualiza a cor após modificar a massa
        }
    }

    public void SaveInitialState()
    {
        initialPosition = transform.position;
    }

    public void Reset()
    {
        transform.position = initialPosition;
        currentMass = initialMass; // Restaura a massa inicial
        UpdateColorAndCollisionState();
        UpdatePopUpText();
        StaticPopUpHandler.HidePopUp(popUpText, this); // Esconde o pop-up imediatamente ao resetar
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player == null)
            {
                player = other.GetComponent<PlayerController>();
            }
            isPlayerNearby = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player == null)
            {
                player = collision.gameObject.GetComponent<PlayerController>();
            }
            isPlayerNearby = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdatePopUpText();
            StaticPopUpHandler.ShowPopUp(popUpText, $"{currentMass*10:F0}/{maxMass*10}", this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            player = null;
            StaticPopUpHandler.HidePopUpAfterDelay(popUpText, 3f, this);
        }
    }

    private void UpdatePopUpText()
    {
        if (popUpText != null)
        {
            StaticPopUpHandler.ShowPopUp(popUpText, $"{currentMass*10:F0}/{maxMass*10}", this);
        }
    }

    private void UpdateColorAndCollisionState()
    {
        if (depositRenderer != null)
        {
            float fillPercentage = currentMass / maxMass;
            Color newColor = Color.Lerp(Color.white, Color.black, fillPercentage);
            depositRenderer.material.color = newColor;
        }

        if (depositCollider != null)
        {
            depositCollider.isTrigger = currentMass < maxMass; // Transponível se não estiver cheio
        }
    }
}
