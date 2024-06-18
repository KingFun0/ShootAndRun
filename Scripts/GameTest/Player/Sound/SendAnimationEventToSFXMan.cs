using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAnimationEventToSFXMan : MonoBehaviour
{
 public PlayerPhotonSoundManager soundManager;

    public void TriggerFootstepSFX()
    {
        soundManager.PlayerFootstepSFX();
    }
}
