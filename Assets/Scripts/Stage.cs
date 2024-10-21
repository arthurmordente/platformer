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

    public GameObject[] throwables; // Plataformas móveis no estágio

    public Vector3 cameraPosition; // Posição desejada da câmera para o estágio
    public Quaternion cameraRotation; // Rotação desejada da câmera para o estágio

    public Vector2 xBounds; // Limites em X para o estágio
    public Vector2 yBounds; // Limites em Y para o estágio

    private Goal goal; // Referência ao Goal do estágio
    private Camera mainCamera; // Referência à câmera principal

    void Start()
    {
        goal = GetComponentInChildren<Goal>();
        if (goal != null)
        {
            goal.totalKeysRequired = totalKeysRequired;
        }
        else
        {
            Debug.LogError("Goal não encontrado como filho do Stage!");
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Câmera principal não encontrada!");
        }

        SaveInitialState();
    }

    private void SaveInitialState()
    {
        foreach (GameObject deposito in depositos)
        {
            var depositoScript = deposito.GetComponent<Deposito>();
            if (depositoScript != null)
            {
                depositoScript.SaveInitialState();
            }
        }

        foreach (GameObject key in keys)
        {
            var keyScript = key.GetComponent<Key>();
            if (keyScript != null)
            {
                keyScript.SaveInitialState();
            }
        }

        foreach (GameObject platform in platforms)
        {
            var platformScript = platform.GetComponent<ElevatorPlatform>();
            if (platformScript != null)
            {
                platformScript.SaveInitialState();
            }
        }

        foreach (GameObject throwable in throwables)
        {
            var throwableScript = throwable.GetComponent<ThrowableObject>();
            if (throwableScript != null)
            {
                throwableScript.SaveInitialState();
            }
        }
    }

    public void ResetStage()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                var enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.Reset();
                }
            }
        }

        foreach (GameObject key in keys)
        {
            var keyScript = key.GetComponent<Key>();
            if (keyScript != null)
            {
                keyScript.Reset();
            }
        }

        foreach (GameObject deposito in depositos)
        {
            var depositoScript = deposito.GetComponent<Deposito>();
            if (depositoScript != null)
            {
                depositoScript.Reset();
            }
        }

        foreach (GameObject platform in platforms)
        {
            var platformScript = platform.GetComponent<ElevatorPlatform>();
            if (platformScript != null)
            {
                platformScript.Reset();
            }
        }

        foreach (GameObject throwable in throwables)
        {
            var throwableScript = throwable.GetComponent<ThrowableObject>();
            if (throwableScript != null)
            {
                throwableScript.Reset();
            }
        }

        if (goal != null)
        {
            goal.ResetGoal();
        }
    }

    public bool IsWithinBounds(Vector3 position)
    {
        return position.x >= xBounds.x && position.x <= xBounds.y &&
               position.y >= yBounds.x && position.y <= yBounds.y;
    }

    [ContextMenu("Configurar Limites Padrão")]
    private void ConfigureDefaultBounds()
    {
        xBounds = new Vector2(-15, 30);
        yBounds = new Vector2(-10, 10);
    }

    [ContextMenu("Capturar Posição e Rotação da Câmera")]
    private void CaptureCameraPositionAndRotation()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraPosition = mainCamera.transform.position;
            cameraRotation = mainCamera.transform.rotation;
            Debug.Log("Posição e rotação da câmera capturadas com sucesso.");
        }
        else
        {
            Debug.LogError("Câmera principal não encontrada!");
        }
    }

    public void UpdateCameraPosition(Vector3 playerPosition)
{
    if (mainCamera == null)
    {
        return;
    }

    // Calcula a nova posição da câmera com base na posição do jogador
    float xRatio = Mathf.InverseLerp(xBounds.x, xBounds.y, playerPosition.x);
    float yRatio = Mathf.InverseLerp(yBounds.x, yBounds.y, playerPosition.y);

    // Define os limites internos da câmera (10 unidades para dentro da área de jogo)
    float innerXMin = xBounds.x;
    float innerXMax = xBounds.y;
    float innerYMin = yBounds.x;
    float innerYMax = yBounds.y;

    // Calcula a nova posição da câmera na direção X e Y proporcional à posição do jogador, dentro dos limites
    float newCameraX = Mathf.Lerp(innerXMin, innerXMax, xRatio);
    float newCameraY = Mathf.Lerp(innerYMin, innerYMax, yRatio);

    // Mantém a posição Z da câmera fixa para evitar deslocamento em perspectiva
    Vector3 newCameraPosition = new Vector3(newCameraX + cameraPosition.x, newCameraY + cameraPosition.y, mainCamera.transform.position.z);

    // Atualiza a posição da câmera suavemente
    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCameraPosition, Time.deltaTime * 2f);
}


}
