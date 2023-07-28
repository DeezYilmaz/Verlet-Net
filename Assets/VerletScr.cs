using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletScr : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float maxRopeLimit = 1.0f;
    public int x, y = 10;
    public int RopeLength;

    public GameObject[,] spheres;

    public RopePoint[,] ropeGrid;
    public float windPower = 0f;
    public RopePoint centerRope;
    // Start is called before the first frame update
    void Start()
    {
        RopeLength = x * y;
        spheres = new GameObject[x ,y];
        ropeGrid = new RopePoint[x, y];
        this.lineRenderer = this.GetComponent<LineRenderer>();
        for (int m = 0; m < y; m++)
        {
            for (int n = 0; n < x; n++)
            {
                spheres[n, m] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                spheres[n, m].transform.localScale = Vector3.one * 0.1f;
               RopePoint rp = new RopePoint(new Vector3(n, m, 0f),false);
                ropeGrid[n,m] = rp;
                

            }
        }
        ropeGrid[x-1, y-1].anchor = true;
        ropeGrid[0, y - 1].anchor = true;
        ropeGrid[x / 2-1, y - 1].anchor = true;
        ropeGrid[x/2, y - 1].anchor = true;
    }

 

    // Update is called once per frame

    void FixedUpdate()
    {
        Simulate();
        applyConstraints();
        DrawPoint();

    }

    void Simulate()
    {
        Vector3 gravity = new Vector3(0f, -1f, 0f);

        Vector3 wind = new Vector3(0f, 0f, 1f);
        for (int m = 0; m < y; m++)
        {
            for (int n = 0; n < x; n++)
            {
                RopePoint rp = ropeGrid[n,m];
                Vector3 velocity = (rp.currPos - rp.oldPos);
                rp.oldPos = rp.currPos;
                rp.currPos += velocity;
                rp.currPos += gravity * Time.fixedDeltaTime;
                rp.currPos += wind * Time.fixedDeltaTime * windPower;
                ropeGrid[n, m] = rp;
            }
        }
    }

    void applyConstraints()
    {
        for (int m = 0; m < y-1; m++)
        {
            for (int n = 0; n < x-1; n++)
            {
                RopePoint firstRope = ropeGrid[n, m];
                RopePoint secondRope = ropeGrid[n + 1, m];

                applyConstraintsForSegment(ref firstRope, ref secondRope);
                ropeGrid[n, m] = firstRope;
                ropeGrid[n + 1, m] = secondRope;

                firstRope = ropeGrid[n, m];
                secondRope = ropeGrid[n, m+1];

                applyConstraintsForSegment(ref firstRope, ref secondRope);

                ropeGrid[n, m] = firstRope;
                ropeGrid[n, m+1] = secondRope;
            }
        }

        for (int m = 0; m < y - 1; m++)
        {
            RopePoint firstRope = ropeGrid[x-1, m];
            RopePoint secondRope = ropeGrid[x-2, m];

            applyConstraintsForSegment(ref firstRope, ref secondRope);
            ropeGrid[x - 1, m] = firstRope;
            ropeGrid[x - 2, m] = secondRope;

            firstRope = ropeGrid[x-1, m];
            secondRope = ropeGrid[x-1, m + 1];

            applyConstraintsForSegment(ref firstRope, ref secondRope);

            ropeGrid[x-1, m] = firstRope;
            ropeGrid[x-1, m + 1] = secondRope;
        }
        for (int n = 0; n < x - 1; n++)
        {
            RopePoint firstRope = ropeGrid[n, y-1];
            RopePoint secondRope = ropeGrid[n + 1, y - 1];

            applyConstraintsForSegment(ref firstRope, ref secondRope);
            ropeGrid[n, y - 1] = firstRope;
            ropeGrid[n + 1, y - 1] = secondRope;

            firstRope = ropeGrid[n, y - 1];
            secondRope = ropeGrid[n, y - 2];

            applyConstraintsForSegment(ref firstRope, ref secondRope);

            ropeGrid[n, y - 1] = firstRope;
            ropeGrid[n, y - 2] = secondRope;
        }
    }

    void applyConstraintsForSegment(ref RopePoint rp,ref RopePoint neigbour)
    {

        RopePoint firstRope = rp;
        RopePoint secondRope = neigbour;

        float dist = Vector3.Distance(secondRope.currPos, firstRope.currPos);
        float error = Mathf.Abs(dist - maxRopeLimit);
        if (dist > maxRopeLimit)
        {
            Vector3 distVector = (secondRope.currPos - firstRope.currPos).normalized;
            secondRope.currPos -= distVector * (error * 0.5f);
            firstRope.currPos += distVector * (error * 0.5f);

        }
        if (!rp.anchor)
        {
            rp.currPos = firstRope.currPos;
            neigbour.currPos = secondRope.currPos;
        }
        else
        {
            rp.currPos = rp.firstPos;
            neigbour.currPos = secondRope.currPos;

        }
        if (!neigbour.anchor)
        {
            rp.currPos = firstRope.currPos;
            neigbour.currPos = secondRope.currPos;
        }
        else
        {

            rp.currPos = firstRope.currPos;
            neigbour.currPos = neigbour.firstPos;

        }


    }
    private void DrawLine()
    {
        float lineWidth = 0.04f;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.RopeLength];
        for (int m = 0; m < y ; m++)
        {
            for (int n = 0; n < x ; n++)
            {
                ropePositions[n+m*y] = this.ropeGrid[n,m].currPos;
            }
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    private void DrawPoint()
    {
        for (int m = 0; m < y ; m++)
        {
            for (int n = 0; n < x ; n++)
            {
                spheres[n, m].transform.position = ropeGrid[n, m].currPos;
            }
        }
    }
    public struct RopePoint
    {

        public Vector3 currPos;
        public Vector3 oldPos;
        public bool anchor;
        public Vector3 firstPos;
        public RopePoint(Vector3 pos,bool anchor)
        {
            firstPos = pos;
            this.anchor = anchor;
            this.currPos = pos;
            this.oldPos = pos;
        }
    }
}
