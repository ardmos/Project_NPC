using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miro_Hard_Manager : MonoBehaviour
{
    //상호작용 오브젝트들
    //문1, 문2, 가시, 수상한 꽃, 힐팩, 바위, 나무판1, 나무판2, 대왕거미줄
    public GameObject 문1, 문2, 가시, 수상한꽃, 힐팩, 바위, 나무판1, 나무판2, 대왕거미줄, 작업바, 타이머, 말풍선;

    //처음엔 가시, 힐팩 비활성화시켜야함
    //문1을 열면 가시 활성화시켜야함
    //문2를 열면 힐팩 활성화시켜야함

    // Start is called before the first frame update
    void Start()
    {
        말풍선.SetActive(false);
        가시.SetActive(false);
        힐팩.SetActive(false);
    }

    //가시 활성화시켜주기
    public void Activate가시()
    {
        가시.SetActive(true);
    }
    //힐팩 활성화시켜주기
    public void Activate힐팩()
    {
        힐팩.SetActive(true);
    }
    //말풍선 활성화시켜주기
    public void Activate말풍선(Vector2 screenPos, string scanObjName)
    {
        if(말풍선.activeSelf != true && GameObject.Find("InterActiveBar") ==null)
        {
            말풍선.SetActive(true);
            Vector2 localPos = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(말풍선.GetComponent<RectTransform>(), screenPos, Camera.main, out localPos);
            //Debug.Log(localPos);

            //문이면 그대로, 다른애들이면 y를 +30 해주기 

            if (scanObjName.Contains("문"))
            {
                말풍선.GetComponent<RectTransform>().localPosition = localPos;
            }
            else
            {
                말풍선.GetComponent<RectTransform>().localPosition = new Vector2(localPos.x, localPos.y+30f);
            }


            //말풍선.GetComponent<RectTransform>().anchoredPosition = localPos;
        }       
    }
    //말풍선 비활성화 시켜주기
    public void DeActivate말풍선()
    {
        if (말풍선.activeSelf == true)
        {
            말풍선.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            말풍선.SetActive(false);
        }
    }
}
