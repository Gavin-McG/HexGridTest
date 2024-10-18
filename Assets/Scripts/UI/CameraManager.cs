using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

enum MouseState
{
    None,
    Waiting,
    Dragging
}


public class CameraManager : MonoBehaviour
{

    [SerializeField] Vector2 camBoundsX = new Vector2(-20, 20);
    [SerializeField] Vector2 camBoundsY = new Vector2(-20, 20);

    [SerializeField] Vector2 camSizeBounds = new Vector2(1, 20);
    [SerializeField] float dragThreshold = 5;

    [SerializeField] float smoothFactor = 0.95f;
    [SerializeField] private float zoomScale = 0.01f;

    public static UnityEvent mouseClick = new UnityEvent();

    Camera cam;
    GameObject camObject;

    float camSize;
    float goalCamSize;

    MouseState state;
    Vector2 clickPos = Vector2.zero;
    private Vector2 mousePos;

    private PlayerInput playerInput;
    
    private InputAction clickAction;
    private InputAction zoomAction;

    private void Awake()
    {
        cam = Camera.main;
        camObject = cam.gameObject;
        camSize = cam.orthographicSize;
        goalCamSize = camSize;
    }

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();

        clickAction = playerInput.actions["Select"];
        zoomAction = playerInput.actions["Zoom"];

        //Define functions for when each action is taken
        clickAction.started += _ => OnClickPerformed();
        clickAction.canceled += _ => OnClickReleased();
        zoomAction.performed += ctx => HandleScrollInput(ctx.ReadValue<Vector2>().y * zoomScale);
    }

    private void OnDisable()
    {
        //remove functions for when each action is taken
        clickAction.started -= _ => OnClickPerformed();
        clickAction.canceled -= _ => OnClickReleased();
        zoomAction.performed -= ctx => HandleScrollInput(ctx.ReadValue<Vector2>().y * zoomScale);
    }

    private void Update()
    {
        if (Application.isFocused)
        {
            //Changed from Input.MousePosition and calculate every frame since it's used a lot
            mousePos = Mouse.current.position.ReadValue();
            
            //If we are already dragging, or the criteria to start dragging is met then drag 
            if (state == MouseState.Waiting && (mousePos - clickPos).magnitude > dragThreshold)
            {
                state = MouseState.Dragging;
            }
            if (state == MouseState.Dragging)
            {
                DraggingState();
            }
            SmoothZoom();
            BoundCamera();

        }
    }

    //Sets waiting when user clicks
    private void OnClickPerformed()
    {
        clickPos = mousePos;
        state = MouseState.Waiting;
    }

    //Called only when user releases the mouse click
    private void OnClickReleased()
    {
        if (state==MouseState.Waiting)
        {
            mouseClick.Invoke();
        }
        state = MouseState.None;
    }

    void DraggingState()
    {
        //caluclate movement
        Vector3 change = clickPos - mousePos;
        change.x /= Screen.width;
        change.y /= Screen.height;
        change.x *= 2*camSize * cam.aspect;
        change.y *= 2*camSize;

        //apply movement
        camObject.transform.Translate(change);

        clickPos = mousePos;
    }



    //use the scroll input of mouse to zoom in/out
    //Callback function for the zoom input
    void HandleScrollInput(float scrollDelta)
    {
        //calculate new size
        float newCamSize = goalCamSize - scrollDelta;
        goalCamSize = Mathf.Clamp(newCamSize, camSizeBounds.x, camSizeBounds.y);
    }


    void SmoothZoom()
    {
        //calculate new size
        float difference = goalCamSize - camSize;
        float newCamSize = camSize + difference * (1-Mathf.Pow(smoothFactor, Time.deltaTime));

        //move camera to zoom towards pointer
        float factor = newCamSize / camSize;
        Vector3 zoomPoint = cam.ScreenToWorldPoint(mousePos);
        Vector3 offset = zoomPoint - camObject.transform.position;
        camObject.transform.position = zoomPoint - factor * offset;

        camSize = newCamSize;
        cam.orthographicSize = camSize;
    }


    //restrict camera to rectangular bounds
    void BoundCamera()
    {
        float x = Mathf.Clamp(camObject.transform.position.x, camBoundsX.x, camBoundsX.y);
        float y = Mathf.Clamp(camObject.transform.position.y, camBoundsY.x, camBoundsY.y);
        float z = camObject.transform.position.z;
        camObject.transform.position = new Vector3(x, y, z);
    }
}
