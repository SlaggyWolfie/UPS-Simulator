using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GXPEngine;
using GXPEngine.Core;

public class Slingshot : GameObject
{
    public int timer;
    public readonly int cooldown = 1000; //milliseconds
    //Reference
    public Level level;
    protected Truck truck;

    //Slingshot stuff
    protected Vec2 startPoint;
    protected Vec2 cameraPoint;
    protected Vec2 mouse;
    protected Vec2 deltaVelocity;

    protected Package package;
    public Package nextPackage;
    public bool slingshotIsEmpty;
    private readonly int _waitingListAmount = 3;

    protected MouseHandler packageHandler = null;
    protected LineSegment catapultLine;

    public Slingshot(Truck pTruck, float cameraX, float cameraY, float offsetX, float offsetY, Level pLevel)
    {
        timer = 0;

        level = pLevel;
        truck = pTruck;

        startPoint = Vec2.zero;
        //startPoint = new Vec2(-truck.x, -truck.y);
        cameraPoint = new Vec2(cameraX - offsetX, cameraY - offsetY);
        //startPoint = new Vec2(cameraX, cameraY);
        mouse = new Vec2(Input.mouseX, Input.mouseY);

        slingshotIsEmpty = true;

        //catapultLine = new LineSegment(startPoint.Clone(), startPoint.Clone(), 0xff006600, 10);
        catapultLine = new DashedLine(startPoint.Clone(), startPoint.Clone(), 4, 15, 0xffffffff, 5);
        AddChildAt(catapultLine, 8);
    }

    private void onPackageMouseDown(GameObject target, MouseEventType type)
    {
        //Console.WriteLine(target + " mouse down.");

        packageHandler.OnMouseMove += onPackageMouseMove;
        packageHandler.OnMouseUp += onPackageMouseUp;

        package.velocity = Vec2.zero;
        catapultLine.end = package.position;
    }

    private void onPackageMouseMove(GameObject target, MouseEventType type)
    {
        //Console.WriteLine(target + " mouse move.");

        Vec2 delta = mouse.Clone().Subtract(cameraPoint).Subtract(startPoint);
        float deltaLength = delta.length;

        Vec2 packagePosition = startPoint.Clone().Add(delta.Normalize().Scale(Mathf.Sqrt(deltaLength)).Scale(10));
        package.position.SetXY(packagePosition);
        catapultLine.end.SetXY(package.position);
    }

    private void onPackageMouseUp(GameObject target, MouseEventType type)
    {
        //Console.WriteLine(target + " mouse up.");
        packageHandler.OnMouseMove -= onPackageMouseMove;
        packageHandler.OnMouseUp -= onPackageMouseUp;

        deltaVelocity = startPoint.Clone().Subtract(package.position);
        deltaVelocity.Scale(0.5f);
        package.velocity = deltaVelocity.Clone();

        //float velocityAngle = deltaVelocity.GetAngleDegrees();
        //float targetAngle = 45;
        //float minimum = 0.5f;
        //velocityAngle -= -targetAngle;
        //float scalar = (targetAngle - Mathf.Abs(velocityAngle)) / targetAngle * (1 - minimum);
        //if (scalar < 0 || scalar > (1 - minimum)) scalar = 0;
        //Console.WriteLine(deltaVelocity.GetAngleDegrees() + " " + (scalar + minimum));
        //package.zSpeed *= minimum + scalar;
        //package.velocity *= minimum + scalar;

        level.ShootPackage(package, truck.x, truck.y, package.zSpeed, -deltaVelocity.GetAngleDegrees());
        catapultLine.end = startPoint.Clone();
    }

    /**
	 * General event handler that can be used for debugging
	 */
    private void OnMouseEvent(GameObject target, MouseEventType type)
    {
        //Console.WriteLine("Eventtype:" + type + " triggered on " + target);
    }

    protected virtual void Update()
    {
        if (PauseManager.GetPause()) return;
        UpdatePackageWaitingList();

        if (slingshotIsEmpty)
        {
            if (Time.time > cooldown + timer)
            {
                Console.WriteLine(1);
                LoadUp_Slingshot(level.packageWaitingList[0]);
                level.packageWaitingList.RemoveAt(0);
            }
        }
        else timer = Time.time;

        Vec2 oldPos = package.position.Clone();

        mouse.SetXY(Input.mouseX, Input.mouseY);
    }

    protected void LoadUp_Slingshot(Package pPackage)
    {
        package = pPackage;

        package.position = startPoint.Clone();
        package.velocity = Vec2.zero;
        package.acceleration = Vec2.zero;

        AddChildAt(package, 7);

        deltaVelocity = null;
        slingshotIsEmpty = false;

        packageHandler = new MouseHandler(package);
        packageHandler.OnMouseDownOnTarget += onPackageMouseDown;
    }

    protected void UpdatePackageWaitingList()
    {
        if (level.packageWaitingList.Count < _waitingListAmount)
        {
            level.packageWaitingList.Add(Package.GetRandomPackage());
            return;
        }
        if ((nextPackage != null && nextPackage != level.packageWaitingList[0]) || level.packageWaitingList[0] != null) nextPackage = level.packageWaitingList[0];
    }
}