using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText, sentenceText;

    public Animator animator;
    
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

        storyPack = new Queue<Dialogue>();
    }

    public void StartDialogue(Dialogue[] storyPack)
    {
        animator.SetBool("isOpen", true);

        this.storyPack.Clear();

        //대화내용 통째로 가져와서 처리 storyPack[] 
        foreach (Dialogue dialogues in storyPack)
        {
            this.storyPack.Enqueue(dialogues);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(this.storyPack.Count == 0)
        {
            EndDialogue();
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

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }


}
