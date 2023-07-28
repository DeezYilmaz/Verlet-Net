using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour
{

    public GameObject netPointPrefab;

    List<NetPoint> pointList=new List<NetPoint>();
    // Start is called before the first frame update
    void Start()
    {


        GameObject go=Instantiate(netPointPrefab);
        NetPoint np = go.GetComponent<NetPoint>();
        np.setValues(Vector3.one, true);
        pointList.Add(np);
    }

    private void Update()
    {
        Simulate();
        ApplyConstraint();
        DrawPoint();
    }

    void DrawPoint()
    {

    }
    void Simulate()
    {

    }

    void ApplyConstraint()
    {

    }
}
