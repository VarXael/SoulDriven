using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO:
    // Store game status (into DB ?)   
    
    // public GameObject UIManager
    public LayoutManager mLayoutManager;
    public InputManager mInputManager;
    public Player mPlayer;

    private bool mCanSwitchLayout = true;
    private bool mGameIsPaused;


    #region events

    #endregion

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        } 
        else
        {
            Destroy(transform.root.gameObject);
            return;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (mCanSwitchLayout) StartCoroutine(WaitForChangeLayout(5f));
    }

    void FixedUpdate()
    {
        float inputX = mInputManager.GetHorizontalJoystick();
        float inputY = mInputManager.GetVerticalJoystick();

        mPlayer.SetInputX(inputX);
        mPlayer.SetInputY(inputY);
    }

    void LateUpdate()
    {
        
    }

    public void OnClickGroundTesting()
    {
        mLayoutManager.OnCreateNewLayout();
    }

    IEnumerator WaitForChangeLayout(float delay)
    {
        mCanSwitchLayout = false;
        yield return new WaitForSeconds(delay);
        mCanSwitchLayout = true;
        mLayoutManager.OnCreateNewLayout();
    }

    private void GameOver() { }
    private void PauseGame() { }
    private void StartGame() { }
    private void RestartGame() { }
    private void StopGame() { }
}
