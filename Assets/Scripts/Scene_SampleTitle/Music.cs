using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021.10.22 다시 작업을 시작하며.  
//타이틀 브금오브젝트를 왜 계속 살려뒀지?? 싶어서 씬 넘어가면 꺼지도록 만든다.
/*
public class Music : DontDestroy<Music>
{
    protected override void OnStart()
    {
        base.OnStart();

        print("happy project! gilsang!");
    }
}
*/

public class Music : MonoBehaviour
{
    private void Start()
    {
        print("happy project! gilsang!");
    }
}
