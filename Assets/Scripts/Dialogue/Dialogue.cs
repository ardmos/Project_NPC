using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    public string smallTitle_;
    public int storyId;             

    [System.Serializable]
    public class DialogueSet
    {
        [System.Serializable]
        public class Details   //스타일, 좌상화, 우상화, 선택팝업, 글자속도 , 애니메이션
        {
            [Range(0f, 1f)]
            public float letterSpeed;
   
            //스타일선택
            public enum Styles
            {
                Stable,
                Slide
            }
            public Styles styles;

            //좌상화, 우상화
            [System.Serializable]
            public struct PortraitSettings
            {
                public bool isLeftPortrait;
                public int leftPortraitNumber;
                public bool isRightPortrait;
                public int rightPortraitNumber;
            }
            public PortraitSettings portraitSettings;

            //선택팝업
            public bool makeSelectionPopup;
            [System.Serializable]
            public struct SelectionData
            {
                [Header("- 선택지  Choices와 Choice_results와 responses는 반드시 짝을 이루어야 한다.")]
                public string ask;
                public string[] choices;
                [Header("- Sentence 입력 방법과 마찬가지로 입력.")]
                public DialogueSet[] choice_results;
                [Header("- Choice_results에 대한 엔피씨의 답변내용 입니다. 입력방법은 Sentence와 동일합니다.")]
                public DialogueSet[] responses;
            }
            public SelectionData selectionPopupData;

            //npc애니메이션
            [System.Serializable]
            public struct NpcAnimData
            {
                [Header("- 움직이고싶은 NPC를 드래그해서 넣어주세요")] 
                public Animation npc;
                [Header("- 할 동작의 animationClip의 이름을 입력해주세요")]
                public string animationClipName;
            }
            public bool startNpcAnimate;
            public NpcAnimData[] npcAnimationData;
            
        }

        public string smallTitle_;
        public Details details;
        public string name;
        [TextArea(8, 10)]
        public string sentence;    
        


    }
   
    public DialogueSet[] dialogueSet;

    [Header("- 초상화")]
    public Sprite[] portraits;

}
