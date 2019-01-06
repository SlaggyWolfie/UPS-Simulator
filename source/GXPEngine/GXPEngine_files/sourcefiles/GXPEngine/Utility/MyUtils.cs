using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GXPEngine;

public static class MyUtils
{
    private static int bounce = 1;
    //private static int loop = 1;
    public static bool RandomBool()
    {
        int number = Utils.Random(0, 2);
        if (number == 0) return false;
        else return true;
    }
    
    //Int functions
    public static int ValueCheckerAndAlternator(int checkValue, int firstValue, int secondValue, bool badValue)
    {
        if (checkValue == firstValue) checkValue = secondValue;
        else if (checkValue == secondValue) checkValue = firstValue;
        else if (badValue == true) checkValue = firstValue;
        return checkValue;
    }

    public static int ValueBouncer(int checkValue, int firstValue, int lastValue, int bounce = 1)
    {
        checkValue += bounce;
        if (checkValue >= lastValue) bounce = -bounce;
        if (checkValue <= firstValue) bounce = -bounce;
        return checkValue;
    }

    public static int ValueLooper(int checkvalue, int firstValue, int lastValue, int loop = 1)
    {
        checkvalue += loop;
        if (checkvalue > lastValue) checkvalue = firstValue;
        return checkvalue;
    }

    //Float overloads
    public static float ValueCheckerAndAlternator(float checkValue, float firstValue, float secondValue, bool badValue)
    {
        if (checkValue == firstValue) checkValue = secondValue;
        else if (checkValue == secondValue) checkValue = firstValue;
        else if (badValue == true) checkValue = firstValue;
        return checkValue;
    }

    public static float ValueBouncer(float checkValue, float firstValue, float lastValue, float bounce = 1f)
    {
        checkValue += bounce;
        if (checkValue >= lastValue) bounce = -bounce;
        if (checkValue <= firstValue) bounce = -bounce;
        return checkValue;
    }

    public static float ValueLooper(float checkvalue, float firstValue, float lastValue, float loop = 1)
    {
        checkvalue += loop;
        if (checkvalue > lastValue) checkvalue = firstValue;
        return checkvalue;
    }

    public static void ControlObject(GameObject o)
    {
        bool controlIsIssued = false;
        if (Input.GetKey(Key.W))
        {
            o.y--;
            controlIsIssued = true;
        }
        if (Input.GetKey(Key.S))
        {
            o.y++;
            controlIsIssued = true;
        }
        if (Input.GetKey(Key.A))
        {
            o.x--;
            controlIsIssued = true;
        }
        if (Input.GetKey(Key.D))
        {
            o.x++;
            controlIsIssued = true;
        }

        if (Input.GetKeyDown(Key.UP))
        {
            o.y -= 100;
            controlIsIssued = true;
        }
        if (Input.GetKeyDown(Key.DOWN))
        {
            o.y += 100;
            controlIsIssued = true;
        }
        if (Input.GetKeyDown(Key.LEFT))
        {
            o.x -= 100;
            controlIsIssued = true;
        }
        if (Input.GetKeyDown(Key.RIGHT))
        {
            o.x += 100;
            controlIsIssued = true;
        }

        if (controlIsIssued)
        {
            controlIsIssued = false;
            Console.WriteLine("Object Tracked: \n\tx: {0} \n\ty: {1}", o.x, o.y);
        }
    }
}

public static class GetAssets
{
    
}
