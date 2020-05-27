using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    // Swipe control
    public delegate void TouchEventHandler(Vector2 drag);
    public static event TouchEventHandler swipeEvent;
    public static event TouchEventHandler swipeEndEvent;

    // Swipe threshold
    public int swipeDistance;

    // Touch movement
    private Vector2 _touchMovement;
    private Vector2 _tapPosition;

    // Tap control
    public static event TouchEventHandler tapEvent;
    public float tapTimeMax = 0;
    public float tapTime = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            // If touch began
            if(touch.phase == TouchPhase.Began)
            {
                // Swipe control
                _touchMovement = Vector2.zero;
                tapTimeMax = Time.time + tapTime;

                // For tap control
                _tapPosition = touch.position;
            }
            else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                _touchMovement += touch.deltaPosition;

                // If swipe is greater than the threshold then triiger events
                if(_touchMovement.magnitude > swipeDistance)
                {
                    OnSwipe();
                }
            }
            // Finger is off the screen
            else if(touch.phase == TouchPhase.Ended)
            {
                if(_touchMovement.magnitude > swipeDistance)
                {
                    OnSwipeEnd();
                }

                else if(Time.time < tapTimeMax)
                {
                    OnTap();
                }
            }
        }
        
    }

    /// <summary>
    /// When the swipe is detected, trigger relative events.
    /// </summary>
    private void OnSwipe()
    {
        if (swipeEvent != null)
            swipeEvent(_touchMovement);
    }

    /// <summary>
    /// When the swipe end is detected, trigger relative events.
    /// </summary>
    private void OnSwipeEnd()
    {
        if (swipeEndEvent != null)
            swipeEndEvent(_touchMovement);
    }

    /// <summary>
    ///  When the touch is detected, trigger relative events.
    /// </summary>
    private void OnTap()
    {
        if(tapEvent != null)
        {
            tapEvent(_tapPosition); 
        }
    }

}
