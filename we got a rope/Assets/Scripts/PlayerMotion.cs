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
    [SerializeField]
    private RopeHeadBehavior myRopeHead;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myPlayerNumName = myPlayerNum.ToString();
        myRopeScript = GetComponent<RopeScript>();
    }

    private void Update()
    {
        if (myRopeHead.myHeadState == RopeHeadState.Retracted)
        {
            if (Input.GetButtonDown(myPlayerNumName + "Fire"))
            {
                FireRope();
            }
        }
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

    private void FireRope()
    {
        myRopeHead.myHeadState = RopeHeadState.Extending;
        //we want this one to fire a segmented rope in a line in the direction of the other player, what do we need.
       //take it in steps: first we calculate which direction the rope will fire in
       //then we send the end of it in that direction the end must be mobile and able to collide with things but ignore the player's movement to begin
       //then upon collision the rope needs to check how it interacts with things.

        //trouble: if the rope connects to something how do its segments know what sort of an offset they need to account their movement according to the attached player's
        //what sort of interactions does this rope actually need?
    }
}
