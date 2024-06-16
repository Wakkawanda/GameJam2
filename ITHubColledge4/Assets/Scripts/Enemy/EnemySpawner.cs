using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weed;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private float timeoutInSeconds;
    [Tooltip("Enemy might spawn on the player; so to avoid that, we'll first check the radius and if the player will be there; if not, we'll spawn the enemy.")]
    [SerializeField] private float radiusOfPlayerSafety;
    [SerializeField] private Vector3 point1; // maybe gameobject?
    [SerializeField] private LayerMask layer;
    [SerializeField] private Vector3 point2;
    [SerializeField] private GameObject smokeFX;
    private readonly string spawnFunc = "Spawn";
    private bool failSpawn = true;

    [SerializeField] private float contDiff = 0;
    [SerializeField] private int beginDiff = 0;

    // check timer && overtime is harder
    // each First/secnod/third aiblity makes more mobs at spawn

    private void Start()
    {
        failSpawn = true;
        
        // this shit will be theoretical
        if (UnlockSpells.First) beginDiff++;
        if (UnlockSpells.Second) beginDiff++;
        if (UnlockSpells.Three) beginDiff++;
        


        StartCoroutine(spawnFunc);
    }

    IEnumerator Spawn() 
    {
        while (true)
        {
            for (int i = 0; i < beginDiff + 1; i++) 
            {
                int enemyIndex = RandomBetweenFloor(0, enemyList.Count);
                GameObject enemyToSpawn = enemyList[enemyIndex]; 
                Vector3 spot = RandomBetweenFloor(point1, point2);
                /*may need adjustments, needs to be >0 Z level to not be hidden*/
                Instantiate(smokeFX, spot, Quaternion.identity);
                Instantiate(enemyToSpawn, spot, Quaternion.identity, this.transform);
            } 
            contDiff += Time.deltaTime;
            yield return new WaitForSeconds(
                Mathf.Max(timeoutInSeconds - contDiff, 0)
            );
        }
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
}
