using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamePlayUIManager : MonoBehaviour {

    #region Singleton
    public static GamePlayUIManager Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion
    public LocalizedDynamicText ratickAmountText;
    public Slider timer;

    GamePlayManager GPM;
    // Use this for initialization
    void Start () {
        GPM = GetComponent<GamePlayManager>();
        timer.maxValue = GPM.GameTime;
        timer.value = GPM.GameTime;

    }

    // Update is called once per frame
    void Update () {
        ratickAmountText.text = "X " + GPM.Ratick;
        timer.value = GPM.GameTime;

    }
}
