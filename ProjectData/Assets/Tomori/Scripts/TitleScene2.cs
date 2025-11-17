using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene2 : MonoBehaviour
{
    [SerializeField] GameObject[] players1;
    [SerializeField] GameObject[] players2;
    [SerializeField] GameObject[] players3;
    [SerializeField] float moveSpeed = 3f;
    private Vector3 startPosition1 = new Vector3(131.14f, 22.9f, -41f);
    private Vector3 startPosition2 = new Vector3(144.13f, 22.9f, 64.3f);
    private Vector3 startPosition3 = new Vector3(160.4f, 22.9f, -41f);
    private string yellStateName = "Yell"; // Animator のステート名に合わせる

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        startPosition1 = players1[0].transform.position;
        startPosition2 = players2[0].transform.position;
        startPosition3 = players3[0].transform.position;
    }

    void Update()
    {
        foreach (GameObject player in players1)
        {
            Animator anim = player.GetComponent<Animator>();
            float z = player.transform.position.z;

            // Zが-9以上でアニメーション開始（Yell）
            if (z >= -9f && !anim.GetCurrentAnimatorStateInfo(0).IsName(yellStateName))
            {
                anim.SetBool("Yell", true);
                anim.Play(yellStateName, 0, 0f); // 0フレームから再生
            }

            // Zが52以上なら完全リセット＆Yell再スタート
            if (z >= 72f)
            {
                // 位置リセット
                player.transform.position = new Vector3(
                    startPosition1.x,
                    player.transform.position.y,
                    startPosition1.z);
            }

            // 横移動は Translate で制御
            player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        }

        /*foreach (GameObject player in players2)
        {
            Animator anim = player.GetComponent<Animator>();
            float z = player.transform.position.z;

            // Zが-9以上でアニメーション開始（Yell）
            if (z >= -9f && !anim.GetCurrentAnimatorStateInfo(0).IsName(yellStateName))
            {
                anim.SetBool("Yell", true);
                anim.Play(yellStateName, 0, 0f); // 0フレームから再生
            }

            // Zが-52以下なら完全リセット＆Yell再スタート
            if (z <= -33f)
            {
                // 位置リセット
                player.transform.position = new Vector3(
                    startPosition2.x,
                    player.transform.position.y,
                    startPosition2.z);
            }

            // 横移動は Translate で制御（逆方向）
            player.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }

        foreach (GameObject player in players3)
        {
            Animator anim = player.GetComponent<Animator>();
            float z = player.transform.position.z;

            // Zが-9以上でアニメーション開始（Yell）
            if (z >= -9f && !anim.GetCurrentAnimatorStateInfo(0).IsName(yellStateName))
            {
                anim.SetBool("Yell", true);
                anim.Play(yellStateName, 0, 0f); // 0フレームから再生
            }

            // Zが52以上なら完全リセット＆Yell再スタート
            if (z >= 73f)
            {
                // 位置リセット
                player.transform.position = new Vector3(
                    startPosition3.x,
                    player.transform.position.y,
                    startPosition3.z);
            }

            // 横移動は Translate で制御
            player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        }*/
    }
}
