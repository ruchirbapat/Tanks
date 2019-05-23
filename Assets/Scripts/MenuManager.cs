using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public Button playButton;

    Transform buttonTransform;
    Color buttonColour;

    void Start()
    {
        buttonTransform = playButton.transform;
        buttonColour = playButton.GetComponent<SpriteRenderer>().color;
    }

    public void PlayButtonClicked()
    {
        print("clicked");
        GetComponent<Image>().GetComponent<SpriteRenderer>().color = Color.green; 
        //FindObjectOfType<GameManager>().NextLevel();
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
