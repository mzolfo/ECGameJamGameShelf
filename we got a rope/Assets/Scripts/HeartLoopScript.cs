using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartLoopScript : MonoBehaviour
{
    private Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void BeginLoop()
    {
        myAnimator.SetBool("StartLoop", true);
    }
}
