using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProjectileSO", menuName = "Scriptable Objects/EnemyProjectileSO")]
public class EnemyProjectileSO : ScriptableObject
{
    public float damage;
    public float speed;
    public Vector3 size;
    public Sprite projectileImage;
}
