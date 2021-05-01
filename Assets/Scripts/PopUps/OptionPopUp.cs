using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopUp : MonoBehaviour
{
    [SerializeField]
    Slider slider_BGM, slider_SFX;

    private void Start()
    {
        //초기 볼륨 값 읽어와서 Slider에 적용.
        slider_BGM.value = AudioSystem.Instance.GetVolume_BGM();
        slider_SFX.value = AudioSystem.Instance.GetVolume_SFX();
    }


    #region BGM Slider 관련
    public void SettingVolume_BGM()
    {
        AudioSystem.Instance.SetVolume_BGM(slider_BGM.value);
    }
    #endregion

    #region SFX Slider 관련
    public void SettingVolume_SFX()
    {
        AudioSystem.Instance.SetVolume_SFX(slider_SFX.value);
    }
    #endregion

    #region 옵션팝업의 오픈과 클로즈 처리    
    public void OpenThisPopup()
    {
        gameObject.GetComponent<Animator>().SetBool("isOpen", true);
    }

    public void OnButtonOKClicked()
    {
        gameObject.GetComponent<Animator>().SetBool("isOpen", false);
        //그리고 변경사항들 저장시키는 부분
        //
        //추가할곳
        //딱히 필요 없음. 수정할 때 직접 AudioSystem의 볼륨을 바꿔주기 때문에. 
    }
    #endregion
}
