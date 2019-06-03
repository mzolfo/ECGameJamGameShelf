using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTestRopeScript : MonoBehaviour
{

    // Need to add functionality to be able to change target of this since this will
    // be used for multiple players
    private AudioSource myAudioSource;
    private LineRenderer myLine;
    public bool AudioIsPlaying;
    [SerializeField]
    private Transform target; 

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myLine = GetComponent<LineRenderer>();
        myLine.positionCount = 2;
        transform.Rotate(Vector3.forward * -90);
    }

    void Update()
    {
        myLine.SetPosition(0, transform.position);
        myLine.SetPosition(1, target.position);

        float distance = Vector2.Distance(transform.position, target.position);
        myLine.material.mainTextureScale = new Vector2(distance * 2, 1);

    }

    public void PlayRopeNoise()
    {
        myAudioSource.Play();
        AudioIsPlaying = true;
    }

    public void StopRopeNoise()
    {
        myAudioSource.Stop();
        AudioIsPlaying = false;
    }
}
