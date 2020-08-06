using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveItem : MonoBehaviour
{
    private void Update()
    {
        if(blood<= 0)
        {
            Explode();
        }
    }

    public GameObject explosiveBodyPrefab;
    public float explosiveBodyRange = 1;
    public int explodeNum = 3;
    public int blood = 3;
    public SpriteMask spritemask;

    public void Hurt()
    {
        //Debug.Log(this.transform.position);
        //Debug.Log(this.gameObject);
        blood -= 1;
        ShowFlash();
    }

    private void ShowFlash()
    {
        spritemask.enabled = true;

        Invoke("ShutDownFlash", 0.1f);

    }

    private void ShutDownFlash()
    {
        spritemask.enabled = false;

    }


    public void Explode()
    {
        for (int i = 0; i < explodeNum; i++)
        {
            if (explosiveBodyPrefab)
            {
                Instantiate(explosiveBodyPrefab, (Vector2)transform.position + Random.insideUnitCircle * explosiveBodyRange, transform.rotation);

            }
        }
        Destroy(this.gameObject);
    }


    public void HurtByExplode(Vector2 location)
    {
        ShowFlash();
        Invoke("Explode", 0.3f);

    }
}
