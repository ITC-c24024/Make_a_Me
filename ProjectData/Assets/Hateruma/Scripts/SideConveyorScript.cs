using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideConveyorScript : MonoBehaviour
{

    [SerializeField, Header("開始位置")]
    public Vector3 startPos;

    [SerializeField, Header("終了位置")]
    public Vector3 finishPos;

    [SerializeField, Header("クローンオブジェクト")]
    public List<ScrollObjScript> cloneSC;

    [SerializeField, Header("スクロール速度")]
    public float scrollSpeed;

    public virtual void AddClone(ScrollObjScript scrollObj, int playerNum)
    {

    }
}
