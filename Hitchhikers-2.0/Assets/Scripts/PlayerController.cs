using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Racer myRacer;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask jumpableLayer, carLayer;
    [SerializeField] GameObject jumpMarker, jumpRangeDisplay;
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem confettiFX, speedFX;

    public CarController currentCar, targetCar;
    [SerializeField] float carDetectionRange, carJumpRange;
    [SerializeField] float slowmoSpeed, variationSpeed;

    public bool slowmo, jumping;
  
    private void Update()
    {
        if(GameManager.instance.currentGameState != GameState.EndScreen)
        JumpControl();

        if(GameManager.instance.currentGameState != GameState.MainScreen)
        {
            if (!speedFX.isPlaying)
                speedFX.Play();
        }
        else
        {
            if (speedFX.isPlaying)
                speedFX.Stop();
        }

        if (currentCar != null)
            transform.position = currentCar.playerSpot.position;

        if(jumpMarker.activeInHierarchy)
        {
            Vector3 levelPos = new Vector3(jumpMarker.transform.position.x, transform.position.y, jumpMarker.transform.position.z);
            transform.LookAt(levelPos);
        }
    }

    void JumpControl()
    {
        if (Input.GetButtonDown("Fire1") && !jumping)
            slowmo = true;
        if (Input.GetButtonUp("Fire1"))
        {
            float d = (transform.position - jumpMarker.transform.position).magnitude;

            if (d <= carJumpRange && !jumping)
            {
                Jump();               
            }

            slowmo = false;
        }

        if (slowmo)
        {
            PositionMarker();
            targetCar = PickTargetCar();

            if (Time.timeScale != slowmoSpeed)
                Time.timeScale = Mathf.Lerp(Time.timeScale, slowmoSpeed, variationSpeed * Time.deltaTime);
        }
        else
        {
            if (jumpMarker.activeInHierarchy)
            {
                jumpMarker.SetActive(false);
                jumpRangeDisplay.SetActive(false);
            }

            if (Time.timeScale != 1)
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1, variationSpeed * Time.deltaTime);
        }
    }

    void PositionMarker()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, jumpableLayer))
        {
            if (!jumpMarker.activeInHierarchy)
            {
                jumpMarker.SetActive(true);
                jumpRangeDisplay.SetActive(true);
            }

            jumpMarker.transform.position = hit.point;
            jumpRangeDisplay.transform.position = new Vector3(jumpRangeDisplay.transform.position.x, jumpMarker.transform.position.y + 0.5f, jumpRangeDisplay.transform.position.z);
        }
        else
        {
            jumpMarker.transform.position = transform.position;
            jumpMarker.SetActive(false);
            jumpRangeDisplay.SetActive(false);
        }
    }

    CarController PickTargetCar()
    {
        if (Physics.CheckSphere(jumpMarker.transform.position, carDetectionRange, carLayer))
        {
            Collider[] carsNearby = Physics.OverlapSphere(jumpMarker.transform.position, carDetectionRange, carLayer);

            if (carsNearby[0].GetComponent<CarController>() != currentCar && !carsNearby[0].GetComponent<CarController>().hostingAI)
                return carsNearby[0].GetComponent<CarController>();
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
            JumpToCar(targetCar);

            if (GameManager.instance.currentGameState == GameState.MainScreen)
                GameManager.instance.LevelStart();           
        }
    }

    public void JumpToCar(CarController car)
    {
        jumping = true;
        anim.SetBool("Jumping", jumping);

        transform.SetParent(car.transform);

        Invoke("EndJumpState", 0.9f);

        transform.DOLocalJump(car.playerSpot.localPosition, 2.5f, 1, 1.2f).OnComplete(() => { EndJump(); });       
    }

    void EndJump()
    {
        ResetCars();
    }

    void ResetCars()
    {
        if (currentCar)
            currentCar.hostingPlayer = false;

        currentCar = targetCar;
        currentCar.hostingPlayer = true;

        currentCar.touched = true;
        targetCar = null;
    }

    void EndJumpState()
    {
        jumping = false;
        anim.SetBool("Jumping", jumping);
    }

    public void ResetPlayer()
    {
        transform.position = GameManager.instance.racerSpawnPoints[0].position;
        transform.rotation = GameManager.instance.racerSpawnPoints[0].rotation;

        currentCar = null;
        transform.parent = null;

        anim.SetTrigger("Reset");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EndTile")
        {
            if(GameManager.instance.currentGameState == GameState.InGame)
            {
                GameManager.instance.LevelEnd();
                confettiFX.Play();
            }
        }
    }
}
