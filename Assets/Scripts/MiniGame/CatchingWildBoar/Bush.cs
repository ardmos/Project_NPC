using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public bool generateSwitch;
    public GameObject animalFriendsPref;

    //public int frameCounter;
    private void Update()
    {
        //동물들 생성

        /*
        if (generateSwitch)
        {
            if (frameCounter == 200)
            {
                frameCounter = 0;
                GenerateAnimalFriends();
            }
            frameCounter++;
          
        }
        */
        if (generateSwitch)
        {
            GenerateAnimalFriends();
        }
    }



    public void GenerateAnimalFriends()
    {

        //동물친구들 생성.  
        //animalFriendsPref = Resources.Load("Prefabs/MiniGame/CatchingWildBoar/WildBoar") as GameObject;
        animalFriendsPref = Resources.Load("Prefabs/MiniGame/CatchingWildBoar/NewWildBoar") as GameObject;
        GameObject prefObj = Instantiate(animalFriendsPref) as GameObject;
        prefObj.transform.SetParent(gameObject.transform);
        prefObj.transform.localPosition = Vector3.zero;
        prefObj.GetComponent<NewWildBoar>().DecideWhereToGo();

        //한 마리씩만 생성.
        generateSwitch = false;
    }
}
