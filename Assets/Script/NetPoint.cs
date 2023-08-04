using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPoint : MonoBehaviour
{

    public NetPoint netPoint;

    public LineRenderer pointLine;

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

    internal void simulate(Vector3 force)
    {
        Vector3 velocity = (currPos - oldPos);
        oldPos = currPos;
        currPos += velocity;
        currPos += force*Time.fixedDeltaTime;
    }

    internal void ApplyConstraint(float maxRopeLimit,float minRopeLimit)
    {
        foreach (NetPoint second in neighbours)
        {
            float dist = Vector3.Distance(second.currPos, this.currPos);
            float error = Mathf.Abs(dist - maxRopeLimit);
            if (dist > maxRopeLimit * NetManager.netStrength && NetManager.canBeRipped)
            {
                second.neighbours.Remove(this);
                neighbours.Remove(second);
                return;
            }
            if (dist > maxRopeLimit)
            {
                Vector3 distVector = (second.currPos - this.currPos).normalized;
                second.currPos -= distVector * (error * 0.5f);
                this.currPos += distVector * (error * 0.5f);

            }
            if (dist < minRopeLimit)
            {
                Vector3 distVector = (second.currPos - this.currPos).normalized;
                second.currPos += distVector * (error * 0.5f);
                this.currPos -= distVector * (error * 0.5f);

            }
            if (this.anchor)
            {
                this.currPos = this.firstPos;
            }

        }

    }
    internal void DrawLine()
    {
        float lineWidth = 0.04f;
        pointLine.startWidth = lineWidth;
        pointLine.endWidth = lineWidth;


        if (neighbours.Count < 1)
        {
            pointLine.enabled = false;
            return;
        }

        Vector3[] horizontalRopePositions = new Vector3[2*neighbours.Count-1];

        for (int i = 0; i < neighbours.Count-1; i++)
        {
            horizontalRopePositions[2*i] = neighbours[i/2].currPos;
            horizontalRopePositions[2*i+1] = currPos;

        }
        horizontalRopePositions[2 * (neighbours.Count - 1)] = neighbours[(neighbours.Count - 1)].currPos;

        pointLine.positionCount = 2 * neighbours.Count - 1;
        pointLine.SetPositions(horizontalRopePositions);

        

    }


    // Start is called before the first frame update

}
