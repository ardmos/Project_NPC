using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public Image key1, key2;
    public Sprite key, key_hole;

    //플레이어 정보
    public PlayerStat playerStat;
    //플레이어 오브젝트
    public GameObject player;
    //동료 오브젝트
    public GameObject fellower;

    //방해물---
    //싱크홀
    [SerializeField]
    SinkHole sinkhole;


    //플레이어 죽음시 심장박동 애니메이션 정지를 위한.
    public Animator heartBeatAnimator;
    //게임오버까만화면효과
    public Image gameOverBlackImage;
    //게임오버팝업
    public GameObject gameOverPopup;


    #region 열쇠처리부분
    public void IGotKey1()
    {
        playerStat.isGotkey1 = true;
        key1.sprite = key;
        foreach (ParticleSystem ps in key1.gameObject.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
    }
    public void IGotKey2()
    {
        playerStat.isGotkey2 = true;
        key2.sprite = key;
        foreach (ParticleSystem ps in key2.gameObject.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
    }

    public void ResetKeyStat()
    {
        playerStat.isGotkey1 = false;
        playerStat.isGotkey2 = false;
        key1.sprite = key_hole;
        key2.sprite = key_hole;
    }
    #endregion


    public void Start()
    {
        gameOverBlackImage.enabled = false;
        gameOverPopup.SetActive(false);
    }
    //게임오버팝업 열기
    public void StartGameOverPopup()
    {
        //심장박동애니메이션 정지 (부활하면 재시작 해줘야함.)
        heartBeatAnimator.enabled = false;
        //겸사겸사 회색으로 색깔도 변경. (얘도 부활하면 원복 해줘야함.)
        heartBeatAnimator.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        //회색화면효과도. (부활하면 없애기.)
        gameOverBlackImage.enabled = true;
        //게임오버팝업도.
        gameOverPopup.SetActive(true);
    }
    //게임오버팝업 닫기
    public void EndGameoverPopup()
    {
        gameOverBlackImage.enabled = false;
        gameOverPopup.SetActive(false);
    }

    //게임재도전
    public void RestartGame()
    {        
        //플레이어 위치 초기화
        player.transform.position = new Vector2(-15f, -1.5f);
        //플레이어 일어남.  어느쪽으로 누워있는지 확인 후 일으키기
        //일어날 때 그림자도 원복시키기
        Transform playerShadowTransform = GameObject.FindWithTag("PlayerShadow").transform;
        playerShadowTransform.localPosition = new Vector3(0f, 0f, 0f);
        playerShadowTransform.localScale = new Vector3(1f, 1f, 1f);
        if (player.transform.rotation.z <= 0)
        {
            player.transform.Rotate(0f, 0f, 90f);
            playerShadowTransform.Rotate(0f, 0f, -90f);
        }
        else
        {
            player.transform.Rotate(0f, 0f, -90f);
            playerShadowTransform.Rotate(0f, 0f, 90f);
        }

        //플레이어 컨트롤권한 부여
        player.GetComponent<KeyInput_Controller>().isControllable = true;
        player.GetComponent<KeyInput_Controller>().isDied = false;
        player.GetComponent<KeyInput_Controller>().is3SecPassed = false;
        player.GetComponent<KeyInput_Controller>().isGetHit = false;   //얘 안해줘서 재도전시 리스폰 위치 버그가 있었다!!
        //플레이어 콜라이더 부활
        player.GetComponent<CircleCollider2D>().enabled = true;
        //플레이어 HP 초기화
        playerStat.hP = 10;
        //심박 부활
        FindObjectOfType<HeartBeater>().beatType = Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType2;
        //열쇠 초기화
        ResetKeyStat();
        //싱크홀 초기화
        sinkhole.GetComponent<Animator>().SetBool("sinkHoleOn", false);

        //동료 위치 초기화 (플레이어 위치 x좌표 -1.)
        Vector2 vector = new Vector2(-16.5f, -1.5f);
        fellower.transform.position = vector;
        //동료 desPos 갱신 (동료 부활하는 위치로)
        fellower.GetComponent<NPC>().desPos = new Vector2(-16.5f, -1.5f);



        //하트비팅 애니메이션 초기화
        heartBeatAnimator.enabled = true;
        heartBeatAnimator.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        //게임오버팝업 닫기
        EndGameoverPopup();
    }
}
