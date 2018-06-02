using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour {
    public GameObject[] Enemies;
    Vector2 tt;
    GameObject g;
    GamePlayManager GPM;
	IEnumerator Start()
    {
        GPM = GamePlayManager.Instance;
        yield return new WaitForSeconds(Random.Range(1, 2));
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if (GPM.gameState != GamePlayState.Finished)
        {
            for (int i = 0; i < 5; i++)
            {
                tt = (Vector2)transform.position + Random.insideUnitCircle * 1;
                g = Instantiate(Enemies[Random.Range(0, Enemies.Length)], tt, Quaternion.identity);
            }
            yield return new WaitForSeconds(3);
            StartCoroutine(Spawn());
        }
    }
}
