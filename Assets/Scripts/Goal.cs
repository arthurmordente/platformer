using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour
{
    public int totalKeysRequired; // Número total de chaves necessárias para o estágio atual
    private int keysCollected = 0;

    public Material offMaterial; // Material usado quando o objeto está desligado
    public Material onMaterial;  // Material usado quando o objeto está ligado

    public TMP_Text popUpText; // Componente de texto para mostrar as chaves coletadas
    public Stage stage; // Referência ao Stage
    private StageManager stageManager; // Referência ao StageManager

    private Renderer objectRenderer;
    private bool isActivated = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        stage = GetComponentInParent<Stage>();
        if (stage != null)
        {
            totalKeysRequired = stage.totalKeysRequired;
        }
        else
        {
            Debug.LogError("O Stage não foi encontrado no pai do Goal!");
        }

        stageManager = FindObjectOfType<StageManager>();

        if (popUpText == null)
        {
            Debug.LogError("PopUpText não encontrado no Goal!");
        }

        UpdateState();
    }

    public void CollectKey()
    {
        keysCollected++;
        UpdateState();
    }

    private void UpdateState()
    {
        if (keysCollected >= totalKeysRequired)
        {
            isActivated = true;
            objectRenderer.material = onMaterial;
        }
        else
        {
            isActivated = false;
            objectRenderer.material = offMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isActivated)
            {
                GoToNextStage();
            }
            else
            {
                UpdatePopUpText();
                StaticPopUpHandler.ShowPopUp(popUpText, $"{keysCollected}/{totalKeysRequired} chaves coletadas", this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StaticPopUpHandler.HidePopUpAfterDelay(popUpText, 3f, this);
        }
    }

    private void UpdatePopUpText()
    {
        if (popUpText != null)
        {
            StaticPopUpHandler.ShowPopUp(popUpText, $"{keysCollected}/{totalKeysRequired} chaves coletadas", this);
        }
    }

    public void GoToNextStage()
    {
        if (stageManager != null)
        {
            stageManager.GoToNextStage();
        }
        else
        {
            Debug.LogError("StageManager não encontrado!");
        }
    }

    public void ResetGoal()
    {
        keysCollected = 0;
        UpdateState();
        StaticPopUpHandler.ShowPopUp(popUpText, $"{keysCollected}/{totalKeysRequired} chaves coletadas", this);
        StaticPopUpHandler.HidePopUp(popUpText, this); // Esconde o pop-up imediatamente ao resetar
    }
}
