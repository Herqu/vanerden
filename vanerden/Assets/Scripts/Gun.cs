using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "枪械", menuName = "互动组件/枪械")]
public class Gun : ScriptableObject
{
    public string gunName;
    public Sprite gunSprite;
    public GameObject gunPrefab;

}
