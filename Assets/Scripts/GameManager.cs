using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Singleton
    public static GameManager Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FirstTimeChanges()
    {

    }
}
