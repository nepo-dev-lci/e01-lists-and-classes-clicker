using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject enemyPrefab;
    public float spawnY = 5f;
    public float minX = -7f;
    public float maxX = 7f;

    [Header("Round (author in Inspector)")]
    public List<EnemyTemplate> roundRoster = new List<EnemyTemplate>();

    [Header("Defeat")]
    public float defeatLineY = -5f;

    [Header("UI")]
    public TextMeshProUGUI countText;
    public GameObject bombButton;

    List<Enemy> active = new List<Enemy>();
    bool gameOver = false;

    void Start()
    {
        SpawnRound();
    }

    void SpawnRound()
    {
        foreach (EnemyTemplate t in roundRoster)
        {
            for (int i=0; i<t.howManyToSpawn; i++)
            {
                GameObject go = Instantiate(enemyPrefab);

                Vector3 pos = new Vector3(Random.Range(minX, maxX), spawnY, 0f);
                go.transform.position = pos;

                Enemy e = go.GetComponent<Enemy>();
                e.Init(t, this);
                active.Add(e);
            }
        }
        UpdateHUD();
    }

    void Update()
    {
        if (gameOver) return;

        foreach (Enemy e in active)
        {
            if (e.transform.position.y < defeatLineY)
            {
                gameOver = true;
                Debug.Log("Game over — an enemy reached the bottom!");
                return;
            }
        }
    }

    public void OnEnemyClicked(Enemy e)
    {
        if (gameOver) return;

        e.TakeDamage(1);

        if (e.health <= 0)
        {
            active.Remove(e);
            Destroy(e.gameObject);
        }

        UpdateHUD();
        if (active.Count == 0) RoundCleared();
    }

    public void OnBombPressed()
    {
        if (gameOver) return;

        List<Enemy> toRemove = new List<Enemy>();

        foreach (Enemy e in active)
        {
            e.TakeDamage(5);
            if (e.health <= 0) toRemove.Add(e);
            // active.Remove(e);  // <-- uncomment to show the exception
        }

        foreach (Enemy e in toRemove)
        {
            active.Remove(e);
            Destroy(e.gameObject);
        }

        UpdateHUD();
        if (active.Count == 0) RoundCleared();
    }

    void RoundCleared()
    {
        gameOver = true;
        Debug.Log("Round cleared!");
        countText.text = "Round cleared!";
    }

    void UpdateHUD()
    {
        if (countText != null)
            countText.text = "Enemies: " + active.Count;
    }

    void OnDrawGizmos()
    {
        // draw defeat line
        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.6f);
        Vector3 a = new Vector3(minX, defeatLineY, 0f);
        Vector3 b = new Vector3(maxX, defeatLineY, 0f);
        Gizmos.DrawLine(a, b);

        // add box after line so it reads as a boundary
        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.15f);
        Vector3 center = new Vector3((minX + maxX) * 0.5f, defeatLineY - 0.25f, 0f);
        Vector3 size = new Vector3(maxX - minX, 0.5f, 0.1f);
        Gizmos.DrawCube(center, size);
    }

    void OnDrawGizmosSelected()
    {
        // thicker preview of defeat line
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minX, defeatLineY, 0f),
                        new Vector3(maxX, defeatLineY, 0f));

        // spawn line
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, spawnY, 0f),
                        new Vector3(maxX, spawnY, 0f));
    }
}