using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Gun : MonoBehaviour
{
    public Gun[] guns;
    public float turnRate;

    void Update()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            print("gun " + i.ToString());
            guns[i].transform.Rotate(Vector3.up * Time.deltaTime * turnRate);
            guns[i].Shoot();
        }
    }
}
