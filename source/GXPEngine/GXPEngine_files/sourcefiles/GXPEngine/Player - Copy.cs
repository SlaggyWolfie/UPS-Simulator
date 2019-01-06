using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GXPEngine;

public class PlayerP : GameObject
{
    //Reference
    private Level _level;
    private Truck _truck;

    //Actual player data
    public int score;

    //Slingshot stuff
    private Ball _ball;

    private Vec2 _startPoint;
    private Vec2 _mouse;
    private Vec2 _deltaVelocity;

    private Arrow _debugArrow;
    private MouseHandler _ballHandler = null;
    private LineSegment _catapultLine;
    private Canvas _lineCanvas;

    public PlayerP(Truck truck, float cameraX, float cameraY, Level pLevel)
    {
        _level = pLevel;
        _truck = truck;

        _startPoint = new Vec2(cameraX, cameraY);
        //_startPoint = new Vec2(truck.x, truck.y);
        _mouse = new Vec2(Input.mouseX, Input.mouseY);

        _lineCanvas = new Canvas(game.width, game.height);
        AddChild(_lineCanvas);

        _catapultLine = new LineSegment(_startPoint.Clone(), _startPoint.Clone(), 0xff00ff00, 3);
        game.AddChild(_catapultLine);

        _ball = new Ball(40, _startPoint.Clone(), null, Color.Green);
        game.AddChild(_ball);

        _debugArrow = new Arrow(Vec2.zero, Vec2.zero, 1);
        AddChild(_debugArrow);

        _ballHandler = new MouseHandler(_ball);
        _ballHandler.OnMouseDownOnTarget += onBallMouseDown;
    }
    private void onBallMouseDown(GameObject target, MouseEventType type)
    {
        //Console.WriteLine(target + " mouse down.");

        _ballHandler.OnMouseMove += onBallMouseMove;
        _ballHandler.OnMouseUp += onBallMouseUp;
        //DONE: record start position
        //Vec2 startPos = _ball.position.Clone();

        //DONE: reset velocity
        _ball.velocity = Vec2.zero;
        //DONE: show line from start position to current position
        _catapultLine.end = _ball.position;
    }

    private void onBallMouseMove(GameObject target, MouseEventType type)
    {
        //Console.WriteLine(target + " mouse move.");
        //DONE: update ball position,
        //also check out _ballHandler.offsetToTarget

        Vec2 delta = _mouse.Clone().Subtract(_startPoint);
        float deltaLength = delta.length;

        Vec2 ballPos = _startPoint.Clone().Add(delta.Normalize().Scale(Mathf.Sqrt(deltaLength)).Scale(10));
        _ball.position.SetXY(ballPos);
        _catapultLine.end.SetXY(_ball.position);
    }

    private void onBallMouseUp(GameObject target, MouseEventType type)
    {
        //Console.WriteLine(target + " mouse up.");
        _ballHandler.OnMouseMove -= onBallMouseMove;
        _ballHandler.OnMouseUp -= onBallMouseUp;

        //DONE: calculate ball velocity
        _deltaVelocity = _startPoint.Clone().Subtract(_ball.position);
        _deltaVelocity.Scale(0.5f);
        //Ball newBall = new Ball(_ball.radius, _ball.position.Clone(), _ball.velocity.Clone(), Color.Green);
        //game.RemoveChild(_ball);
        //_level.AddChild(_ball);
        _ball.velocity = _deltaVelocity;
        Ball.ShootBall(_level.playerLayer, _ball, _truck.x, _truck.y);
        _catapultLine.end = _startPoint.Clone();
    }

    /**
	 * General event handler that can be used for debugging
	 */
    private void OnMouseEvent(GameObject target, MouseEventType type)
    {
        //Console.WriteLine("Eventtype:" + type + " triggered on " + target);
    }

    private void Update()
    {
        //MyInput();
        //if (paused) return;
        
        Vec2 oldPos = _ball.position.Clone();

        //_ball.Step();
        _mouse.SetXY(Input.mouseX, Input.mouseY);


        //foreach (LineSegment line in _segmentList)
        //{
        //    NewCollision(line, oldPos, false);
        //    NewCollision(line, oldPos, true);
        //}


        //BoundaryCollision(oldPos);

        _lineCanvas.graphics.DrawLine(Pens.Magenta, oldPos.x, oldPos.y, _ball.position.x, _ball.position.y);
    }
}