using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
    public float speed = 25.0f;
    public float rotationSpeed = 90;
    public float force = 1f;
    public float invertCameraSpeed = .5f;

    public GameObject playerCamera;
    public float cameraRotaton = 0f;

    Rigidbody rb;
    Transform t;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        playerCamera = gameObject.transform.Find("PlayerCamera").gameObject;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        Debug.Log(t.up.y);
        Debug.Log(Physics.gravity.normalized.y);
        if(t.up.y != -Physics.gravity.normalized.y & (Mathf.Abs(t.up.y + Physics.gravity.normalized.y) > .001)){
            t.RotateAround(t.position, this.transform.forward, 1);
            // t.rotation *= Quaternion.Euler(0,0,180);
        } else{
            if (Input.GetKey(KeyCode.D))
            t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
            else if (Input.GetKey(KeyCode.A))
                t.rotation *= Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0);

            if (Input.GetKey(KeyCode.W))
                rb.velocity += this.transform.forward * speed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.S))
                rb.velocity -= this.transform.forward * speed * Time.deltaTime;

            // Quaternion returns a rotation that rotates x degrees around the x axis and so on

            if (Input.GetKeyDown(KeyCode.Space))
                rb.AddForce(t.up * force);
        }
    }

    public override void OnStartLocalPlayer(){
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}