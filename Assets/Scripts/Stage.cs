using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform spawnPoint; // Ponto de spawn do jogador
    public float initialMass = 10f;
    public int totalKeysRequired; // Número total de chaves necessárias para o estágio
    public GameObject[] keys; // Chaves no estágio
    public GameObject[] enemies; // Inimigos no estágio
    public GameObject[] depositos; // Depósitos no estágio
    public GameObject[] platforms; // Plataformas móveis no estágio

    public Vector3 cameraPosition; // Posição desejada da câmera para o estágio
    public Quaternion cameraRotation; // Rotação desejada da câmera para o estágio

    public Vector2 xBounds; // Limites em X para o estágio
    public Vector2 yBounds; // Limites em Y para o estágio

    private Goal goal; // Referência ao Goal do estágio

    void Start()
    {
        // Encontra o Goal filho e armazena a referência
        goal = GetComponentInChildren<Goal>();
        if (goal != null)
        {
            goal.totalKeysRequired = totalKeysRequired; // Define o total de chaves necessárias no Goal
        }
        else
        {
            Debug.LogError("Goal não encontrado como filho do Stage!");
        }

        // Armazena os estados iniciais dos depósitos, chaves e plataformas
        SaveInitialState();
    }

    private void SaveInitialState()
    {
        // Armazena as posições e estados iniciais dos depósitos
        foreach (GameObject deposito in depositos)
        {
            var depositoScript = deposito.GetComponent<Deposito>();
            if (depositoScript != null)
            {
                depositoScript.SaveInitialState();
            }
        }

        // Armazena as posições iniciais das chaves
        foreach (GameObject key in keys)
        {
            var keyScript = key.GetComponent<Key>();
            if (keyScript != null)
            {
                keyScript.SaveInitialState();
            }
        }

        // Armazena as posições iniciais das plataformas
        foreach (GameObject platform in platforms)
        {
            var platformScript = platform.GetComponent<ElevatorPlatform>();
            if (platformScript != null)
            {
                platformScript.SaveInitialState();
            }
        }
    }

    public void ResetStage()
    {
        // Reseta a posição e estado dos inimigos
        foreach (GameObject enemy in enemies)
        {
            var enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Reset();
            }
        }

        // Reseta a posição das chaves
        foreach (GameObject key in keys)
        {
            var keyScript = key.GetComponent<Key>();
            if (keyScript != null)
            {
                keyScript.Reset();
            }
        }

        // Reseta os depósitos aos seus estados iniciais
        foreach (GameObject deposito in depositos)
        {
            var depositoScript = deposito.GetComponent<Deposito>();
            if (depositoScript != null)
            {
                depositoScript.Reset();
            }
        }

        // Reseta as plataformas móveis aos seus estados iniciais
        foreach (GameObject platform in platforms)
        {
            var platformScript = platform.GetComponent<ElevatorPlatform>();
            if (platformScript != null)
            {
                platformScript.Reset();
            }
        }

        // Reseta o Goal, se existir
        if (goal != null)
        {
            goal.ResetGoal();
        }
    }

    // Método para verificar se uma posição está dentro dos limites do estágio
    public bool IsWithinBounds(Vector3 position)
    {
        return position.x >= xBounds.x && position.x <= xBounds.y &&
               position.y >= yBounds.x && position.y <= yBounds.y;
    }

    // Adiciona um botão ao inspector para configurar automaticamente os limites
    [ContextMenu("Configurar Limites Padrão")]
    private void ConfigureDefaultBounds()
    {
        xBounds = new Vector2(-15, 30);
        yBounds = new Vector2(-5, 30);
    }
}
