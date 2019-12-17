using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public float LevelFadeTime;
    public Image FadeImage;
    bool fade = false;

    GameObject lastselect;
    private bool loading;

    public void Start()
    {
        FadeImage.canvasRenderer.SetAlpha(0.0f);
        lastselect = new GameObject();
    }

    public void Update()
    {

        if (fade)
        {
            //FadeImage.GetComponent<RawImage>().CrossFadeColor(new Color(0,0,0,1), LevelFadeTime, true, true);
            FadeImage.CrossFadeAlpha(1, LevelFadeTime-2, false);
            Debug.Log("fade");
        }
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }
    public void ButtonStart()
    {
        StartCoroutine(LoadLevel(1));
    }
    public void ButtonQuit()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    IEnumerator LoadLevel(int id)
    {
        
        if (loading != true)
        {
            loading = true;
            fade = true;
            yield return new WaitForSeconds(LevelFadeTime + 1);
            SceneManager.LoadScene(id);
        }


    }
}
