using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "物品", menuName = "子弹数据")]
public class BulletData : ScriptableObject
{
    public int harm = 1;
    public Sprite bulletSprite;
    public Vector2 direction;
}