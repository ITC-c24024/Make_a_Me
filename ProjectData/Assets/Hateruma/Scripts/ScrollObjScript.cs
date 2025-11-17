using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScrollObjScript : MonoBehaviour
{
    public SideConveyorScript conveyorSC;

    [SerializeField, Header("PlayerÇÃÉiÉìÉoÅ[")]
    public int playerNum = 0;

    public IEnumerator Move(Vector3 startPos, Vector3 targetPos, float speed)
    {
        transform.position = startPos;

        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speed * Time.deltaTime
            );
            yield return null;
        }

        conveyorSC.AddClone(this, playerNum);
    }


}
