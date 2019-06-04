// Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Gun : MonoBehaviour
{
    // An array of all the guns the boss (multiple)
    public Gun[] guns;

    // Turning speed
    public float turnRate;

    // Update function called once per frame
    void Update()
    {
        // Iterate through the boss' guns
        for (int i = 0; i < guns.Length; i++)
        {
            // Rotate the gun
            guns[i].transform.Rotate(Vector3.up * Time.deltaTime * turnRate);

            // Shoot the gun... bullet shot timing is automatically handled by the Gun class
            guns[i].Shoot();
        }
    }
}
