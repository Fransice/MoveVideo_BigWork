using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLoad : MonoBehaviour
{
    public Transform newLoad;
    void Update()
    {
        if (transform.childCount == 0)
        {
            newLoad.gameObject.SetActive(true);
        }
        else
        {
            newLoad.gameObject.SetActive(false);
        }
    }
}
