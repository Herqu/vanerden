using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthersController: MonoBehaviour
{

    private void Start()
    {
        allcharacterController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AllCharacterController>();
        rigidebody2d = this.GetComponent<Rigidbody2D>();
        if(moveState == MoveState.stay)
        {
            rigidebody2d.drag = 100;
        }
    }
    private void Update()
    {
        if (NearestEnemy)
        {
            nearestEnemyDirection = NearestEnemy.transform.position - transform.position;
            nearestEnemyDirection.Normalize();
        }
        DeathCheck();
        if(!isDeath)
        switch (camp)
        {
            case CharacterCamp.enemy:
                MoveByMode();
                UpdateControlAttackMode();
                break;
            case CharacterCamp.comrades:

                MoveByMode();
                UpdateControlAttackMode();
                break;
            case CharacterCamp.neutrality:
                break;

        }

    }

    [Header("移动相关。")]
    private Rigidbody2D rigidebody2d;
    public Vector2 nearestEnemyDirection = Vector2.zero;
    public float marchVelocity = 1;
    public float speed = 1;
    public float maxSpeed = 10;
    public float Movetime = 0;
    public float maxTime = 3;
    public GameObject moveAroundObj;
    public float moveAroundDistance = 5f;
    public Vector2 direction;
    public MoveState moveState = MoveState.toward;
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// 这一段都是处理移动的。暂时没有安置关于移动模式改变的动作。
    /// </summary>
    /// <param name="direction"></param>
    private void MoveChara(Vector2 direction)
    {

        if (direction != Vector2.zero)
        {
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

    private void MoveForIdle()
    {

        if (Movetime <= 0)
        {
            direction = Random.insideUnitCircle;
            Movetime = Random.Range(0, maxTime);

        }
        else
        {
            Movetime -= Time.deltaTime;
            MoveChara(direction);
        }
        ChangeDirectionByVelocity();
    }
    private void MoveForMarch()
    {

        if (Movetime <= 0)
        {
            switch (camp)
            {
                case CharacterCamp.comrades:
                    direction = Random.insideUnitCircle.normalized + Vector2.right * 0.5f;
                    break;
                case CharacterCamp.neutrality:
                    break;
                case CharacterCamp.enemy:
                    direction = Random.insideUnitCircle.normalized + Vector2.left * 0.5f;

                    break;
            }
            Movetime = Random.Range(0, maxTime);

        }
        else
        {
            Movetime -= Time.deltaTime;
            MoveChara(direction);
        }
        ChangeDirectionByVelocity();
    }
    private void MoveTowardEnemy()
    {
        ChangeDirectionByVelocity();

        if (Movetime <= 0)
        {
            switch (camp)
            {
                case CharacterCamp.comrades:
                    direction = Random.insideUnitCircle.normalized + nearestEnemyDirection * marchVelocity;
                    break;
                case CharacterCamp.neutrality:
                    break;
                case CharacterCamp.enemy:
                    direction = Random.insideUnitCircle.normalized + nearestEnemyDirection * marchVelocity;

                    break;
            }
            Movetime = Random.Range(0, maxTime);

        }
        else
        {
            Movetime -= Time.deltaTime;
            MoveChara(direction);
        }
    }
    private void MoveAround()
    {
        if (!moveAroundObj)
        {
            Debug.LogError("没有环绕目标");
        }
        if (Movetime <= 0)
        {
            //direction = (Vector2)(moveAroundObj.transform.position - this.transform.position).normalized + Random.insideUnitCircle;
            direction = (Vector2)(moveAroundObj.transform.position + (Vector3)Random.insideUnitCircle * moveAroundDistance - this.transform.position);
            Movetime = Random.RandomRange(0, maxTime);
        }
        else
        {
            Movetime -= Time.deltaTime;
            MoveChara(direction);
        }
    }

    private void ChangeDirectionByVelocity()
    {
        if (rigidebody2d.velocity.x >= 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;

        }
    }
    private void MoveByMode()
    {
        switch (moveState)
        {
            case MoveState.idle:
                MoveForIdle();
                break;
            case MoveState.march:
                MoveForMarch();
                break;
            case MoveState.stay:
                break;
            case MoveState.toward:
                MoveTowardEnemy();
                break;
            case MoveState.moveAround:
                MoveAround();
                break;
        }
    }


    /// <summary>
    /// 和死亡相关的部分。暂时也没有处理干净。
    /// </summary>
    [Header("控制死亡和受伤。")]
    public int bloods = 3;
    public bool isDeath = false;
    public float deathSpeed = 5;
    public ParticleSystem deathParticle;
    public Sprite deathBody;
    public float deathDrag = 1;
    public SpriteMask spritemask;

    public void Hurt(string s)
    {
        ShowFlash();
        deathParticle.Play();
        speed = deathSpeed;
        bloods -= 1;
    }

    private void DeathCheck()
    {
        if(bloods > 0)
        {
            ;
        }
        else
        {
            spriteRenderer.sprite = deathBody;
            this.gun.gameObject.active = false ;
            rigidebody2d.drag += deathDrag * Time.deltaTime;

            if (!isDeath)
            {
                switch (camp)
                {
                    case CharacterCamp.comrades:
                        allcharacterController.Remove(camp, this.gameObject);
                        break;

                    case CharacterCamp.enemy:
                        TimeScaleSlow();
                        allcharacterController.Remove(camp, this.gameObject);
                        break;
                    case CharacterCamp.neutrality:
                        break;
                }
            }
            isDeath = true;

            if (rigidebody2d.velocity.magnitude <= 0)
            {
                this.GetComponent<Collider2D>().enabled = false;
                //this.rigidebody2d.bodyType = RigidbodyType2D.Static;
                deathParticle.Pause();
                if(testGameObj != null)
                {
                    Instantiate(testGameObj, transform);

                }
                this.enabled = false;
            }
        }
    }
    public GameObject testGameObj;

    private void ShowFlash()
    {
        spritemask.enabled = true;

        Invoke("ShutDownFlash", 0.1f);

    }

    private void ShutDownFlash()
    {
        spritemask.enabled = false;

    }

    public float explodeForce = 10;
    public void HurtByExplode(Vector2 source)
    {
        ShowFlash();
        bloods -= 3;
        rigidebody2d.AddForce(-(source - (Vector2)transform.position).normalized * explodeForce);
    }
    public float timeSlowScale = 0.5f;
    public float timeSlowTime = 0.2f;
    public void TimeScaleSlow()
    {
        Time.timeScale = timeSlowScale;
        Invoke("TimeScaleBacktoNormal",timeSlowTime);
    }
    public void TimeScaleBacktoNormal()
    {
        Time.timeScale = 1f;
    }




    /// <summary>
    /// 关于方式改变和运行的方式。暂时没有做好AI。
    /// </summary>
    [Header("关于攻击")]
    public GameObject NearestEnemy;
    private AllCharacterController allcharacterController;

    private GameObject FindCLosetEnemy()
    {
        GameObject NearrestEnmey;

        switch (camp)
        {
            case CharacterCamp.comrades:
                if (allcharacterController.enemies.Count != 0)
                {
                    NearrestEnmey = allcharacterController.enemies[0];
                    if (!NearestEnemy)
                    {
                        //Debug.Log("不存在最近敌人。");
                    }
                    foreach (GameObject enemy in allcharacterController.enemies)
                    {
                        if ((enemy.transform.position - this.transform.position).magnitude <= (NearrestEnmey.transform.position - this.transform.position).magnitude)
                        {
                            NearrestEnmey = enemy;
                        }
                    }
                    return NearrestEnmey;
                }
                else
                {
                    Debug.Log("这里没有多余的敌人了。");
                    return null;
                }

            case CharacterCamp.enemy:
                if (allcharacterController.comrades.Count != 0)
                {
                    NearrestEnmey = allcharacterController.comrades[0];
                    if (!NearestEnemy)
                    {
                        //Debug.Log("不存在最近敌人。");
                    }
                    foreach (GameObject enemy in allcharacterController.comrades)
                    {
                        if ((enemy.transform.position - this.transform.position).magnitude <= (NearrestEnmey.transform.position - this.transform.position).magnitude)
                        {
                            NearrestEnmey = enemy;
                        }
                    }
                    return NearrestEnmey;
                }
                else
                {
                    Debug.Log("这里没有多余的敌人了。");
                    return null;
                }
            case CharacterCamp.neutrality:
                Debug.Log("I'm just neutrality!" + this.gameObject);
                return null;

            default:
                return null;
        }

    }

    public float CheckNearestEnemyTime = 0;
    public float CheckNearestEnemyRatio = 4;
    private void ShiftClosetEnmeyByTimeAndDistance()
    {

        if (CheckNearestEnemyTime <= 0)
        {
            CheckNearestEnemyTime = Random.value * CheckNearestEnemyRatio;
            NearestEnemy = FindCLosetEnemy();
        }
        else
        {
            CheckNearestEnemyTime -= Time.deltaTime;
        }
    }

    public float AttackTimeInterval = 0;
    public float AttackTimeRatio = 3;
    public float AttackSustainedTimeRatio = -1;
    public float AttackSustainedTime = -0.5f;
    private void UpdateControlAttackMode()
    {
        GunDirectionController();
        ShiftClosetEnmeyByTimeAndDistance();
        if (AttackTimeInterval >= 0)
        {
            AttackTimeInterval -= Time.deltaTime;
        }
        else if (AttackTimeInterval >= AttackSustainedTime)
        {
            AttackTimeInterval -= Time.deltaTime;
            Attack();
        }
        else
        {
            AttackTimeInterval = Random.value * AttackTimeRatio;
            AttackSustainedTime = Random.value * AttackSustainedTimeRatio;
        }
    }

    public GameObject aimAnchor;
    private void GunDirectionController()
    {
        Vector2 aimPosition;
        if (NearestEnemy)
        {
            aimPosition = NearestEnemy.transform.position;

        }
        else
        {
            if (spriteRenderer.flipX)
            {
                aimPosition = (Vector2)this.transform.position + Vector2.right + Vector2.down;

            }
            else
            {
                aimPosition = (Vector2)this.transform.position + Vector2.left + Vector2.down;

            }
        }
        aimAnchor.transform.position = aimPosition;

    }

    public GunController gun;
    private void Attack()
    {
        if (NearestEnemy)
        {
            gun.ShootOnce((NearestEnemy.transform.position - this.transform.position).normalized);
        }
    }


    [Header("阵营和职能")]
    public CharacterCamp camp = CharacterCamp.enemy;

}

public enum MoveState
{
    normal,
    stay,
    idle,
    toward,
    march,
    moveAround,
}


public enum CharacterCamp
{
    comrades,
    enemy,
    neutrality
}


