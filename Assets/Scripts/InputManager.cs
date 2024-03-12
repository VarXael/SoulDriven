using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Joystick m_joystick;
	public RectTransform m_attackButton;

	public bool simulateTouchWithMouse = false;
	public bool detectSwipeAfterRelease = false;
	public float SWIPE_THRESHOLD = 20f;

	public event Action OnClickAttackButton;

	private Vector2 m_fingerDownPos;
	private Vector2 m_fingerUpPos;

	private bool m_clickBegan = false;
	private Vector2 m_mouseDownPos;
	private Vector2 m_mouseUpPos;

	private void Update ()
	{
		if ( !IsOnUI(m_joystick.BackgroundJoystick) && !IsOnUI(m_attackButton) ) {
			foreach (Touch touch in Input.touches)
			{
				if (touch.phase == TouchPhase.Began)
				{
					m_fingerUpPos = touch.position;
					m_fingerDownPos = touch.position;
				}

				//Detects Swipe while finger is still moving on screen
				/*
				if (touch.phase == TouchPhase.Moved) {
					if (!detectSwipeAfterRelease) {
						m_fingerDownPos = touch.position;
						DetectSwipe(m_fingerDownPos, m_fingerUpPos);
					}
				}

				*/

				//Detects swipe after finger is released from screen
				if (touch.phase == TouchPhase.Ended)
				{
					m_fingerDownPos = touch.position;
					DetectSwipe(m_fingerDownPos, m_fingerUpPos);
				}
			}
			
			if (simulateTouchWithMouse)
			{
				if (!m_clickBegan && Input.GetMouseButtonDown(0))
				{
					//Debug.Log("HELLO!");
					m_mouseUpPos = Input.mousePosition;
					m_mouseDownPos = Input.mousePosition;
					m_clickBegan = true;
				}

				//Detects Swipe while mouse click is still moving on screen
				/*
				if (Input.GetMouseButton(0)) {
					if (!detectSwipeAfterRelease) {
						m_mouseDownPos = Input.mousePosition;
						DetectSwipe(m_mouseDownPos, m_mouseUpPos);
					}
				}
				*/

				//Detects swipe after mouse click is released from screen
				if (Input.GetMouseButtonUp(0))
				{
					//Debug.Log("HELLO2!");
					m_mouseDownPos = Input.mousePosition;
					DetectSwipe(m_mouseDownPos, m_mouseUpPos);
					m_clickBegan = false;
				}
			}
		}
	}

	private bool IsOnUI(RectTransform rectTransform)
    {
		Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
		if (rectTransform.rect.Contains(localMousePosition))	{
			return true;
		}

		return false;
	}

	private void DetectSwipe (Vector2 downPos, Vector2 upPos)
	{		
		if (VerticalMoveValue (downPos.y,upPos.y) > SWIPE_THRESHOLD && VerticalMoveValue (downPos.y,upPos.y) > HorizontalMoveValue (downPos.x,upPos.x)) {
			Debug.Log ("Vertical Swipe Detected!");
			if (downPos.y - upPos.y > 0) {
				OnSwipeUp ();
			} else if (downPos.y - upPos.y < 0) {
				OnSwipeDown ();
			}
			upPos = downPos;

		} else if (HorizontalMoveValue (downPos.x,upPos.x) > SWIPE_THRESHOLD && HorizontalMoveValue (downPos.x,upPos.x) > VerticalMoveValue (downPos.y,upPos.y)) {
			Debug.Log ("Horizontal Swipe Detected!");
			if (downPos.x - upPos.x > 0) {
				OnSwipeRight ();
			} else if (downPos.x - upPos.x < 0) {
				OnSwipeLeft ();
			}
			upPos = downPos;

		} else {
			//Debug.Log ("No Swipe Detected!");
		}
	}

	float VerticalMoveValue (float yDown, float yUp)
	{
		return Mathf.Abs (yDown - yUp);
	}

	float HorizontalMoveValue (float xDown, float xUp)
	{
		return Mathf.Abs (xDown - xUp);
	}

	void OnSwipeUp ()
	{	
		//Do something when swiped up
		Debug.Log ("Class Up");
	}

	void OnSwipeDown ()
	{
		//Do something when swiped down
		Debug.Log ("Class Down");
	}

	void OnSwipeLeft ()
	{
		//Do something when swiped left
		Debug.Log ("Class Left");
	}

	void OnSwipeRight ()
	{
		//Do something when swiped right
		Debug.Log ("Class Right");
	}

	public void InvokeOnClickAttackButton()
    {
		OnClickAttackButton?.Invoke();
    }

	public float GetHorizontalJoystick() => m_joystick.Horizontal();
	public float GetVerticalJoystick() => m_joystick.Vertical();
}
