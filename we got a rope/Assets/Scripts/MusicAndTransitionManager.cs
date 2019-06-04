using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MusicAndTransitionManager : MonoBehaviour
{
    public static bool Paused;
    [SerializeField]
   // private GameObject PauseMenu;
    private bool returningToMenu;
    [SerializeField]
    //private GameObject whitePlate;
    public bool WhitePlateIsWhite;
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
    private int waitForPlayerMessyTimer = -1;
    [SerializeField]
    private int CurrentLevel = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        MainMusicSource = GetComponent<AudioSource>();
        MainMusicSource.clip = MainMenuMusicStart;
        MainMusicSource.Play();
        WhitePlateIsWhite = false;
        //whitePlate.GetComponent<WhitePlate>().FadeFromWhite();
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        CheckSoundTrackMustBeLooped();
        CheckPlayerHasWon();
        frigginTimerChecks();
        //if (whitePlate != null)
       //{
          //  WhitePlateIsWhite = whitePlate.GetComponent<WhitePlate>().fadedToWhite;
        //}

        //if (WaitingOnTransition)
       // {
          //  if (WhitePlateIsWhite)
           // {
            //    SceneTransition();
          //  }
       // }
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
    
    private void CheckPlayerHasWon()
    {
        if (PlayerMotion.hasWon && waitForPlayerMessyTimer == -1)
        {
            waitForPlayerMessyTimer = 1000;
        }
    }
    public void EnterLevelFromMainMenu()
    {
        SceneManager.LoadScene(1);
        MainMusicSource.clip = LevelMusicStart;
        MainMusicSource.Play();
        CurrentLevel = CurrentLevel + 1;
        //PauseMenu = GameObject.Find("PauseMenu");
        //fade to white
        //null white plate and pause menu
        //load first level
        //find new level's whiteplate and pause menu
        //fade from white
    }

    public void BeginSceneTransition()
    {
        waitForPlayerMessyTimer = 0;
        //whitePlate.GetComponent<WhitePlate>().FadeToWhite();
        // WaitingOnTransition = true;
        
        //fade to white
        //null your white plate and pause menu
        //load new scene
        //find new white plate and pause menu
        //fade from white.
        
        //at some point in this set your white plate equal to null and load the new scene then search for the new white plate.
    }

    private void frigginTimerChecks()
    {
        if (waitForPlayerMessyTimer > 0)
        {
            waitForPlayerMessyTimer = waitForPlayerMessyTimer - 1;
        }
        else if (waitForPlayerMessyTimer == 0)
        {
            SceneTransition();
            waitForPlayerMessyTimer = -1;
        }
    }

    private void SceneTransition()
    {
         // whitePlate = null;
       // DontDestroyOnLoad(whitePlate);
        //PauseMenu = null;
        WaitingOnTransition = false;
        if (!returningToMenu)
        {
            if (CurrentLevel == 0)
            {
                EnterLevelFromMainMenu();
            }
            else
            {
                SceneManager.LoadScene(CurrentLevel + 1);
                CurrentLevel = CurrentLevel + 1;
               // PauseMenu = GameObject.Find("PauseMenu");
            }
        }
        else
        {
            SceneManager.LoadScene(0);
            CurrentLevel = 0;
            ResetMenuMusic();
            returningToMenu = false;
        }
        // whitePlate = GameObject.Find("WhitePlate");
        // whitePlate.GetComponent<WhitePlate>().FadeFromWhite();
        //WhitePlateIsWhite = false;
        PlayerMotion.hasWon = false;
        
    }

    public void ReturnToMainMenu()
    {
        returningToMenu = true;
        BeginSceneTransition();
    }

    public void ResetMenuMusic()
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

   
}
