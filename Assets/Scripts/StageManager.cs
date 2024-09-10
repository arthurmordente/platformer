using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public Stage[] stages; // Array de estágios na cena
    [SerializeField]
    private int currentStageIndex = 0; // Índice do estágio atual
    private Stage currentStage;
    private Camera mainCamera; // Referência à câmera principal
    public PlayerController player;

    void Start()
    {
        mainCamera = Camera.main; // Obtém a referência à câmera principal

        // Desativa todos os objetos do tipo Stage na cena
        DeactivateAllStages();

        // Inicia com o primeiro estágio
        SetCurrentStage(stages[currentStageIndex]);
    }

    private void DeactivateAllStages()
    {
        // Obtém todos os objetos do tipo Stage na cena
        Stage[] allStages = FindObjectsOfType<Stage>();

        // Desativa todos os objetos Stage encontrados
        foreach (Stage stage in allStages)
        {
            stage.gameObject.SetActive(false);
        }
    }

    public void SetCurrentStage(Stage stage)
    {
        // Desativa o estágio anterior, se houver
        if (currentStage != null)
        {
            currentStage.gameObject.SetActive(false);
            ResetGoals();
        }

        // Ativa o novo estágio
        currentStage = stage;
        currentStage.gameObject.SetActive(true);

        // Move o jogador para o novo Spawn Point
        player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.Spawn();
        }
        else
        {
            Debug.LogError("Player não encontrado!");
        }

        // Posiciona a câmera de acordo com o estágio atual
        if (mainCamera != null)
        {
            RespawnCamera();
        }
        else
        {
            Debug.LogError("Câmera principal não encontrada!");
        }
    }

    public Stage GetCurrentStage()
    {
        return currentStage;
    }

    public void GoToNextStage()
    {
        if (currentStageIndex < stages.Length - 1)
        {
            currentStageIndex++;
            SetCurrentStage(stages[currentStageIndex]);
            RespawnPlayer();
        }
        else
        {
            SceneManager.LoadScene("Win");
        }
    }

    public void ResetCurrentStage()
    {
        currentStage.ResetStage();
    }

    public void RespawnPlayer()
    {
        if (currentStage != null)
        {
            // Reseta o jogador no SpawnPoint do estágio ativo
            player.transform.position = currentStage.spawnPoint.position;
            player.transform.rotation = currentStage.spawnPoint.rotation;
            player.currentMass = currentStage.initialMass;

            // Reinicia o estado do estágio atual
        }
    }

    public void RespawnCamera()
    {
        if (currentStage != null)
        {
            mainCamera.transform.position = currentStage.cameraPosition;
            mainCamera.transform.rotation = currentStage.cameraRotation;
        }
    }

    private void ResetGoals()
    {
        // Reseta todos os objetos Goal no estágio atual
        var goals = FindObjectsOfType<Goal>();
        foreach (var goal in goals)
        {
            if (goal.stage == currentStage)
            {
                goal.ResetGoal();
            }
        }
    }
}
