using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePlayInput : MonoBehaviour
{


    #region Singleton
    public static GamePlayInput Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion


    public JoyStick JS;
    public Vector2 direction;

    [HideInInspector]
    public bool move;

    GamePlayManager GPM;
    Touch[] touches;
    void Start()
    {
        JS.gameObject.SetActive(false);
        GPM = GamePlayManager.Instance;
    }

    void Update()
    {

        if (GPM.gameState == GamePlayState.Finished)
            return;

        #region Inputs
        if (Application.isMobilePlatform)
        {

            touches = Input.touches;
            foreach (var item in touches)
            {
                print(item.tapCount);
            }
            if (touches.Length == 1)
                switch (touches[0].phase)
                {
                    case TouchPhase.Began:
                        if (touches[0].tapCount == 1&& !EventSystem.current.IsPointerOverGameObject(touches[0].fingerId))
                            JoyStickTurnOn(Camera.main.ScreenToWorldPoint(touches[0].position));
                        break;
                    case TouchPhase.Ended:
                        if (touches[0].tapCount >= 2)
                            GatherCharacters();
                        JoyStickTurnOff();
                        break;
                }
        }
        else if (Application.isEditor)
        {

            touches = Input.touches;

            if (touches.Length == 1)
            {
                switch (touches[0].phase)
                {
                    case TouchPhase.Began:
                        if (touches[0].tapCount == 1 && !EventSystem.current.IsPointerOverGameObject(touches[0].fingerId))
                            JoyStickTurnOn(Camera.main.ScreenToWorldPoint(touches[0].position));
                        break;
                    case TouchPhase.Ended:
                        if (touches[0].tapCount >= 2)
                            GatherCharacters();
                        JoyStickTurnOff();
                        break;
                }
            }

            if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
                JoyStickTurnOn(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            else if (Input.GetMouseButtonUp(0))
                JoyStickTurnOff();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GatherCharacters();
        }
        #endregion
        direction = JS.direction;

    }


    void JoyStickTurnOn(Vector2 a)
    {

        JS.gameObject.transform.position = a;
        JS.gameObject.SetActive(true);
        move = true;
    }
    void JoyStickTurnOff()
    {
        JS.gameObject.SetActive(false);
        move = false;
    }
    void GatherCharacters()
    {
        Character[] tt = GameObject.FindObjectsOfType<Character>();
        foreach (var item in tt)
        {
            item.StartGatthering();
        }
    }


}
