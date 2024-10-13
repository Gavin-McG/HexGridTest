using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

enum MouseState
{
    None,
    Waiting,
    Dragging
}


public class CameraManager : MonoBehaviour
{

    public static UnityEvent mouseClick = new UnityEvent();

    GameObject camObject;
    Camera cam;

    MouseState state;
    Vector3 clickPos = Vector3.zero;

    private void Awake()
    {
        camObject = Camera.main.gameObject;
        cam = Camera.main;
    }

    private void Update()
    {
        HandleClickInput();
    }


    void HandleClickInput()
    {
        switch (state)
        {
            case MouseState.None:
                NoneState();
                break;
            case MouseState.Waiting:
                WaitingState();
                break;
            case MouseState.Dragging:
                DraggingState();
                break;
        }
    }


    void NoneState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPos = Input.mousePosition;
            state = MouseState.Waiting;
        }
    }

    void WaitingState()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseClick.Invoke();
            state = MouseState.None;
        }
        else if (Input.mousePosition != clickPos)
        {
            state = MouseState.Dragging;
        } 
    }

    void DraggingState()
    {
        if (Input.GetMouseButtonUp(0))
        {
            state = MouseState.None;
        }

        //caluclate movement
        Vector3 change = clickPos - Input.mousePosition;
        change.x /= Screen.width;
        change.y /= Screen.height;
        change.x *= 2*cam.orthographicSize * cam.aspect;
        change.y *= 2*cam.orthographicSize;

        //apply movement
        camObject.transform.Translate(change);

        clickPos = Input.mousePosition;
    }
}
