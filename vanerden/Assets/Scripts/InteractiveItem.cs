using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractiveItem : MonoBehaviour
{
    public Item item;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == Tag.player)
        {
            item.Play(collision.transform);
            Destroy(this.gameObject);
        }
    }

}




/// <summary>
/// 给tag做了一个静态调用。减少明文。
/// </summary>
public static class Tag
{
    public static string player = "Player";
}
