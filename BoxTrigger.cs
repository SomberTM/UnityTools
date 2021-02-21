using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoxTrigger : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<string, bool> RecentTriggers = new Dictionary<string, bool>();
    public List<GameObject> TriggerableBy;

    public bool TriggerableByTag = false;
    public string Tag = "Player";

    public bool DrawGizmoTriggerBox = true;
    public Vector3 TriggerPosition;
    public Vector3 TriggerScale;

    private Bounds TriggerBounds;

    protected void Init()
    {
        if (this.TriggerableByTag) 
        {
            GameObject[] Tagged = GameObject.FindGameObjectsWithTag(this.Tag);
            if (Tagged.Length > 0)
            {
                foreach (GameObject Object in Tagged)
                {
                    if (!this.TriggerableBy.Contains(Object))
                        this.TriggerableBy.Add(Object);
                }
            }
        }
        this.TriggerBounds = new Bounds(TriggerPosition, TriggerScale);
    }

    protected void Tick()
    {
        foreach (GameObject By in this.TriggerableBy)
        {
            if (By.TryGetComponent<Collider>(out Collider ObjectCollider))
            {
                try { 
                    bool exists = this.RecentTriggers[By.name]; 
                } catch(KeyNotFoundException exception) {
                    using (exception as System.IDisposable) { }
                    this.SetRecentTrigger(By.name, false);
                }
                
                if (AbsBounds(ObjectCollider.bounds).Intersects(AbsBounds(this.TriggerBounds)))
                {
                    if (!this.RecentTriggers[By.name])
                    {
                        this.OnTriggerEnter();
                        this.OnTriggerEnter(By);
                    }
                    
                    this.OnTrigger();
                    this.OnTrigger(By);
                    this.SetRecentTrigger(By.name, true);
                } else if (this.RecentTriggers[By.name]) {
                    this.OnTriggerExit();
                    this.OnTriggerExit(By);
                    this.SetRecentTrigger(By.name, false);
                }
            }
        }
    }

    private void SetRecentTrigger(string key, bool status)
    {
        if (this.RecentTriggers.ContainsKey(key))
        {
            if (this.RecentTriggers[key] != status)
                this.RecentTriggers[key] = status;
        } 
        else 
        {
            this.RecentTriggers.Add(key, status);
        }
    }

    private delegate T Function<T>(T x);
    private Bounds AbsBounds(Bounds bounds) {
        Function<float> abs = Mathf.Abs;
        Vector3 size = bounds.size;
        Bounds AbsoluteBounds = new Bounds(bounds.center, size);
        AbsoluteBounds.size = new Vector3(abs(size.x), abs(size.y), abs(size.z));
        return AbsoluteBounds;
    }

    public virtual void OnTrigger() { }
    public virtual void OnTrigger(GameObject TriggeredBy) { }

    public virtual void OnTriggerEnter() { }
    public virtual void OnTriggerEnter(GameObject TriggeredBy) { }

    public virtual void OnTriggerExit() { }
    public virtual void OnTriggerExit(GameObject TriggeredBy) { }

    void OnDrawGizmos() {
        if (this.DrawGizmoTriggerBox) {
            if (this.TriggerableBy.Any(By => {
                if (By.TryGetComponent<Collider>(out Collider ByCollider))
                    return AbsBounds(ByCollider.bounds).Intersects(AbsBounds(this.TriggerBounds));
                return false;
            })) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawWireCube(this.TriggerPosition, this.TriggerScale);
        }
    }

}
