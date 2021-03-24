using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public Image key1, key2;
    public Sprite key, key_hole;

    public bool gotKey1, gotKey2;

    public void IGotKey1()
    {
        gotKey1 = true;
        key1.sprite = key;
    }
    public void IGotKey2()
    {
        gotKey2 = true;
        key2.sprite = key;
    }
}
