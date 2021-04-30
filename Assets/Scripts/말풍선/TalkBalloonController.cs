using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkBalloonController : MonoBehaviour
{

    //public TalkBalloonDataContainer[] talkBalloonDataContainers;
    List<TalkBalloonDataContainer> talkBalloonDataContainersList;
    Queue<Dialogue.DialogueSet> talkBalloonDatasQueue;
    //TextMeshPro textMesh;
    TextMeshProUGUI textUI;
    public GameObject balloonReal;
    public RectTransform bgspriteRectT;


    //말풍선 살짝살짝 오른쪽 왼쪽 이동시키기 위한 
    public float 살짝만큼;
    Vector2 thisPos;

    // Start is called before the first frame update
    void Start()
    {
        //크기 최소화가 기본. 
        transform.localScale = Vector3.zero;
    }

    //말풍선은 시작하면 끝날때까지 자기들끼리 쭈우우욱 진행한 다음 끝나야함. 
    //시작 - 말풍선 생성
    //한사람 말 하고 2초 후에 대화 넘어가기. 
    //끝 - 말풍선 삭제

    private void Update()
    {
        thisPos = new Vector2(transform.position.x + 살짝만큼, transform.position.y);
        //위치 지속 갱신
        balloonReal.transform.position = Camera.main.WorldToScreenPoint(thisPos);
    }



    //말풍선 시작
    public void StartTalkBalloon(int id, Queue<Dialogue.DialogueSet> dialogueSetsQue)
    {
        Debug.Log("StartTalkBalloon()");
        //말풍선 시작하면 끝날때까지 자동진행돼야함!

        //말풍선 생성!
        transform.localScale = Vector3.one;

        //받아온 queue 정보대로 출력 작업 시작하자~!
        talkBalloonDatasQueue = new Queue<Dialogue.DialogueSet>(dialogueSetsQue);

        //순서대로 하나씩 빼주자. 
        LoadNextTalkData();
    }

    public void LoadNextTalkData()
    {
        Debug.Log("LoadNextTalkData()");
        //남은게 없으면 엔드 해야지!
        if (talkBalloonDatasQueue.Count == 0)
        {
            EndTalkBalloon();
            return;
        }

        Dialogue.DialogueSet talkBalloonData = talkBalloonDatasQueue.Dequeue();

        //textMesh = new TextMeshPro();

        //설정된 왼 오 기준에 따라 텍스트오브젝트 정해서  a:를 포함하면 왼, b:를 포함하면 오
        //0이면 left고 1이면 right.  오브젝트 자식 위치 순서 바꾸면 안됨~!
        //if (talkBalloonData.offSet == TalkBalloonDataContainer.TalkBalloonData.OffSet.Left)
        if(talkBalloonData.smallTitle_.Contains("a:"))
        {
            //textMesh = gameObject.GetComponentsInChildren<TextMesh>()[0];
            textUI = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
            //textUI.alignment = TextAnchor.UpperLeft;
            textUI.alignment = TextAlignmentOptions.TopLeft;
            Debug.Log("Left");
            살짝만큼 = -0.2f;

            //textMesh.text
            //gameObject.GetComponentsInChildren<TextMeshPro>()[1].text = "";
        }
        else if (talkBalloonData.smallTitle_.Contains("b:"))
        {
            //textMesh = gameObject.GetComponentsInChildren<TextMesh>()[1];
            textUI = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
            //textUI.alignment = TextAnchor.UpperRight;
            textUI.alignment = TextAlignmentOptions.TopRight;
            Debug.Log("Right");
            살짝만큼 = +0.2f;
            //gameObject.GetComponentsInChildren<TextMeshPro>()[0].text = "";
        }
        
        Debug.Log(textUI);


        //글씨설정에 따라 샐깔, 크기 설정해주기. 
        //
        if (talkBalloonData.detail.fontColorSettings.changeColor) textUI.color = talkBalloonData.detail.fontColorSettings.fontColor;

        textUI.color = new Color(textUI.color.r, textUI.color.g, textUI.color.b, 1f);

        ///
        //글씨 잠깐 투명으로 하고 다 넣어서

        //너비 높이 재고 배경 이미지 크기 변경시켜준 다음 

        //출력 직전에 
        ///

        if (talkBalloonData.detail.fontSizeSettings.changeSize) textUI.fontSize = talkBalloonData.detail.fontSizeSettings.fontSize;
        
        //Sentence 출력 //다음 대사! 2초 뒤에! 호출!
        StartCoroutine(TypeSentence(talkBalloonData.sentence));

        //굳!
    }

    public void EndTalkBalloon()
    {
        Debug.Log("EndTalkBalloon()");
        //말풍선 닫기!
        transform.localScale = Vector3.zero;
    }



    //한글자씩 도도도 찍기
    IEnumerator TypeSentence(string sentence)
    {
        Debug.Log("TypeSentence");
        //textMesh.text = "";

        textUI.text = sentence;

        //foreach (char letter in sentence.ToCharArray())
        //{
        //textMesh.text += letter;
        //배경 이미지 크기 변경
        bgspriteRectT.sizeDelta = new Vector2(bgspriteRectT.sizeDelta.x, textUI.preferredHeight+46f);
        //배경이미지 위치 재조정        
        //bgspriteRectT.anchoredPosition = new Vector2(bgspriteRectT.anchoredPosition.x, bgspriteRectT.anchoredPosition.y - (bgspriteRectT.sizeDelta.y/2));

        //bgspriteRectT.sizeDelta = new Vector2(textMesh.preferredWidth, textMesh.preferredHeight);
        //Debug.Log("textMesh.preferredWidth:"+textMesh.preferredWidth+ ", textMesh.preferredHeight:" + textMesh.preferredHeight);

        //  yield return new WaitForSeconds(0.075f); 
        //}

        //효과음 아직 없음



        //출력 끝나고 2초 있다가 다음 문장 출력. 
        Debug.Log("출력 끝! 2초 대기 후 다음 문장 로드 시도!");
        yield return new WaitForSeconds(2f);
        LoadNextTalkData();
    }
}
