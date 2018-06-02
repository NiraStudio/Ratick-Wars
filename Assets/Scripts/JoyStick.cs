﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MainBehavior
{
    public GameObject handle;
    public float distance;
    public Vector2 direction;
    Vector2 t;
    Vector2 mousePos;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        SpriteMethod();
    }
    void SpriteMethod() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(transform.position, mousePos) <= distance)
            handle.transform.position = mousePos;
        else
        {
            t = mousePos - (Vector2)transform.position;
            t = t.normalized;
            handle.transform.position = (Vector2)transform.position + t * distance;
        }
       
        direction = (handle.transform.position - transform.position);
        direction.Normalize();

    
    }


    
    void UiMethod()
    {
        mousePos = Input.mousePosition;
        if (Vector2.Distance(transform.position, mousePos) <= distance)
            handle.transform.position = mousePos;
        else
        {
            t = mousePos - (Vector2)transform.position;
            t = t.normalized;
            handle.transform.position = (Vector2)transform.position + t * distance;
        }
        direction = (handle.transform.position - transform.position).normalized;
    
    }
}
