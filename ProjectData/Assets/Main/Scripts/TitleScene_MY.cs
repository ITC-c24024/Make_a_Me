using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]

public class TitleScene_MY : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;
    [SerializeField]
    ShutterScript shutterScript;

    bool input = false;

    InputAction decideAction;

    void Start()
    {
        audioManager.Title();

        //ActionMap‚ðŽæ“¾
        var input = GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //‘Î‰ž‚·‚éƒAƒNƒVƒ‡ƒ“‚ðŽæ“¾
        decideAction = actionMap["Throw"];
    }

    void Update()
    {
        var decide = decideAction.triggered;
        if (decide && !input)
        {
            input = true;

            audioManager.TitleStop();
            StartCoroutine(shutterScript.CloseShutter());
            Invoke("MainScene", 2.5f);
        }
    }

    void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
