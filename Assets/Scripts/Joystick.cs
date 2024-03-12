using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*
 * IPointerDownHandler, IPointerUpHandler, IDragHandler used to receive OnPointerDown, OnPointerUp and OnDrag callbacks
 */
public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    #region Variables
    private RectTransform m_backgroundJoystick;
    public RectTransform BackgroundJoystick { get { return m_backgroundJoystick; } }
    private RectTransform m_touchJoystick;
    public RectTransform TouchJoystick { get { return m_touchJoystick; } }  

    private Vector2 m_point;    
    private Vector2 m_normalizedPoint;  //Joystick Horizontal and Vertical Input
    private float m_maxLength;  //max distance from center of m_touchJoystick
    private bool m_isTouching = false;
    public bool IsTouching { get { return m_isTouching; } }

    //Custom Unity Actions
    public UnityAction OnJoystickDownAction;    
    public UnityAction OnJoystickUpAction;

    private PointerEventData m_pointerEventData;    //event payload associated with pointer events
    private Camera m_camera;
    #endregion

    private void OnEnable()
    {
        OnPointerUp(null);
    }

    public void Awake()
    {
        m_backgroundJoystick = GetComponent<RectTransform>();
        m_touchJoystick = m_backgroundJoystick.GetChild(0).GetComponent<RectTransform>();

        m_maxLength = (m_backgroundJoystick.sizeDelta.x / 2f) - (m_touchJoystick.sizeDelta.x / 2f) - 5f;
    }

    public void Update()
    {
        /*
        ScreenPointToLocalPointInRectangle = Transform a screen space point to a position in the local space of a RectTransform that is on the plane of its rectangle.
        in this case:
        m_backgroundJoystick -> the RectTransform to find a point inside
        m_pointerEventData.position -> Screen space position
        m_camera -> The camera associated with the screen space position
        m_point -> Point in local space of the rect transform
        */
        if ( m_isTouching && RectTransformUtility.ScreenPointToLocalPointInRectangle(m_backgroundJoystick, m_pointerEventData.position, m_camera, out m_point)) {
            m_point = Vector2.ClampMagnitude(m_point, m_maxLength);
            m_touchJoystick.anchoredPosition = m_point;

            float length = Mathf.InverseLerp(0f, m_maxLength, m_point.magnitude);
            m_normalizedPoint = Vector3.ClampMagnitude(m_point, length);
        }
    }

    public void OnPointerDown(PointerEventData e)
    {
        m_isTouching = true;
        m_camera = e.pressEventCamera;
        OnDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        m_pointerEventData = e;
    }

    public void OnPointerUp(PointerEventData e)
    {
        m_isTouching = false;
        m_normalizedPoint = Vector3.zero;
        m_touchJoystick.anchoredPosition = Vector3.zero;
    }

    public float Horizontal() 
    {
        return m_normalizedPoint.x;
    }

    public float Vertical() 
    {
        return m_normalizedPoint.y;
    }
}
