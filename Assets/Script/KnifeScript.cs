using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {

        NetPoint np = other.GetComponent<NetPoint>();

        for (int i = 0; i < np.neighbours.Count; i++)
        {
            np.neighbours[i].neighbours.Remove(np);
            np.neighbours.RemoveAt(i);
        }
    }

}
