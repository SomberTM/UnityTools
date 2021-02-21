﻿using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Usable : MonoBehaviour
{
    public GameObject[] UsableBy;
    public float UsableRadius = 5;
    public bool DrawUsableRadiusGizmo = true;
    public bool DebugOnUnusedVirtuals = true;
    public Color GizmoUsableColor = Color.green;
    public Color GizmoUnusableColor = Color.red;

    public KeyCode UsableKey = KeyCode.E;
    public Text HoverText;

    protected bool Available = true;

    protected void Init()
    {
        this.FormatHoverText();
        this.DisplayHoverText(false);
    }

    protected void Tick() 
    {
        foreach (GameObject By in this.UsableBy) {
            // Simple way to check if a GameObject is a player by attempting to get its player controller script
            if (By.TryGetComponent<PlayerController>(out PlayerController Controller))
            {
                // Get the camera assigned to this player
                Camera PlayerCamera = Controller.GetPlayerCamera();

                Vector3 CameraCenter = PlayerCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, PlayerCamera.nearClipPlane));
                RaycastHit Hit;
                
                if (Physics.Raycast(CameraCenter, PlayerCamera.transform.forward, out Hit, this.UsableRadius))
                {
                    // Check that the given camera is actually looking at us
                    if (Hit.transform.position.Equals(this.transform.position))
                    {
                        if (this.Available)
                        {
                            this.DisplayHoverText(true);
                        } else {
                            this.DisplayHoverText(false);
                        }

                        if (Input.GetKeyDown(this.UsableKey) && this.Available)
                        {
                            EventHandler<UseEventArgs> UseEvent = OnUseEvent;
                            if (UseEvent != null)
                                UseEvent(this, new UseEventArgs(By));
                            this.OnUse();
                            this.OnUse(By);
                        }
                    } else {
                        this.DisplayHoverText(false);
                    }
                } else {
                    this.DisplayHoverText(false);
                }
            }
        }
    }

    public event EventHandler<UseEventArgs> OnUseEvent;

    protected string UsableKeyToString()
    {
        return this.UsableKey.ToString().ToUpper();
    }

    protected void SetHoverText(string NewText)
    {
        this.HoverText.text = NewText;
    }

    protected void FormatHoverText()
    {
        this.SetHoverText(this.FormatText(this.HoverText.text));        
    }

    protected void DisplayHoverText(bool active)
    {
        this.HoverText.gameObject.SetActive(active);
    }

    protected string FormatText(string text)
    {
        return text.Replace("%k", this.UsableKeyToString());
    }

    protected string FormatText(string text, KeyCode replacer)
    {
        return text.Replace("%k", replacer.ToString().ToUpper());
    }

    protected void SetAndFormatHoverText(string NewText)
    {
        this.HoverText.text = NewText;
        this.FormatHoverText();
    }

    protected string GetHoverText()
    {
        return this.HoverText.text;
    }

    public virtual void OnUse() 
    {
        if (this.DebugOnUnusedVirtuals)
            Debug.Log(this.name + " Used");
    }

    public virtual void OnUse(GameObject UsedBy)
    {
        if (this.DebugOnUnusedVirtuals)
            Debug.Log("Used By " + UsedBy.name);
    }

    void OnDrawGizmos() {
        if (this.DrawUsableRadiusGizmo) {
            if (this.UsableBy.Any(By => Mathf.Abs(Vector3.Distance(this.transform.position, By.transform.position)) <= this.UsableRadius))
                Gizmos.color = this.GizmoUsableColor;
            else
                Gizmos.color = this.GizmoUnusableColor;

            Gizmos.DrawWireSphere(this.transform.position, this.UsableRadius);
        }
    }

}

public class UseEventArgs : EventArgs
{
    public UseEventArgs(GameObject By)
    {
        this.UsedBy = By;
    }

    public GameObject UsedBy { get; }
}
