using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    // Rotation speed for each axis
    [SerializeField] private float rotationSpeedX = 3f, rotationSpeedY = 3f, rotationSpeedZ = 3f;
    [SerializeField] private float moveSpeedX = -0.2f;
    private float minX = -14f, maxX = 44f, minY = -10f, maxY = 10f, minZ = -4f, maxZ = 4f;

    // Update is called once per frame
    void Update()
    {
        Move();
        ResetPosition();
    }

    void ResetPosition()
    {
        if (transform.position.x <= minX || transform.position.x >= maxX ||
            transform.position.y <= minY || transform.position.y >= maxY ||
            transform.position.z <= minZ || transform.position.z >= maxZ)
        {
            float randomY = Random.Range(minY, maxY);
            float randomZ = Random.Range(minZ, maxZ);

            transform.position = new Vector3(43f, randomY, randomZ);
        }
    }

    void Move()
    {
        // Rotate the object around its own axes
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);

        // Moves only on X-axis
        transform.Translate(moveSpeedX * Time.deltaTime, 0f, 0f);
    }
}
