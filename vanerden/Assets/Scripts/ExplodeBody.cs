using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ExplodeBody : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player" || collision.tag == "Comrades" || collision.tag == "Enemy" || collision.tag == "ExplosiveItem")
        {
            gameObjInArea.Add(collision.gameObject);
        }
    }

 

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Comrades" || collision.tag == "Enemy"|| collision.tag =="ExplosiveItem")
        {
            gameObjInArea.Remove(collision.gameObject);

        }
    }

    public List<GameObject> gameObjInArea = new List<GameObject>();
    public float hurtImpulsePower = 10;
    public CinemachineImpulseSource impulseSource;

    public void ExplodeEnd()
    {

        Destroy(this.gameObject);
    }

    public void Impulse()
    {
        foreach (GameObject item in gameObjInArea)
        {
            item.SendMessage("HurtByExplode", (Vector2)this.transform.position);
        }
        impulseSource.GenerateImpulse(Vector2.down*hurtImpulsePower);
    }
}
