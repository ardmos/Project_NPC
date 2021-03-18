using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : DontDestroy<AudioSystem>
{
    //Dialogue 추가 효과음 처리 위한 부분
    public Dialogue.DialogueSet set;

    //Dialogue 넘길 때 효과음
    public AudioClip dialogFlip;

    //마우스 클릭 효과음
    public AudioClip furnitureClick;


    #region Dialogue추가 효과음
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
    #endregion

    #region Dialogue넘길때 소리
    public void PlayDialogueFlipSFX()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(dialogFlip);
    }
    #endregion

    #region 마우스 클릭 소리
    public void PlayFurnitureClickSFX()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(furnitureClick);
    }
    #endregion


}
