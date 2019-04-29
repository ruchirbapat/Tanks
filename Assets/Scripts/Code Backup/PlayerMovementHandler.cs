using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    public Camera camera;
    public float moveSpeed;
    public GameObject gun;

    [System.NonSerialized]
    public Vector3 deltaPosition;
    private float prog;
    public float rotDuration;
    Quaternion targetRot;
    Vector3 prevInput;

    void Awake()
    {
        deltaPosition = Vector3.zero;
        prog = 0;
        prevInput = Vector3.zero;
    }

    void Start() {
        camera = FindObjectOfType<Camera>();

        if (gun == null)
            Debug.LogWarning("Gun object not assigned to player");
    }

    void HandleKeyInputs()
    {
        Vector3 latestInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        /*
        if ((latestInput != Vector3.zero) || (transform.forward.normalized != Vector3.zero)) {
            targetRot = Quaternion.LookRotation(latestInput);
            if ((!(prog > rotDuration)) || (transform.rotation != targetRot)) {
                prog += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, prog / rotDuration);
            } else {
                prog = 0;
            }
        }*/

        if (latestInput != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(latestInput);

        deltaPosition = latestInput * moveSpeed;

        /*

        if ((latestInput != Vector3.zero) && (latestInput != prevInput)) {

            targetRot = Quaternion.LookRotation(latestInput);


            while (transform.rotation != targetRot) {
                while (prog < rotDuration) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, prog / rotDuration);
                    prog += Time.deltaTime;
                }
                prog = 0;
            }
            deltaPosition = latestInput * moveSpeed;
        }
        prevInput = latestInput;

        */
    }

    void HandleRaycasting()
    {
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        float distanceToIntersection;

        // intersect with a plane at the same level as the gun to fix a weird parallax issue
        Plane eyeLevelIntersectionPlane = new Plane(Vector3.up, Vector3.up * (gun.transform.position.y - transform.position.y));

        Debug.DrawRay(mouseRay.origin, mouseRay.direction, Color.red);

        if (eyeLevelIntersectionPlane.Raycast(mouseRay, out distanceToIntersection)) {
            Vector3 hit = mouseRay.GetPoint(distanceToIntersection);
            gun.transform.LookAt(new Vector3(hit.x, gun.transform.position.y, hit.z));
        }
    }

    void Update() {
        HandleKeyInputs();
        HandleRaycasting();
    }
}
