using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeRange : ActionScript
{
    [SerializeField]
    ScoreScript scoreScript;
    EnergyBatteryScript batteryScript;
    PlayerController playerController;
    public PlayerController[] playerControllers = new PlayerController[4];    

    //取れる判定
    public bool canTake = false;

    void Start()
    {
        playerController = robot.GetComponent<PlayerController>();       
    }

    void Update()
    {
        var takeAct = throwAction.triggered;
        if (!playerController.haveBattery && takeAct && canTake && !isTimer && !playerController.isStun && !playerController.invincible && !scoreScript.isWork)
        {
            TakeBattery();
        }
    }

    /// <summary>
    /// インスタンス化したPlayerのPlayerControllerを取得
    /// </summary>
    /// <param name="i">配列の要素数をGameControllerから指定</param>
    /// <param name="pc">格納するPlayerControllerをGameControllerから指定</param>
    public void SetPlayerSC(int i,PlayerController pc)
    {
        playerControllers[i] = pc;
    }

    /// <summary>
    /// バッテリー取得処理
    /// </summary>
    void TakeBattery()
    {
        if (batteryScript != null)
        {
            var ownerNum = batteryScript.OwnerCheck();
            
            //バッテリーの所持者がいるとき
            if (ownerNum != 0)
            {
                //奪った相手の所持判定をfalse
                playerControllers[ownerNum - 1].ChangeHaveBattery(false);
            }
            //バッテリー所持者を自分にする
            canTake = false;
            batteryScript.ChangeOwner(playerController.playerNum, handObj);
            playerController.ChangeBatterySC(batteryScript);
        }   
    }

    private void OnTriggerStay(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery") && !scoreScript.isWork)
        {
            var energyBatterySC = other.gameObject.GetComponent<EnergyBatteryScript>();
            if (!energyBatterySC.bombSwitch)
            {
                canTake = true;
                batteryScript = other.gameObject.GetComponent<EnergyBatteryScript>();
            }                      
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery"))
        {
            canTake = false;               
        } 
    }
}
