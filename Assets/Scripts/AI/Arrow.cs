using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed;

    private BattleAI target;
    private int deal;

    public void SetTarget(BattleAI enemy)
    {
        this.target = enemy;
        StartCoroutine(ArrowRoutine());
    }

    IEnumerator ArrowRoutine()
    {
        Vector2 startPoint = transform.position;
        Vector2 endPoint = target.transform.position;
        float time = Vector2.Distance(startPoint, endPoint) / arrowSpeed;

        float rate = 0f;
        while (rate < 1f)
        {
            if (target != null)
            {
                transform.LookAt(target.transform.position);
                endPoint = target.transform.position;
            }

            rate += Time.deltaTime / time;
            transform.position = Vector2.Lerp(startPoint, endPoint, rate);
            yield return null;
        }

        transform.position = endPoint;
        Destroy(gameObject);
        yield return null;

        //if (target != null)
        //{
        //    target?.TakeDamage(deal);
        //}
        //Destroy(gameObject);
    }

    public void SetDamage(int damage)
    {
        this.deal = damage;
    }
}
