using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockInteractionState { Idle, Grabbed };
public class MoveableBlockScript : MonoBehaviour
{

    public enum BlockMovementType { Horizontal, Vertical, Pivot, OmniDirectional};
    public BlockMovementType myBlockMovementType;

    //public enum InteractionState { Idle, Grabbed };
    public BlockInteractionState currentState = BlockInteractionState.Idle;

    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySpriteRenderer;

    public Transform attachedPlayer;

    [SerializeField]
    private Sprite activeSprite;
    private Sprite idleSprite;

    [SerializeField]
    private bool pushable;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        idleSprite = mySpriteRenderer.sprite; //Thought this might just save some editor work for us
    }

    void Update()
    {
        /* Needs a way to decern that the grappling hook has attached itself. Probably done in the 
         * Ropehead behavior script changing the current state of the block that it collides with */


        if (currentState == BlockInteractionState.Grabbed)
        {

            mySpriteRenderer.sprite = activeSprite;

            if (myBlockMovementType == BlockMovementType.Horizontal)
                HorizontalMovement();
            else if (myBlockMovementType == BlockMovementType.Vertical)
                VerticalMovement();
            else if (myBlockMovementType == BlockMovementType.Pivot)
                PivotMovement();
            else if (myBlockMovementType == BlockMovementType.OmniDirectional)
                OmniDirectionalMovement();
        }
        else if (!pushable)
        {
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            mySpriteRenderer.sprite = idleSprite;
        }
        else
            mySpriteRenderer.sprite = idleSprite;
    }

    private void HorizontalMovement()
    {
        //When a player tries to move a Horizontal movement type block
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    private void VerticalMovement()
    {
        //When a player tries to move a Vertical movement type block
        //float distance = Vector2.Distance(transform.position, attachedPlayer.position);
        //Debug.Log("Current distance between block and player: " + distance);
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        
    }

    private void PivotMovement()
    {
        //When a player tries to move a Pivot movement type block
        myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
    }

    private void OmniDirectionalMovement()
    {
        //When a player tries to move an OmniDirectional movement type block
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

}
