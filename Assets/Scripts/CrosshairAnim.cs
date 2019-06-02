using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairAnim : MonoBehaviour
{
    public LayerMask hitMask;
    public SpriteRenderer dotRend;
    public Color dotHighlightedColor;
    public float turnSpeed = 50f;
    Color dotOriginalColor;

    private void Start()
    {
        dotOriginalColor = dotRend.color;
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
    }

    public void MatchTargets(Ray ray)
    {
        if(Physics.Raycast(ray, 1000, hitMask))
        {
            dotRend.color = dotHighlightedColor;
        } else
        {
            dotRend.color = dotOriginalColor;
        }
    }
}
