using System;
using GXPEngine;
public class Vec3
{
    //Note z is scale
    public static Vec3 zero { get { return new Vec3(0, 0, 0); } }
    public static Vec3 zeroScale { get { return new Vec3(0, 0, 1); } }

    public float x = 0;
    public float y = 0;
    public float z = 0;

    public Vec3(float pX = 0, float pY = 0, float pZ = 0)
    {
        x = pX;
        y = pY;
        z = pZ;
    }

    public override string ToString()
    {
        return String.Format("({0}, {1}, {2})", x, y, z);
    }

    public Vec3 Add(Vec3 other)
    {
        x += other.x;
        y += other.y;
        z += other.z;
        return this;
    }

    public Vec3 Subtract(Vec3 other)
    {
        x -= other.x;
        y -= other.y;
        z -= other.z;
        return this;
    }

    public Vec3 Scale(float scalar)
    {
        x *= scalar;
        y *= scalar;
        z *= scalar;
        return this;
    }

    public float Length()
    {
        float length = Mathf.Sqrt(x * x + y * y + z * z);
        return length;
    }

    public float length
    {
        get
        {
            float length = Mathf.Sqrt(x * x + y * y + z * z);
            return length;
        }
    }

    public Vec3 Normalize()
    {
        float localLength = length;
        if (localLength != 1 && localLength != 0)
        {
            x /= localLength;
            y /= localLength;
            z /= localLength;
        }
        return this;
    }
    public Vec3 Clone()
    {
        return new Vec3(x, y, z);
    }

    public Vec3 SetXY(float newX = 0, float newY = 0, float newZ = 0)
    {
        x = newX;
        y = newY;
        z = newZ;
        return this;
    }

    public Vec3 SetXY(Vec3 pVec)
    {
        return SetXY(pVec.x, pVec.y, pVec.z);
    }
    //^Finished.

    public static float Deg2Rad(float pDegrees)
    {
        return pDegrees * Mathf.PI / 180;
    }

    public static float Rad2Deg(float pRads)
    {
        return pRads * 180 / Mathf.PI;
    }

    public static Vec3 GetUnitVectorDegrees(float pDegrees)
    {
        //Code Reuse
        return GetUnitVectorRadians(Deg2Rad(pDegrees));
    }

    public static Vec3 GetUnitVectorRadians(float pRads)
    {
        return new Vec3(Mathf.Cos(pRads) * 1, Mathf.Sin(pRads) * 1);
    }

    public static Vec3 RandomUnitVector()
    {
        float newAngle = Utils.Random(0, 360);
        return GetUnitVectorDegrees(newAngle);
    }

    public Vec3 SetAngleDegrees(float pDegrees)
    {
        return SetAngleRadians(Deg2Rad(pDegrees));
    }

    public Vec3 SetAngleRadians(float pRads)
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

    public Vec3 RotateDegrees(float pDegrees)
    {
        return RotateRadians(Deg2Rad(pDegrees));
    }

    public Vec3 RotateRadians(float pRads)
    {
        float newCos = Mathf.Cos(pRads);
        float newSin = Mathf.Sin(pRads);
        float localX = x;
        float localY = y;
        x = localX * newCos - localY * newSin;
        y = localX * newSin + localY * newCos;
        return this;
    }

    public Vec3 RotateAroundDegrees(float pX, float pY, float pDegrees)
    {
        return RotateAroundRadians(pX, pY, Deg2Rad(pDegrees));
    }

    public Vec3 RotateAroundRadians(float pX, float pY, float pRad)
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

    public Vec3 RotateAroundRadians(Vec3 pVector, float pRad)
    {
        return RotateAroundRadians(pVector.x, pVector.y, pRad);
    }

    public Vec3 RotateAroundDegrees(Vec3 pVector, float pDegrees)
    {
        return RotateAroundDegrees(pVector.x, pVector.y, pDegrees);
    }

    public Vec3 Perpendicular()
    {
        return Clone().RotateDegrees(90);
    }

    public Vec3 Normal()
    {
        return Clone().RotateDegrees(90).Normalize();
    }

    public float Dot(Vec3 pVector)
    {
        return x * pVector.x + y * pVector.y;
    }

    public Vec3 DotVector(Vec3 pVector)
    {
        //Vector projection
        float dotProd = Dot(pVector);
        return new Vec3(pVector.x * dotProd, pVector.y * dotProd);
    }

    public Vec3 Reflect(Vec3 pNormal, float pBounciness = 1)
    {
        Subtract(pNormal.Clone().Scale(Dot(pNormal) * (1 + pBounciness)));
        return this;
    }
    public Vec3 ReflectVecProj(Vec3 pNormal, float pBounciness = 1)
    {
        Subtract(DotVector(pNormal).Scale(1 + pBounciness));
        return this;
    }

    public static Vec3 operator +(Vec3 pVec1, Vec3 pVec3)
    {
        return pVec1.Add(pVec3);
    }

    public static Vec3 operator -(Vec3 pVec1, Vec3 pVec3)
    {
        return pVec1.Subtract(pVec3);
    }

    public static Vec3 operator *(Vec3 pVec, float multiplier)
    {
        return pVec.Scale(multiplier);
    }

    public static Vec3 operator /(Vec3 pVec, float divider)
    {
        return pVec.Scale(1 / divider);
    }

    //public static Vec3 operator = (Vec3 pVec1, Vec3 pVec3)
    //{
    //    return pVec1.SetXY(pVec3);
    //}
}