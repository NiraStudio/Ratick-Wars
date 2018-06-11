using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( GamePlayCharacterController))]
[RequireComponent(typeof( GamePlayUIManager))]
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
    public int maxEnemies;




    public int Ratick
    {
        get { return _ratick >> 3; }
        set
        {
            int a = _ratick >> 3;
            a += value;
            _ratick = a << 3;
            a = 0;
        }
    }

    [SerializeField]
    int _ratick;


    //===========Test Vars==========
    //==============================


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (gameState == GamePlayState.Finished)
            return;


        GameTime -= Time.deltaTime;
        if (GameTime <= 0)
        {
            GameTime = 0;
            EndOfGame();
        }
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
