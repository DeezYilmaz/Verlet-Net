using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPoint : MonoBehaviour
{

    public NetPoint netPoint;

    public Vector3 currPos;
    public Vector3 oldPos;
    public bool anchor;
    public Vector3 firstPos;
    public List<NetPoint> neighbours;
    public void setValues(Vector3 pos, bool anchor)
    {
        firstPos = pos;
        this.anchor = anchor;
        this.currPos = pos;
        this.oldPos = pos;
    }
    // Start is called before the first frame update
    
}
