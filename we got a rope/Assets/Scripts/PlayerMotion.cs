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
    private RopeHeadBehavior myRopeHeadScript;
    private SpriteRenderer mySpriteRend;
    [SerializeField]
    private Transform myRopeHead;
    private Transform attachedObject;

    private Animator myAnimator;

    [SerializeField]
    private Transform otherPlayer;

    private bool facingRight = true;
    private float heldDistance = -1;
    public static bool hasWon = false;

    [SerializeField]
    private Transform player1WinPos;
    [SerializeField]
    private Transform player2WinPos;
    [SerializeField]
    private Transform victoryHeartsPos;
    [SerializeField]
    private GameObject victoryHearts;

    private Vector2 myWinPos;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myPlayerNumName = myPlayerNum.ToString();
        myAnimator = GetComponent<Animator>();
        mySpriteRend = GetComponent<SpriteRenderer>();
        myRopeHeadScript = myRopeHead.GetComponent<RopeHeadBehavior>();
 
    }

    private void Update()
    {

        if (!hasWon)
        {
            if (myRopeHeadScript.myHeadState == RopeHeadState.Retracted)
            {
                if (Input.GetButtonDown(myPlayerNumName + "Fire"))
                {
                    FireRope();
                }
            }

            if (myRopeHeadScript.myHeadState == RopeHeadState.Attached)
            {
                if (Input.GetButton(myPlayerNumName + "Hold"))
                    HoldRope();
                else
                {
                    if (attachedObject != null)
                    {
                        attachedObject.GetComponent<Rigidbody2D>().velocity = new Vector2();
                        attachedObject.GetComponent<MoveableBlockScript>().ropeHeld = false;
                    }
                    attachedObject = null;
                    heldDistance = -1;
                }

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
        else
        {
            if (myWinPos != (Vector2)transform.position)
            {
                MovePlayerTowardsWinPosition();
                myAnimator.SetFloat("Speed", 0);
                myAnimator.SetBool("Side", true);
                myAnimator.SetBool("Up", false);
                myAnimator.SetBool("Down", false);
                mySpriteRend.sortingOrder = 0;
                if (myPlayerNumName == "Player2")
                {
                    if (facingRight)
                        Flip();
                }
            }
            else if (myWinPos == (Vector2)transform.position)
            {
                myRigidbody.velocity = new Vector2();
                if (!GameObject.Find("Hearts"))
                {
                    GameObject hearts = Instantiate(victoryHearts, victoryHeartsPos);
                    hearts.name = "Hearts";
                }
            }
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
      

        myRopeHeadScript.myHeadState = RopeHeadState.Extending;
        //we want this one to fire a segmented rope in a line in the direction of the other player, what do we need.
       //take it in steps: first we calculate which direction the rope will fire in
       //then we send the end of it in that direction the end must be mobile and able to collide with things but ignore the player's movement to begin
       //then upon collision the rope needs to check how it interacts with things.

        //trouble: if the rope connects to something how do its segments know what sort of an offset they need to account their movement according to the attached player's
        //what sort of interactions does this rope actually need?
    }

    private void HoldRope()
    {
        attachedObject = myRopeHeadScript.attachedObject;
        attachedObject.GetComponent<MoveableBlockScript>().ropeHeld = true;

        if (attachedObject.GetComponent<MoveableBlockScript>().myBlockMovementType == BlockMovementType.Vertical)
            VerticalBlockMove();
        if (attachedObject.GetComponent<MoveableBlockScript>().myBlockMovementType == BlockMovementType.Horizontal)
            HorizontalBlockMove();
        if (attachedObject.GetComponent<MoveableBlockScript>().myBlockMovementType == BlockMovementType.OmniDirectional)
            OmniDirectionalMove();
    }

    private void VerticalBlockMove()
    {
        Rigidbody2D attachedRB = attachedObject.GetComponent<Rigidbody2D>();
        if (attachedObject == null)
            Debug.LogError("Nothing attached, why is this showing.");
        if (heldDistance == -1)
            heldDistance = Vector3.Distance(transform.position, attachedObject.position);
        float currentDistance = Vector3.Distance(transform.position, attachedObject.position);
        if (currentDistance > heldDistance)
        {
            if (transform.position.y > myRopeHead.position.y + 1)
            {
                Debug.Log("You are above");
                if (Input.GetAxisRaw(myPlayerNumName + "Vertical") >= 0)
                    attachedRB.velocity = myRigidbody.velocity;
            }
            else if (transform.position.y < myRopeHead.position.y - 1)
            {
                Debug.Log("You are below");
                if (Input.GetAxisRaw(myPlayerNumName + "Vertical") <= 0)
                    attachedRB.velocity = myRigidbody.velocity;
            }
            else
                attachedRB.velocity = new Vector2();
        }
        else if (currentDistance < heldDistance)
        {
            attachedRB.velocity = new Vector2();
            heldDistance = currentDistance;
        }
    }
    private void HorizontalBlockMove()
    {
        Rigidbody2D attachedRB = attachedObject.GetComponent<Rigidbody2D>();

        if (attachedObject == null)
            Debug.LogError("Nothing attached, why is this showing.");

        if (heldDistance == -1)
            heldDistance = Vector3.Distance(transform.position, attachedObject.position);

        float currentDistance = Vector3.Distance(transform.position, attachedObject.position);


        if (currentDistance > heldDistance)
        {

            if (transform.position.x > myRopeHead.position.x + 1)
            {
                Debug.Log("You are above");

                if (Input.GetAxisRaw(myPlayerNumName + "Horizontal") >= 0)
                    attachedRB.velocity = myRigidbody.velocity;
            }
            else if (transform.position.x < myRopeHead.position.x - 1)
            {
                Debug.Log("You are below");

                if (Input.GetAxisRaw(myPlayerNumName + "Horizontal") <= 0)
                    attachedRB.velocity = myRigidbody.velocity;
            }
            else
                attachedRB.velocity = new Vector2();
        }
        else if (currentDistance < heldDistance)
        {
            attachedRB.velocity = new Vector2();
            heldDistance = currentDistance;
        }
    }
    private void OmniDirectionalMove()
    {
        Rigidbody2D attachedRB = attachedObject.GetComponent<Rigidbody2D>();
        if (attachedObject == null)
            Debug.LogError("Nothing attached, why is this showing.");
        if (heldDistance == -1)
            heldDistance = Vector3.Distance(transform.position, attachedObject.position);
        float currentDistance = Vector3.Distance(transform.position, attachedObject.position);
        if (currentDistance > heldDistance)
        {
            if (transform.position.y > myRopeHead.position.y + 1 || transform.position.x > myRopeHead.position.x + 1)
            {
                Debug.Log("You are above");
                if (Input.GetAxisRaw(myPlayerNumName + "Vertical") >= 0 || Input.GetAxisRaw(myPlayerNumName + "Horizontal") >= 0)
                    attachedRB.velocity = myRigidbody.velocity;
            }
            else if (transform.position.y < myRopeHead.position.y - 1|| transform.position.x < myRopeHead.position.x - 1)
            {
                Debug.Log("You are below");
                if (Input.GetAxisRaw(myPlayerNumName + "Vertical") <= 0 || Input.GetAxisRaw(myPlayerNumName + "Horizontal") <= 0)
                    attachedRB.velocity = myRigidbody.velocity;
            }
            else
                attachedRB.velocity = new Vector2();
        }
        else if (currentDistance < heldDistance)
        {
            attachedRB.velocity = new Vector2();
            heldDistance = currentDistance;
        }
    }


    private void MovePlayerTowardsWinPosition()
    {
        if (GetComponent<BoxCollider2D>().enabled)
            GetComponent<BoxCollider2D>().enabled = false;

        if (myPlayerNumName == "Player1")
        {
            myWinPos = player1WinPos.position;
            transform.position = Vector2.MoveTowards(transform.position, myWinPos, movementSpeed * Time.deltaTime);
        }
        else if (myPlayerNumName == "Player2")
        {
            myWinPos = player2WinPos.position;
            transform.position = Vector2.MoveTowards(transform.position, myWinPos, movementSpeed * Time.deltaTime);
        }
    }
}
