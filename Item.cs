using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Usable 
{
    protected GameObject CurrentOwner;
    public KeyCode DropKey = KeyCode.G;

    void Start()
    {
        base.OnStart();
    }

    void Update() 
    {
        this.OnUpdate();
    }

    #pragma warning disable CS0108
    protected void OnUpdate() {
        base.OnUpdate();
        if (Input.GetKeyDown(this.DropKey)) {
            this.Available = true;
            this.OnDrop(this.CurrentOwner);
        }
    }

    public override void OnUse(GameObject By)
    {
        this.CurrentOwner = By;
        if (this.OnPickup(By)) {
            this.Available = false;
        } else this.CurrentOwner = null;
    }

    public abstract void OnDrop(GameObject By);

    public abstract bool OnPickup(GameObject By);

    public bool GetCurrentOwner(out GameObject Owner) {
        Owner = this.CurrentOwner;
        if (this.CurrentOwner != null)
            return true;
        return false;
    }

    public bool GetCurrentOwnerCamera(out Camera OwnerCamera) {
        OwnerCamera = null;
        if (this.GetCurrentOwner(out GameObject Owner))
            if (Utils.ExtractCamera(Owner, out OwnerCamera))
                return true;
        return false;
    }

    public Camera GetCurrentOwnerCamera() {
        Camera OwnerCamera = null;
        if (this.GetCurrentOwner(out GameObject Owner))
            OwnerCamera = Utils.ExtractCamera(Owner);
        return OwnerCamera;
    }
}
