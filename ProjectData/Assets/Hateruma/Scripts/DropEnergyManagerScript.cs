using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnergyManagerScript : MonoBehaviour
{
    [SerializeField,Header("ドロップオブジェクト")] 
    GameObject[] dropObj = new GameObject[3];

    //ドロップオブジェクトのスクリプト
    DropEnergyScript[] dropEnergySC;

    //プレイヤーオブジェクト
    public GameObject playerObj;

    [SerializeField,Header("使用されていないドロップオブジェクトを格納するためのリスト")] 
    List<DropEnergyScript> dropList = new List<DropEnergyScript>();

    void Start()
    {
        dropEnergySC = new DropEnergyScript[dropObj.Length];//オブジェクトの数に応じて配列の範囲を指定

        playerObj = this.gameObject;

        for (int i = 0; i < dropObj.Length; i++)
        {
            dropEnergySC[i] = dropObj[i].GetComponent<DropEnergyScript>();//ドロップオブジェクトのスクリプトを取得

            dropEnergySC[i].playerObj = playerObj;//対応したプレイヤーを割り当てる

            dropEnergySC[i].dropManagerSC = this;//ドロップオブジェクトにDropManagerScriptを割り当て

            dropEnergySC[i].SetNum(i);//各オブジェクトに番号を付ける(リストに戻す際に識別するため)

            dropList.Add(dropEnergySC[i]);//ドロップオブジェクトをリストに追加
        }
    }

    /// <summary>
    /// エネルギーオブジェクトをドロップさせる
    /// </summary>
    /// <param name="amount">ドロップするエネルギーの量</param>
    public void Drop(int amount)
    {
        //リストから使用されていないオブジェクトを選んでドロップ
        if(dropList.Count > 0)
        {
            StartCoroutine(dropList[0].SetHoneyAmount(amount));
            dropList.RemoveAt(0);
        }
    }
    /// <summary>
    /// 拾われたオブジェクトをリストに加える
    /// </summary>
    /// <param name="num">オブジェクトナンバー</param>
    public void AddDrop(int num)
    {
        dropList.Add(dropEnergySC[num]);
    }
}
