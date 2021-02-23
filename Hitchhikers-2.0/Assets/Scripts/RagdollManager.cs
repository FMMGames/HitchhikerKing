using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    [SerializeField] Rigidbody[] rbs;
    public Rigidbody hip;
    [SerializeField] Collider[] cols;
    [SerializeField] GameObject weapon;
    [SerializeField] Animator anim;
    [SerializeField] float deathPushForce;

    public void Setup()
    {
        anim = GetComponent<Animator>();

        if (GetComponentInChildren<ParticleSystem>())
            weapon = GetComponentInChildren<ParticleSystem>().transform.parent.gameObject;

        rbs = GetComponentsInChildren<Rigidbody>();
        cols = GetComponentsInChildren<Collider>();

        hip = rbs[1];

        for (int i = 0; i < rbs.Length; i++)
        {
            if (i > 0)
            {
                rbs[i].isKinematic = true;
                rbs[i].useGravity = false;
            }
        }

        for (int i = 0; i < cols.Length; i++)
        {
            if (i > 0)
                cols[i].enabled = false;
        }
    }

    public void ToggleRagdoll(Vector3 impactOrigin)
    {
        anim.enabled = false;

        if (weapon)
        {
            weapon.transform.parent = null;
            Destroy(weapon, 10f);
        }

        for (int i = 0; i < rbs.Length; i++)
        {
            if(i>0)
            {
                if (rbs[i])
                {
                    rbs[i].isKinematic = false;
                    rbs[i].useGravity = true;
                }
            }
        }

        for (int i = 0; i < cols.Length; i++)
        {
            if (i > 0)
            {
                if(cols[i])
                cols[i].enabled = true;
            }
        }

        Vector3 dir = (transform.position - impactOrigin).normalized;
        hip.AddForce(deathPushForce * dir, ForceMode.Impulse);
    }
}
