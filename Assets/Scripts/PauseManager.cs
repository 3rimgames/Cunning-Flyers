﻿using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {

    public static PauseManager instance;

    public GameObject pausePanel;

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        GameController.control.isPaused = pauseStatus;
        if (pauseStatus == true)
            PauseGame();
    }

    public void UnPauseGame()
    {
        pausePanel.SetActive(false);
        GameController.control.isPaused = false;
        Time.timeScale = 1;
    }
}