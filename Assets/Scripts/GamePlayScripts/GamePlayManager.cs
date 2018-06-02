using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour {
    #region Singleton
    public static GamePlayManager Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion

    public GamePlayState gameState;
    public float GameTime;

    [SerializeField]
    CameraController CMC;

    List<GameObject> characters = new List<GameObject>();
    public int CharacterCount
    {
        get { return characters.Count; }
    }

    //===========Test Vars==========
    public Transform startPos;
    public GameObject testCharacter;
    //==============================


    // Use this for initialization
    void Start () {
        SpawnCharacters();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameState == GamePlayState.Finished)
            return;

        CheckForCharacters();

        GameTime -= Time.deltaTime;
        if (GameTime <= 0)
        {
            GameTime = 0;
            EndOfGame();
        }
	}

    void SpawnCharacters()
    {
        GameObject p = new GameObject();
        p.name = "_Characters_";
        Vector2 tt = new Vector2();
        GameObject g;
        for (int i = 0; i < 50; i++)
        {
            tt = (Vector2)startPos.position + Random.insideUnitCircle * 1;
            g = Instantiate(testCharacter, tt, Quaternion.identity);
            g.transform.SetParent(p.transform);
            AddCharacter(g);
        }
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
            gameState = GamePlayState.Finished;
    }

    public void AddCharacter(GameObject ch)
    {
        characters.Add(ch);
    }

    public void EndOfGame()
    {
        gameState = GamePlayState.Finished;
    }
}
public enum GamePlayState
{
    Playing,Finished
}
