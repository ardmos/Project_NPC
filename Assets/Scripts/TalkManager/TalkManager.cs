using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//public class IntDialogues : SerializableDictionary<int, Dialogue> { }
public class IntStrings : SerializableDictionary<int, string[]> { }

public class TalkManager : MonoBehaviour
{
    [Header("---전체 이야기 보따리---    (+ 추가, - 제거)")]
    public IntStrings talkData;


    private void Awake()
    {
        //talkData = new Dictionary<int, Dialogue[]>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //talkData.Add(12, new Dialogue("oh"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
