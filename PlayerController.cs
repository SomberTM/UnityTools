using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerController : Inventory
{
    [Range(0, 75)]
    public float CameraPanRange = 60.0f;

    private Camera PlayerCamera;
    private Rigidbody Rigid;
    private bool Grounded;

    public float SensitivityY = 180.0f;
    public float SensitivityX = 360.0f;

    public float JumpHeight = 2.0f;
    public float JumpForce = 2.0f;
    private Vector3 Jump;

    public float Step = 5;
    public float SprintStepIncrease = 5;
    public KeyCode SprintKey = KeyCode.LeftShift;

    [HideInInspector]
    public bool IsMoving = false;

    [HideInInspector]
    public bool IsSprinting = false;
    
    [HideInInspector]
    public bool IsJumping = false;

    void Start()
    {
        base.OnStart();
        this.PlayerCamera = Utils.ExtractCamera(this);
        this.Rigid = this.GetComponent<Rigidbody>();
        this.Jump = new Vector3(0.0f, this.JumpHeight, 0.0f);
    }

    void Update()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Space) && this.Grounded) {
            this.Rigid.AddForce(this.Jump * this.JumpForce, ForceMode.Impulse);
            this.Grounded = false;
            this.IsJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.B))
            this.Blink();

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

        if (!Desired.Equals(Utils.GetEmptyVector3()))
            this.IsMoving = true;
        else {
            this.IsMoving = false;
            this.IsSprinting = false;
        }

        if (Input.GetKey(this.SprintKey)) {
            this.transform.Translate(Desired * (Step + this.SprintStepIncrease) * Time.deltaTime);
            this.IsSprinting = true;
        } else {
            this.transform.Translate(Desired * Step * Time.deltaTime);
        }
    }

    void Blink(float range = 100.0f) {
        if (Physics.Raycast(this.PlayerCamera.transform.position, this.PlayerCamera.transform.forward, out RaycastHit Hit, range)) {
            Vector3 blinkPos = new Vector3(Hit.point.x, Hit.transform.position.y + (this.transform.localScale.y * 0.5f), Hit.point.z);
            this.transform.position = blinkPos;
        }
    }

    // public bool GetAttachedItem(out Item Attached) {
    //     Attached = this.AttachedItem;
    //     if (this.AttachedItem != null) return true;
    //     return false;
    // }
    // public void AttachItem(Item Attacher) {
    //     this.AttachedItem = Attacher;
    // }

    // public void DetachItem() {
    //     if (this.AttachedItem != null && this.AttachedItem.TryGetComponent<Rigidbody>(out Rigidbody Body))
    //     {
    //         Body.AddForce(this.transform.forward, ForceMode.Impulse);
    //         Body.AddForce(this.transform.up, ForceMode.Impulse);
    //         this.AttachedItem = null;
    //     }  
    // }

    void OnCollisionEnter() {
        this.Grounded = true;
        this.IsJumping = false;
    }

    public Camera GetPlayerCamera()
    {
        return this.PlayerCamera;
    }
    
}
