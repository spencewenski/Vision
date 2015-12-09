﻿using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
    public Transform menu;
    bool paused = false;
    // Use this for initialization
    void Awake() {
        menu = this.gameObject.transform;
        //ToggleMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            if (paused)
            {
                ToggleMenu(true);
            }
            else
            {
                ToggleMenu(false);
            }
        }
    }
    void ToggleMenu(bool toggle)
    {
        if(toggle)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        Utility.Hide(!toggle);
        menu.GetChild(0).gameObject.SetActive(toggle);
    }
    public void GotoMain()
    {
        ToggleMenu(false);
        Application.LoadLevel("_SceneStart");
    }
    public void Restart()
    {
        paused = false;
        ToggleMenu(false);
        Application.LoadLevel(Application.loadedLevel);
    }
    public void Resume()
    {
        paused = false;
        ToggleMenu(false);
    }
}
