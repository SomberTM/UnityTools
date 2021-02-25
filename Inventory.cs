using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    public int hotbarSize = 9;
    public GameObject[] hotbar { get; private set; }
    private int nextHotbarIndex = 0;
    
    public int activeHotbarItemIndex = 0;
    public GameObject activeHotbarItem;


    public int backpackSize = 27;
    public GameObject[] backpack { get; private set; }
    private int nextBackpackIndex = 0;

    void Start()
    {
        this.OnStart();
    }

    protected void OnStart() {
        this.hotbar = new GameObject[this.hotbarSize];
        this.backpack = new GameObject[this.backpackSize];

        this.activeHotbarItem = this.hotbar[this.activeHotbarItemIndex];
    }

    void Update()
    {
        this.OnUpdate();
    }

    protected void OnUpdate() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0.0f) {

        } else if (scroll < 0.0f) {

        }
    }

    public bool IsValidHotbarIndex(int index) {
        return (index >= 0 && index <= this.hotbarSize);
    }

    public bool IsValidBackpackIndex(int index) {
        return (index >= 0 && index < this.backpackSize);
    }

    public bool IsHotbarFull() {
        return this.nextHotbarIndex >= this.hotbarSize;
    }

    public bool IsBackpackFull() {
        return this.nextBackpackIndex >= this.backpackSize;
    }

    public void AddToHotbar(GameObject item) {
        if (!this.IsHotbarFull()) {
            this.hotbar[this.nextHotbarIndex] = item;
            this.nextHotbarIndex++;
        }
    }

    public void RemoveFromHotbar(int index) {
        if (this.IsValidHotbarIndex(index)) {
            this.hotbar[index] = null;
            this.nextHotbarIndex = index;
        }
    }

    public void AddToBackpack(GameObject item) {
        if (!this.IsBackpackFull()) {
            this.backpack[this.nextBackpackIndex] = item;
            this.nextBackpackIndex++;
        }
    }

    public void SetHotbarSlot(int index, GameObject item) {
        if (this.IsValidHotbarIndex(index))
            this.hotbar[index] = item;
    }

    public void SetBackpackSlot(int index, GameObject item) {
        if (this.IsValidBackpackIndex(index))
            this.backpack[index] = item;
    }

    public void RemoveFromHotbar(GameObject item) {
        for (int i = 0; i < this.hotbarSize; i++) 
            if (this.hotbar[i].Equals(item)) 
                this.RemoveFromHotbar(i);
    }

    // ----

    public void RemoveFromBackpack(GameObject item) {
        for (int i = 0; i < this.backpackSize; i++) 
            if (this.backpack[i].Equals(item)) 
                this.RemoveFromBackpack(i);
    }

    public void RemoveFromBackpack(int index) {
        if (this.IsValidBackpackIndex(index)) {
            this.backpack[index] = null;
            this.nextBackpackIndex = index;
        }
    }

    public enum SwapItemAction {
        HOTBAR_TO_BACKPACK,
        BACKPACK_TO_HOTBAR,
        BACKPACK_TO_BACKPACK,
        HOTBAR_TO_HOTBAR,
    }

    public void SwapItem(SwapItemAction action, int index0, int index1) {

        switch(action) {

            case SwapItemAction.HOTBAR_TO_BACKPACK:
                if (!IsValidHotbarIndex(index0)) throw this.InvalidHotbarIndexException(index0);
                if (!IsValidBackpackIndex(index1)) throw this.InvalidBackpackIndexException(index1);
                if (!this.IsBackpackFull()) {
                    Utils.Swap(this.hotbar, this.backpack, index0, index1);
                    this.nextHotbarIndex--;
                    this.nextBackpackIndex++;
                }
                break;

            case SwapItemAction.BACKPACK_TO_HOTBAR:
                if (!IsValidBackpackIndex(index0)) throw this.InvalidBackpackIndexException(index0);
                if (!IsValidHotbarIndex(index1)) throw this.InvalidHotbarIndexException(index1);
                if (!this.IsHotbarFull()) {
                    Utils.Swap(this.backpack, this.hotbar, index0, index1);
                    this.nextHotbarIndex++;
                    this.nextBackpackIndex--;
                }
                break;

            case SwapItemAction.BACKPACK_TO_BACKPACK:
                if (!IsValidBackpackIndex(index0)) throw this.InvalidBackpackIndexException(index0);
                if (!IsValidBackpackIndex(index1)) throw this.InvalidBackpackIndexException(index1);
                Utils.Swap(this.backpack, index0, index1);
                break;

            case SwapItemAction.HOTBAR_TO_HOTBAR:
                if (!IsValidHotbarIndex(index0)) throw this.InvalidHotbarIndexException(index0);
                if (!IsValidHotbarIndex(index1)) throw this.InvalidHotbarIndexException(index1);
                Utils.Swap(this.hotbar, index0, index1);
                break;
        }

    }

    protected System.Exception InvalidHotbarIndexException(int index) {
        return new System.Exception($"Invalid hotbar index. 0 <= {index} < {this.hotbarSize}");
    }

    protected System.Exception InvalidBackpackIndexException(int index) {
        return new System.Exception($"Invalid backpack index. 0 <= {index} < {this.backpackSize}");
    }
}
