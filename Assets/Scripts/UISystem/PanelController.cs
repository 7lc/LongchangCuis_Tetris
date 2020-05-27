/*
 * Developer Name: Longchang Cui
 * Date: May-24-2020
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Auto-correct the rect transform size of the top panel based on the mobile's aspect ratio.
/// </summary>
public class PanelController : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _mainCamera = GameObject.FindObjectOfType<Camera>();

        // For different mobile device's screen aspect ratio(Width:Height), set different rect transform height value.
        double aspectRatio = Math.Truncate(_mainCamera.aspect * 100) / 100;
        Debug.Log("The Aspect ratio is" + aspectRatio);

        // For 9:16 and 10:18, set rectheight 380
        if (aspectRatio >= 0.55 && aspectRatio <= 0.56)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 380);
        }

        // For 9:18 set rect height 400
        else if (aspectRatio == 0.49)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400);
        }

        // For 9:19 set rect height 412
        else if (aspectRatio > 0.44 && aspectRatio <= 0.47)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 412);
        }
        
        // For 10:19 set rect height 390
        else if (aspectRatio == 0.52)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 390);
        }
        
        // For 9:20 set rect height 425
        else if (aspectRatio >= 0.44 && aspectRatio < 0.47)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 425);
        }
    }

}
