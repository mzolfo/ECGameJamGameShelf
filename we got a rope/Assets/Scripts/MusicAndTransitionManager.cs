using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MusicAndTransitionManager : MonoBehaviour
{
    public static bool Paused;
    [SerializeField]
    private GameObject PauseMenu;
    [SerializeField]
    private GameObject whitePlate;
    [SerializeField]
    private AudioClip MainMenuMusicStart;
    [SerializeField]
    private AudioClip MainMenuMusicLoop;
    [SerializeField]
    private AudioClip LevelMusicStart;
    [SerializeField]
    private AudioClip LevelMusicLoop;
    private bool WaitingOnTransition;
    private AudioSource MainMusicSource;
    [SerializeField]
    private int CurrentLevel = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        MainMusicSource.clip = MainMenuMusicStart;
        MainMusicSource.Play();
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        CheckSoundTrackMustBeLooped();
        if (WaitingOnTransition)
        {

        }
    }

    private void CheckSoundTrackMustBeLooped()
    {
        if (!MainMusicSource.isPlaying)
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
            {
                MainMusicSource.clip = MainMenuMusicLoop;
                MainMusicSource.Play();
            }
            else
            {
                MainMusicSource.clip = LevelMusicLoop;
                MainMusicSource.Play();
            }
        }

    }
    private void FadeToWhite()
    {
        whitePlate.GetComponent<WhitePlate>().FadeToWhite();
        WaitingOnTransition = true;
    }
    public void EnterLevelFromMainMenu()
    {
        SceneManager.LoadScene(1);
        MainMusicSource.clip = LevelMusicStart;
        MainMusicSource.Play();
        //fade to white
        //null white plate and pause menu
        //load first level
        //find new level's whiteplate and pause menu
        //fade from white
    }

    public void BeginSceneTransition()
    {
        
        //fade to white
        //null your white plate and pause menu
        //load new scene
        //find new white plate and pause menu
        //fade from white.



        //at some point in this set your white plate equal to null and load the new scene then search for the new white plate.
    }

    public void ReturnToMainMenu()
    {
        //fade to white
        //null your whiteplate and pause menu
        //stop music
        //load new scene
        //find new whiteplate
        //start menu music
        //fade from white
        MainMusicSource.Stop();
        MainMusicSource.clip = MainMenuMusicStart;
        MainMusicSource.Play();
    }

    private void PauseGame()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0))
        {
            Paused = true;
            PauseMenu.SetActive(true);
        }
        
    }

    public void ResumeGame()
    {
        Paused = false;
        PauseMenu.SetActive(false);
    }
}
