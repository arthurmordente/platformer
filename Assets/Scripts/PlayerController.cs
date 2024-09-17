using UnityEngine;
using System.Collections.Generic; // Necessário para usar listas
using System.Linq; // Necessário para usar métodos de consulta

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float currentMass = 10f; // Massa atual do jogador
    public float interactionDistance = 5f; // Distância mínima para interagir com o depósito

    [SerializeField]
    private bool isGrounded;
    private Rigidbody rb;
    private StageManager stageManager;
    [SerializeField]
    public Transform groundCheck; // Ponto de verificação do chão
    public float groundCheckRadius = 0.2f; // Raio da verificação
    public LayerMask groundLayer; // Camada do chão

    private Deposito nearestDeposito; // Referência ao depósito mais próximo

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
       FHandleMovement();
    }	


    // Método para lidar com a movimentação do jogador
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveInput, 0, moveVertical) * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
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
            stageManager.RespawnPlayer();
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
}
