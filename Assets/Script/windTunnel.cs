using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windTunnel : MonoBehaviour
{

    public float WindPower = 1.0f;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<NetPoint>().simulate(transform.up*WindPower );
    }
    
}
