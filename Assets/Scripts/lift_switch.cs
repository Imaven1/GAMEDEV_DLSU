using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LiftSwitch : MonoBehaviour
{
    //Object where trigger is attached to.
    [SerializeField]
    GameObject obj;

    List<GameObject> allPlatforms = new();

    Vector3 pos;
    bool isLifted = false;

    void Start()
    {
        // get position of the object
        pos = obj.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isLifted)
            {
                obj.transform.Translate(Vector3.up * 5);
                isLifted = true;
            } else
            {
                obj.transform.Translate(Vector3.down * 5);
                isLifted = false;
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {

    }
}
