using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //Private Serialized Fields
    [SerializeField]
    private Transform exitPoint;
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private float navigationUpdate;
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int rewardAmt;

    //Private Variables
    private int target = 0;
    private Transform enemy;
    private Collider2D enemyCollider;
    private Animator anima;
    private float navigationTime;
    private bool isDead = false;

    //Getters
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

	// Start Function
	void Start () {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anima = GetComponent<Animator>();
        GameManager.Instance.RegisterEnemy(this);
	}
	
	// Update Function
	void Update () {
		if (waypoints != null && !isDead)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                if (target < waypoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
	}

    //Void Functions
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Checkpoint")
        {
            target += 1;
        } else if(other.tag == "Finish")
        {
            GameManager.Instance.RoundEscaped += 1;
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.UnregisterEnemy(this);
            GameManager.Instance.isWaveOver();
        } else if (other.tag == "projectiles"){
            Projectile newP = other.gameObject.GetComponent<Projectile>();
            enemyHit(newP.AttackStrength);
            Destroy(other.gameObject);
        }
    }

    //Public Void Functions
    public void enemyHit(int hitPoints)
    {
        if (healthPoints - hitPoints > 0)
        {
            healthPoints -= hitPoints;
            anima.Play("Hurt");
        } else {
            anima.SetTrigger("didDie");
            die();
        }
    }

    public void die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        GameManager.Instance.TotalKilled += 1;
        GameManager.Instance.addMoney(rewardAmt);
        GameManager.Instance.isWaveOver();
    }

    public void DestroyAllProjectiles()
    {

    }
}
