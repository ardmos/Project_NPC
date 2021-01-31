using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue 
{
    [Header("- \'Dialog 보따리\'의 제목")]
    public string smallTitle_;
    [Header("- 지금 이 대화보따리를 부여하고자 하는 NPC or Object의 id값을 입력해주세요.")]
    public int storyId;             

    [System.Serializable]
    public class DialogueSet
    {
        [System.Serializable]
        public class Details   //스타일, 좌상화, 우상화, 선택팝업, 글자속도 , 애니메이션
        {
            [Range(0f, 1f), Header("- 대사 출력 속도")]
            public float letterSpeed;
   
            //스타일선택
            public enum Styles
            {
                Stable,
                Slide
            }
            [Header("- \'Dialog\'의 등장 방식")]
            public Styles styles;

            //좌상화, 우상화
            [System.Serializable]
            public struct PortraitSettings
            {
                [Header("- 좌측 초상화              (체크박스에 체크 후, 원하는 초상화 저장소의 번호를 입력하면 초상화가 출력됩니다.)")]
                public bool showLeftPortrait;
                public int leftPortraitNumber;
                [Header("- 우측 초상화")]
                public bool showRightPortrait;
                public int rightPortraitNumber;
            }
            [Header("- 초상화 설정"), Space(5)]
            public PortraitSettings portraitSettings;

            [Header("- 선택대화창 활성화"), Space(5)]
            //선택팝업
            public bool activateSelectionPopup;
            [System.Serializable]
            public struct SelectionData
            {
                [Header("- 선택팝업창 상단의 질문")]
                public string question;
                [Header("- 보기(최대 8개)                     주의!  Choices와 Choice_results와 Responses의 size는 반드시 같아야 한다.")]
                public string[] choices;
                [Header("- 보기(Choices)중 하나가 선택됐을시 출력되는 Dialog _ 빈 칸 가능")]
                public DialogueSet[] choice_results;
                [Header("- Choice_results의 뒤를 이어 출력되는 Dialog _ 빈 칸 가능")]
                public DialogueSet[] responses;
            }
            [Header("- 선택대화창이 갖게될 데이터를 입력하는 곳")]
            public SelectionData selectionPopupData;

            //npc애니메이션
            [System.Serializable]
            public struct NpcAnimData
            {
                [Header("- 움직이고싶은 NPC를 드래그해서 넣어주세요")] 
                public Animation npc;
                [Header("- 실행시킬 animation의 이름을 입력해주세요.")]
                public string animationName;
            }
            [Header("- NPC 애니메이션 활성화"), Space(5)]
            public bool activateNpcAnimate;
            [Header("- 활성화시킬 애니메이션의 데이터를 넣는 곳")]
            public NpcAnimData[] npcAnimationData;
            
        }

        [Header("- \'Dialog\'의 제목")]
        public string smallTitle_;
        [Header("- \'Dialog\'세부 설정"), Space(5)]
        public Details detail;
        [Header("- NPC 이름"), Space(15)]
        public string name;
        [TextArea(8, 10), Header("- NPC 대사")]
        public string sentence;           
    }

    [Header("- 현 \'Dialog보따리\'에 포함될 \'Dialog\'(대화)의 갯수를 입력해주세요."), Space(10)]
    public DialogueSet[] dialogue;
}
