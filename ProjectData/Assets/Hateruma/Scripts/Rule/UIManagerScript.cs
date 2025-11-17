using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class UIManagerScript : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public InputAction selectAction;
    public InputAction decisionAction;

    public bool isCoolTime;

    private void Awake()
    {
        var actionMap = inputActions.FindActionMap("UI"); // UI—p‚ÌActionMap–¼
        selectAction = actionMap.FindAction("Select");
        decisionAction = actionMap.FindAction("Decision");

        selectAction.Enable();
        decisionAction.Enable();
    }

    public IEnumerator SelectCoolTime()
    {
        if (isCoolTime) yield break;

        isCoolTime = true;
        yield return new WaitForSeconds(0.2f);
        isCoolTime = false;
    }
}


