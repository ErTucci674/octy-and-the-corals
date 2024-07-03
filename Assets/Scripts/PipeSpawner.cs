using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipe;
    public float spawnTime = 2;
    public float verticalVariation = 2;
    private float timer;

    public float pipeSpeed = 3;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        // Spawn Pipes
        if (timer >= spawnTime)
        {
            // Generate new pipe on a random vertical position
            GameObject newPipe = Instantiate(pipe);
            newPipe.transform.position += new Vector3(0, Random.Range(-verticalVariation, verticalVariation), 0);
            newPipe.GetComponent<PipeMovement>().speed = pipeSpeed;
            timer -= spawnTime;
        }
    }
}
