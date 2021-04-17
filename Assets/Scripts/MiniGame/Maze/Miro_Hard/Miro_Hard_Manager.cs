using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miro_Hard_Manager : MonoBehaviour
{
    //상호작용 오브젝트들
    //문1, 문2, 가시, 수상한 꽃, 힐팩, 바위, 나무판1, 나무판2, 대왕거미줄
    public GameObject 문1, 문2, 가시, 수상한꽃, 힐팩, 바위, 나무판1, 나무판2, 대왕거미줄, 작업바;

    //처음엔 가시, 힐팩 비활성화시켜야함
    //문1을 열면 가시 활성화시켜야함
    //문2를 열면 힐팩 활성화시켜야함

    // Start is called before the first frame update
    void Start()
    {
        가시.SetActive(false);
        힐팩.SetActive(false);
    }

    //가시 활성화시켜주기
    public void Active가시()
    {
        가시.SetActive(true);
    }
    //힐팩 활성화시켜주기
    public void Active힐팩()
    {
        힐팩.SetActive(true);
    }
}
