using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum EnemyState
    {
        IDLE,
        MOVE,
        ATTACK,
        RETURN,
        DAMAGE,
        DIE
    }
    EnemyState myState = EnemyState.IDLE;

    [SerializeField] private float idleDelayTime = 2.0f;    //일정시간
    [SerializeField] private Transform target;              //에너미가 따라갸아할 타겟을 생성
    [SerializeField] private float enemySpeed;              //에너미 이동속도 
    [SerializeField] private float attackRange;             //공격 감지 범위
    [SerializeField] private float attackStartRange;        //공격 감지 범위
    [SerializeField] private float gravity;                 //에너미에게 작용하는 중력값
    private float distance;
    private float enemyCanMoveRange;                        //에너미가 움직일 수 있는 공간 범위
    private float curTime = 0.0f;                           //현재시간 
    private Vector3 startPos;                               //에너미가 움직이기 시작하는 시작 지점 
    private Vector3 currentPos;                             //에너미의 현재 지점
    private Vector3 finalPos;                               //에너미가 마지막으로 있는 지점 
    private CharacterController cc;                         //캐.컨 선언
    private Vector3 primaryPos;

    [SerializeField] private GameObject bombFactory;        //폭탄 프리팹
    [SerializeField] private GameObject bomFirePoint;       //폭탄 발사 위치 
    [SerializeField] private float power = 20.0f;           //폭탄 뎀지 
    [SerializeField] private float bombRangePower;          //폭탄 사거리 힘
    [SerializeField] private int enemyHp;                   //에너미 체력 
    [SerializeField] private float attackCoolTime = 2.0f;   //에너미 공격 쿨타임
    private Vector3 moveDir;                                //에너미의 이동 방향
    private float attackCount = 0.0f;
    Vector3 dist;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        startPos = new Vector3(10, 1, 10);
        moveDir = Vector3.zero;
    }
    void Update()
    {
        //타겟(플레이어)와의 방향을 구하는 포지션 벡터는 자주 쓰이니, Update에 딱 선언해, 쭉 dist만 갖고와서 쓴다. 
        dist = target.transform.position - transform.position;
        //에너미 상태에 따른 행동처리 
        switch (myState)
        {
            case EnemyState.IDLE:
                
                break;
            case EnemyState.MOVE:
                break;
            case EnemyState.ATTACK:
                break;
            case EnemyState.RETURN:
                break;
            case EnemyState.DAMAGE:
                break;
            case EnemyState.DIE:
                break;

        }
    }
    private void IDLE()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
        if(distance < attackRange)
        {
            myState = EnemyState.MOVE;
        }
    }
    private void MOVE()
    {
        cc.SimpleMove(dist.normalized * enemySpeed);
        dist.y -= gravity * Time.deltaTime;
        if (dist.magnitude > attackRange)
            myState = EnemyState.RETURN;
        if (dist.magnitude < attackStartRange)
            myState = EnemyState.ATTACK;
    }
    private void ATTACK()
    {
        attackCount += Time.deltaTime;
        if(attackCount > attackCoolTime)
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = bomFirePoint.transform.position;
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            Vector3 dir = transform.forward + (transform.up * bombRangePower);
            dir = dist.normalized;
            transform.forward = dir;
            dir.Normalize();
            rb.AddForce(dir * power, ForceMode.Impulse);
            if (dist.magnitude > attackStartRange)
                myState = EnemyState.MOVE;
            attackCount = 0.0f;
        }
    }
    private void RETURN()
    {
        Vector3 backRoute = startPos - transform.position;
        Vector3 dist = target.transform.position - transform.position;
        cc.Move(backRoute.normalized * enemySpeed * Time.deltaTime);
        if (dist.magnitude < attackRange)
            myState = EnemyState.MOVE;
    }
    public void DAMAGE(int p_damage)
    {
        int damage = p_damage;
        enemyHp -= damage;
        if (enemyHp <= 0)
            DIE();
    }
    private void DIE()
    {
        Destroy(gameObject);
    }
}
