using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public Button playButton;
    public TextMeshProUGUI titleText;
    Transform buttonTransform;
    Color buttonColour;
    GameManager gameManager;

    void Start()
    {
        print("On Menu Screen");
        gameManager = FindObjectOfType<GameManager>();

        print("button is null: " + (playButton == null).ToString());

        titleText = GameObject.FindGameObjectWithTag("Title Text").GetComponent<TextMeshProUGUI>();
        titleText.gameObject.SetActive(true);

        playButton.gameObject.SetActive(true);
        buttonTransform = playButton.transform;
   //        buttonColour = playButton.GetComponent<SpriteRenderer>().color;
    }

    public void PlayButtonClicked()
    {
        //GetComponent<Image>(). // GetComponent<SpriteRenderer>().color = Color.green; 
        FindObjectOfType<GameManager>().ProgressLevel(true);
        titleText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false); 
    }

#if false
    public override void OnPointerEnter(PointerEventData data)
    {   
        GetComponent<SpriteRenderer>().color = Color.green;
        transform.localScale *= 1.2f;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        GetComponent<SpriteRenderer>().color = buttonColour;
        GetComponent<Transform>().localScale = buttonTransform.localScale;
    }
#endif
}
