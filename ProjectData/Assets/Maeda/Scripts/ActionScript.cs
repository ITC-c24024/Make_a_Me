using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class ActionScript : MonoBehaviour
{
    //ロボットオブジェクト
    public GameObject robot;
    //バッテリー追従オブジェクト
    public GameObject handObj;

    //入力制限
    public bool isTimer = false;

    //移動アクション
    public InputAction moveAction;
    //取る、投げるアクション
    public InputAction throwAction;

    private void Awake()
    {
        //ActionMapを取得
        var input = robot.GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //対応するアクションを取得
        moveAction = actionMap["Move"];
        throwAction = actionMap["Throw"];
    }

    /// <summary>
    /// 入力クールタイム
    /// </summary>
    /// <returns></returns>
    public IEnumerator PickupDelay()
    {
        isTimer = true;
        yield return new WaitForSeconds(0.2f); 
        isTimer = false;
    }
}
