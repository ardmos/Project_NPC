using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : DontDestroy<AudioSystem>
{
    public Dialogue.DialogueSet set;


    public void DialogSFXHelper(Dialogue.DialogueSet dialogueSet)
    {
        set = dialogueSet;
        StartCoroutine(enumerator(dialogueSet));
    }

    IEnumerator enumerator(Dialogue.DialogueSet dialogueSet)
    {
        //타이핑 말고 그냥 효과음을 실행하는 부분!
        if (dialogueSet.detail.sFXSettings.enableSFX)
        {
            for (int i = 0; i < dialogueSet.detail.sFXSettings.playTime; i++)
            {
                gameObject.GetComponent<AudioSource>().PlayOneShot(dialogueSet.detail.sFXSettings.audioClip);
                yield return new WaitForSeconds(dialogueSet.detail.sFXSettings.delayTime);
            }
        }
    }
}
