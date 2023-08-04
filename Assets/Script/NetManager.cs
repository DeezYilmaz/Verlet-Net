using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour
{

    public GameObject netPointPrefab;


    public int x = 10;
    public int y = 10;

    public float maxRopeLimit = 1.0f;
    public float minRopeLimit = .2f;
    public Vector3 additForce = Vector3.zero;

    NetPoint[,] ropeGrid;


    List<NetPoint> pointList = new List<NetPoint>();
    public static float netStrength=1.5f;
    public static bool canBeRipped=false;
    MeshFilter mf;

    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
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

        generateMesh();
    }

    public bool drawLine, drawPoint, drawMesh;

    private void FixedUpdate()
    {
        Simulate();
        ApplyConstraint();
        if(drawLine) DrawLine();
        DrawPoint(drawPoint);
        if (drawMesh) DrawMesh();
    }
    void generateMesh()
    {

        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[x * y];
        int[] tri = new int[(x - 1) * (y - 1) * 6];

        int k = 0;
        int f = 0;
        for (int m = 0; m < y; m++)
        {
            for (int n = 0; n < x; n++)
            {
                verts[k] = ropeGrid[m,n].currPos;

                if(m<y-1 && n < x - 1)
                {
                    tri[6*f] = k;
                    tri[6*f + 1] = k + 1 + x;
                    tri[6*f + 2] = k + x;

                    tri[6*f + 3] = k;
                    tri[6*f + 4] = k + 1;
                    tri[6*f + 5] = k + x + 1;

                    f++;     
                }
                k++;
            }
        }

        mesh.vertices = verts;
        mesh.triangles = tri;

        mf.mesh = mesh;

    }
    void DrawMesh()
    {
        Mesh msh = mf.mesh;
        int k = 0;

        Vector3[] verts = new Vector3[x * y];
        for (int m = 0; m < y; m++)
        {
            for (int n = 0; n < x; n++)
            {
                verts[k] = pointList[k].currPos;
                k++;
            }
        }
        msh.vertices = verts;
        msh.RecalculateNormals();
        mf.mesh = msh;
        
    }
    void DrawLine()
    {
        foreach (NetPoint item in pointList)
        {
            item.DrawLine();
        }
    }

    Dictionary<NetPoint,MeshRenderer> meshRendererDict= new Dictionary<NetPoint,MeshRenderer>();
    void DrawPoint(bool drawPoint)
    {
        foreach (NetPoint item in pointList)
        {
            if (!meshRendererDict.ContainsKey(item))
                meshRendererDict.Add(item, item.GetComponent<MeshRenderer>());
            meshRendererDict[item].enabled = drawPoint;
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
            item.ApplyConstraint(maxRopeLimit,minRopeLimit);
        }
    }
}
