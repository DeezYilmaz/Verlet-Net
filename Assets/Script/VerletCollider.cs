using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public float power = 1f;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Point"))
        {
            return;
        }

        NetPoint np = other.GetComponent<NetPoint>();
        Vector3 moveVec = (np.oldPos - np.currPos);
        np.currPos += moveVec*power;
        np.oldPos = np.currPos;
    }

}
