using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] targets;
    public int targetCount;
    public Vector3 spawnValues;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < targetCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                GameObject hazard = targets[Random.Range(0, targets.Length)];
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(Random.Range(0.5f, spawnWait));
            }
            yield return new WaitForSeconds(waveWait);
        }
    }
}
