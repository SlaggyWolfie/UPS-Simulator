using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GXPEngine;

public class Line : NLineSegment
{
    public float depth;
    public float startDepth;
    public float endDepth;

    public Line(Vec2 pStart, Vec2 pEnd, uint pColor = 0xffffffff, uint pLineWidth = 1, bool pGlobalCoords = false) : base(pStart, pEnd, pColor, pLineWidth, pGlobalCoords)
    {

    }

    public override string ToString()
    {
        string stringToReturn = base.ToString();
        if (Mathf.Abs(startDepth) > float.Epsilon) stringToReturn += "\n\t start depth: " + startDepth;
        if (Mathf.Abs(endDepth) > float.Epsilon) stringToReturn += "\n\t end depth: " + endDepth;
        return stringToReturn;
    }
}