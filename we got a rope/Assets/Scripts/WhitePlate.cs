using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePlate : MonoBehaviour
{
    public bool fadedToWhite;
    private Animator myAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    private void Awake()
    {
        fadedToWhite = false;
        //DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeToWhite()
    {
        //myAnimator.SetInteger("FadeState", 2);
       
    }

    public void FadeFromWhite()
    {
        //myAnimator.SetInteger("FadeState", 1);
    }
    public void Endframe()
    {
        fadedToWhite = true;
        
    }
}
