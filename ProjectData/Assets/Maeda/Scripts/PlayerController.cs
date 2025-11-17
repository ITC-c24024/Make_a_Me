using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : ActionScript
{
    [SerializeField]
    GameController gameController;
    [SerializeField]
    CharaBlinkScript charaBlinkScript;
    [SerializeField]
    ScoreScript scoreScript;
    [SerializeField]
    TakeRange takeRangeSC;
    [SerializeField]
    GameObject takeRange;
    [SerializeField]
    CatchRange catchRangeSC;
    [SerializeField]
    GameObject catchRange;
    EnergyBatteryScript batteryScript;
    //エネルギー管理スクリプト
    EnergyScript energyScript;

    Vector3 takePos;
    Vector3 takeRot;
    Vector3 catchPos;
    Vector3 catchRot;

    //プレイヤーの番号
    public int playerNum = 0;
    
    [SerializeField,Header("プレイヤーの移動速度")]
    float MoveSpeed = 1.0f;
    [SerializeField, Header("振り向き速度")]
    float rotateSpeed = 720f;

    //バッテリーの所持判定
    public bool haveBattery = false;

    [SerializeField, Header("スタン効果時間")]
    float stanTime = 2.0f;
    //スタン判定
    public bool isStun = false;
    [SerializeField, Header("無敵時間")]
    float invincibleTime = 1.0f;
    //無敵判定
    public bool invincible = false;

    Animator animator;

    void Start()
    {
        takePos = takeRange.transform.localPosition;
        takeRot = takeRange.transform.localEulerAngles;
        catchPos = catchRange.transform.localPosition;
        catchRot = catchRange.transform.localEulerAngles;

        energyScript = GetComponent<EnergyScript>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!isStun && !gameController.isFinish)
        {
            //入力値をVector2型で取得
            Vector2 move = moveAction.ReadValue<Vector2>();

            if ((move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1) /*&& !scoreScript.isArea*/)
            {
                //スティックの角度を計算
                //float angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
                //プレイヤーを徐々に回転
                Quaternion to = Quaternion.LookRotation(new Vector3(move.x, 0, move.y));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, to, rotateSpeed * Time.deltaTime);
            }

            if (!gameController.isStart)
            {
                move = Vector2.zero;
            }
            if (move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1)
            {
                animator.SetBool("Iswalk", true);

                //プレイヤーを移動
                transform.position += new Vector3(move.x, 0, move.y) * MoveSpeed * Time.deltaTime;
            }
            else
            {
                animator.SetBool("Iswalk", false);
            }
        }
        else animator.SetBool("Iswalk", false);

        //ボタンを押した判定
        var throwAct = throwAction.triggered;
        if (haveBattery && throwAct && !isTimer && !isStun)
        {
            ChangeHaveBattery(false);
            StartCoroutine(takeRangeSC.PickupDelay());
            StartCoroutine(catchRangeSC.PickupDelay());
            
            batteryScript.Throw();
            batteryScript = null;
            animator.SetBool("IsThrow", true);
            animator.SetBool("IsThrow", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Discharge") && !isStun && !invincible && !gameController.isFinish)
        {
            StartCoroutine(Stan());
        }
    }

    public void Drop()
    {
        if (batteryScript != null)
        {
            ChangeHaveBattery(false);
            batteryScript.Drop();
            batteryScript = null;
        }
    }

    /// <summary>
    /// バッテリースクリプトを指定
    /// </summary>
    /// <param name="batterySC">TakeRangeで取ったバッテリーのスクリプト</param>
    public void ChangeBatterySC(EnergyBatteryScript batterySC)
    {
        ChangeHaveBattery(true);
        batteryScript = batterySC;

        StartCoroutine(PickupDelay());
    }

    /// <summary>
    /// バッテリー所持判定を切り替え
    /// チャージ判定を切り替え
    /// </summary>
    /// <param name="have">所持判定に代入するbool</param>
    public void ChangeHaveBattery(bool have)
    {
        haveBattery = have;
        animator.SetBool("IsHave", haveBattery);
        
        energyScript.ChargeSwitch(haveBattery);
    }

    /// <summary>
    /// スタン処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Stan()
    {
        //エネルギードロップ
        energyScript.LostEnergy();

        invincible = true;
        

        isStun = true;
        animator.SetBool("Isstun", true);
        
        ChangeHaveBattery(false);
        batteryScript = null;

        yield return new WaitForSeconds(stanTime);

        isStun = false;
        animator.SetBool("Isstun", false);

        charaBlinkScript.BlinkStart(true);

        transform.localRotation = Quaternion.Euler(
            0, 
            transform.localEulerAngles.y, 
            transform.localEulerAngles.z
            );
        takeRange.transform.localPosition = takePos;
        takeRange.transform.localEulerAngles = takeRot;
        catchRange.transform.localPosition = catchPos;
        catchRange.transform.localEulerAngles = catchRot;

        yield return new WaitForSeconds(invincibleTime);
        charaBlinkScript.BlinkStart(false);
        invincible = false;
    }

    public void JobAnim(bool isWalk)
    {
        if (animator != null)
        {
            animator.SetBool("IsJob", isWalk);
        }
    }
}