using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public TimelineManager timelineManager;
    public void OnNextPress()
    {
        timelineManager.PlayTimelineNext();
    }
}
