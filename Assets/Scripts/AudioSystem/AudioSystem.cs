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

    //AudioSource들.  FX용, BGM용
    public AudioSource audioSource_FX, audioSource_BGM;

    private void Start()
    {
        audioSource_FX = gameObject.GetComponent<AudioSource>();
    }

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
                audioSource_FX.PlayOneShot(dialogueSet.detail.sFXSettings.audioClip);
                yield return new WaitForSeconds(dialogueSet.detail.sFXSettings.delayTime);
            }
        }
    }
    #endregion

    #region Dialogue넘길때 소리
    public void PlayDialogueFlipSFX()
    {
        audioSource_FX.PlayOneShot(dialogFlip);
    }
    #endregion

    #region 마우스 클릭 소리
    public void PlayFurnitureClickSFX()
    {
        audioSource_FX.PlayOneShot(furnitureClick);
    }
    #endregion

    #region 브금조절
    public void ActivateBGM(AudioClip audioClip)
    {
        //Debug.Log("audioClip: " + audioClip);
        if (audioClip == null)
        {
            return;
        }
        audioSource_BGM.clip = audioClip;
        audioSource_BGM.Play();
    }
    public void StopBGM()
    {
        if (audioSource_BGM.isPlaying)
            audioSource_BGM.Stop();
    }
    #endregion
}
