using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    [SerializeField, Header("子オブジェクトを含むかどうか")]
    bool all;

    Coroutine blinkCoroutine;

    GameObject[] childTargets;

    void Start()
    {
        if (all)
        {
            childTargets = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                childTargets[i] = transform.GetChild(i).gameObject;
            }
        }
    }

    public void BlinkStart(int time, float speed, float lastSpeed)
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkCount(time, speed, lastSpeed));
    }

    IEnumerator BlinkCount(int time, float speed, float lastSpeed)
    {
        float currentTime = 0f;
        MeshRenderer mesh = GetComponent<MeshRenderer>();

        while (currentTime < time)
        {
            // 親はメッシュだけ切り替え
            if (mesh != null) mesh.enabled = !mesh.enabled;

            // 子オブジェクトはSetActiveで切り替え
            if (all)
            {
                foreach (var child in childTargets)
                {
                    child.SetActive(!child.activeSelf);
                }
            }

            if (time - currentTime <= 2f)
            {
                speed = lastSpeed;
            }

            yield return new WaitForSeconds(speed);
            currentTime += speed;
        }

        // 最終的に全部表示
        if (mesh != null) mesh.enabled = true;
        if (all)
        {
            foreach (var child in childTargets) child.SetActive(true);
        }
    }
    public void StopBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        // 親を表示
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = true;

        // 子オブジェクトも全表示
        if (all && childTargets != null)
        {
            foreach (var child in childTargets)
            {
                child.SetActive(true);
            }
        }
    }

}

