using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    private void Update()
    {
        GenerataSoldierByTimeInterval();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                if(camp != CharacterCamp.enemy)
                    camp = CharacterCamp.enemy;
                break;

            case "Comrades":
                if (camp != CharacterCamp.comrades)
                    camp = CharacterCamp.comrades;
                break;
            case "Player":
                if (camp != CharacterCamp.comrades)
                {
                    GenerateBox();
                    camp = CharacterCamp.comrades;
                }
                break;
        }
    }

    [Header("阵营转换相关")]
    public CharacterCamp camp;
    public List<GameObject> items;
    public void GenerateBox()
    {
        foreach (GameObject item in items)
        {
            Instantiate(item, (Vector2)transform.position + Random.insideUnitCircle, transform.rotation, transform);
        }
    }



    [Header("生成士兵相关。")]
    public bool isGenerate = false;
    public float intervalTime = 5;
    private float currentTime = 0;
    public List<GameObject> soldierPrefab;
    public AllCharacterController allcharactercontroller;


    private void GenerataSoldierByTimeInterval()
    {
        if (isGenerate)
        {

            if (currentTime <= 0)
            {
                GenerateSoldier();
                currentTime = intervalTime;
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
    }

    public void GenerateSoldier()
    {
        GameObject newSoldier = null;

        switch (camp)
        {
            case CharacterCamp.comrades:
                if (allcharactercontroller.comrades.Count < allcharactercontroller.maxcomNum)
                {
                    newSoldier = Instantiate(soldierPrefab[0], transform.position, transform.rotation, transform);
                }
                break;
            case CharacterCamp.enemy:
                if (allcharactercontroller.comrades.Count < allcharactercontroller.maxcomNum)
                {
                    newSoldier = Instantiate(soldierPrefab[1], transform.position, transform.rotation, transform);

                }
                break;
            case CharacterCamp.neutrality:
                break;
        }
        if (newSoldier!= null)
        {

            switch (newSoldier.GetComponent<OthersController>().camp)
            {
                case CharacterCamp.comrades:
                    allcharactercontroller.comrades.Add(newSoldier);
                    break;
                case CharacterCamp.enemy:
                    allcharactercontroller.enemies.Add(newSoldier);

                    break;
                case CharacterCamp.neutrality:
                    allcharactercontroller.neutrality.Add(newSoldier);
                    break;

            }
        }
    }
}
