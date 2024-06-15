using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private float timeoutInSeconds;
    [Tooltip("Enemy might spawn on the player; so to avoid that, we'll first check the radius and if the player will be there; if not, we'll spawn the enemy.")]
    [SerializeField] private float radiusOfPlayerSafety;
    [SerializeField] private Vector3 point1; // maybe gameobject?
    [SerializeField] private LayerMask layer;
    [SerializeField] private Vector3 point2;
    private readonly string spawnFunc = "Spawn";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(spawnFunc);
    }

    IEnumerator Spawn() 
    {
        int enemyIndex = RandomBetweenFloor(0, enemyList.Count);
        GameObject enemyToSpawn = enemyList[enemyIndex];
        bool failSpawn = true; 
        do {
            Vector3 spot = RandomBetweenFloor(point1, point2);
            failSpawn = CheckForPlayer(spot);
            if (!failSpawn) 
            {
                Instantiate(enemyToSpawn, spot, Quaternion.identity, this.transform);
            };
            yield return new WaitForSeconds(timeoutInSeconds); 
        } while (failSpawn);
        yield break;
    }

    int RandomBetweenFloor(float min, float max) 
    {
        return (int) (min + (max - min) * Random.value);
    }

    Vector3 RandomBetweenFloor(Vector3 min, Vector3 max) 
    {
        return new Vector3(
            Mathf.Lerp(min.x, max.x, Random.value),
            Mathf.Lerp(min.y, max.y, Random.value),
            Mathf.Lerp(min.z, max.z, Random.value)
        );
    }

    bool CheckForPlayer(Vector3 here)
    {
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(here,
            radiusOfPlayerSafety, 
            layer);
        return groundColliders.Length > 1;
    }
}
