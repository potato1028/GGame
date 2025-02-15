using UnityEngine;
using System.Collections;

public class EnemyWander : MonoBehaviour, EnemySkill_Interface {
    [Header("Enemy State")]
    public int[] direction;
    public int selectDirection;
    public float wanderSpeed;
    public float randTime;

    public bool isCanWander;
    public RaycastHit2D hit;

    [Header("Component")]
    public Rigidbody2D rb2D;

    void Awake() {
        rb2D = this.GetComponent<Rigidbody2D>();
    }

    void Start() {
        direction = new int[] {-1, 0, 1};
        selectDirection = direction[Random.Range(0, direction.Length)];
        wanderSpeed = 1f;

        isCanWander = true;

        StartCoroutine(changeDirection());
    }

    void Update() {
        detectWall();
    }

    public string GetName() {
        return "EnemyWander";
    }

    public void UseSkill(Vector2 directionToPlayer, float distanceToPlayer, Vector2[] detectionPoints) {
        rb2D.linearVelocity = new Vector2(wanderSpeed * selectDirection, rb2D.linearVelocity.y);
    }

    public bool CanUseSkill() {
        return isCanWander;
    }

    public void detectWall() {
        hit = Physics2D.Raycast(new Vector2(this.transform.position.x + 0.55f * selectDirection, this.transform.position.y), Vector2.down, 0.55f);
        if (hit.collider != null) {
            if((Layer.wallLayer & (1 << hit.collider.gameObject.layer)) != 0) {
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
        selectDirection = direction[Random.Range(0, direction.Length)];

        randTime = Random.Range(4f, 7f);

        yield return new WaitForSeconds(randTime);

        StartCoroutine(changeDirection());
    }
}