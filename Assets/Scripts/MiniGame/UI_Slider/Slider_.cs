using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_ : MonoBehaviour
{
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value <= 0f)
        {
            slider.value = 0f;
            gameObject.transform.Find("Fill Area").gameObject.SetActive(false);
        }
        else gameObject.transform.Find("Fill Area").gameObject.SetActive(true);
    }
}
