using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform spawnPoint; // Ponto de spawn do jogador
    public float initialMass = 10f;
    public int totalKeysRequired; // Número total de chaves necessárias para o estágio
    public GameObject[] keys; // Chaves no estágio
    public GameObject[] enemies; // Inimigos no estágio
    public GameObject[] depositos; // Depósitos no estágio

    public Vector3 cameraPosition; // Posição desejada da câmera para o estágio
    public Quaternion cameraRotation; // Rotação desejada da câmera para o estágio

    private Goal goal; // Referência ao Goal do estágio

    void Start()
    {
        // Encontra o Goal filho e armazena a referência
        goal = GetComponentInChildren<Goal>();
        if (goal != null)
        {
            // Define o total de chaves necessárias no Goal
            goal.totalKeysRequired = totalKeysRequired;
        }
        else
        {
            Debug.LogError("Goal não encontrado como filho do Stage!");
        }

        // Armazena os estados iniciais dos depósitos e estoques
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
        // Reseta o Goal, se existir
        if (goal != null)
        {
            goal.ResetGoal();
        }
    }
}
