using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager : MonoBehaviour
{
    private int CurrentScene;
    [SerializeField]
    private AudioClip MenuMusic;
    [SerializeField]
    private AudioClip StartLevelMusic;
    [SerializeField]
    private AudioClip LoopLevelMusic;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
