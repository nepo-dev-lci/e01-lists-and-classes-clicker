using UnityEngine;

[System.Serializable]
public class EnemyTemplate
{
    public string enemyName = "Goblin";
    public int howManyToSpawn = 1;
    public int maxHealth = 3;
    public float speed = 1.5f;

    [Header("Visuals")]
    public string animationName;
    public RuntimeAnimatorController animatorController;
}