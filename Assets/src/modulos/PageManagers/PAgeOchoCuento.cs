using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PAgeOchoCuento : PageManager
{
    private PlayableDirector playableDirector;

    private void Awake()
    {
        // Obtiene el PlayableDirector del mismo GameObject
        playableDirector = GetComponent<PlayableDirector>();
    }

    public override void OnActivatePage()
    {
        if (playableDirector != null)
        {
            playableDirector.time = 0; // Reinicia al principio
            playableDirector.Play();  // Reproduce la Timeline
        }
    }

    public override void OnDeactivatePage()
    {
        if (playableDirector != null)
        {
            playableDirector.Stop(); // Detiene la Timeline
        }
    }
}
