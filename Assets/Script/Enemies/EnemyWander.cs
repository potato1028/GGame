using UnityEngine;
using System.Collections;

public class EnemyWander : MonoBehaviour, EnemySkill_Interface {
    [Header("Enemy State")]
    public int[] directToPlayer;
    public int selectDirection;
    public int stareDirection;

    public float wanderSpeed;
    public float randTime;

    public bool isCanWander;
    public RaycastHit2D obstacleHit;

    [Header("Component")]
    public Rigidbody2D rb2D;
    private EnemyDetect enemyDetect;
    private EnemyState enemyState;

    void Start() {
        rb2D = this.GetComponent<Rigidbody2D>();

        enemyDetect = this.GetComponent<EnemyDetect>();
        enemyState = this.GetComponent<EnemyState>();

        directToPlayer = new int[] {-1, 0, 1};
        selectDirection = directToPlayer[Random.Range(0, directToPlayer.Length)];
        stareDirection = 1;

        wanderSpeed = enemyState.wanderSpeed;

        isCanWander = true;

        StartCoroutine(changeDirection());
    }

    public string GetName() {
        return "EnemyWander";
    }

    public void UseSkill() {
        detectWall();
        rb2D.linearVelocity = new Vector2(wanderSpeed * selectDirection, rb2D.linearVelocity.y);
        
        if(selectDirection != 0) stareDirection = selectDirection;
        enemyDetect.DetectOfPlayer(stareDirection);
        enemyDetect.DetectBackOfPlayer(stareDirection);
    }

    public bool CanUseSkill() {
        return isCanWander;
    }

    public void detectWall() {
        obstacleHit = Physics2D.Raycast(new Vector2(this.transform.position.x + 0.55f * selectDirection, this.transform.position.y), Vector2.down, 0.55f);
        if (obstacleHit.collider != null) {
            if((Layer.wallLayer & (1 << obstacleHit.collider.gameObject.layer)) != 0) {
                selectDirection *= -1;
                return;
            }
        }   
        else {
            selectDirection *= -1;
        }

        #if UNITY_EDITOR    
        Debug.DrawRay(new Vector2(this.transform.position.x + 0.55f * selectDirection, this.transform.position.y), Vector2.down * 0.55f, Color.green);
        #endif
    }

    IEnumerator changeDirection() {
        selectDirection = directToPlayer[Random.Range(0, directToPlayer.Length)];

        randTime = Random.Range(4f, 7f);

        yield return new WaitForSeconds(randTime);

        StartCoroutine(changeDirection());
    }
}