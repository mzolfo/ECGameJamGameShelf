﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject CreditsScreen;
    [SerializeField]
    private GameObject MainMenuObject;

    // Start is called before the first frame update
    void Start()
    {
        CreditsScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreditsButton()
    {
        MainMenuObject.SetActive(false);
        CreditsScreen.SetActive(true);

    }
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void BackButton()
    {
        MainMenuObject.SetActive(true);
        CreditsScreen.SetActive(false);
    }

}