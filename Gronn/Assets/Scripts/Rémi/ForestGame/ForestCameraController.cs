using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestCameraController : MonoBehaviour
{

    public ForestGameManager manager;

    [Header("Translation and Rotation")]
    public float movementTime;
    public float normalSpeed;
    public float fastSpeed;
    public float rotationAmount;

    [Header("Zoom")]
    public Transform cameraTransform;
    public Vector3 zoomAmount;
    

    private Vector3 Position;
    private Quaternion rotation;
    private Vector3 zoom;

    // Start is called before the first frame update
    private void Start()
    {
        Position=new Vector3(manager.levelWidth/2,0,manager.levelLenght/2);
        rotation=transform.rotation;
        zoom=cameraTransform.localPosition; 
    }

    // Update is called once per frame
    private void Update()
    {
        MovementInput();
    }

    private void MovementInput()
    {

        //displacement
        float movementSpeed=normalSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed=fastSpeed;
        }

        movementSpeed*=zoom.magnitude/40;

        if(Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
        {
            Position += transform.forward*movementSpeed;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Position -= transform.forward*movementSpeed;
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Position += transform.right*movementSpeed;
        }
        if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            Position -= transform.right*movementSpeed;
        }

        //change the new position if it's out of the map
        BorderControl();

        transform.position = Vector3.Lerp(transform.position, Position, Time.deltaTime*movementTime);

        //rotation
        if(Input.GetKey(KeyCode.A))
        {
            rotation *= Quaternion.Euler(Vector3.up*rotationAmount);
        }

           if(Input.GetKey(KeyCode.E))
        {
            rotation *= Quaternion.Euler(Vector3.up*-rotationAmount);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime*movementTime);


        //zoom
        if(Input.GetKey(KeyCode.KeypadPlus))
        {
            zoom+= zoomAmount;
        }
        if(Input.GetKey(KeyCode.KeypadMinus))
        {
            zoom-= zoomAmount;
        }
        if(Input.mouseScrollDelta.y!=0)
        {
            zoom+= Input.mouseScrollDelta.y*200*zoomAmount;
        }

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoom, Time.deltaTime*movementTime);

    }

    private void BorderControl()
    {
        if(Position.x>manager.levelWidth)
        {
            Position.x=manager.levelWidth;
        }
        if(Position.x<0)
        {
            Position.x=0;
        }
        if(Position.z>manager.levelLenght)
        {
            Position.z=manager.levelLenght;
        }
        if(Position.z<0)
        {
            Position.z=0;
        }
    }

}
