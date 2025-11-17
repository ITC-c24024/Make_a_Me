using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShutterScript : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;
    [SerializeField, Header("開閉にかかる時間")]
    float maxTime = 2.0f;

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// シャッターを閉める
    /// </summary>
    /// <returns></returns>
    public IEnumerator CloseShutter()
    {
        audioManager.Shutter();
        yield return new WaitForSeconds(0.2f);

        float time = 0;
        
        while (time < maxTime)
        {
            time += Time.deltaTime;
            float currentY = Mathf.Lerp(1080, 0, time / maxTime);

            rectTransform.localPosition = new Vector3(
                rectTransform.localPosition.x,
                currentY,
                rectTransform.localPosition.z
                );

            yield return null;
        }
    }
    /// <summary>
    /// シャッターを開ける
    /// </summary>
    /// <returns></returns>
    public IEnumerator OpenShutter()
    {
        audioManager.Shutter();
        yield return new WaitForSeconds(0.2f);

        float time = 0;

        while (time < maxTime)
        {
            time += Time.deltaTime;
            float currentY = Mathf.Lerp(0, 1080, time / maxTime);

            rectTransform.localPosition = new Vector3(
                rectTransform.localPosition.x,
                currentY,
                rectTransform.localPosition.z
                );

            yield return null;
        }
    }
}
