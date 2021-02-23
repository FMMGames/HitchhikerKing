using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    public Transform level;
    [SerializeField] GameObject[] tilePrefabs;
    [SerializeField] GameObject startTile, endTile;
    [SerializeField] int maxTiles;
    [SerializeField] Material roadMat;
    [SerializeField] Color[] roadColors;
    public float levelSize;

    [SerializeField] float tileSize;

    public void GenerateLevel()
    {
        roadMat.color = roadColors[Random.Range(0, roadColors.Length)];

        for (int i = 0; i < maxTiles; i++)
        {
            Vector3 tilePos = new Vector3(transform.position.x, transform.position.y, transform.position.z + (tileSize * i));

            if(i == 0)
            {
                GameObject tile = Instantiate(startTile, tilePos, level.transform.rotation);
                tile.transform.SetParent(level);
            }
            else if(i == maxTiles - 1)
            {
                GameObject tile = Instantiate(endTile, tilePos, level.transform.rotation);
                tile.transform.SetParent(level);
                GameManager.instance.carTargets = tile.GetComponent<Tile>().carTargets;
            }
            else
            {
                int r = Random.Range(0, tilePrefabs.Length);

                GameObject tile = Instantiate(tilePrefabs[r], tilePos, level.transform.rotation);
                tile.transform.SetParent(level);
            }
        }

        level.GetComponent<NavMeshSurface>().BuildNavMesh();
        levelSize = tileSize * maxTiles;
        GameManager.instance.levelGenerated = true;
    }
}
