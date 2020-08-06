using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(this.GetComponent<Rigidbody2D>().velocity.y, this.GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (lifeTime>= 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("关于碰撞检测和子弹敌我的区分")]
    public float lifeTime = 3;
    public BulletSource source;
    public GameObject bulletParticle;
    public int bulletHarm = 1;
    public BulletData bulletdata;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(bulletParticle, collision.contacts[0].point, this.transform.rotation);
        if(collision.transform.tag == "ExplosiveItem"|| collision.transform.tag == "Bunker")
        {
            collision.transform.SendMessage("Hurt");
        }

        switch (source)
        {
            case BulletSource.Enemy:
                if (collision.transform.tag == "Player")
                {
                    bulletdata.direction = this.GetComponent<Rigidbody2D>().velocity;
                    collision.transform.SendMessage("Hurt", "v");

                    Destroy(this.gameObject);
                }
                if (collision.transform.tag == "Comrades")
                {
                    bulletdata.direction = this.GetComponent<Rigidbody2D>().velocity;
                    collision.transform.SendMessage("Hurt", "v");
                    Destroy(this.gameObject);
                }
                break;
            case BulletSource.SelfAndComarades:
                if (collision.transform.tag == "Enemy")
                {
                    collision.transform.SendMessage("Hurt", "sdfaj");
                    Destroy(this.gameObject);
                }
                break;
        }

        Destroy(this.gameObject);

    }





}

public enum BulletSource
{
    SelfAndComarades,
    Enemy
}

