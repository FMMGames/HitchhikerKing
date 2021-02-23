using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Transform[] carTargets;
    [SerializeField] CarSpawner[] mySpawners;
    [SerializeField] float mySpawnInterval;

    private void OnEnable()
    {
        if(mySpawners.Length > 0)
        {
            foreach (var spawner in mySpawners)
            {
                spawner.spawnRate = mySpawnInterval;
            }
        }
        
    }
}
