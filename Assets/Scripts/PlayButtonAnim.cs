using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonAnim : MonoBehaviour
{
    public void OnMouseOver()
    {
        print("shit");
        GetComponent<Image>().color = Color.green;
    }
}
