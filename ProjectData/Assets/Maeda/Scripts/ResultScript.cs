using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ChildArrey
{
    public Image[] childArrey;
}

public class ResultScript : MonoBehaviour
{
    [SerializeField]
    ShutterScript shutterScript;
    ScoreManager scoreManager;

    public ChildArrey[] arrey;

    float targetY = -1000f;

    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        StartCoroutine(Result());    
    }

    IEnumerator Result()
    {
        StartCoroutine(shutterScript.OpenShutter());
        yield return new WaitForSeconds(2.5f);

        
        int score = 0;
        
        for (int i = 0; i < scoreManager.maxScore; i++)
        {
            for (int n = 0; n < 4; n++)
            {
                //ƒXƒRƒA‚ª‘«‚è‚È‚¢‚Æ‚«
                if (scoreManager.players[n].score < score + 1) continue;               
                
                float startY = arrey[n].childArrey[score].rectTransform.localPosition.y;
                float moveT = 0;
                arrey[n].childArrey[score].enabled = true;
                while (moveT < 0.1f)
                {
                    moveT += Time.deltaTime;
                    float currentY = Mathf.Lerp(startY, targetY, moveT / 0.1f);
                    arrey[n].childArrey[score].rectTransform.localPosition = new Vector3(
                        arrey[n].childArrey[score].rectTransform.localPosition.x,
                        currentY,
                        arrey[n].childArrey[score].rectTransform.localPosition.z
                        );

                    yield return null;
                }           
            }
            score++;
            targetY += 10;
        }
    }
}
