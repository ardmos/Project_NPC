using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public Image key1, key2;
    public Sprite key, key_hole;

    public PlayerStat playerStat;

    public void IGotKey1()
    {
        playerStat.isGotkey1 = true;
        key1.sprite = key;
        foreach(ParticleSystem ps in key1.gameObject.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
    }
    public void IGotKey2()
    {
        playerStat.isGotkey2 = true;
        key2.sprite = key;
        foreach (ParticleSystem ps in key2.gameObject.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
    }
}
