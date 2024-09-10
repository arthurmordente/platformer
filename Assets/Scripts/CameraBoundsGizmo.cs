using UnityEngine;

[ExecuteAlways] // Permite que o script seja executado no modo de edição
public class CameraBoundsGizmo : MonoBehaviour
{
    public Stage currentStage; // Referência ao estágio atual
    public float drawDistance = 10f; // Distância da câmera para desenhar os limites

    private Camera mainCamera;

    void OnDrawGizmos()
    {
        // Obtém a câmera principal se não estiver definida
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Verifica se o estágio atual está definido
        if (currentStage == null)
        {
            Debug.LogWarning("Stage não definido para o CameraBoundsGizmo!");
            return;
        }

        // Certifica-se de que a câmera está em modo perspectiva
        if (!mainCamera.orthographic)
        {
            DrawPerspectiveCameraBounds();
        }
        else
        {
            Debug.LogWarning("A câmera está no modo ortográfico. Este script é para câmeras em modo perspectiva.");
        }

        // Desenha os limites do estágio
        DrawStageBounds();
    }

    private void DrawPerspectiveCameraBounds()
    {
        // Calcula o tamanho da visão da câmera no ponto da distância definida
        float halfHeight = Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * drawDistance;
        float halfWidth = halfHeight * mainCamera.aspect;

        // Define os quatro pontos dos limites da visão da câmera
        Vector3 topLeft = mainCamera.transform.position + mainCamera.transform.forward * drawDistance - mainCamera.transform.right * halfWidth + mainCamera.transform.up * halfHeight;
        Vector3 topRight = mainCamera.transform.position + mainCamera.transform.forward * drawDistance + mainCamera.transform.right * halfWidth + mainCamera.transform.up * halfHeight;
        Vector3 bottomLeft = mainCamera.transform.position + mainCamera.transform.forward * drawDistance - mainCamera.transform.right * halfWidth - mainCamera.transform.up * halfHeight;
        Vector3 bottomRight = mainCamera.transform.position + mainCamera.transform.forward * drawDistance + mainCamera.transform.right * halfWidth - mainCamera.transform.up * halfHeight;

        // Desenha as linhas que mostram os limites da câmera
        Gizmos.color = Color.green;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    private void DrawStageBounds()
    {
        // Calcula os pontos dos limites do estágio com base nos xBounds e yBounds
        Vector3 bottomLeft = new Vector3(currentStage.xBounds.x, currentStage.yBounds.x, transform.position.z);
        Vector3 bottomRight = new Vector3(currentStage.xBounds.y, currentStage.yBounds.x, transform.position.z);
        Vector3 topLeft = new Vector3(currentStage.xBounds.x, currentStage.yBounds.y, transform.position.z);
        Vector3 topRight = new Vector3(currentStage.xBounds.y, currentStage.yBounds.y, transform.position.z);

        // Desenha as linhas que mostram os limites do estágio
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
