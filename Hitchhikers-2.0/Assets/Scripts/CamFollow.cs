using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Transform target;
    [SerializeField] float speed;

    private void Update()
    {
        if (player.currentCar)
            target = player.currentCar.transform;
        else
            target = player.transform;

        if (GameManager.instance.currentGameState == GameState.InGame)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, target.transform.position.z);

            transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }
}
