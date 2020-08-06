using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "物品", menuName = "互动组件/物品")]
public class Item : ScriptableObject
{
    public string name;
    public Sprite itemSprite;
    public GameObject particle;
    public int amount = 1;

    public void Play(Transform transform)
    {
        if (name == "BloodPackage")
        {
            Instantiate(particle).transform.position = transform.position;
            transform.GetComponent<Controller>().AddBlood(amount);
        }
        else if (name == "Magazine")
        {
            Instantiate(particle).transform.position = transform.position;
            transform.GetComponent<Controller>().AddBullet(amount);

        }

    }
}