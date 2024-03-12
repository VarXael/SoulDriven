using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    public GameObject optionPanel;

    public void Play()
    {
        SceneManager.LoadScene("PlayScene");
    }
    
    public void ShowOptions()
    {
        optionPanel.SetActive(true);
    }

    public void HideOptions()
    {
        optionPanel.SetActive(false);
    }
}
