using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{

    //Found a thing online to help with rope generation and such, hopefully it'll work.


    [SerializeField]
    private Transform theHook;

    [SerializeField]
    private float resolution = 1; //  Sets the amount of joints there are in the rope (1 = 1 joint for every 1 unit)
    [SerializeField]
    private float ropeDrag = .1f;
    [SerializeField]
    private float ropeMass = .1f;
    [SerializeField]
    private float ropeColRadius = .5f;

    private Vector2[] segmentPos;
    private GameObject[] joints;

    private LineRenderer myLineRenderer;
    private int segments = 0;
    private bool ropeExists = false;

    [SerializeField]
    private Sprite ropeSprite;

    private Vector2 swingAxis = new Vector2(1,1);
    [SerializeField]
    private float lowTwistLimit = -100f;
    [SerializeField]
    private float highTwistLimit = 100f;
    [SerializeField]
    private float swingLimit = 20f;

    public bool buildRope;
    public bool destroyRope;

    private void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        BuildRope();
    }

    void Update()
    {
        if (buildRope)
        {
            BuildRope();
            buildRope = false;
        }
        if (destroyRope)
        {
            DestroyRope();
            destroyRope = false;
        }
    }

    private void LateUpdate()
    {
        if (ropeExists)
        {

            for (int i = 0; i < segments; i++)
            {
                

                if (i == 0)
                {
                    myLineRenderer.SetPosition(i, transform.position);
                }
                else if (i == segments - 1)
                {
                    myLineRenderer.SetPosition(i, theHook.position);
                }
                else
                {
                    myLineRenderer.SetPosition(i, joints[i].transform.position);
                }
                myLineRenderer.enabled = true;
            }
        }
        else
        {
            myLineRenderer.enabled = false;
        }
    }

    private void BuildRope()
    {
        //myLineRenderer = gameObject.GetComponent<LineRenderer>();

        segments = (int)(Vector2.Distance(transform.position, theHook.position) * resolution);
        myLineRenderer.positionCount = segments;
        segmentPos = new Vector2[segments];
        joints = new GameObject[segments];
        segmentPos[0] = transform.position;
        segmentPos[segments - 1] = theHook.position;

        var segs = segments - 1;
        var separation = ((theHook.position - transform.position) / segs);

        for (int s = 1; s < segments; s++)
        {
            Vector3 vector = (separation * s) + transform.position;
            segmentPos[s] = vector;

            AddJointPhysics(s);
        }

        FixedJoint2D end = theHook.gameObject.AddComponent<FixedJoint2D>();
        end.connectedBody = joints[joints.Length - 1].transform.GetComponent<Rigidbody2D>();
        //end.s = swingAxis;
        //SoftJointLimit limit_setter = end.lowTwistLimit;
        //limit_setter.limit = lowTwistLimit;
        //end.lowTwistLimit = limit_setter;
        //limit_setter = end.highTwistLimit;
        //limit_setter.limit = highTwistLimit;
        //end.highTwistLimit = limit_setter;
        //limit_setter = end.swing1Limit;
        //limit_setter.limit = swingLimit;
        //end.swing1Limit = limit_setter;
        

        //theHook.parent = transform;

        ropeExists = true;

    }

    private void AddJointPhysics(int n)
    {
        joints[n] = new GameObject("joint_" + n);
        joints[n].transform.parent = transform;
        Rigidbody2D rigid = joints[n].AddComponent<Rigidbody2D>();
        CircleCollider2D col = joints[n].AddComponent<CircleCollider2D>();
        FixedJoint2D ph = joints[n].AddComponent<FixedJoint2D>();
        SpriteRenderer sr = joints[n].AddComponent<SpriteRenderer>();
        sr.sprite = ropeSprite;
        joints[n].transform.Rotate(Vector3.forward * -90);
        //ph.swingAxis = swingAxis;
        //SoftJointLimit limit_setter = ph.lowTwistLimit;
        //limit_setter.limit = lowTwistLimit;
        //ph.lowTwistLimit = limit_setter;
        //limit_setter = ph.highTwistLimit;
        //limit_setter.limit = highTwistLimit;
        //ph.highTwistLimit = limit_setter;
        //limit_setter = ph.swing1Limit;
        //limit_setter.limit = swingLimit;
        //ph.swing1Limit = limit_setter;
        
        
        


        joints[n].transform.position = segmentPos[n];

        rigid.drag = ropeDrag;
        rigid.mass = ropeMass;
        rigid.gravityScale = 0;
        //rigid.freezeRotation = true;
        col.radius = ropeColRadius;

        if (n == 1)
        {
            ph.connectedBody = transform.GetComponent<Rigidbody2D>();
        }
        else
        {
            ph.connectedBody = joints[n - 1].GetComponent<Rigidbody2D>();
        }

    }

    private void DestroyRope()
    {
        ropeExists = false;
        for (int dj = 0; dj < joints.Length - 1; dj++)
        {
            Destroy(joints[dj]);
        }

        segmentPos = new Vector2[0];
        joints = new GameObject[0];
        segments = 0;
    }


}
