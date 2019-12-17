using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Selectable focusButton;
    public bool pause = false;
    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!pause)
            {
                Pause();
            }
            else if (pause)
            {
                UnPause();
            }
        }
    }

    public void Start()
    {
        pausePanel.SetActive(false);
    }

    public void Pause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        focusButton.Select();
        pause = true;
    }
    public void UnPause()
    {
        Time.timeScale = 1;

        pausePanel.SetActive(false);
        pause = false;

    }

    public void ButtonContinue()
    {
        UnPause();
        


    }

    public void ButtonQuit()
    {

#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
