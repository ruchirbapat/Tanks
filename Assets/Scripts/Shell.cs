using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour
{

    public Rigidbody attachedRigidbody;
    public float forceMin;
    public float forceMax;

    public float lifetime = 4;
    public float fadetime = 2;

    void Start()
    {
        float force = Random.Range(forceMin, forceMax);
        attachedRigidbody.AddForce(transform.right * force);
        attachedRigidbody.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1 / fadetime;
        Material mat = GetComponent<Renderer>().material;
        Color initialCol = mat.color;
        while (percent < 1) {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialCol, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }
}