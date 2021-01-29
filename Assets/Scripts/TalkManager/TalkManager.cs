using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[System.Serializable]
//public class IntString : SerializableDictionary<int, string> { }
public class TalkManager : MonoBehaviour
{
    //public IntString talks;

    [System.Serializable]
    public struct MyStruct
    {
        public int id;
        public string name;
        [TextArea(8, 10)]
        public string[] vs;
    }

    public MyStruct[] addHere;

    //public Dictionary<int,string[]> keyValuePairs;

    public Dictionary<int, MyStruct[]> keyValuePairs;


    private void Awake()
    {
        keyValuePairs = new Dictionary<int, MyStruct[]>();
    }

    // Start is called before the first frame update
    void Start()
    {

        
        /*
        for (int i = 0; i < addHere.Length; i++)
        {

            keyValuePairs.Add(addHere[i].id, addHere);
        }


        foreach (KeyValuePair<int, MyStruct[]> item in keyValuePairs)
        {
            foreach (MyStruct str in item.Value)
            {
                print(item.Key + " ,  " + str.id + ", " + str.name + ", ");

                foreach (string s in str.vs)
                {
                    print(s);
                }
            }
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
