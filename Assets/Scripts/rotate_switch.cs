using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RotateSwitch : MonoBehaviour
{
    //Object where trigger is attached to.
    [SerializeField]
    GameObject obj;

    List<GameObject> allPlatforms = new();

    Vector3 pos;

    void Start()
    {
        // get position of the object
        pos = obj.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            obj.transform.Rotate(Vector3.up, 45);
        }


    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {

    }
}
