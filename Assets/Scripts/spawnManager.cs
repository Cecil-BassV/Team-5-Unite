using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    [SerializeField] GameObject basicZombie;
    [SerializeField] GameObject tankZombie;
    [SerializeField] int tankRound;
    [SerializeField] float tankZombieChance;
    [SerializeField] int startSpawnCount;
    [SerializeField] int maxSpawnCount;
    [SerializeField] float spawnInterval;
    [SerializeField] int waveIncrease;
    [SerializeField] float roundDelay;

    //int tankRound = 5;
    int spitterRound = 10;
    //float tankZombieChance = 0.25f;

    bool gameStarted = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnWave());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator spawnWave()
    {
        while (gameStarted)
        {
            GameManager.instance.updateRoundCount();

            for (int i = 0; i < maxSpawnCount; i++)
            {
                int randomX = Random.Range(-5, 10);
                int randomZ = Random.Range(-5, 10);
                Vector3 randomPos = new Vector3(randomX, 0, randomZ);

                GameObject zombieToSpawn;

                if (GameManager.instance.roundNumber % tankRound == 0)
                    zombieToSpawn = Random.value < tankZombieChance ? tankZombie : basicZombie;
                else
                    zombieToSpawn = basicZombie;

                GameManager.instance.spawnObject(basicZombie, (transform.position + randomPos)); // Do this so they dont stack up on each other

                yield return new WaitForSeconds(spawnInterval);
            }

            while (GameManager.instance.goalCount > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(roundDelay);
        }
    }

}