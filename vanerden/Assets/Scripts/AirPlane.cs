using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlane : MonoBehaviour
{
    public float speed = 1;
    public float generateBombRange = 4;
    public GameObject explodeBodyPrefab;
    public float delayExplodeTime = 3;
    public float lifeTime = 100;

    private void FlyAcrossBattleField()
    {
        this.transform.Translate(Vector2.left * speed* Time.deltaTime);
    }

    private void GenerateBomb(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(explodeBodyPrefab, (Vector2)transform.position + Random.insideUnitCircle * generateBombRange - Vector2.right*delayExplodeTime, transform.rotation);

        }
    }

    public float intervalBombTime = 4;
    public float currentTime = 0;

    private void Update()
    {
        if(currentTime >= intervalBombTime)
        {
            currentTime = 0;
            GenerateBomb(3);
        }
        currentTime += Time.deltaTime;

        FlyAcrossBattleField();

        lifeTime -= Time.deltaTime;
        if(lifeTime<= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
