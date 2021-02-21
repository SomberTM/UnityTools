using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerController : MonoBehaviour
{
    [Range(0, 75)]
    public float CameraPanRange = 60;

    private Camera PlayerCamera;
    private Rigidbody Rigid;
    private bool Grounded;

    public float SensitivityY = 180f;
    public float SensitivityX = 360f;

    public float JumpHeight = 2.0f;
    public float JumpForce = 2.0f;
    private Vector3 Jump;

    public float Step = 5;
    public float SprintStepIncrease = 5;
    public KeyCode SprintKey = KeyCode.LeftShift;

    void Start()
    {
        this.PlayerCamera = this.GetComponentInChildren<Camera>();
        this.Rigid = this.GetComponent<Rigidbody>();
        this.Jump = new Vector3(0.0f, this.JumpHeight, 0.0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.Grounded) {
            this.Rigid.AddForce(this.Jump * this.JumpForce, ForceMode.Impulse);
            this.Grounded = false;
        }

        float MouseX = (Input.mousePosition.x / Screen.width ) - 0.5f;
        float MouseY = (Input.mousePosition.y / Screen.height) - 0.5f;

        float YRotation = Mathf.Min(this.CameraPanRange, Mathf.Max(-this.CameraPanRange, -1f * (MouseY * SensitivityY)));
        PlayerCamera.transform.localRotation = Quaternion.Euler(new Vector4(YRotation, MouseX * SensitivityX, PlayerCamera.transform.localRotation.z));

        float Horizontal = Input.GetAxis("Horizontal"); 
        float Vertical = Input.GetAxis("Vertical");

        Vector3 Forward = PlayerCamera.transform.forward;
        Vector3 Right = PlayerCamera.transform.right;

        Forward.y = 0.0f;
        Right.y = 0.0f;

        Forward.Normalize();
        Right.Normalize();

        Vector3 Desired = (Forward * Vertical) + (Right * Horizontal);

        if (Input.GetKey(this.SprintKey))
            this.transform.Translate(Desired * (Step + this.SprintStepIncrease) * Time.deltaTime);
        else
            this.transform.Translate(Desired * Step * Time.deltaTime);
    }

    void OnCollisionEnter() {
        this.Grounded = true;
    }

    public Camera GetPlayerCamera()
    {
        return this.PlayerCamera;
    }
    
}
