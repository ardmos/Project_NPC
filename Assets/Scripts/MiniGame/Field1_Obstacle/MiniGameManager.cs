﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public Image key1, key2;
    public Sprite key, key_hole;

    public void IGotKey1()
    {
        PlayerStat.instance.isGotkey1 = true;
        key1.sprite = key;
    }
    public void IGotKey2()
    {
        PlayerStat.instance.isGotkey2 = true;
        key2.sprite = key;
    }
}
