using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerBindScript : MonoBehaviour
{
    GameController gameController;
    [SerializeField] private GameObject[] playersObj; // シーン上のP1〜P4

    [SerializeField]
    WaitScript waitScript;

    int slotNum = 0;
    [SerializeField, Header("必要人数")]
    int num = 3;

    void Start()
    {
        gameController = GetComponent<GameController>();
    }

    public void OnPlayerJoined(PlayerInput joined)
    {
        if (!gameController.isOpen)
        {
            // 仮オブジェクトを削除
            Destroy(joined.gameObject);
            return;
        }

        // 仮オブジェクトかどうか判定
        if (joined.gameObject.tag.StartsWith("P"))
        {
            Debug.Log("本物のプレイヤーに割り当て済みなのでスキップ");
            return;
        }

        Debug.Log($"Player {slotNum} joined");

        // 対応する本物プレイヤーのPlayerInput
        var player = playersObj[slotNum].GetComponent<PlayerInput>();

        playersObj[slotNum].SetActive(true);

        // 仮オブジェクトのデバイスを本物に移す
        player.SwitchCurrentControlScheme(joined.devices.ToArray());

        // 仮オブジェクトを削除
        Destroy(joined.gameObject);

        waitScript.ChangeUI(slotNum);

        if (slotNum == num)
        {
            StartCoroutine(gameController.GameStart());
        }
        slotNum++;
    }
}


