using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public bool generateSwitch;
    public GameObject animalFriendsPref;

    public int frameCounter, genTiming;

    private void Update()
    {
        //동물들 생성
        if (generateSwitch)
        {
            if (frameCounter == genTiming)
            {
                frameCounter = 0;
                GenerateAnimalFriends();
            }            
            frameCounter++;
        }
    }

    public void GenerateAnimalFriends()
    {

        //동물친구들 생성.  
        animalFriendsPref = Resources.Load("Prefabs/MiniGame/CatchingWildBoar/WildBoar") as GameObject;
        GameObject prefObj = Instantiate(animalFriendsPref) as GameObject;
        prefObj.transform.SetParent(gameObject.transform);
        prefObj.transform.localPosition = Vector3.zero;
    }

    public void StopGenerating()
    {
        //생성 멈추기
        generateSwitch = false;
    }
}
