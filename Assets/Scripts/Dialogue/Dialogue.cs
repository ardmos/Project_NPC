using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue 
{
    [Header("- ↑ 이 \'Dialog 보따리\'의 제목")]
    public string smallTitle_;
    [Header("- 지금 이 대화보따리를 부여하고자 하는 Object의 id값을 입력하거나 // 스토리라인의 번호를 입력하세요.")]
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

            //선택창팝업
            [System.Serializable]
            public struct SelectionPopupSettings
            {
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
            }
            [Header("- 선택대화창 설정"), Space(5)]
            public SelectionPopupSettings selectionPopupSettings;


            //애니메이션 세팅
            [System.Serializable]
            public struct AnimationSettings
            {
                //npc애니메이션
                [System.Serializable]
                public struct ObjectAnimData
                {
                    [Header("- 움직이고싶은 Object를 드래그해서 넣어주세요")]
                    public KeyInput_Controller objToMakeMove;

                    public enum MoveDir
                    {
                        None,
                        Up,
                        Down,
                        Left,
                        Right,
                        UpRight,
                        UpLeft,
                        DownRight,
                        DownLeft
                    }
                    [Header("- 이동할 방향")]
                    public MoveDir dir;
                    //public bool up;
                    //public bool down, left, right;
                    [Header("- 이동할 거리(m)")]
                    public int distance;

                    public enum EndDir
                    {
                        Up,
                        Down,
                        Left,
                        Right
                    }
                    [Header("- 정지시 쳐다볼 방향")]
                    public EndDir endDir;
                }
                [Header("- NPC 애니메이션 활성화"), Space(5)]
                public bool activateObjAnimate;
                [Header("- 활성화시킬 애니메이션의 데이터를 넣는 곳")]
                public ObjectAnimData[] objectAnimationData;
            }
            [Header("- 애니메이션 설정"), Space(5)]
            public AnimationSettings animationSettings;

            //효과음 설정
            [System.Serializable]
            public struct SFXSettings
            {
                [Header("- 타이핑사운드 끄려면! 체크"), Space(5)]
                public bool turnOffTypingSound;
                [Header("- 효과음 넣으려면 체크"), Space(5)]
                public bool enableSFX;
                [Header("- 사용할 효과음을 넣어주세요"), Space(5)]
                public AudioClip audioClip;
                [Header("- 재생할 횟수"), Space(5)]
                public int playTime;
                [Header("- 반복 사이 시간"), Space(5)]
                public float delayTime;
            }
            [Header("- 사운드 효과 설정"), Space(5)]
            public SFXSettings sFXSettings;

            //이벤트 컷신 설정
            [System.Serializable]
            public struct CutSceneSettings
            {
                [Header("- 컷씬 활성화"), Space(5)]
                public bool activateCutScene;
                [Header("- 컷씬에 사용할 이미지"), Space(5)]
                public Sprite image;
            }
            [Header("- 이벤트 컷신 설정"), Space(5)]
            public CutSceneSettings cutSceneSettings;
        }

        [Header("- ↑ 이 \'Dialog\'의 제목")]
        public string smallTitle_;
        [Header("- \'Dialog\'세부 설정"), Space(5)]
        public Details detail;

        [Header("- 사용할 타이핑 사운드 번호. 기본 0"), Space(15)]
        public int soundNumber;
        [Header("- Object 이름"), Space(5)]
        public string name;
        [TextArea(8, 10), Header("- Object 대사")]
        public string sentence;           
    }

    [Header("- 현 \'Dialog보따리\'에 포함될 \'Dialog\'(대화)의 갯수를 입력해주세요."), Space(10)]
    public DialogueSet[] dialogue;
}
