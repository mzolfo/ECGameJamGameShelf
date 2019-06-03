using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockInteractionState { Idle, Grabbed };
public enum BlockMovementType { Horizontal, Vertical, Pivot, OmniDirectional };
public class MoveableBlockScript : MonoBehaviour
{
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
    public bool ropeHeld = false;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        idleSprite = mySpriteRenderer.sprite; //Thought this might just save some editor work for us
        ConstrainMovementAccordingtoMovementType();
        if (!pushable)
            myRigidbody.mass = 99999;
        else
        {
            myRigidbody.mass = 1;
            myRigidbody.drag = 100;
        }
    }

    void Update()
    {
        /* Needs a way to decern that the grappling hook has attached itself. Probably done in the 
         * Ropehead behavior script changing the current state of the block that it collides with */
        if (currentState == BlockInteractionState.Grabbed)
        {
            mySpriteRenderer.sprite = activeSprite;
        }
        else
        {
            mySpriteRenderer.sprite = idleSprite;
        }
            
    }

    private void ConstrainMovementAccordingtoMovementType()
    {
        if (myBlockMovementType == BlockMovementType.Horizontal)
        { myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY; }
        else if (myBlockMovementType == BlockMovementType.Vertical)
        { myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX; }
        else if (myBlockMovementType == BlockMovementType.Pivot)
        { myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX; }
        else if (myBlockMovementType == BlockMovementType.OmniDirectional)
        { myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation; }
    }

}
