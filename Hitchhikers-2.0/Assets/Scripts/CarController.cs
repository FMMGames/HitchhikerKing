using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] NavMeshAgent mySelf;
    [SerializeField] SpriteRenderer selectionRing;
    [SerializeField] Collider col;
    public Transform target;
    public Transform playerSpot;
    public float moveSpeed, spdMultiplier;
    [SerializeField] ParticleSystem coinFX;
    [SerializeField] GameObject enemyPrefab;

    public bool hostingPlayer, hostingAI, touched, touchedByAI, hostingEnemy;
    Collider jumpRange;
    float d;

    public void ToggleSelectionRing(bool state)
    {
        selectionRing.gameObject.SetActive(state);
    }

    private void Start()
    {
        mySelf.speed = (Random.Range(0.75f * moveSpeed, 1.25f * moveSpeed)) * spdMultiplier;
        col = GetComponent<Collider>();

        if(GameManager.instance.enemySpawnRate >= Random.value)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        hostingEnemy = true;
        GameObject enemy = Instantiate(enemyPrefab, playerSpot.transform.position, playerSpot.transform.rotation);
        enemy.transform.SetParent(playerSpot);
        enemy.GetComponent<Enemy>().myCar = this;
    }

    private void Update()
    {
        d = (transform.position - target.transform.position).magnitude;

        if (d > 1)
            mySelf.SetDestination(target.position);
        else
            gameObject.SetActive(false);

        if (hostingPlayer)
        {
            if (mySelf.speed < 1.25f * moveSpeed * spdMultiplier)
                mySelf.speed = Mathf.Lerp(mySelf.speed, 1.25f * moveSpeed * spdMultiplier, Time.deltaTime);
        }

        if (jumpRange)
        {
            if (col.bounds.Intersects(jumpRange.bounds) || jumpRange.bounds.Contains(transform.position))
            {
                if (!hostingPlayer && !hostingAI)
                    ToggleSelectionRing(true);
                else
                    ToggleSelectionRing(false);
            }
            else
                ToggleSelectionRing(false);
        }
        else
            ToggleSelectionRing(false);
    }

    public void CoinFX()
    {
        coinFX.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "JumpRange")
            jumpRange = other;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "JumpRange")
            jumpRange = null;
    }
}
