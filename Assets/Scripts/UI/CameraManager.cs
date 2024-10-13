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

    [SerializeField] Vector2 camSizeBounds = new Vector2(1, 20);




    public static UnityEvent mouseClick = new UnityEvent();

    Camera cam;
    GameObject camObject;
    float camSize;

    MouseState state;
    Vector3 clickPos = Vector3.zero;

    private void Awake()
    {
        cam = Camera.main;
        camObject = cam.gameObject;
        camSize = cam.orthographicSize;
    }

    private void Update()
    {
        HandleClickInput();
        HandleScrollInput();
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
        change.x *= 2*camSize * cam.aspect;
        change.y *= 2*camSize;

        //apply movement
        camObject.transform.Translate(change);

        clickPos = Input.mousePosition;
    }




    void HandleScrollInput()
    {
        camSize -= Input.mouseScrollDelta.y;
        camSize = Mathf.Clamp(camSize, camSizeBounds.x, camSizeBounds.y);
        cam.orthographicSize = camSize;
    }
}
