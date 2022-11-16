using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float verticalVelocity;     // y vale for our moveVector
    private float inputDirection;       // x value for our moveVector

    private float gravity = 17.0f;
    private float speed = 5.0f;
    private float jumpForce = 10.0f;
    private bool secondJumpAvail = false;


    private CharacterController controller;
    private Vector3 moveVector;
    private Vector3 lastMotion;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        IsControllerGrounded();
        moveVector = Vector3.zero;
        inputDirection = Input.GetAxis("Horizontal") * speed;

        if (IsControllerGrounded())
        {
            verticalVelocity = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                secondJumpAvail = true;
                Debug.Log("spacebar!");
            }

            moveVector.x = inputDirection;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (secondJumpAvail)
                {
                    verticalVelocity = jumpForce;
                    secondJumpAvail = false;
                }
                
            }

            verticalVelocity-= gravity * Time.deltaTime;
            moveVector.x = lastMotion.x;
        }

       
        moveVector.y = verticalVelocity;
        
        controller.Move(moveVector * Time.deltaTime);
        lastMotion = moveVector;
    }

    private bool IsControllerGrounded()
    {
        Vector3 leftRayStart;  //start point 
        Vector3 rightRayStart;

        leftRayStart = controller.bounds.center;        // set the rayCasting at both ends of the collider and center theme
        rightRayStart = controller.bounds.center;

        leftRayStart.x -= controller.bounds.extents.x;
        rightRayStart.x += controller.bounds.extents.x;

        Debug.DrawRay(leftRayStart, Vector3.down,Color.red);    //visual 
        Debug.DrawRay(rightRayStart, Vector3.down, Color.green);

       if (Physics.Raycast(leftRayStart, Vector3.down, (controller.height / 2) + 0.1f)) // RayCasting and the length
            return true;        // if the character touch the ground yes or no

       if (Physics.Raycast(rightRayStart, Vector3.down, (controller.height / 2) + 0.1f))
            return true;

        return false;
    }

    private void  OnControllerColliderHit(ControllerColliderHit hit) // when the character collide with the wall it will give a debug.
    {
       if (controller.collisionFlags == CollisionFlags.Sides) // are you touching the side of the capsules
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 2.0f);
                moveVector = hit.normal * speed;
                verticalVelocity = jumpForce;
                secondJumpAvail = true;
            }
        }

       //collectables
       switch(hit.gameObject.tag)
        {
            case "Coin":
                Destroy(hit.gameObject);
                break;
            case "JumpPad":
                verticalVelocity = jumpForce * 1.5f;
                break;
            case "Teleport":
               transform.position = hit.transform.GetChild(0).position;
                break;
            default:
                break;
        }
    }
}
