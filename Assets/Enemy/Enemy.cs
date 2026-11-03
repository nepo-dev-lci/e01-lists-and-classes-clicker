using Unity.Tutorials.Editor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Runtime state (set by Init)")]
    public string enemyName;
    public int health;
    public float speed;

    [Header("References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    GameManager manager;
    bool isDead = false;

    // used to make the speed numbers easier to write
    private float speedCorrection = 0.05f;

    // called by GameManager right after Instantiate.
    public void Init(EnemyTemplate template, GameManager owner)
    {
        manager = owner;
        enemyName = template.enemyName;
        health = template.maxHealth;
        speed = template.speed * speedCorrection;

        if (animator != null && template.animatorController != null)
            animator.runtimeAnimatorController = template.animatorController;

        if (animator != null && template.animationName.IsNotNullOrEmpty())
        {
            animator.Rebind();
            animator.Play(template.animationName, 0, 0f);
        }
    }

    void Update()
    {
        if (isDead) return;
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    void OnMouseDown()
    {
        if (isDead) return;
        manager.OnEnemyClicked(this);
    }
}