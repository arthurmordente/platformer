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
        }
        else
        {
            Debug.LogError("Rigidbody não encontrado no PlayerController!");
        }
        stageManager = FindObjectOfType<StageManager>(); // Obtém a referência ao StageManager
    }

    void Update()
    {
        // Verificar se o personagem está no chão
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimento lateral
        float moveInput = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // MovePosition (MoveSpeed = 0.5, JumpForce = 50)
        Vector3 movement = new Vector3(moveInput, 0, moveVertical) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // velocity (MoveSpeed = 20, JumpForce = 500)
        /* Vector3 movement = new Vector3(moveInput, 0, moveVertical) * moveSpeed;
        if (!isGrounded) { movement.y  = -5; }
        rb.velocity = movement;*/

        // Pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Detecta a entrada do jogador para interagir com o depósito mais próximo que possa realizar a ação
        if (Input.GetKey(KeyCode.Q))
        {
            Deposito depositoParaReceber = GetNearestDepositoToReceive();
            if (depositoParaReceber != null)
            {
                depositoParaReceber.TransferMassToPlayer(this); // Recebe massa do depósito
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            Deposito depositoParaDepositar = GetNearestDepositoToGive();
            if (depositoParaDepositar != null)
            {
                depositoParaDepositar.ReceiveMassFromPlayer(this); // Deposita massa no depósito
            }
        }

        // Respawn automático ao cair fora da plataforma
        if (CheckOutOfBounds())
        {
            Respawn();
        }
    }

    private bool CheckOutOfBounds(){
        if (transform.position.y < -5 || transform.position.y > 50)
        {
            return true;
        }
        return false;
    }
    private Deposito GetNearestDepositoToReceive()
    {
        // Obter todos os depósitos próximos ao jogador
        Deposito[] allDepositos = FindObjectsOfType<Deposito>();
        List<Deposito> validDepositos = allDepositos
            .Where(d => Vector3.Distance(transform.position, d.transform.position) <= interactionDistance && d.currentMass > 0)
            .OrderBy(d => Vector3.Distance(transform.position, d.transform.position))
            .ToList();

        return validDepositos.FirstOrDefault(); // Retorna o depósito mais próximo capaz de realizar a ação
    }

    private Deposito GetNearestDepositoToGive()
    {
        // Obter todos os depósitos próximos ao jogador
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
    }

    public void AddMass(float mass)
    {
        currentMass += mass; // Atualiza a massa do jogador
        UpdateRigidbodyMass(); // Atualiza a massa do Rigidbody
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

    public void Spawn()
    {
        if (stageManager != null)
        {
            stageManager.RespawnPlayer();
            ResetPlayerVelocity(); // Zera a velocidade do jogador ao respawnar
            UpdateRigidbodyMass();
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

    // Desenha o indicador visual da distância mínima de interação no editor
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // Cor do gizmo
        Gizmos.DrawWireSphere(transform.position, interactionDistance); // Desenha a esfera com o raio de interação
    }*/
}
