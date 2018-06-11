using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayCharacterController : MonoBehaviour {


    #region Singleton
    public static GamePlayCharacterController Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion

    #region UI


    [SerializeField]
    Image meleeMinionPersentImg, rangeMinionPersentImg;
    [SerializeField]
    Slider relationSlider,SpawnSlider;

    #endregion




    [SerializeField]
    CharacterData meleeMinion, rangeMinion;
    [SerializeField]
    int characterSpawnAmount, enemyNeeded, increasePerTimeNeed;

    CameraController CMC;
    List<GameObject> characters = new List<GameObject>();
    public int CharacterCount
    {
        get { return characters.Count; }
    }

    int enemyKilled,characterSpawnTimes;
    GamePlayManager GPM;
    GameObject p;
    // Use this for initialization
    void Start () {
        GPM = GetComponent<GamePlayManager>();
        CMC = CameraController.Instance;
        p  = new GameObject();
        p.name = "_Characters_";
        relationSlider.maxValue = characterSpawnAmount;
        relationSlider.value = characterSpawnAmount / 2; ;
        SpawnSlider.value = 0;
        SpawnSlider.maxValue = enemyNeeded;
        Spawn();
	}


    void Update()
    {
        relationSlider.value = (int)relationSlider.value;
        CheckForCharacters();
    }

    // Update is called once per frame

    public void AddEnemyKill(int amount)
    {
        enemyKilled += amount;
        SpawnSlider.value = enemyKilled;

        if (enemyKilled >= enemyNeeded)
        {
            enemyNeeded += increasePerTimeNeed;
            enemyKilled = 0;
            SpawnSlider.value = 0;
            SpawnSlider.maxValue = enemyNeeded;
            Spawn();
        }
    }

    void Spawn()
    {

        Vector2 tt = CMC.transform.position;
        GameObject g;

        int a = (int)relationSlider.value;
        for (int i = 0; i < a; i++)
        {
            tt +=  Random.insideUnitCircle * 0.3f;
            g = Instantiate(meleeMinion.prefab, tt, Quaternion.identity);
            g.transform.SetParent(p.transform);
            AddCharacter(g);
        }

        a = characterSpawnAmount - a;
        for (int i = 0; i < a; i++)
        {
            tt += Random.insideUnitCircle * 0.3f;
            g = Instantiate(rangeMinion.prefab, tt, Quaternion.identity);
            g.transform.SetParent(p.transform);
            AddCharacter(g);
        }
    }

    void AddCharacter(GameObject g)
    {
        characters.Add(g);
    }

    void CheckForCharacters()
    {
        foreach (var item in characters.ToArray())
        {
            if (item == null)
                characters.Remove(item);
        }
        CMC.ChangeTargets(characters);
        if (CharacterCount == 0)
            GPM.gameState = GamePlayState.Finished;
    }

    public void ImageCheck()
    {
        float a = (0.8f / characterSpawnAmount) * relationSlider.value;

        a += 0.3f;
        meleeMinionPersentImg.transform.localScale = Vector3.one * a;

        a =1 - a;
        a += 0.3f;
        rangeMinionPersentImg.transform.localScale = Vector3.one * a;

    }
}
