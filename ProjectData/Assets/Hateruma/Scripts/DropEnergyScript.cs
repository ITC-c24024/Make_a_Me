using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnergyScript : MonoBehaviour
{
    //エネルギーの量
    public int dropEnergyAmount;

    //オブジェクトナンバー
    int objNum;

    [SerializeField, Header("スタートのポジション")]
    Vector3 startPos;

    //ドロップオブジェクトに対応するプレイヤーを紐づけ
    public GameObject playerObj;

    //拾ったプレイヤーを紐づけ
    GameObject pickPlayer;

    [SerializeField, Header("ドロップにかかる時間")]
    float dropTime = 1f;

    [SerializeField, Header("ドロップオブジェクトがリセットされるまでの時間")]
    int deleteTime;

    [SerializeField, Header("取得可能な状態かどうかの判定")]
    bool drop;

    [SerializeField, Header("//ドロップ中かどうかの判定")]
    bool start;

    //ドロップ、取得どちらなのかの判定
    bool isDrop;

    //現在地点
    Vector3 currentPos;

    //ターゲット地点
    Vector3 targetPos;

    //ドロップ中の時間管理
    float currentTime = 0f;

    //Y軸の動きを制御する
    AnimationCurve animCurveY;

    //加速度を調整する
    public AnimationCurve animCurveDrop;
    public AnimationCurve animCurvePick;

    //ドロップマネージャースクリプト
    public DropEnergyManagerScript dropManagerSC;

    //点滅処理のスクリプト
    BlinkScript blinkSC;

    private void Start()
    {
        blinkSC = this.gameObject.GetComponent<BlinkScript>();
    }
    private void Update()
    {
        //ドロップ時の挙動処理
        if (start)
        {
            //拾う際のターゲットをプレイヤーにする
            if (!isDrop)
            {
                targetPos = pickPlayer.transform.localPosition;
            }

            var pos = new Vector3();

            pos.x = currentPos.x + (targetPos.x - currentPos.x) * currentTime / dropTime;
            pos.z = currentPos.z + (targetPos.z - currentPos.z) * currentTime / dropTime;

            pos.y = animCurveY.Evaluate(currentTime);

            transform.localPosition = pos;

            //加速度をアニメーションカーブで調整
            if (isDrop)
            {
                currentTime += Time.deltaTime * animCurveDrop.Evaluate(currentTime);
            }
            else
            {
                currentTime += Time.deltaTime * animCurvePick.Evaluate(currentTime);
            }

            //完全に落ちていなくても取得できるようにする
            if (currentTime >= dropTime / 2 && isDrop)
            {
                drop = true;
            }

            if (currentTime >= dropTime)
            {
                start = false;
                currentTime = 0f;
            }
        }
    }

    /// <summary>
    /// DropEnergyManager側からオブジェクトの番号を割り当てる
    /// </summary>
    /// <param name="num">番号</param>
    public void SetNum(int num)
    {
        objNum = num;
    }

    /// <summary>
    /// EnergyScriptからドロップ分のエネルギーを引継ぎ、ドロップさせる
    /// </summary>
    /// <param name="amount">エネルギーの量</param>
    public IEnumerator SetHoneyAmount(int amount)
    {
        // 点滅が動いていたら止める
        if (blinkSC != null)
        {
            blinkSC.StopBlink();
        }

        dropTime = 1f;

        dropEnergyAmount = amount;

        gameObject.transform.localScale = new Vector3(0.25f + (0.002f * amount), 0.25f + (0.002f * amount), 0.25f + (0.002f * amount));

        //プレイヤーのポジションでドロップ
        currentPos = playerObj.transform.position;

        //ドロップ地点から半径3の円周上のランダムな地点にターゲットを指定
        var angle = Random.Range(0, 360);
        var rad = angle * Mathf.Deg2Rad;
        var px = Mathf.Cos(rad) * 5f + currentPos.x;
        var pz = Mathf.Sin(rad) * 5f + currentPos.z;

        //ターゲット地点がステージ外にならないようにする
        if (px <= -15.22f)//X軸
        {
            px = -15.22f;
        }
        else if (px >= 15.23f)
        {
            px = 15.23f;
        }

        if (pz <= -12.98f)//Z軸
        {
            pz = -12.98f;
        }
        else if (pz >= 0.43f)
        {
            pz = 0.43f;
        }
        targetPos = new Vector3(px, 0.5f, pz);


        //山なりにドロップするようにする
        animCurveY = new AnimationCurve(
            new Keyframe(0, currentPos.y, 0, 10),
            new Keyframe(dropTime, targetPos.y, -10, 0)
            );


        start = true;
        isDrop = true;

        //ドロップ後五秒待って点滅させる
        yield return new WaitForSeconds(dropTime + 5f);
        blinkSC.BlinkStart(5, 0.3f, 0.1f);

        //点滅処理が終わると同時にリセットさせる
        yield return new WaitForSeconds(5);
        PosReset();
        drop = false;
        dropManagerSC.AddDrop(objNum);
    }


    private void OnTriggerStay(Collider other)
    {
        //プレイヤーが触れたときの取得判定
        if (drop)
        {
            if (other.gameObject.tag.StartsWith("P") && isDrop)
            {
                pickPlayer = other.gameObject;
                StartCoroutine(PickUp(other.gameObject));
            }
        }
    }

    /// <summary>
    /// 拾った時の挙動制御、加算処理
    /// </summary>
    /// <param name="player">拾ったプレイヤー</param>
    IEnumerator PickUp(GameObject player)
    {
        isDrop = false;

        // 点滅が動いていたら止める
        if (blinkSC != null)
        {
            blinkSC.StopBlink();
        }

        dropTime = 0.5f; //拾う時間

        //拾った際のY軸の動き
        animCurveY = new AnimationCurve(
            new Keyframe(0, currentPos.y, 0, 10),
            new Keyframe(dropTime, targetPos.y + 0.5f, -10, 0)
        );

        currentPos = this.transform.localPosition; //現在の位置

        start = true;

        //拾われるのを待ってから加算
        yield return new WaitForSeconds(dropTime);
        var energySC = player.GetComponent<EnergyScript>();
        energySC.ChargeEnergy(dropEnergyAmount);

        drop = false;

        PosReset();

        dropManagerSC.AddDrop(objNum); //拾われた際のリスト追加
    }

    /// <summary>
    /// 取得後、消滅後のポジションリセット
    /// </summary>
    void PosReset()
    {
        // 点滅が動いていたら止める
        if (blinkSC != null)
        {
            blinkSC.StopBlink();
        }

        this.transform.position = startPos;
    }
}
