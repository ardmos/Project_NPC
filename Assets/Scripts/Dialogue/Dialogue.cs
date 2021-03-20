﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Dialogue
{
    [Header("- ↑ \'대화묶음\'의 제목")]
    public string smallTitle_;
    [Header("- 대화묶음을 갖게될 오브젝트의 id값을 정해주세요.")]
    public int storyId;

    [Serializable]
    public class DialogueSet
    {
        [Serializable]
        public class Details   //스타일, 좌상화, 우상화, 선택팝업, 글자속도 , 애니메이션
        {
            [Header("- 다이얼로그 시작 딜레이 타임(초)")]
            public float delayTime;

            [Range(0f, 1f), Header("- Object 대사 출력 속도 (기본 0.92)")]
            public float letterSpeed;

            //스타일선택
            public enum Styles
            {
                Stable,
                Slide
            }
            [Header("- 대화창의 등장 방식")]
            public Styles styles;


            //초상화 설정 
            //김탐정, 천지현, 백강, 베라, 플라키, 청마, 대학생4, 대학생5, 오상식, 흰강아지수인, 흰상아지수인_후드, 경찰1, 경찰2
            public enum Portraits
            {
                김탐정,
                천지현,
                백강,
                베라,
                플라키,
                청마,
                대학생4,
                대학생5,
                오상식,
                흰강아지수인,
                흰강아지수인_후드,
                경찰1,
                경찰2
            }
            //좌상화, 우상화
            [Serializable]
            public struct PortraitSettings
            {
                [Header("- 좌측 초상화              (체크박스에 체크 후, 원하는 초상화 저장소의 번호를 입력하면 초상화가 출력됩니다.)")]
                public bool showLeftPortrait;
                public Portraits leftPortraitNumber;
                [Header("- 우측 초상화")]
                public bool showRightPortrait;
                public Portraits rightPortraitNumber;
            }
            [Header("- 초상화 설정"), Space(5)]
            public PortraitSettings portraitSettings;



            //선택창팝업
            [Serializable]
            public struct Choices
            {
                [Header("- 선택지 문장")]
                public string sentence;
                [Header("- 선택 결과로 보여줄 스토리 다이얼로그의 ID")]
                public int linkedStoryDialogueIdNumber;
                [Header("- 이 선택지를 정답처리 하려면 체크")]
                public bool isItCorrectAnswer;
            }
            [Serializable]
            public struct SelectionPopupSettings
            {
                [Header("- 선택대화창(팝업) 활성화"), Space(5)]
                //선택팝업
                public bool activateSelectionPopup;
                [Serializable]
                public struct SelectionData
                {
                    [Header("- 선택팝업창 상단의 질문")]
                    public string question;
                    [Header("- 보기(최대 8개)")]                    
                    public Choices[] choices;
                }
                [Header("- 선택대화창이 갖게될 데이터를 입력하는 곳")]
                public SelectionData selectionPopupData;
            }
            [Header("- 선택대화창 설정"), Space(5)]
            public SelectionPopupSettings selectionPopupSettings;


            //이동 애니메이션 세팅
            [Serializable]
            public struct AnimationSettings
            {
                //npc애니메이션
                [Serializable]
                public struct ObjectAnimData
                {
                    [Header("- 움직이고싶은 Object를 드래그해서 넣어주세요")]
                    public GameObject objToMakeMove;

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
                    [Serializable]
                    public struct MoveSet
                    {
                        [Header("- 이 이동이 시작하기 전에 주고싶은 딜레이(초)")]
                        public float delayTime;
                        [Header("- 이동할 방향")]
                        public MoveDir dir;
                        [Header("- 이동할 거리(m)")]
                        public float distance;

                    }

                    [Header("- 총 이동 횟수")]
                    public MoveSet[] moveSet;

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
                [Header("- 이동 애니메이션 활성화"), Space(5)]
                public bool activateObjAnimate;
                [Header("- 활성화시킬 이동 정보를 넣는 곳")]
                public ObjectAnimData[] objectAnimationData;
            }
            [Header("- 이동 애니메이션 설정"), Space(5)]
            public AnimationSettings oldAnimationSettings;
            //이동 애니메이션 세팅 - New (Follow Ver.)
            [Serializable]
            public struct NewAnimationSettings
            {
                public bool activeNewAnimate;
                public Vector2 destinationPos;
                public GameObject objToMove;
                public enum EndDir
                {
                    Down,
                    Up,
                    Right,
                    Left
                }
                [Header("- 정지시 쳐다볼 방향")]
                public EndDir endDir;
            }
            public NewAnimationSettings newAnimationSettings;

            //이동 제외 기타 애니메이션 세팅 
            [Serializable]
            public struct EtcAnimationSettings
            {
                [Serializable]
                public struct EtcAnimSet
                {
                    public enum TOTO
                    {
                        실행,
                        정지
                    }
                    [Header("- 실행/정지 설정"), Space(5)]
                    public TOTO activateOrDeActiveObjAnimate;

                    [Serializable]
                    public struct MyStruct
                    {
                        //오브젝트
                        [Header("- Object를 드래그해서 넣어주세요")]
                        public Animator obj;
                        //Bool 파라미터 이름
                        [Header("- 파라미터의 이름을 넣어주세요")]
                        public string paramName;
                    }
                    [Header("- 정보를 넣는 곳")]
                    public MyStruct objectAnimationData;
                }
                [Header("- 실행/정지 시킬 애니메이션 개체의 갯수")]
                public EtcAnimSet[] etcAnimSets;
            }
            [Header("- 이동 제외 기타 애니메이션 설정"), Space(5)]
            public EtcAnimationSettings etcAnimationSettings;

            //감정표현 애니메이션 설정
            public enum Emotions
            {
                물음표_띠용,
                점점점_황당,
                빠직_화남,
                보글보글_복잡,
                느낌표_떠오름,
                느낌표_놀람,
                느낌표_엄청놀람
            }
            [Serializable]
            public struct EmotionSettings
            {
                [Header("- 감정표현을 원하면 체크해주세요")]
                public bool activeEmotion;
                [Header("- 감정표현 시키고 싶은 오브젝트를 넣어주세요")]
                public GameObject object_;
                [Header("- 발동시키고싶은 감정표현 애니메이션을 선택하세요")]
                public Emotions animation;
            }
            [Header("- 감정표현 애니메이션 설정")]
            public EmotionSettings emotionSettings;

            //효과음 설정
            [Serializable]
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
                [Header("- Sentence 없이 효과음만 재생할 경우에만, 몇 초 후에 다음 다이얼로그를 불러올지 정해주세요."), Space(5)]
                public float dialogueLifeTime;
            }
            [Header("- 사운드 효과 설정"), Space(5)]
            public SFXSettings sFXSettings;

            //이벤트 컷신 설정
            [Serializable]
            public struct CutSceneSettings
            {
                [Header("- 컷씬 활성화"), Space(5)]
                public bool activateCutScene;
                [Header("- 컷씬에 사용할 이미지"), Space(5)]
                public Sprite image;
            }
            [Header("- 이벤트 컷신 설정"), Space(5)]
            public CutSceneSettings cutSceneSettings;

            //글씨 색 설정
            [Serializable]
            public struct FontColorSettings 
            {
                [Header("- 글씨 색 변경을 원하면 체크해주세요")]
                public bool changeColor;
                [Header("- 원하시는 색 선택")]
                public Color fontColor;
            }
            [Header("- 글씨 색 설정")]
            public FontColorSettings fontColorSettings;

            //글씨 크기 설정
            [Serializable]
            public struct FontSizeSettings
            {
                [Header("- 글씨 크기 변경을 원하면 체크해주세요")]
                public bool changeSize;
                [Header("- 글씨 크기 입력(기본 30)")]
                public int fontSize;
            }
            [Header("- 글씨 크기 설정")]
            public FontSizeSettings fontSizeSettings;
        }

        [Header("- ↑ \'대화\'의 제목")]
        public string smallTitle_;
        [HideInInspector]
        public string dialogueSetName;  //인스펙터상에서는 숨겨둠. 나중에 혹시 다이얼로그셋 접근하고싶으면 이걸 찾아서 접근하기. 
        [Header("- 대화 세부 설정"), Space(5)]
        public Details detail;


        //타이핑 사운드 설정
        public enum TypingSounds
        {
            One,
            Two
        }
        [Header("- 사용할 타이핑 사운드 번호. 기본 0"), Space(15)]
        public TypingSounds soundType;

        //이름 선택
        public enum Names
        {
            김탐정,
            천형사,
            경찰1,
            경찰2,
            익명,
            경비병,
            농부,
            데릭,
            뱅크,
            학생1,
            학생2,
            학생3,
            학생4,
            학생5,
            빈칸,           
            하얀개1,
            하얀개2,
            하얀개3,
            수상한개,
            GM청마,
            베라
        }
        [Header("- Object 이름"), Space(5)]
        public Names name;
        [TextArea(8, 10), Header("- Object 대사")]
        public string sentence;
    }

    [Header("- 대화묶음에 포함될 대화의 갯수를 정해주세요."), Space(10)]
    public DialogueSet[] dialogue;
}
