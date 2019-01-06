using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using GXPEngine.OpenGL;

public class DashedLine : LineSegment
{
    public int numberOfDashes;
    public int distanceBetweenDashes;

    public DashedLine(float pStartX, float pStartY, float pEndX, float pEndY, int pNumberOfDashes = 4, int pDistanceBetweenDashes = 15, uint pColor = 0xffffffff, uint pLineWidth = 1, bool pGlobalCoords = false)
			: this (new Vec2 (pStartX, pStartY), new Vec2 (pEndX, pEndY), pNumberOfDashes, pDistanceBetweenDashes, pColor, pLineWidth)
		{
    }

    public DashedLine(Vec2 pStart, Vec2 pEnd, int pNumberOfDashes = 4, int pDistanceBetweenDashes = 15, uint pColor = 0xffffffff, uint pLineWidth = 1, bool pGlobalCoords = false)
            : base(pStart, pEnd, pColor, pLineWidth, pGlobalCoords)
    {
        numberOfDashes = pNumberOfDashes;
        distanceBetweenDashes = pDistanceBetweenDashes;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //														RenderSelf()
    //------------------------------------------------------------------------------------------------------------------------
    override protected void RenderSelf(GLContext glContext)
    {
        if (game != null && start != null && end != null)
        {
            if (start.x == end.x && start.y == end.y) return;
            Vec2 lineVector = end.Clone().Subtract(start);

            List<Vec2> s = new List<Vec2> { };
            List<Vec2> e = new List<Vec2> { };
            
            float length = lineVector.length;
            float lengthOfDashes = (length - (numberOfDashes - 1) * distanceBetweenDashes) / numberOfDashes;

            float offsetXEmpty = distanceBetweenDashes * Mathf.Cos(lineVector.GetAngleRadians());
            float offsetYEmpty = distanceBetweenDashes * Mathf.Sin(lineVector.GetAngleRadians());

            float offsetX = lengthOfDashes * Mathf.Cos(lineVector.GetAngleRadians());
            float offsetY = lengthOfDashes * Mathf.Sin(lineVector.GetAngleRadians());

            for (int i = 0; i < numberOfDashes; i++)
            {
                if (s.ElementAtOrDefault(0) != null)
                {
                    if (e.ElementAtOrDefault(0) != null) s.Add(new Vec2(e[i - 1].x + offsetXEmpty,e[i - 1].y + offsetYEmpty));
                }
                else s.Add(start.Clone());

                if (i == numberOfDashes - 1) e.Add(end.Clone());
                else e.Add(new Vec2(s[i].x + offsetX, s[i].y + offsetY));
            }

            for (int i = 0; i < numberOfDashes; i++)
            {
                RenderLine(s[i], e[i], color, lineWidth);
            }
        }
    }
}