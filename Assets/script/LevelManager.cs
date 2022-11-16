using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int hitpoint = 3;
    private int score = 0;


    public Vector3 spawnPosition;
    public Transform playerTransform;


    //every single frame
    private void FixedUpdate()
    {
        if (playerTransform.position.y < -10)
        {

            playerTransform.position = spawnPosition;
            hitpoint--;
            if (hitpoint <= 0)
            {
                Debug.Log("Failure");
            }

        }
    }

}
