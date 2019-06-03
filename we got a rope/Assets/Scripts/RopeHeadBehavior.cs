using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RopeHeadState { Retracted, Extending, Attached, Retracting };
public class RopeHeadBehavior : MonoBehaviour
{
    
    public RopeHeadState myHeadState;
    [SerializeField]
    private GameObject Player1;
    [SerializeField]
    private GameObject Player2;
    [SerializeField]
    private int MyAssignedPlayer;
    [SerializeField]
    private float speed;
    [SerializeField]
    private LayerMask player1Mask;
    [SerializeField]
    private LayerMask player2Mask;

    private Transform myPlayer;
    private Transform otherPlayer;
    public Transform attachedObject;
    private MoveableBlockScript attachedObjectScript;
    private LineRenderer myLineRenderer;
    private BoxCollider2D myOwnCollider;
    private Rigidbody2D myRigidBody;

    private bool hasReachedOtherPlayer = false;

    //this is the end of the rope each player fires and it needs a few rules for how it works.

    //first, if its x or y position is offset from the player it is meant to be attached to by any amount it needs to rotate itself to
    //point its back side at that player.

    //second is that its motion relys on when/where it was fired from and whether it has collided with anything
    //while moving through the air its motion vector should simply be away from whichever player fired it at a given frame.
    //after colliding it should check to see what it collided with to see if it should remain stationary or detach/bounce off
    //when retracting it should simply be directly at the player who fired it.

    //button pressed, calculatedirectionofextension, begin moving in direction of calculated extension 
    //and check if you've collided with anything other than your own player.

    // Start is called before the first frame update
    void Start()
    {//do we need this to repeat as we enter new scenes?
        myOwnCollider = GetComponent<BoxCollider2D>();
        myLineRenderer = GetComponent<LineRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
        if (MyAssignedPlayer == 1)
        {
            myPlayer = Player1.transform;
            otherPlayer = Player2.transform;
        }
        else if (MyAssignedPlayer == 2)
        {
            myPlayer = Player2.transform;
            otherPlayer = Player1.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myHeadState == RopeHeadState.Retracted)
        {
            RetractedMove();
        }
        else if (myHeadState == RopeHeadState.Extending)
        {
            if (myPlayer.GetComponent<PlayerMotion>().myPlayerNumName == "Player1")
            {
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, 2, otherPlayer.position, Mathf.Infinity, player1Mask);
                if (hit.collider.CompareTag("Player2"))
                    Debug.Log("Hit player");  
                //yay win

                else
                    ExtendingMove();
            }
            
        }
        else if (myHeadState == RopeHeadState.Retracting)
        {
            RetractingMove();
        }
        else if (myHeadState == RopeHeadState.Attached)
        {
            AttachedMove();
        }
        FaceAwayFromPlayer();
    }

    void RetractedMove()
    {
        transform.position = myPlayer.position;
        if (myOwnCollider.enabled)
        {
            myOwnCollider.enabled = false;
        }

    }

    void ExtendingMove()
    {

        if (!myOwnCollider.enabled)
        {
            myOwnCollider.enabled = true;
        }
        CalculateDirectionOfExtension();

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, otherPlayer.position, step);
        //when extending move in the direction of the other player while always pointing your back to your own player.
    }

    void AttachedMove()
    {
        if (myOwnCollider.enabled)
        {
            myOwnCollider.enabled = false;
        }
        //deactivate your own head's sprite and collider, remain attached to the postion of the attached object. 
        transform.SetParent(attachedObject, true);

        attachedObjectScript = attachedObject.GetComponent<MoveableBlockScript>();
        attachedObjectScript.attachedPlayer = myPlayer;
        attachedObjectScript.currentState = BlockInteractionState.Grabbed;

        myRigidBody.constraints = attachedObject.GetComponent<Rigidbody2D>().constraints;
        myRigidBody.velocity = attachedObject.GetComponent<Rigidbody2D>().velocity;

        if (Input.GetButton(myPlayer.GetComponent<PlayerMotion>().myPlayerNumName + "Fire"))
        {
            myHeadState = RopeHeadState.Retracting;
        }
    }

    void RetractingMove()
    {
        if (myOwnCollider.enabled)
        {
            myOwnCollider.enabled = false;
        }
        if (attachedObjectScript != null)
        {
            attachedObjectScript.currentState = BlockInteractionState.Idle;
        }

        attachedObject = null;
        attachedObjectScript = null;
        transform.SetParent(null);

        if (transform.position == myPlayer.position)
        {
            myHeadState = RopeHeadState.Retracted;
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, myPlayer.position, step);
        }
        //move directly at your player, keep your back pointing directly at that point. collider should be inactive.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string playerName = myPlayer.GetComponent<PlayerMotion>().myPlayerNumName;


        if (playerName == "Player1")
        {
            if (collision.gameObject.CompareTag("Player2"))
            {
                myHeadState = RopeHeadState.Retracting;
            }
            else if (collision.gameObject.CompareTag("MoveableBlock") || collision.gameObject.layer == 12)
            {
                attachedObject = collision.transform;
                myHeadState = RopeHeadState.Attached;
            }
            else if (collision.gameObject.CompareTag("ImmobileBlock") || collision.gameObject.CompareTag("PushableBlock") || collision.gameObject.layer == 13)
                myHeadState = RopeHeadState.Retracting;
        }
        if (playerName == "Player2")
        {
            if (collision.gameObject.CompareTag("Player1"))
            {
                myHeadState = RopeHeadState.Retracting;
            }
            else if (collision.gameObject.CompareTag("MoveableBlock") || collision.gameObject.layer == 13)
            {
                attachedObject = collision.transform;
                myHeadState = RopeHeadState.Attached;
            }
            else if (collision.gameObject.CompareTag("ImmobileBlock") || collision.gameObject.CompareTag("PushableBlock") || collision.gameObject.layer == 12)
                myHeadState = RopeHeadState.Retracting;
        }
    }

    private Vector3 CalculateDirectionOfMyPlayer()
    {
        return transform.position - myPlayer.position;
    }

    private Vector3 CalculateDirectionOfExtension()
    {
        return transform.position - otherPlayer.position;
    }
    
    private void FaceAwayFromPlayer()
    {
        Vector3 ForwardsDirection = CalculateDirectionOfExtension();

        float rotZ = Mathf.Atan2(ForwardsDirection.y, ForwardsDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);

    }
    

}
