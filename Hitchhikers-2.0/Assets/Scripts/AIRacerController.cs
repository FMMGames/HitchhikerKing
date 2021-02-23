using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIRacerController : MonoBehaviour
{
    [SerializeField] Racer myRacer;
    [SerializeField] LayerMask jumpableLayer, carLayer;
    public Animator anim;
    [SerializeField] Renderer rend;
    [SerializeField] Material[] skins;

    public CarController currentCar, targetCar;
    [SerializeField] float reactionTime, carJumpRange;

    [SerializeField] float reactionCooldown;
    [SerializeField] bool jumping;

    private void Start()
    {
        reactionCooldown = reactionTime;
        ResetAppearance();
    }

    public void ResetAppearance()
    {

        rend.material = skins[Random.Range(0, skins.Length)];
    }

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameState.InGame)
            JumpControl();

        if (currentCar != null)
            transform.position = currentCar.playerSpot.position;
    }

    void JumpControl()
    {
        if (!jumping)
        {
            if (!targetCar)
                targetCar = PickTargetCar();
            else
            {
                reactionCooldown -= Time.deltaTime;

                float d = (transform.position - targetCar.transform.position).magnitude;

                if (d > carJumpRange)
                {
                    ResetDetection();
                }

                if (reactionCooldown <= 0)
                {
                    if (targetCar.hostingAI || targetCar.hostingPlayer)
                    {
                        ResetDetection();
                    }
                    else
                    {
                        if (d <= carJumpRange && !jumping)
                        {
                            if (GameManager.instance.AIDifficulty >= Random.value)
                            {
                                Jump();
                                reactionCooldown = Random.Range(reactionTime * 0.9f, reactionTime * 1.1f);
                            }
                            else
                                ResetDetection();
                        }
                    }
                }
            }
        }     
    }

    void ResetDetection()
    {
        reactionCooldown = Random.Range(reactionTime * 0.9f, reactionTime * 1.1f);
        targetCar = null;
        return;
    }

    CarController PickTargetCar()
    {
        if (Physics.CheckSphere(transform.position, carJumpRange, carLayer))
        {
            Collider[] carsNearby = Physics.OverlapSphere(transform.position, carJumpRange, carLayer);

            if (carsNearby[0].GetComponent<CarController>() != currentCar)
            {
                if (!carsNearby[0].GetComponent<CarController>().hostingAI && !carsNearby[0].GetComponent<CarController>().hostingPlayer)
                    return carsNearby[0].GetComponent<CarController>();
                else
                {
                    //competition mechanics
                    return null;
                }
            }
            else
                return null;
        }
        else
            return null;
    }

    void Jump()
    {
        if (targetCar != null)
        {
            Vector3 pos = new Vector3(targetCar.transform.position.x, transform.position.y, targetCar.transform.position.z);
            transform.LookAt(pos);

            JumpToCar(targetCar);
        }
    }

    public void JumpToCar(CarController car)
    {
        jumping = true;
        anim.SetBool("Jumping", jumping);

        targetCar.hostingAI = true;

        if (currentCar)
            currentCar.hostingAI = false;

        transform.SetParent(car.transform);

        Invoke("EndJumpState", 0.9f);

        transform.DOLocalJump(car.playerSpot.localPosition, 2.5f, 1, 1.2f).OnComplete(() => { EndJump();});
    }

    void EndJump()
    {
        ResetCars();
        ResetDetection();
    }

    void ResetCars()
    {
        currentCar = targetCar; 
        targetCar = null;
    }

    void EndJumpState()
    {
        jumping = false;
        anim.SetBool("Jumping", jumping);
    }
}
