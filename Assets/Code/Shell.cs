using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{

    private Rigidbody attachedRigidbody;
    private float maxForce = 120;
    private float minForce = 90;
    private float lifetime = 1;
    private float fadeTime = 1;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(minForce, maxForce));
        GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * Random.Range(minForce, maxForce));

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(lifetime);
        float percent = 0;
        //float fadeSpeed = 1 / fadeTime;
        Material material = GetComponent<Renderer>().material;
        Color original = material.color;
        while (percent < 1) {
            percent += Time.deltaTime;
            material.color = Color.Lerp(original, Color.clear, percent);
            yield return null;
        }
        Destroy(gameObject);
    }
}
