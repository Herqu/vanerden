using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GunController : MonoBehaviour
{

    private void Awake()
    {
        currentInterval = intervalTime;
        cinemachineImpulseSource = this.GetComponent<CinemachineImpulseSource>();

    }
    private void Update()
    {
        //if (Input.GetKey(KeyCode.B))
        //{
        //    ShootOnce(Vector2.down);
        //}
        currentInterval += Time.deltaTime;
    }

    public GameObject m_bullet;
    public float speed = 10;//枪械初始力量
    public float shotGunVariable = 10;
    public Transform gunMuzzle;
    public float intervalTime = 2.0f;
    public float currentInterval;
    public ParticleSystem gunMuzzleParticle;
    public ParticleSystem[] shellCaseParticles;
    public CinemachineImpulseSource cinemachineImpulseSource;
    public bool isPlayerGun = false;
    public int shootOnceBulletnum = 1;

    public bool ShootOnce(Vector2 direction)
    {
        if (currentInterval >= intervalTime)
        {
            if( shootOnceBulletnum == 1)
            {
                GameObject bullet = Instantiate(m_bullet, gunMuzzle.position, gunMuzzle.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(direction * speed);

            }
            else
            {

                for (int i = 0; i < shootOnceBulletnum; i++)
                {
                    GameObject bullet = Instantiate(m_bullet, gunMuzzle.position, gunMuzzle.rotation);
                    bullet.GetComponent<Rigidbody2D>().AddForce(direction * speed + Random.insideUnitCircle * shotGunVariable);

                }
            }
            gunMuzzleParticle.Play();
            foreach (ParticleSystem item in shellCaseParticles)
            {
                item.Play();
            }
            currentInterval = 0;
            if(isPlayerGun)
                cinemachineImpulseSource.GenerateImpulse(direction);
            return true;
        }
        return false;

    }

}
