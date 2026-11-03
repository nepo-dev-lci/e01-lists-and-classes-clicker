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
		// TODO foreach enemy template, spawn an enemy of that type
		// TODO write a for to spawn as many enemies as a `howManyToSpawn`
		UpdateHUD();
	}

	void Update()
	{
		if (gameOver) return;

		// TODO foreach enemy, if they are behind the (defeatLineY), set gameOver to true and show a message
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

		// TODO delete all enemies

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