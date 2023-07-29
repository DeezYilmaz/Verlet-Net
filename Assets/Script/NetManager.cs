using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour
{

    public GameObject netPointPrefab;


    public int x = 10;
    public int y = 10;

    public float maxRopeLimit = 1.0f;
    public Vector3 additForce = Vector3.zero;

    NetPoint[,] ropeGrid;


    List<NetPoint> pointList = new List<NetPoint>();
    public static float netStrength=1.5f;
    public static bool canBeRipped=false;

    // Start is called before the first frame update
    void Start()
    {
        ropeGrid = new NetPoint[x, y];

        for (int m = 0; m < y; m++)
        {
            for (int n = 0; n < x; n++)
            {
                GameObject go = Instantiate(netPointPrefab,transform);
                NetPoint np = go.GetComponent<NetPoint>();
                np.setValues(new Vector3(n* maxRopeLimit, m* maxRopeLimit, 0), false);
                ropeGrid[n, m] = np;
                pointList.Add(np);
            }
        }
        for (int m = 0; m < y; m++)
        {
            for (int n = 0; n < x; n++)
            {
                if (n > 0) { ropeGrid[n, m].neighbours.Add(ropeGrid[n - 1, m]); }
                if (n < x - 1) { ropeGrid[n, m].neighbours.Add(ropeGrid[n + 1, m]); }
                if (m > 0) { ropeGrid[n, m].neighbours.Add(ropeGrid[n, m - 1]); }
                if (m < y - 1) { ropeGrid[n, m].neighbours.Add(ropeGrid[n, m + 1]); }
            }
        }

        for (int i = 0; i < x; i++)
        {
            ropeGrid[i, y - 1].anchor = true;
        }


    }

    private void FixedUpdate()
    {
        Simulate();
        ApplyConstraint();
        DrawLine();
        DrawPoint();
    }

    void DrawLine()
    {
        foreach (NetPoint item in pointList)
        {
            item.DrawLine();
        }
    }
    void DrawPoint()
    {
        foreach (NetPoint item in pointList)
        {
            item.transform.position = item.currPos;
        }
    }
    void Simulate()
    {
        foreach (NetPoint item in pointList)
        {
            item.simulate(additForce+new Vector3(0f,-1f,0f));
        }
    }

    void ApplyConstraint()
    {
        foreach (NetPoint item in pointList)
        {
            item.ApplyConstraint(maxRopeLimit);
        }
    }
}
