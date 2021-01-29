using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText, sentenceText;


    public GameObject dialog_Stable, dialog_Slide;

    Animator slide_animator;


    #region For Signleton
    //싱글턴
    public static DialogueManager instance;

    //private Queue<Dialogue> storyPack;

    private void Awake()
    {
        //싱글턴
        instance = this;
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        slide_animator = dialog_Slide.GetComponent<Animator>();
        //storyPack = new Queue<Dialogue>();
    }

    public void StartDialogue(Dialogue[] dialogueData, string dialogueStyle)
    {
        GameObject dialogueObject = new GameObject();
        switch (dialogueStyle)
        {
            case "Stable":
                dialog_Stable.SetActive(true);
                dialogueObject = dialog_Stable;
                break;
            case "Slide":
                slide_animator.SetBool("isOpen", true);
                dialogueObject = dialog_Slide;
                break;
            default:
                Debug.Log("please make sure the dialogueStyle is correct");
                return;               
        }
        //this.storyPack.Clear();

        //대화내용 통째로 가져와서 처리 storyPack[] 
        //foreach (Dialogue dialogues in storyPack)
        //{
        //    this.storyPack.Enqueue(dialogues);
       // }

        DisplayNextSentence(dialogueData, dialogueObject);
    }

    void DisplayNextSentence(Dialogue[] dialogueData, GameObject dialogueObject)
    {
        //dialogueObject.GetCh

        //셋데이타 호출. 데이타 세팅. 

        //if(this.storyPack.Count == 0)
        //{
        //    EndDialogue();
        //    return;
        //}

        //Dialogue dialogue = this.storyPack.Dequeue();
        //nameText.text = dialogue.name;
        //string sentence = dialogue.sentence;
        //StopAllCoroutines();
        //StartCoroutine(TypeSentence(sentence));
    }

    //한글자씩 도도도 찍기
    IEnumerator TypeSentence(string sentence)
    {
        sentenceText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            sentenceText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void EndDialogue()
    {
        slide_animator.SetBool("isOpen", false);
    }


}
