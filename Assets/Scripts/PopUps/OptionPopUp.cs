using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPopUp : MonoBehaviour
{
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
    }
}
