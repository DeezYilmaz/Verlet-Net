using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Point"))
        {
            return;
        }

        NetPoint np = other.GetComponent<NetPoint>();

        for (int i = 0; i < np.neighbours.Count; i++)
        {
            if (np.neighbours[i] == null) 
                   continue;
            np.neighbours[i].neighbours.Remove(np);
            np.neighbours.RemoveAt(i);
        }
    }

}
