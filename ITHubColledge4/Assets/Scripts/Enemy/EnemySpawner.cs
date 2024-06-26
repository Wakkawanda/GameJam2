using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using weed;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private float timeoutInSeconds;
    [Tooltip("Enemy might spawn on the player; so to avoid that, we'll first check the radius and if the player will be there; if not, we'll spawn the enemy.")]
    [SerializeField] private float radiusOfPlayerSafety;
    [SerializeField] private Vector3 point1; // maybe gameobject?
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector3 point2;
    [SerializeField] private GameObject smokeFX;
    private readonly string spawnFunc = "Spawn";
    private bool failSpawn = true;
    private Coroutine _coroutine;

    [SerializeField] private float contDiff = 0;
    [SerializeField] private int beginDiff = 0;
    private Wallet _wallet;

    // check timer && overtime is harder
    // each First/secnod/third aiblity makes more mobs at spawn

    [Inject]
    public void Construct(Wallet wallet)
    {
        _wallet = wallet;
    }

    private void Start()
    {
        failSpawn = true;
        
        // this shit will be theoretical
        if (UnlockSpells.First) beginDiff++;
        if (UnlockSpells.Second) beginDiff++;
        if (UnlockSpells.Three) beginDiff++;
        
        _coroutine = StartCoroutine(spawnFunc);
    }

    IEnumerator Spawn() 
    {
        while (true)
        {
            if (_wallet.GetMoneyValue() > Barman.Prices)
            {
                contDiff += 2;
            }
            
            contDiff += Time.fixedDeltaTime;
            yield return new WaitForSeconds(
                Mathf.Max(timeoutInSeconds - contDiff, 0.5f)
            );
            for (int i = 0; i < beginDiff + 1; i++) 
            {
                int enemyIndex = RandomBetweenFloor(0, enemyList.Count);
                GameObject enemyToSpawn = enemyList[enemyIndex]; 
                Vector3 spot = Vector3.zero;
                int tryAttempts = 0;
                do {
                    spot = RandomBetweenFloor(point1, point2);
                    failSpawn = CheckForPlayer(spot);
                    tryAttempts++;
                } while (failSpawn && tryAttempts > 100);
                if (tryAttempts > 100) Debug.Log("Someone couldn't be lucky enough to get spawned.");

                /*may need adjustments, needs to be >0 Z level for the smoke to not be hidden*/
                if (!failSpawn)
                {
                    Instantiate(smokeFX, spot, Quaternion.identity);

                    yield return new WaitForSeconds(0.3f);
                    
                    Instantiate(enemyToSpawn, spot, Quaternion.identity, this.transform);
                }
            } 
        }
    }

    public void Remove()
    {
        StopCoroutine(_coroutine);
        gameObject.SetActive(false);
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
        Collider2D[] playerCollider = Physics2D.OverlapCircleAll(here,
            radiusOfPlayerSafety,
            playerLayer);
        return playerCollider.Length > 0;
    }
}
