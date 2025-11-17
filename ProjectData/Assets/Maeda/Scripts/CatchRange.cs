using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatchRange : ActionScript
{
    EnergyBatteryScript batteryScript;
    PlayerController playerController;
    [SerializeField] ScoreScript scoreScript;

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
            ChatchBattery();
        }
    }

    void ChatchBattery()
    {
        Debug.Log("Catch");
        if (batteryScript != null)
        {
            batteryScript.ChangeOwner(playerController.playerNum, handObj);
            canTake = false;
            playerController.ChangeBatterySC(batteryScript);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery"))
        {           
            var energyBatterySC= other.gameObject.GetComponent<EnergyBatteryScript>();
            //バッテリーが投げられているなら
            if (energyBatterySC.bombSwitch)
            {
                canTake = true;
                batteryScript = energyBatterySC;
            }             
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery"))
        {            
            canTake = false;
            batteryScript = null;           
        }
    }
}
