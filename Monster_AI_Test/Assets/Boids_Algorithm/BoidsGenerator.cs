using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsGenerator : MonoBehaviour
{
    [SerializeField] private BoidUnit boidUnitPrefab;
    public int boidCount;
    [SerializeField] private float spawnRange = 1;

    void Start()
    {
        for(int i = 0; i< boidCount; i++)
        {
            Vector2 randomVec = Random.insideUnitCircle;
            randomVec *= spawnRange;

            Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0, 360f));
            BoidUnit currUnit = Instantiate(boidUnitPrefab, randomVec, randomRot);
            currUnit.transform.SetParent(this.transform);
            currUnit.InitializeUnit(1);
        }
    }
}
