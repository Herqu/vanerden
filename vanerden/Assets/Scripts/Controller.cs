using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Controller : MonoBehaviour
{

    private void Awake()
    {
        GameObject cinemachine = GameObject.FindGameObjectWithTag("Cinemachine");
        cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = this.anchor.transform;
        UIcontroller uiController = GameObject.FindGameObjectsWithTag("UIIcon")[0].GetComponent<UIcontroller>();
        uiController.ChangeChara(this);
        if (!gameoveranimator)
        {
            gameoveranimator = GameObject.FindGameObjectsWithTag("UIIcon")[1].GetComponent<Animator>();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        rigidebody2d = this.GetComponent<Rigidbody2D>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Cursor.visible = false;
        allCharacterController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AllCharacterController>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Fire1") > 0.5)
        {
            StartShoot();
        }

        LookAtAim();


        if (isAlive)
        {
            MoveChara(GenerateDirection());

        }
        else
        {
            DeathCheck();
        }
    }

    [Header("移动速度相关。")]
    private Rigidbody2D rigidebody2d;
    public float speed = 1;
    public float maxSpeed = 10;
    private Vector2 previousSpeed = Vector2.zero;
    public Vector2  GenerateDirection()
    {
        Vector2 direction = Vector2.zero ;



        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }

        return direction.normalized;
    }
    private void MoveChara(Vector2 direction)
    {

        if (direction != Vector2.zero)
        {

            if (direction != previousSpeed)
            {
                rigidebody2d.velocity = rigidebody2d.velocity.magnitude * direction;
                previousSpeed = direction;
            }

            if (rigidebody2d.velocity.magnitude <= maxSpeed)
            {
                rigidebody2d.AddForce(speed * direction);
            }
            else
            {
                rigidebody2d.velocity = maxSpeed * direction;
            }

        }
        else
        {
            rigidebody2d.velocity = Vector2.zero;
        }

    }



    [Header("鼠标点")]
    public GameObject m_Aim;
    private  Camera camera;
    public GameObject anchor;
    public SpriteRenderer spriteRender;
    public int pixelWidth;

    private void LookAtAim()
    {
        if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height)
        {
            Vector3 tVect = Input.mousePosition;
            tVect /= (Screen.width/pixelWidth); 
            Vector3 vector3 = camera.ScreenToWorldPoint(tVect);
            vector3.z = 0;

         

            m_Aim.transform.position = vector3;
            anchor.transform.position = (this.transform.position + vector3) / 2;
            if (isAlive)
            {

                Vector3 direction = vector3 - this.transform.position;

                if (direction.x >= 0)
                {
                    spriteRender.flipX = true;
                }
                else
                {
                    spriteRender.flipX = false;
                }
            }
        }

    }


    
    [Header("枪械控制")]
    public GunController gun;
    public int bulletNum;
    internal void AddBullet(int bullet)
    {
        bulletNum += bullet;
    }
    private void StartShoot()
    {
        if(bulletNum > 0)
        {

            Vector2 gundirection = m_Aim.transform.position - gun.transform.position;
            if (gun.ShootOnce(gundirection.normalized))
            {
                bulletNum -= 1;
            }

        }
    }





    [Header("受伤相关")]
    public int bloodNum;
    internal void AddBlood(int blood)
    {
        bloodNum += blood;
    }
    public CinemachineImpulseSource impulseSource;
    public Sprite deathSprite;
    public ParticleSystem deathParticle;
    private AllCharacterController allCharacterController;
    private bool isAlive = true;
    public float deathDrag = 1;
    public float hurtImpulsePower = 1;
    public SpriteMask spritemask;
    public GameObject deathBodyPrefab;
    public Vector2 startPosition;
    public float deathLastTime = 3;
    public Animator gameoveranimator;
    public Animator gameeffectanimator;

    ///关于被攻击
    public void Hurt(string s)
    {
        impulseSource.GenerateImpulse(Vector2.down*hurtImpulsePower);
        bloodNum -= 1;
        deathParticle.Play();
        ShowFlash();
        if(bloodNum <= 0)
        {
            isAlive = false;
            Death();
        }
    }

    private void ShowFlash()
    {
        spritemask.enabled = true;
        //gameeffectanimator.SetTrigger("hurt");
        Invoke("ShutDownFlash", 0.1f);

    }

    private void ShutDownFlash()
    {
        spritemask.enabled = false;

    }

    internal void Death()
    {
        spriteRender.sprite = deathSprite;

        this.gun.gameObject.active = false;

        allCharacterController.comrades.Remove(this.gameObject);

        gameoveranimator.SetBool("gameover",true);
        
    }

    public float explodeForce = 10;
    public void HurtByExplode(Vector2 source)
    {

        //impulseSource.GenerateImpulse(Vector2.down*hurtImpulsePower);
        //ShowFlash();
        isAlive = false;
        //bloodNum = 0;
        bloodNum = 0;
        Death();
        rigidebody2d.AddForce(-(source - (Vector2)transform.position).normalized * explodeForce);
    }



    //在update中运行，然后当速度为零的时候操作。
    private void DeathCheck()
    {
        rigidebody2d.drag += deathDrag*Time.deltaTime;

        if (rigidebody2d.velocity.magnitude <= 0)
        {

            this.GetComponent<Collider2D>().enabled = false;
            deathParticle.Pause();
            GameObject deathBody = Instantiate(deathBodyPrefab, transform.position, transform.rotation);
            deathBody.GetComponentInChildren<SpriteRenderer>().flipX = spriteRender.flipX;
            gameoveranimator.SetBool("gameover",false);
            allCharacterController.NewPlayer();
            Destroy(this.gameObject);

        }

        if (deathLastTime <= 0)
        {
            gameoveranimator.SetBool("gameover",false);
            allCharacterController.NewPlayer();
            Destroy(this.gameObject);

        }
        deathLastTime -= Time.deltaTime;
    }


}


