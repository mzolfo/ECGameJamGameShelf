using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlockScript : MonoBehaviour
{

    public enum BlockMovementType { Horizontal, Vertical, Pivot, OmniDirectional };
    public BlockMovementType myBlockMovementType;

    public enum InteractionState { Idle, Grabbed };
    public InteractionState currentState = InteractionState.Idle;

    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySpriteRenderer;

    [SerializeField]
    private Sprite activeSprite;
    
    private Sprite idleSprite;

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


        if (currentState == InteractionState.Grabbed)
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
        else
            mySpriteRenderer.sprite = idleSprite;
    }

    private void HorizontalMovement()
    {
        //When a player tries to move a Horizontal movement type block
    }

    private void VerticalMovement()
    {
        //When a player tries to move a Vertical movement type block

    }

    private void PivotMovement()
    {
        //When a player tries to move a Pivot movement type block
    }

    private void OmniDirectionalMovement()
    {
        //When a player tries to move an OmniDirectional movement type block
    }

}
