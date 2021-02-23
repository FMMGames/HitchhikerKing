using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    public int racerIndex;

    public void UpdateIndex()
    {
        for (int i = 0; i < GameManager.instance.racers.Length; i++)
        {
            if (GameManager.instance.racers[i].name == gameObject.name)
                racerIndex = i;
        }
    }
}
