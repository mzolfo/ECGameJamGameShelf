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
    
    private Transform myPlayer;
    private Transform otherPlayer;
    private LineRenderer myLineRenderer;
    private BoxCollider2D myOwnCollider;

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
            ExtendingMove();
        }
        else if (myHeadState == RopeHeadState.Retracting)
        {
            RetractingMove();
        }
        else if (myHeadState == RopeHeadState.Attached)
        {
            AttachedMove();
        }
        //myLineRenderer.SetPosition(0, transform.position);
        //myLineRenderer.SetPosition(1, myPlayer.position);
        FaceAwayFromPlayer();
    }

    void RetractedMove()
    {
        this.transform.position = myPlayer.position;
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        //when retracted, stay attached at your player's origin point and do not divert from it, keep rotation at 0

    }

    void ExtendingMove()
    {
        CalculateDirectionOfExtension();
        if (HasReachedOtherPlayer())
        { //this normally will mean just finish the stage
            myHeadState = RopeHeadState.Retracting;
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, otherPlayer.position, step);
        }
        //when extending move in the direction of the other player while always pointing your back to your own player.
    }

    void AttachedMove()
    {
        //deactivate your own head's sprite and collider, remain attached to the postion of the attached object. 
    }

    void RetractingMove()
    {
        if (this.transform.position == myPlayer.position)
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

    private bool HasReachedOtherPlayer()
    {
        if (this.transform.position == otherPlayer.position)
        {
            return true;
        }
        else { return false; }       
    }
    private Vector3 CalculateDirectionOfMyPlayer()
    {
        //this object.position - player.position
        return this.transform.position - myPlayer.position;
    }

    private Vector3 CalculateDirectionOfExtension()
    {
        //myplayer.position - otherplayer.position
        return this.transform.position - otherPlayer.position;
    }
    /*
    float step = speed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, aerialObjective, step);
    */
    private void FaceAwayFromPlayer()
    {
        if (myHeadState != RopeHeadState.Retracted)
        {
            if (!myOwnCollider.enabled)
            {
                myOwnCollider.enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
            }
            Vector3 ForwardsDirection = CalculateDirectionOfExtension();

            float rotZ = Mathf.Atan2(ForwardsDirection.y, ForwardsDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);
            /*
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // find the angle in degrees
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
            */
        }
        else
        {
            if (myOwnCollider.enabled)
            {
                myOwnCollider.enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
