using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public float speed;
    private const float despawnZone = -11;

    // Update is called once per frame
    void Update()
    {
        // Move Pipe
        transform.position += Vector3.left * speed * Time.deltaTime;
        if (transform.position.x <= despawnZone)
        {
            Destroy(gameObject);
        }
    }
}
