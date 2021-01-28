using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText, sentenceText;

    public GameObject dialogue_Slide, dialogue_Stable;
    Animator dialogue_Slide_animator;
    
    //싱글턴
    public static DialogueManager instance;

    private Queue<Dialogue> storyPack;

    private void Awake()
    {
        //싱글턴
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogue_Slide_animator = dialogue_Slide.GetComponent<Animator>();

        storyPack = new Queue<Dialogue>();
    }



    #region 다이얼로그 열고 닫기

    public void StartDialogue(Dialogue[] storyPack, string dialogueStyle)
    {

        switch (dialogueStyle)
        {
            case "Slide":

                dialogue_Slide_animator.SetBool("isOpen", true);
                break;
            case "Stable":
                dialogue_Stable.SetActive(true);
                break;
            default:
                break;
        }

        this.storyPack.Clear();

        //대화내용 통째로 가져와서 처리 storyPack[] 
        foreach (Dialogue dialogues in storyPack)
        {
            this.storyPack.Enqueue(dialogues);
        }

        DisplayNextSentence();
    }


    public void EndDialogue(string dialogueStyle)
    {
        switch (dialogueStyle)
        {
            case "Slide":
                dialogue_Slide_animator.SetBool("isOpen", false);
                break;
            case "Stable":
                dialogue_Stable.SetActive(false);
                break;
            default:
                break;
        }

    }
    #endregion

    public void DisplayNextSentence()
    {
        if (this.storyPack.Count == 0)
        {
            EndDialogue("Stable");
            return;
        }

        Dialogue dialogue = this.storyPack.Dequeue();
        nameText.text = dialogue.name;
        string sentence = dialogue.sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
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
}
