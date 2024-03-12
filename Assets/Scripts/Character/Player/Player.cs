using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public enum PlayerClass {Warrior,Mage,Ranger};
    private PlayerClass currentPlayerClass;
    private ISoulInterface iSoul;


    //Movement Inputs
    private float inputX;
    private float inputY;


    //Strategies

    private new void Start()
    {
        base.Start();
        Debug.LogWarning("Keyboard Inputs are in use, remove them!");
        iSoul = gameObject.AddComponent<WarriorSoul>();
    }

    private void Update()
    {
        //inputX = Input.GetAxisRaw("Horizontal");
        //inputY = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.B))
        {
            iSoul.Attack();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove(inputX, inputY);
    }



    public void PlayerMove(float m_inputX, float m_inputY)
    {
        rb.velocity = new Vector3(m_inputX,m_inputY,0) * moveSpeed * Time.deltaTime;
    }


    public void SwitchClass(PlayerClass playerClass)
    {
        currentPlayerClass = playerClass;
    }

    #region Getter and Setter
    //Getter and setter for every player variable
    public float GetInputX()
    {
        return inputX;
    }
    public void SetInputX(float m_inputX)
    {
        inputX = m_inputX;
    }
    public float GetInputY()
    {
        return inputY;
    }
    public void SetInputY(float m_inputY)
    {
        inputY = m_inputY;
    }
    public PlayerClass GetPlayerClass()
    {
        return currentPlayerClass;
    }
    public void SetPlayerClass(PlayerClass playerClass)
    {
        SwitchClass(playerClass);
    }

    public ISoulInterface GetSoulInterface()
    {
        return iSoul;
    }
    public void SetSoulInterface(ISoulInterface soulInterface)
    {
        iSoul = soulInterface;
    }



    #endregion





}
