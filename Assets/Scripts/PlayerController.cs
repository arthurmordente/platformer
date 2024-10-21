using UnityEngine;
using System.Collections.Generic; // Necessário para usar listas
using System.Linq; // Necessário para usar métodos de consulta

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 50f;
    public float currentMass = 10f; // Massa atual do jogador
    public float interactionDistance = 5f; // Distância mínima para interagir com o depósito
    public Transform[] playerHands;
    public float throwForce = 10f; // Força usada para arremessar o objeto

    [SerializeField]
    private bool isGrounded;
    private Rigidbody rb;
    private StageManager stageManager;
    [SerializeField]
    public Transform groundCheck; // Ponto de verificação do chão
    public float groundCheckRadius = 0.2f; // Raio da verificação
    public LayerMask groundLayer; // Camada do chão

    private Deposito nearestDeposito; // Referência ao depósito mais próximo
    private GameObject heldObject = null;
    private int currentHandIndex = 0; // Índice da mão atual no array

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = currentMass; // Sincroniza a massa inicial do Rigidbody com a variável currentMass
            UpdateSizeBasedOnMass(); // Ajusta o tamanho inicial com base na massa
        }
        else
        {
            Debug.LogError("Rigidbody não encontrado no PlayerController!");
        }
        stageManager = FindObjectOfType<StageManager>(); // Obtém a referência ao StageManager
    }

    /*void Update()
    {
        if (stageManager != null)
        {
            stageManager.GetCurrentStage().UpdateCameraPosition(transform.position);
        }
    }*/

    void Update()
    {
        // Verifica se o personagem está no chão
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Modularização das atividades
        //HandleMovement(); // Movimentação do jogador
        HandleJump(); // Pulo do jogador
        HandleReceiveMassInput(); // Input para receber massa
        HandleGiveMassInput(); // Input para depositar massa
        HandleObjectInteraction();
        HandleHandSwitching(); // Troca de mãos

        // Verifica se o jogador está fora dos limites do estágio atual
        if (!IsPlayerWithinBounds())
        {
            Respawn();
        }
        if (stageManager != null)
        {
            stageManager.GetCurrentStage().UpdateCameraPosition(transform.position);
        }
    }

    void FixedUpdate()
    {
       HandleMovement();
    }	


    // Método para lidar com a movimentação do jogador
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveInput, 0, moveVertical) * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
        //rb.AddForce(movement, ForceMode.VelocityChange);
    }

    private void FHandleMovement()
    {       
        float moveInput = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Cria um vetor de movimento baseado nas entradas horizontais e verticais
        Vector3 movement = new Vector3(moveInput, 0, moveVertical).normalized;

        // Aplica a força ao Rigidbody
        if(movement != Vector3.zero){
            Debug.Log("Aplicando forca");
        }
        rb.AddForce(movement * moveSpeed, ForceMode.Acceleration);
          
    }


    // Método para lidar com o pulo do jogador
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Método para lidar com o input de receber massa do depósito
    private void HandleReceiveMassInput()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Deposito depositoParaReceber = GetNearestDepositoToReceive();
            if (depositoParaReceber != null)
            {
                depositoParaReceber.TransferMassToPlayer(this); // Recebe massa do depósito
                UpdateSizeBasedOnMass(); // Atualiza o tamanho após receber massa
            }
        }
    }

    // Método para lidar com o input de depositar massa no depósito
    private void HandleGiveMassInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Deposito depositoParaDepositar = GetNearestDepositoToGive();
            if (depositoParaDepositar != null)
            {
                depositoParaDepositar.ReceiveMassFromPlayer(this); // Deposita massa no depósito
                UpdateSizeBasedOnMass(); // Atualiza o tamanho após depositar massa
            }
        }
    }

    private bool IsPlayerWithinBounds()
    {
        Stage currentStage = stageManager.GetCurrentStage();
        if (currentStage != null)
        {
            return currentStage.IsWithinBounds(transform.position); // Verifica se o jogador está dentro dos limites do estágio
        }
        return true; // Retorna true por padrão se não houver estágio atual
    }

    private Deposito GetNearestDepositoToReceive()
    {
        Deposito[] allDepositos = FindObjectsOfType<Deposito>();
        List<Deposito> validDepositos = allDepositos
            .Where(d => Vector3.Distance(transform.position, d.transform.position) <= interactionDistance && d.currentMass > 0)
            .OrderBy(d => Vector3.Distance(transform.position, d.transform.position))
            .ToList();

        return validDepositos.FirstOrDefault(); // Retorna o depósito mais próximo capaz de realizar a ação
    }

    private Deposito GetNearestDepositoToGive()
    {
        Deposito[] allDepositos = FindObjectsOfType<Deposito>();
        List<Deposito> validDepositos = allDepositos
            .Where(d => Vector3.Distance(transform.position, d.transform.position) <= interactionDistance && d.currentMass < d.maxMass)
            .OrderBy(d => Vector3.Distance(transform.position, d.transform.position))
            .ToList();

        return validDepositos.FirstOrDefault(); // Retorna o depósito mais próximo capaz de realizar a ação
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Deposito"))
        {
            nearestDeposito = other.GetComponent<Deposito>(); // Armazena o depósito mais próximo
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Deposito"))
        {
            nearestDeposito = null; // Limpa a referência ao depósito quando sai do alcance
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Respawn();
        }
        else if (collision.gameObject.CompareTag("Trampoline"))
        {
            isGrounded = true;
            jumpForce = 80f;
        }
    }
    
    public void RemoveMass(float mass)
    {
        currentMass = Mathf.Max(0f, currentMass - mass); // Atualiza a massa do jogador
        UpdateRigidbodyMass(); // Atualiza a massa do Rigidbody
        UpdateSizeBasedOnMass(); // Atualiza o tamanho após remover massa
    }

    public void AddMass(float mass)
    {
        currentMass += mass; // Atualiza a massa do jogador
        UpdateRigidbodyMass(); // Atualiza a massa do Rigidbody
        UpdateSizeBasedOnMass(); // Atualiza o tamanho após adicionar massa
    }

    public float GetCurrentMass()
    {
        return currentMass;
    }

    private void UpdateRigidbodyMass()
    {
        if (rb != null)
        {
            rb.mass = currentMass;
        }
    }

    private void UpdateSizeBasedOnMass()
    {
        // Calcula o tamanho do cubo com base na massa
        float minSize = 0.4f; // Tamanho mínimo para massa de 1
        float maxSize = 2f;   // Tamanho máximo para massa de 20
        float minMass = 1f;   // Massa mínima
        float maxMass = 20f;  // Massa máxima

        // Mapeia a massa atual para o tamanho correspondente
        float scaleFactor = Mathf.Lerp(minSize, maxSize, (currentMass - minMass) / (maxMass - minMass));
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    public void Spawn()
    {
        if (stageManager != null)
        {
            stageManager.RespawnPlayer();
            ResetPlayerVelocity(); // Zera a velocidade do jogador ao respawnar
            UpdateRigidbodyMass();
            UpdateSizeBasedOnMass(); // Atualiza o tamanho ao respawnar
        }
        else
        {
            Debug.LogError("StageManager não encontrado!");
        }
    }

    public void Respawn()
    {
        if (stageManager != null)
        {
            stageManager.RespawnPlayer();
            ResetPlayerVelocity(); // Zera a velocidade do jogador ao respawnar
            UpdateRigidbodyMass();
            UpdateSizeBasedOnMass(); // Atualiza o tamanho ao respawnar
            stageManager.ResetCurrentStage();
        }
        else
        {
            Debug.LogError("StageManager não encontrado!");
        }
    }

    private void ResetPlayerVelocity()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Zera a velocidade linear
            rb.angularVelocity = Vector3.zero; // Zera a velocidade angular
        }
    }

    public StageManager GetStageManager()
    {
        return stageManager;
    }

    public void WalkBost(Vector3 direction, float velocity)
    {
        rb.AddForce(direction * velocity, ForceMode.VelocityChange);
    }

    private void HandleObjectInteraction()
    {
        if (Input.GetMouseButtonDown(0)) // Botão esquerdo do mouse
        {
            if (heldObject == null)
            {
                TryPickupObject(); // Tenta pegar o objeto
            }
            else
            {
                ThrowObject(); // Arremessa o objeto
            }
        }
    }

    private void TryPickupObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("PushableObject"))
            {
                heldObject = collider.gameObject;
                heldObject.transform.SetParent(playerHands[currentHandIndex]); // Define o objeto como filho da mão atual
                heldObject.transform.localPosition = Vector3.zero; // Move o objeto para a posição da mão
                Rigidbody heldObjectRb = heldObject.GetComponent<Rigidbody>();
                if (heldObjectRb != null)
                {
                    heldObjectRb.isKinematic = true; // Torna o objeto estático enquanto está sendo segurado
                }
                break;
            }
        }
    }

    private void HandleHandSwitching()
    {
        if (Input.GetMouseButtonDown(1)) // Botão direito do mouse
        {
            if (heldObject != null)
            {
                currentHandIndex = (currentHandIndex + 1) % playerHands.Length; // Troca para a próxima mão
                heldObject.transform.SetParent(playerHands[currentHandIndex]); // Move o objeto para a nova mão
                heldObject.transform.localPosition = Vector3.zero; // Posiciona o objeto na nova mão
            }
        }
    }

    // Solta o objeto que está sendo segurado
    // Arremessa o objeto com base na mão atual
    private void ThrowObject()
    {
        if (heldObject != null)
        {
            Rigidbody heldObjectRb = heldObject.GetComponent<Rigidbody>();
            if (heldObjectRb != null)
            {
                heldObject.transform.SetParent(null); // Remove o objeto da mão
                heldObjectRb.isKinematic = false; // Permite que o objeto volte a ser afetado pela física

                // Determina a direção do arremesso com base na mão atual
                Vector3 throwDirection = GetThrowDirectionForCurrentHand();
                heldObjectRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            }
            heldObject = null; // Limpa a referência do objeto após o arremesso
        }
    }

    // Retorna a direção do arremesso com base na mão atual (direções cardeais)
    private Vector3 GetThrowDirectionForCurrentHand()
    {
        switch (currentHandIndex)
        {
            case 0: // Mão 1 (Norte)
                return Vector3.forward; // Arremessa para o Norte (frente)
            case 1: // Mão 2 (Leste)
                return Vector3.right; // Arremessa para o Leste (direita)
            case 2: // Mão 3 (Sul)
                return Vector3.back; // Arremessa para o Sul (trás)
            case 3: // Mão 4 (Oeste)
                return Vector3.left; // Arremessa para o Oeste (esquerda)
            default:
                return Vector3.forward; // Arremessa para frente por padrão (caso inesperado)
        }
    }

}
