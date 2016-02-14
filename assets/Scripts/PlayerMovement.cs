using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour{

    public float movementSpeed = 5.0f;
    public float rotateSpeed = 50.0f;
    public float catchTime = 0.25f;
    public AudioClip squeak;
    public AudioClip chatter;
    public float jumpPower = 20.0f;
    public bool grounded = true;
    public bool climbing = false;

    bool hasJumped = false;
    Rigidbody rb;
    float lastClickTime;
    CarryingController carryingController;

    void Start(){
        rb = GetComponentInChildren<Rigidbody>();
        TP_Camera.UseExistingOrCreateNewMainCamera();
        carryingController = GetComponent<CarryingController>();
    }

    void Movement(){
        if (!climbing && !carryingController.sliding){
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.5f){
                Quaternion angleNow = transform.rotation;
                Quaternion angleGoal = Quaternion.LookRotation(Quaternion.AngleAxis(TP_Camera.Instance.mouseX, Vector3.up) * Vector3.forward);
                transform.rotation = Quaternion.Slerp(angleNow, angleGoal, Time.deltaTime * 5.0f);
                //				transform.LookAt (transform.position + Quaternion.AngleAxis (TP_Camera.Instance.mouseX, Vector3.up) * Vector3.forward);
            }
            transform.position += transform.forward * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
            transform.position += transform.right * (Time.deltaTime * movementSpeed * Input.GetAxis("Horizontal"));

        }
        else if (climbing){
            transform.position += transform.up * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
            transform.position += transform.right * (Time.deltaTime * movementSpeed * Input.GetAxis("Horizontal"));
        }
        else{
            transform.position += transform.forward * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
        }
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R)){
            transform.rotation = Quaternion.identity;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded == true && !carryingController.sliding){
            hasJumped = true;
        }

        if (hasJumped){
            rb.AddForce(transform.up * jumpPower);
            grounded = false;
            hasJumped = false;
        }

        if (Input.GetMouseButtonDown(1)){
            if (Time.time - lastClickTime < catchTime){
                SoundManager.instance.PlayNamedClipOn(chatter, transform.position, gameObject.name, "chatter", 1.0f, transform);
            }
            else{
                SoundManager.instance.PlayNamedClipOn(squeak, transform.position, gameObject.name, "squeak", 1.0f, transform);
            }
            lastClickTime = Time.time;
        }
        Movement();
    }

    void FixedUpdate(){
        // moved to fixedupdate so after force affects velocity
        if (grounded == false && Mathf.Abs(rb.velocity.y) < 0.01f){
            grounded = true;
        }
    }

    public void startClimbing(){
        climbing = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameObject ratBody = GameObject.Find("CrudeRat");
        ratBody.transform.position += transform.forward * 0.6f;
        ratBody.transform.Rotate(0.0f, 0.0f, -90.0f);
    }

    public void stopClimbing(){
        climbing = false;
        GetComponent<Rigidbody>().useGravity = true;
        GameObject ratBody = GameObject.Find("CrudeRat");
        ratBody.transform.position -= transform.forward * 0.6f;
        ratBody.transform.Rotate(0.0f, 0.0f, +90.0f);
    }
}
