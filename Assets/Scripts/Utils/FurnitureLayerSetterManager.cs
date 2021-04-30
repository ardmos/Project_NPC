using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureLayerSetterManager : MonoBehaviour
{
    //지금 작동중인 레이어세터가 있는지 확인 가능한 List 형태의 변수 존재.
    //작동중이면 쪽지 붙이는 식으로.  뗄 땐  false라고 적어둬야함. 
    //그래서 List의 길이가 0이거나 어쨌든 True인 애가 없으면 작동중인 레이어세터가 없는것. 

    public List<bool> noteBoard;

    private void Start()
    {
        //기본 쪽찌 한 장.  0번 자리에 붙어있는녀석. 묵음.
        noteBoard.Add(false);
    }


    //쪽지를 붙이면, 몇 번 자리에 붙였는지 번호표를 돌려줌. 번호표 잘 간직하세요~!  번호표 관리는 각자가! 
    public int AddNoteOnBoard()
    {
        int n = noteBoard.Count;
        noteBoard.Add(true);
        return n;
    }

    public void MakeTrueOnBoard(int n)
    {
        noteBoard[n] = true;
    }
    public void MakeFalseOnBoard(int n)
    {
        noteBoard[n] = false;
    }

    public bool IsThereTrueValueOnBoard()
    {
        return noteBoard.Find(argue => argue == true);
    }
}
