using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

public class EmptyDetector : MonoBehaviour
{
    public UnityEvent onAreaEmpty;
    public UnityEvent<int> onCountChanged;

    private readonly HashSet<Collider> inside = new HashSet<Collider>();
    private bool hadObjects;


    public int Count => inside.Count;

   private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(inside.Add(other))
        {
            hadObjects = true;
            onCountChanged?.Invoke(inside.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!inside.Remove(other)) return;
        NotifyChange();
    }

    private void NotifyChange()
    {
        onCountChanged?.Invoke(inside.Count);
        if (hadObjects && inside.Count == 0) onAreaEmpty!.Invoke();
    }

    private void Update()
    {
        int removed = inside.RemoveWhere(c => c == null || !c.gameObject.activeInHierarchy);
        if (removed > 0) NotifyChange();
    }
}
