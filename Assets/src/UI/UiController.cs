using UnityEngine;

public class UiController : MonoBehaviour
{
    public TimelineManager timelineManager;
    public void OnNextPress()
    {
        timelineManager.PlayTimelineNext();
    }
}
