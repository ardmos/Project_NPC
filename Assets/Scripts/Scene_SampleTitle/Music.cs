using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : DontDestroy<Music>
{
    protected override void OnStart()
    {
        base.OnStart();

        print("happy project! gilsang!");
    }
}
