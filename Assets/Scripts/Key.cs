using UnityEngine;

public class Key : MonoBehaviour
{
    public Vector3 startPosition; // Posição inicial da chave
    public bool isCollected; // Indica se a chave foi coletada

    void Start()
    {
        startPosition = transform.position;
    }

    public void Reset()
    {
        // Reseta a chave ao estado inicial
        transform.position = startPosition;
        isCollected = false;
        gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<Goal>().CollectKey();
            isCollected = true;
            gameObject.SetActive(false);
        }
    }
}
