using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    public enum PlayerNum { Player1, Player2 };
    public PlayerNum myPlayerNum;
    [SerializeField]
    private float movementSpeed;
    public string myPlayerNumName;
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private RopeHeadBehavior myRopeHead;
    private SpriteRenderer mySpriteRend;

    private Animator myAnimator;

    private bool facingRight = true;
    private float distance = -1;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myPlayerNumName = myPlayerNum.ToString();
        myAnimator = GetComponent<Animator>();
        mySpriteRend = GetComponent<SpriteRenderer>();

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

        if (myRopeHead.myHeadState == RopeHeadState.Attached)
        {
            if (Input.GetButton(myPlayerNumName + "Hold"))
                HoldRope();
        }

        //Player Animation Stuff
        myAnimator.SetFloat("Speed", myRigidbody.velocity.magnitude);

        if (Input.GetAxisRaw(myPlayerNumName + "Horizontal") != 0 && myRigidbody.velocity.magnitude > 0)
        {
            myAnimator.SetBool("Side", true);
            myAnimator.SetBool("Up", false);
            myAnimator.SetBool("Down", false);
            mySpriteRend.sortingOrder = 0;
        }
        else if (Input.GetAxisRaw(myPlayerNumName + "Vertical") > 0 && myRigidbody.velocity.magnitude > 0 &&
            Input.GetAxis(myPlayerNumName + "Horizontal") != 1 && Input.GetAxis(myPlayerNumName + "Horizontal") != -1)
        {
            myAnimator.SetBool("Up", true);
            myAnimator.SetBool("Down", false);
            myAnimator.SetBool("Side", false);
            mySpriteRend.sortingOrder = 2;
        }
        else if (Input.GetAxisRaw(myPlayerNumName + "Vertical") < 0 && myRigidbody.velocity.magnitude > 0 &&
            Input.GetAxis(myPlayerNumName + "Horizontal") != 1 && Input.GetAxis(myPlayerNumName + "Horizontal") != -1)
        {
            myAnimator.SetBool("Down", true);
            myAnimator.SetBool("Up", false);
            myAnimator.SetBool("Side", false);
            mySpriteRend.sortingOrder = 0;
        }
    }

    private void FixedUpdate()
    {
        Move();

        float h = Input.GetAxis(myPlayerNumName + "Horizontal");
        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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

    private void HoldRope()
    {
        Transform attachedObject = myRopeHead.attachedObject;

        if (attachedObject == null)
            Debug.LogError("Nothing attached, why is this showing.");

        if (distance == -1)
            distance = Vector3.Distance(transform.position, attachedObject.position);


    }
}
