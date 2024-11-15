using UnityEngine;

public class ControleNave : MonoBehaviour
{
    public float velocidade = 10f;           // Velocidade de movimento da nave
    public float velocidadeSubida = 5f;      // Velocidade de subida (tecla Space)
    public float rotacaoVelocidade = 100f;   // Velocidade de rota��o da nave
    public LayerMask layerSolo;              // Camada que define o que � considerado solo
    private Rigidbody rb;                    // Refer�ncia ao Rigidbody da nave
    private bool noAr = false;               // Verifica se a nave est� no ar
    private FixedJoint fixedJoint;          // The joint will be attached to this object
    private Transform cameraPrincipal;       // Refer�ncia � c�mera principal
    private bool isEquiped = false;
    private MeshRenderer playerMeshRenderer;
    private BoxCollider playerBoxCollider;

    void Start()
    {
        // Obt�m o componente Rigidbody anexado � nave
        rb = GetComponent<Rigidbody>();

        // Obt�m a refer�ncia � c�mera principal
        cameraPrincipal = Camera.main.transform;
    }

    void FixedUpdate()
    {
        // Verifica se a nave est� no ar usando um Raycast
        noAr = !Physics.Raycast(transform.position, Vector3.down, 1.5f, layerSolo);
        Vector3 movimento;
        bool inputSubir;

        // Atualiza a rota��o da nave para sempre estar de frente para a c�mera
        Vector3 direcaoParaCamera = cameraPrincipal.forward;
        direcaoParaCamera.y = 0; // Ignora a inclina��o vertical para manter a nave no plano horizontal
        transform.rotation = Quaternion.LookRotation(direcaoParaCamera);

        if (!noAr && isEquiped)
        {
            inputSubir = Input.GetKey(KeyCode.Space);       // Tecla Space
            
            // Adiciona movimento de subida se a tecla Space for pressionada
            if (inputSubir)
            {
                movimento = transform.up * velocidadeSubida * Time.deltaTime;
                // Aplica a for�a de movimento � nave
                rb.MovePosition(rb.position + movimento);
            }

            
        }
        
        // S� permite movimento se a nave estiver no ar
        if (noAr && isEquiped)
        {
            // Captura a entrada do usu�rio para movimento horizontal (WASD)
            float inputHorizontal = Input.GetAxis("Horizontal"); // Teclas A e D
            float inputVertical = Input.GetAxis("Vertical");     // Teclas W e S
            inputSubir = Input.GetKey(KeyCode.Space);       // Tecla Space

            // Movimenta a nave para frente e para tr�s (W/S)
            movimento = transform.forward * inputVertical * velocidade * Time.deltaTime;

            // Movimenta a nave para os lados (A/D) sem inclina��o ou rota��o
            movimento += transform.right * inputHorizontal * velocidade * Time.deltaTime;

            // Adiciona movimento de subida se a tecla Space for pressionada
            if (inputSubir)
            {
                movimento += transform.up * velocidadeSubida * Time.deltaTime;
            }

            // Aplica a for�a de movimento � nave
            rb.MovePosition(rb.position + movimento);

            // Captura a entrada do usu�rio para rota��o
            float rotacaoInput = Input.GetAxis("Horizontal");
            // Aplica rota��o � nave
            Vector3 rotacao = Vector3.up * rotacaoInput * rotacaoVelocidade * Time.deltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotacao));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with another object (you can specify a tag or other conditions here)
        if (collision.gameObject.CompareTag("Player"))
        {
            isEquiped = true;
            // If this object doesn't already have a joint, attach one
            if (fixedJoint == null)
            {
                fixedJoint = gameObject.AddComponent<FixedJoint>();

                // Attach the other object to the joint
                fixedJoint.connectedBody = collision.rigidbody;

                // Hide the Player Mesh
                playerMeshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
                playerMeshRenderer.enabled = false;
                playerBoxCollider = collision.gameObject.GetComponent<BoxCollider>();
                playerBoxCollider.enabled = false;
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collide");
            ReleaseJoint();
        }
    }

    void ReleaseJoint()
    {
        // Check if a joint exists and destroy it
        if (fixedJoint != null)
        {
            Destroy(fixedJoint);
            fixedJoint = null;
        }

        // Re-enable the player's MeshRenderer and BoxCollider
        if (playerMeshRenderer != null)
        {
            playerMeshRenderer.enabled = true;
        }

        if (playerBoxCollider != null)
        {
            playerBoxCollider.enabled = true;
        }

        // Update the isEquiped status
        isEquiped = false;

        Destroy(this.gameObject);

    }
}
