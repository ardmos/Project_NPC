using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public bool generateSwitch;
    public GameObject animalFriendsPref;
    public string[] prefabNames;

    public int frameCounter;
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
            StartCoroutine(GenerateAnimalFriends());
        }
        
    }


    IEnumerator GenerateAnimalFriends()
    {
        gameObject.GetComponent<Animator>().SetTrigger("IsGrassMoving");

        //한 마리씩만 생성.
        generateSwitch = false;

        yield return new WaitForSeconds(0.5f);

        prefabNames = new string[] { "NewWildBoar", "NewWildBoar", "NewWildBoar", "NewWildBoar", "NewWildBoar", "NewWildBoar", "GoldenWildBoar", "Squirrel", "Squirrel", "Squirrel" };

        //동물친구들 생성.  
        //animalFriendsPref = Resources.Load("Prefabs/MiniGame/CatchingWildBoar/WildBoar") as GameObject;
        //animalFriendsPref = Resources.Load("Prefabs/MiniGame/CatchingWildBoar/NewWildBoar") as GameObject;
        animalFriendsPref = Resources.Load("Prefabs/MiniGame/CatchingWildBoar/" + prefabNames[Random.Range(0, prefabNames.Length)]) as GameObject;
        GameObject prefObj = Instantiate(animalFriendsPref);
        prefObj.transform.SetParent(gameObject.transform);
        prefObj.transform.localPosition = Vector3.zero;
        prefObj.GetComponent<NewWildBoar>().DecideWhereToGo();


    }

}
