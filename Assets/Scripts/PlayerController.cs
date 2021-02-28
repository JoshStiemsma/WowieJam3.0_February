using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public float thrust = 5;
    public float maxSpeed = 5f;
    bool grounded;
    bool blocking = false;
    bool swinging = false;

    KeyCode Left, Right, Up, Down;
    KeyCode Attack, CallThrow;

    Vector3 InitPosition;

    public float callThrowTimer = 0;
    public Action OnCalledThrow;


    public Player playerType;

    public GameObject punchArm;
    public PunchTrigger poonch;

    bool inArena = false;

    public int PlayerHealth = 100;

    public GameObject Arm;
    private Transform ArmPivot;
    private Vector3 initArmRotation;
    private Vector3 restingArmRotation,AttackArmRotation;

    private float faceingDirection = -1f;
    public bool faceLeft;
    public CameraShake Shake;
    public PlayerController enemy;
    public ParticleSystem blood;
    public float airStrafe;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitPosition = this.transform.position;
        ArmPivot = Arm.transform.parent;
        initArmRotation = ArmPivot.localEulerAngles;
        if (playerType == Player.Left)
        {
            Left = KeyCode.A;
            Right = KeyCode.D;
            Up = KeyCode.W;
            Down = KeyCode.S;
            Attack = KeyCode.Space;
            CallThrow = KeyCode.LeftShift;
        }
        else
        {
            Left = KeyCode.Keypad4;
            Right = KeyCode.Keypad6;
            Up = KeyCode.Keypad8;
            Down = KeyCode.Keypad5;
            Attack = KeyCode.Keypad0;
            CallThrow = KeyCode.KeypadEnter;

        }

        FightSceneManager.instance.OnStartScene += OnStartArena;
        FightSceneManager.instance.OnRoundEnd += OnRoundEnd;
        OnCalledThrow += () => FightSceneManager.instance.PlayerCalledThrow(playerType);

    }

    public void Update()
    {
        if (!inArena) return;

        if (!swinging)
        {
            HandleMovementInput();
            if (Input.GetKeyDown(Attack))
            {   
                if(poonch.canHitPlayer)
                {
                    if(!blocking)
                    {
                        if (enemy.blocking == false)
                        {
                            enemy.PlayerHealth-= 10;
                            enemy.blood.Play();
                            // play blood particals
              
                        StartCoroutine(Shake.Shake(.15f,.4f));
                        }

                    }
                    
                    // play damage animation
                    // shake screen

                }
                //punchArm.gameObject.SetActive(true);

                
                
            }
            if (Input.GetKeyUp(Attack))
            {
                //HandleAttack();
                //punchArm.gameObject.SetActive(false);

            }
        }
        if (Input.GetKey(Down))
        {
            blocking = true;
           // poonch.isBlocking = true;

        }else {
               blocking = false;
              // poonch.isBlocking = false;
        }

        Debug.Log(poonch.isBlocking);

        
        HandleHealth();
        HandleCallThrowInput();


        FaceDirection(faceLeft);

       
    }

    Coroutine AttackRoutine;
    void HandleAttack()
    {
        if(AttackRoutine ==null && !blocking)
        AttackRoutine = StartCoroutine(ArmSwingRoutine());
    }

    IEnumerator ArmSwingRoutine()
    {
        swinging = true;
        float timer = 0;
        float swingTimer = .25f;
        while(timer < swingTimer)
        {
            timer += Time.fixedDeltaTime;
            ArmPivot.localEulerAngles = Vector3.Lerp(restingArmRotation, AttackArmRotation, timer/ swingTimer);
            yield return new WaitForSeconds(.01f);
        }
        while (timer < swingTimer *2)
        {
            timer += Time.fixedDeltaTime;
            ArmPivot.localEulerAngles = Vector3.Lerp(AttackArmRotation, restingArmRotation, timer-swingTimer / swingTimer);
            yield return new WaitForSeconds(.01f);
        }

        ArmPivot.localEulerAngles = restingArmRotation;
        AttackRoutine = null;
        swinging = false;
    }


    void HandleHealth()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && playerType == Player.Left)
        {
            PlayerHealth = 0;
        }

        if (PlayerHealth <= 0)
        {
            FightSceneManager.instance.PlayerDied(playerType);
        }
    }

    void FaceDirection(bool isLeft)
    {
        //faceingDirection = isLeft ? -1: 1;
        //ArmPivot.localPosition = new Vector3(75 * faceingDirection, 5, 0);

        //restingArmRotation = isLeft ? new Vector3(0,0,-60) : new Vector3(0, 0, 230);
       // AttackArmRotation = isLeft ? new Vector3(0, 0, 0) : new Vector3(0, 0, 180);
        //ArmPivot.localEulerAngles =  restingArmRotation;
       // Debug.Log(ArmPivot.localEulerAngles);
    }

    void HandleMovementInput()
    {

        if (Input.GetKey(Right)&& grounded == true)
        {
            rb.velocity += new Vector2(thrust,rb.velocity.y);
            
           // rb.velocity.x = thrust; 
            //rigidbody.velocity = Vector3(0,10,0);
            //rb.velocity = rb.velocity.normalized * speed;

           // rb.AddForce(transform.right * thrust);
           // FaceDirection(false);
        }else if (Input.GetKey(Left)&& grounded == true)
        {
             rb.velocity += new Vector2(-thrust,rb.velocity.y);
           // rb.AddForce(-transform.right * thrust);
           // FaceDirection(true);

        }
        if (Input.GetKeyDown(Up) && grounded == true)
        {
            rb.AddForce(transform.up * (thrust * 100));
            grounded = false;
        }

        if (Input.GetKey(Right)&& grounded == false)
        {
                rb.AddForce(transform.right * airStrafe);
         } 
         else if (Input.GetKey(Left)&& grounded == false)
        {
            rb.AddForce(-transform.right * airStrafe);
        }


        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

    }


    void HandleCallThrowInput()
    {
        if (Input.GetKey(CallThrow))
        {
            callThrowTimer += Time.fixedDeltaTime;
        }
        else if(callThrowTimer < 3)
        {
            callThrowTimer = 0;
        }


        if(callThrowTimer > 3)
        {
            OnCalledThrow.Invoke();
            callThrowTimer = 0;
        }
    }
    

    void OnRoundEnd()
    {
        inArena = false;

    }

    void OnStartArena() {
        Reset();
        inArena = true;
    }

    public void Reset()
    {
        PlayerHealth = 100;
        rb.velocity *= 0;
        this.transform.position = InitPosition;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
       if (col.gameObject.tag == "Ground")
       {
           grounded = true;
       }
    }


}
