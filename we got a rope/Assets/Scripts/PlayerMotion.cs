using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    public enum PlayerNum { Player1, Player2 };
    public PlayerNum myPlayerNum;
    [SerializeField]
    private float movementSpeed;
    private string myPlayerNumName;
    private Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myPlayerNumName = myPlayerNum.ToString();
    }


    private void FixedUpdate()
    {
        Move(); 
    }
    
    

    private void Move()
    {
         float horizontalInput;
         float verticalInput;
         horizontalInput = Input.GetAxisRaw(myPlayerNumName + "Horizontal");
         verticalInput = Input.GetAxisRaw(myPlayerNumName + "Vertical");
        Vector2 movementVector = new Vector2(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime);
        myRigidbody.velocity = movementVector;
        /*
        horizontalInput = Input.GetAxisRaw(inputType + "_Horizontal");
        Vector2 movementVector = new Vector2(horizontalInput * movementSpeed * Time.deltaTime, myRigidBody2d.velocity.y);
        myRigidBody2d.velocity = movementVector;
        */
    }
}
