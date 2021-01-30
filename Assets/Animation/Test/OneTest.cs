using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTest : MonoBehaviour
{
    public void StartAnim(string animationClipName)
    { 
        gameObject.GetComponent<Animation>().Play(animationClipName);
    }
}
