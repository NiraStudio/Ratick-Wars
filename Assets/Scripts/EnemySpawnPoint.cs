using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour {
    public GameObject[] Enemies;
    public float firstAmount, nAmount;
    public int MaxEnemies;
    Vector2 tt;
    GameObject g;
    GamePlayManager GPM;
    float a;
	void Start()
    {
        GPM = GamePlayManager.Instance;
        MaxEnemies = GPM.maxEnemies;
        
        StartCoroutine(Spawn(firstAmount));
    }

    IEnumerator Spawn(float amount)
    {
        if (GPM.gameState != GamePlayState.Finished)
        {
            if (GameObject.FindObjectsOfType<Enemy>().Length < MaxEnemies)
            {
                for (int i = 0; i < amount; i++)
                {
                    tt = (Vector2)transform.position + Random.insideUnitCircle * 0.3f;
                    g = Instantiate(Enemies[Random.Range(0, Enemies.Length)], tt, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(3);
            if (nAmount < 3)
                nAmount += 0.3f;
            else
                nAmount = 3;
            StartCoroutine(Spawn(nAmount));
        }
    }
}
