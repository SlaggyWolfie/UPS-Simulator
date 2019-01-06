using System;
using GXPEngine;
public class Vec2 
{
	public static Vec2 zero { get { return new Vec2(0,0); }}

	public float x = 0;
	public float y = 0;

    public Vec2(Vec2 startingVector, Vec2 endingVector, string operation)
    {
        operation.ToLower();
        Vec2 newVector = new Vec2();
        if (operation == "add") newVector = startingVector.Clone().Add(endingVector);
        else if (operation == "sub" || operation == "subtract") newVector = endingVector.Clone().Subtract(startingVector); //Points to End Vector
        else
        {
            //Console.WriteLine("Vector " + nameof(this) + " has use a value of (0, 0) due to incorrect input.");
        }
        x = newVector.x;
        y = newVector.y;
    }

    public Vec2 (float pX = 0, float pY = 0)
	{
		x = pX;
		y = pY;
	}

	public override string ToString ()
	{
		return String.Format ("({0}, {1})", x, y);
	}

	public Vec2 Add (Vec2 other)
    {
		x += other.x;
		y += other.y;
		return this;
	}

    public Vec2 Subtract (Vec2 other)
    {
        x -= other.x;
        y -= other.y;
        return this;
    }

    public Vec2 Scale (float scalar)
    {
        x *= scalar;
        y *= scalar;
        return this;
    }

    public float Length()
    {
        float length = Mathf.Sqrt(x * x + y * y);
        return length;
    }

    public float length
    {
        get
        {
            float length = Mathf.Sqrt(x * x + y * y);
            return length;
        }
    }

    public Vec2 Normalize ()
    {
        float localLength = length;
        if (localLength != 1 && localLength != 0)
        {
            x /= localLength;
            y /= localLength;
        }
        return this;
    }

    public Vec2 Clone()
    {
        return new Vec2(x, y);
    }

    public Vec2 SetXY(float newX = 0, float newY = 0)
    {
        x = newX;
        y = newY;
        return this;
    }

    public Vec2 SetXY (Vec2 pVec)
    {
        return SetXY(pVec.x, pVec.y);
    }

    public static float Deg2Rad(float pDegrees)
    {
        return pDegrees * Mathf.PI / 180;
    }

    public static float Rad2Deg(float pRads)
    {
        return pRads * 180 / Mathf.PI;
    }

    public static Vec2 GetUnitVectorDegrees(float pDegrees)
    {
        //Code Reuse
        return GetUnitVectorRadians(Deg2Rad(pDegrees));
    }

    public static Vec2 GetUnitVectorRadians(float pRads)
    {
        return new Vec2(Mathf.Cos(pRads) * 1, Mathf.Sin(pRads) * 1);
    }

    public static Vec2 RandomUnitVector()
    {
        float newAngle = Utils.Random(0, 360);
        return GetUnitVectorDegrees(newAngle);
    }

    public Vec2 SetAngleDegrees(float pDegrees)
    {
        return SetAngleRadians(Deg2Rad(pDegrees));
    }

    public Vec2 SetAngleRadians(float pRads)
    {
        float localLength = length;
        x = Mathf.Cos(pRads) * localLength;
        y = Mathf.Sin(pRads) * localLength;
        return this;
    }

    public float GetAngleRadians()
    {
        //error prone due floating point errors. Curse FLOATS.
        //Also probably error prone due to calculating only through Y. By the way no Asin exists atleast in this build of GXPEngine.
        return Mathf.Atan2(y, x);
    }

    public float GetAngleDegrees()
    {
        return Rad2Deg(GetAngleRadians());
    }

    public Vec2 RotateDegrees(float pDegrees)
    {
        return RotateRadians(Deg2Rad(pDegrees));
    }

    public Vec2 RotateRadians(float pRads)
    {
        float newCos = Mathf.Cos(pRads);
        float newSin = Mathf.Sin(pRads);
        float localX = x;
        float localY = y;
        x = localX * newCos - localY * newSin;
        y = localX * newSin + localY * newCos;
        return this;
    }

    public Vec2 RotateAroundDegrees(float pX, float pY, float pDegrees)
    {
        return RotateAroundRadians(pX, pY, Deg2Rad(pDegrees));
    }

    public Vec2 RotateAroundRadians(float pX, float pY, float pRad)
    {
        //This hopefully works.

        //Translate over -p;
        x -= pX;
        y -= pY;

        //Rotate
        RotateRadians(pRad);

        //Translate over +p
        x += pX;
        y += pY;

        return this;
    }

    public Vec2 RotateAroundRadians(Vec2 pVector, float pRad)
    {
        return RotateAroundRadians(pVector.x, pVector.y, pRad);
    }

    public Vec2 RotateAroundDegrees(Vec2 pVector, float pDegrees)
    {
        return RotateAroundDegrees(pVector.x, pVector.y, pDegrees);
    }

    public Vec2 Perpendicular()
    {
        return Clone().RotateDegrees(90);
    }

    public Vec2 Normal()
    {
        //return Clone().RotateDegrees(90).Normalize();
        return Clone().SetXY(-y, x).Normalize();
    }

    public float Dot(Vec2 pVector)
    {
        return x * pVector.x + y * pVector.y;
    }

    public Vec2 DotVector(Vec2 pVector)
    {
        //Vector projection
        float dotProd = Dot(pVector);
        return new Vec2(pVector.x * dotProd, pVector.y * dotProd);
    }

    public Vec2 Reflect(Vec2 pNormal, float pBounciness = 1)
    {
        Subtract(pNormal.Clone().Scale(Dot(pNormal) * (1 + pBounciness)));
        return this;
    }
    public Vec2 ReflectVecProj(Vec2 pNormal, float pBounciness = 1)
    {
        Subtract(DotVector(pNormal).Scale(1 + pBounciness));
        return this;
    }

    public static Vec2 operator + (Vec2 pVec1, Vec2 pVec2)
    {
        return pVec1.Add(pVec2);
    }

    public static Vec2 operator - (Vec2 pVec1, Vec2 pVec2)
    {
        return pVec1.Subtract(pVec2);
    }

    public static Vec2 operator * (Vec2 pVec, float multiplier)
    {
        return pVec.Scale(multiplier);
    }

    public static Vec2 operator / (Vec2 pVec, float divider)
    {
        return pVec.Scale(1 / divider);
    }

    //public static Vec2 operator = (Vec2 pVec1, Vec2 pVec2)
    //{
    //    return pVec1.SetXY(pVec2);
    //}
}