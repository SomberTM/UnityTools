using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] Cameras;
    public int CameraIndex = 0;

    public Camera ActiveCamera;
    public bool DrawActiveCameraDirectionGizmo = true;
    public Color ActiveCameraGizmoColor = Color.green;
    public bool DrawNonActiveCamerasDirectionGizmo = false;
    public Color NonActiveCameraGizmoColor = Color.red;
    public float CameraGizmoLength = 10.0f;

    void Start()
    {
        this.InitCamera();
    }

    void Update()
    {
        this.UpdateCamera();
    }

    private bool HasCameras() {
        return this.Cameras.Length > 0;
    }

    private void UpdateCameraState(int index) {
        if (this.CameraIndex == index) {
            this.ActiveCamera = this.Cameras[index];
            this.ActiveCamera.enabled = true;
            this.OnCameraSwitch(this.ActiveCamera);
        } else 
            this.Cameras[index].enabled = false;
    }       

    private void InitCamera() {
        if (this.HasCameras())
            for (int i = 0; i < this.Cameras.Length; i++)   
                this.UpdateCameraState(i);
    }

    private void UpdateCamera() {
        if (this.HasCameras() && Input.GetKeyDown(KeyCode.C)) {
            if (this.CameraIndex < this.Cameras.Length - 1)
                this.CameraIndex++;
            else this.CameraIndex = 0;

            for (int i = 0; i < this.Cameras.Length; i++)
                this.UpdateCameraState(i);
        }
    }

    public virtual void OnCameraSwitch(Camera NewCamera) {
        Debug.Log(string.Format("[{0}]: Display camera is now \"{1}\"", this.name, NewCamera.name));
    }

    void OnDrawGizmos() {
        if (!this.ActiveCamera)
            this.ActiveCamera = this.Cameras[this.CameraIndex];

        if (this.DrawActiveCameraDirectionGizmo) {
            Gizmos.color = this.ActiveCameraGizmoColor;
            Gizmos.DrawLine(this.ActiveCamera.transform.position, this.ActiveCamera.transform.position + this.ActiveCamera.transform.forward * this.CameraGizmoLength);
            Vector3 forward = this.ActiveCamera.transform.forward;
        }

        if (this.DrawNonActiveCamerasDirectionGizmo)
            for (int i = 0; i < this.Cameras.Length; i++)
                if (i != this.CameraIndex) {
                    Gizmos.color = this.NonActiveCameraGizmoColor;
                    Camera current = this.Cameras[i];
                    Gizmos.DrawLine(current.transform.position, current.transform.position + current.transform.forward * this.CameraGizmoLength);
                }
    }
}
