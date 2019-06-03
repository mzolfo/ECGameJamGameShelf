using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePlate : MonoBehaviour
{
    public bool fadedToWhite;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        fadedToWhite = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Endframe()
    {
        fadedToWhite = true;
    }
}
