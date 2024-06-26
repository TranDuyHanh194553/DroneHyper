using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Player : Character
{
    [SerializeField] public float jumpForce;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;

    //Bullets
    //[SerializeField] private Bullet bullet1Prefab;
    [SerializeField] private Transform[] throwPoint;

    //Combat
    [SerializeField] private GameObject attackArea;
    [SerializeField] private GameObject death_VFX;

    //CombatTrigger
    [SerializeField] private float combatTriggerEndTime = 4f;
    [SerializeField] private GameObject jumpForceIndicator;

    //UIIndicator
    [SerializeField] private GameObject jumpForceUpUIIndicator;
    [SerializeField] private GameObject multiBulletUIIndicator;
    [SerializeField] private GameObject slowBulletUIIndicator;

    //ClaimResult
    [SerializeField] private VictoryText victoryTextPrefab;
    [SerializeField] private VictoryText defeatedTextPrefab;
    [SerializeField] private VictoryText timeOutTextPrefab;
    
    //Time
    [SerializeField] private TextMeshProUGUI timerText;
    public float remainTime = 180f;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool canAttack = true;
    private bool isDeath = false;

    //CombatTrigger
    public bool highJump = false;
    public bool multiBullet = false;
    public bool slowBullet = false;

    private float horizontal, vertical;
    
    //Index
    private int coin = 0;
    public int boxNumber = 0;

    private Vector3 savePoint;

    //Lean
    public float tiltAngle = 9f;
    public float tiltSpeed = 3f;
    private float targetAngle = 0f;

    // Start is called before the first frame update
    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
        }
        else
        {
            remainTime = 0;
            AudioManager.Ins.PlaySFX(AudioManager.Ins.die);
            Instantiate(timeOutTextPrefab, transform.position + 2f * Vector3.up, Quaternion.identity);
        }
        int minutes = Mathf.FloorToInt(remainTime / 60);
        int seconds = Mathf.FloorToInt(remainTime % 60);
        timerText.text = string.Format("{00:00}:{01:00}", minutes, seconds);

        //Check dead
        if (IsDead)
        {
            //rb.velocity = Vector2.zero;
            return;
        }

        //CheckForLoss();

        //Debug.Log(CheckGrounded());   
        isGrounded = CheckGrounded();
        
        //-1 -> 0 -> 1
        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }
            //Jump
            if (Input.GetKeyDown(KeyCode.J) && isGrounded)
            {
                Jump();
            }

            //change anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run"); 
            }

            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }
            //throw
            if (Input.GetKeyDown(KeyCode.V) /*&& isGrounded*/)
            {
                Throw();
            }
        }
        
        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        //Moving
        if (Mathf.Abs(horizontal) > 0.1f && !isGrounded)
            {
                //ChangeAnim("run");
                rb.velocity = new Vector2(horizontal * Time.deltaTime * moveSpeed * 3.0f, rb.velocity.y);
                rb.AddForce(jumpForce * Vector2.up * 0.2f);
                
                //Lean
                targetAngle = -tiltAngle;
                float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetAngle, Time.deltaTime * tiltSpeed);

                //transform.localScale = new Vector3(horizontal, 1, 1);
                //horizontal > 0 -> tra ve 0, neu < 0 -> tra ve 180
                transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0 /*angle*/));
            }
        //idle
            else if (isGrounded)
            {
                ChangeAnim("idle");
                rb.velocity = Vector2.zero;
            }
   
    }

    public override void OnInit()
    {
        base.OnInit();
        isDeath = false;
        isAttack = false;
        canAttack = true;

        transform.position = savePoint;

        ChangeAnim("idle");
        DeActiveAttack();

        SavePoint();
        UIManager.instance.SetCoin(0);
    }
        

    public override void OnDespawn()
    {
        Instantiate(death_VFX, transform.position, transform.rotation);
        base.OnDespawn();
        //OnInit();
        GameManager.instance.RestartLevel();
    }
    protected override void OnDeath()
    {
        AudioManager.Ins.PlaySFX(AudioManager.Ins.die);
        Instantiate(defeatedTextPrefab, transform.position + 2f * Vector3.up, Quaternion.identity);
        base.OnDeath();
    }

    private bool CheckGrounded()
    {
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit =  Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        //if (hit.collider != null) 
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}

        return hit.collider != null;
    }
    
    public void Attack() 
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    
    public void Throw()
    {
        if (canAttack)
        {
            ChangeAnim("throw");
            isAttack = true;
            
            AudioManager.Ins.PlaySFX(AudioManager.Ins.bulletShoot);
            if (multiBullet && !slowBullet)
            {
                for (int i = 0; i < throwPoint.Length; i++)
                {
                    //Instantiate(bullet1Prefab, throwPoint[i].position, throwPoint[i].rotation);
                    Bullet b = SimplePool.Spawn<Bullet>(PoolType.Bullet_1, throwPoint[i].position, throwPoint[i].rotation);
                    b.OnInit();
                }
            }
            else if (slowBullet && !multiBullet)
            {
                Bullet b = SimplePool.Spawn<Bullet>(PoolType.Bullet_2, throwPoint[0].position, throwPoint[0].rotation);
                b.OnInit();
            }
            else if (slowBullet && multiBullet)
            {
                for (int i = 0; i < throwPoint.Length; i++)
                {
                    //Instantiate(bullet1Prefab, throwPoint[i].position, throwPoint[i].rotation);
                    Bullet b = SimplePool.Spawn<Bullet>(PoolType.Bullet_2, throwPoint[i].position, throwPoint[i].rotation);
                    b.OnInit();
                }
            }
            else 
            {
                Bullet b = SimplePool.Spawn<Bullet>(PoolType.Bullet_1, throwPoint[0].position, throwPoint[0].rotation);
                b.OnInit();
            }
            canAttack = false;
            Invoke(nameof(ResetAttack), 1.0f);
        }
    }

    //reset attack
    private void ResetAttack()
    {
        ChangeAnim("run");
        isAttack = false;
        canAttack = true;
    }
    public void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up * 1.2f);
        AudioManager.Ins.PlaySFX(AudioManager.Ins.fly);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            AudioManager.Ins.PlaySFX(AudioManager.Ins.coin);
            //Destroy(collision.gameObject);
        }

        if (collision.tag == "DeathZone")
        {
            isDeath = false;
            ChangeAnim("die");
            Invoke(nameof(ReLoadScene), 1f);
        }

        if (collision.tag == "Pretaken" && boxNumber == 1 && coin >= CoinManager.Ins.coinNumber)
        {
            Instantiate(victoryTextPrefab, transform.position + 2f * Vector3.up, Quaternion.identity);
            GameManager.instance.isGameplay = false;
            AudioManager.Ins.PlaySFX(AudioManager.Ins.win);
   
            Invoke(nameof(GameVictory), 1f);
        }

        if(collision.tag == "MultiBulletTrigger")
        {
            multiBullet = true;
            AudioManager.Ins.PlaySFX(AudioManager.Ins.coin);
            multiBulletUIIndicator.SetActive(true);    
            StartCoroutine(SingleBullet());
        }

        if (collision.tag == "SlowBulletTrigger")
        {
            slowBullet = true;
            AudioManager.Ins.PlaySFX(AudioManager.Ins.coin);
            slowBulletUIIndicator.SetActive(true);
            StartCoroutine(NormalBullet());
        }

        if (collision.tag == "JumpForceUpTrigger")
        {
            highJump = true;
            AudioManager.Ins.PlaySFX(AudioManager.Ins.coin);
            jumpForce = jumpForce * 2f;
            jumpForceIndicator.SetActive(true);
            jumpForceUpUIIndicator.SetActive(true);
            StartCoroutine(JumpForceDown());
        }
    }

    private IEnumerator SingleBullet()
    {
        yield return new WaitForSeconds(combatTriggerEndTime);
        multiBullet = false;
        multiBulletUIIndicator.SetActive(false);
    }

    private IEnumerator NormalBullet()
    {
        yield return new WaitForSeconds(combatTriggerEndTime);
        slowBullet = false;
        slowBulletUIIndicator.SetActive(false);
    }

    private IEnumerator JumpForceDown()
    {
        yield return new WaitForSeconds(combatTriggerEndTime);
        highJump = false;
        jumpForce = jumpForce * 0.5f;
        jumpForceIndicator.SetActive(false);
        jumpForceUpUIIndicator.SetActive(false) ;
    }


    private void GameVictory()
    {
        UIManager.instance.OpenFinish();
        UIManager.instance.SetTimeRemain(remainTime);
        //Time.timeScale = 0;
    }

    private void GameTimeOut()
    {
        UIManager.instance.OpenTimeOut();
        //Time.timeScale = 0;
    }

    private void ReLoadScene()
    {
        GameManager.instance.RestartLevel();
    }
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
}
