using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public CarController myCar;
    [SerializeField] GameObject GFX;
    [SerializeField] GameObject deathFX;
    [SerializeField] RagdollManager ragdoll;
    [SerializeField] Animator anim;
    [SerializeField] GameObject pistol, rifle;
    [SerializeField] LayerMask racers, ground;

    [SerializeField] int type;
    [SerializeField] int moneyReward;
    [SerializeField] float detectionRange;

    bool dead;

    private void OnEnable()
    {
        type = Random.Range(0, 3);

        if(type <= 0)
            anim.SetBool("Melee", true);
        else
        {
            if(type <= 1)
            {
                anim.SetBool("Rifle", true);
                rifle.SetActive(true);
            }
            else
            {
                anim.SetBool("Pistol", true);
                pistol.SetActive(true);
            }
        }

        ragdoll.Setup();
    }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, detectionRange, racers))
        {
            Collider[] racersNearby = Physics.OverlapSphere(transform.position, detectionRange, racers);

            transform.LookAt(new Vector3(racersNearby[0].transform.position.x, transform.position.y, racersNearby[0].transform.position.z));
        }

        RaycastHit hit;
        if (Physics.Raycast(ragdoll.hip.transform.position, -transform.up, out hit, 0.5f, ground))
        {
            if (!dead)
            {
                Death();
            }

            dead = true;
        }
    }

    void Death()
    {
        GFX.SetActive(false);
        GameObject fx = Instantiate(deathFX, ragdoll.hip.transform.position, deathFX.transform.rotation);
        Destroy(gameObject, 5f);
        Destroy(fx, 2f);
    }

    void DeathCheck()
    {
        if (!dead)
            Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Racer>() && GameManager.instance.currentGameState == GameState.InGame)
        {
            other.GetComponent<Racer>().UpdateIndex();
            GameManager.instance.EarnKillScore(other.GetComponent<Racer>().racerIndex);
            GameManager.instance.EarnMoney(moneyReward);
            myCar.CoinFX();
            myCar.hostingEnemy = false;
            Invoke("DeathCheck", 5f);
            ragdoll.ToggleRagdoll(other.transform.position);
        }
    }
}
