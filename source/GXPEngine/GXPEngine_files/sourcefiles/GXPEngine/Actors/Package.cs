using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GXPEngine;

public abstract class Package : Sprite
{
    public int ID_TYPE;
    public string filename;

    public float z;

    private Vec2 _old_position;

    private Vec2 _position;
    private Vec2 _velocity;
    private Vec2 _acceleration;
    public Vec2 gravity;
    public Vec2 wind;
    public Vec2 combinedForces;

    protected Level level;
    public float zSpeed = 2;
    public bool calculateZ;
    public bool flying;
    public bool pointsAwarded;

    public float depth;
    public float weight;
    protected float startingScale = 1;
    protected float radius;
    protected int collisions;
    protected readonly int maxCollisions = 1;

    protected float personalHeight;
    public float maxHeight;
    public float startX;
    protected float heightOffset;
    public float shootingAngle;

    public Package(string filename, Level pLevel, Vec2 pPosition = null, Vec2 pVelocity = null) : base(filename)
    {
        this.filename = filename;

        level = pLevel;
        flying = false;
        pointsAwarded = false;
        SetOrigin(width / 2, height / 2);

        position = pPosition ?? Vec2.zero;
        velocity = pVelocity ?? Vec2.zero;
        acceleration = Vec2.zero;

        x = position.x;
        y = position.y;
        SetScaleXY(0.8f);
        z = 0;

        gravity = Vec2.zero;
        wind = Vec2.zero;
        combinedForces = Vec2.zero;
        calculateZ = false;
        //color = 0xff00ff00;
        //radius = Mathf.Sqrt(width * width / 4 + height * height / 4);
        radius = height / 2;
        collisions = 0;

        zSpeed = level.depth / (level.timeToBack) * Time.deltaTime;// * 1000); //* Time.deltaTime;
        zSpeed = Mathf.Min(zSpeed, 15);
    }

    protected virtual void Update()
    {
        if (PauseManager.GetPause()) return;
        //if (collisions >= 2) Console.WriteLine("collisions: " + collisions);
        if (flying)
        {
            //SetOrigin(width / 2, height);
            if (calculateZ)
            {
                //z += level.height / level.depth;
                //z += zSpeed;
                z = Mathf.Tan(Vec2.Deg2Rad(45)) * (position.x - startX) / 6;
                //z = Mathf.Tan(Vec2.Deg2Rad(Mathf.Abs(shootingAngle - 45))) * (position.x - startX) / 2;
                z = Mathf.Min(z, level.depth);
            }
            Console.WriteLine(z);
            //Console.WriteLine(zSpeed);

            if (z > 16) Fall();

            //gravity *= (level.depth - z) / level.depth + z / level.depth / 2;
            wind *= (level.depth - z) / level.depth;
            acceleration = combinedForces.Clone();
        }


        Vec2 oldPos = position.Clone();
        if (flying || zSpeed != 0) Step();
        //Step();
        LayerDetection();
        if (position.y > level.height + height || position.x < 0 - width - level.truck.x || scale <= 0)
        {
            //Console.WriteLine(GetType() + " destroyed.");
            //base.Destroy();
            Destroy();
            level.listPackage.Remove(this);
        }
        if (scale > 1) Console.WriteLine("fuck");
    }

    protected void UpdateZ()
    {
        scale = startingScale - z / level.depth;
        //Console.WriteLine(scale);
        //velocity.x *= scale;
    }

    public Vec2 position
    {
        set
        {
            _position = value ?? Vec2.zero;
        }

        get
        {
            return _position;
        }
    }

    public Vec2 velocity
    {
        set
        {
            _velocity = value ?? Vec2.zero;
        }

        get
        {
            return _velocity;
        }
    }

    public Vec2 acceleration
    {
        set
        {
            _acceleration = value ?? Vec2.zero;
        }

        get
        {
            return _acceleration;
        }
    }

    public void Step()
    {

        if (flying) UpdateZ();

        _old_position = position.Clone();

        //Euler Integration
        if (acceleration != null && acceleration.x != 0 && acceleration.y != 0) velocity += acceleration;
        if (velocity != null && velocity.x != 0 && velocity.y != 0) position += velocity;

        x = position.x;
        y = position.y;
    }

    protected void SetCombinedForces()
    {
        gravity = level.gravity.Clone() / 2 * weight;
        wind = level.wind.Clone() / 2 / weight;
        combinedForces = gravity.Clone() + wind.Clone();
    }

    protected void UpdateCombinedForces()
    {
        if (flying)
        {
            combinedForces = gravity.Clone() + wind.Clone();
        }
    }

    public static Package GetRandomPackage(Vec2 pPosition = null, Vec2 pVelocity = null)
    {
        //Gets a random type of package from all the subclasses of Package and returns a package of that type

        List<Type> listOfTypes = Assembly.GetAssembly(typeof(Package)).
            GetTypes().Where(t => t.IsSubclassOf(typeof(Package))).ToList();
        Type randomType = listOfTypes[Utils.Random(0, listOfTypes.Count)];
        Package package = (Package)Activator.CreateInstance(randomType,
            new object[] { MyGame.currentLevel, pPosition, pVelocity }); //note optional parameters must be given!

        return package;
    }

    public Package Clone()
    {
        Package package = (Package)Activator.CreateInstance(GetType(),
            new object[] { level, position.Clone(), velocity.Clone() });
        package.SetXY(position.x, position.y);
        return package;
    }

    public Package CloneAt(float pos_x, float pos_y)
    {
        Package package = (Package)Activator.CreateInstance(GetType(),
            new object[] { level, new Vec2(pos_x, pos_y), velocity.Clone() });
        package.SetXY(position.x, position.y);
        return package;
    }

    private bool CollisionDetection()
    {
        bool collision = false;

        if (flying)
        {
            if (z - depth <= level.skybox.depth && HitTest(level.skybox)/*|| scale < 0.33f*/) collision = true;//return true;

            foreach (Package package in level.listPackage)
            {
                if (package != this && !package.flying && (z >= package.z && z + depth <= package.z) && HitTest(package))
                {
                    //To be polished.(package.z >= z - 16 && package.z <= z + 16)
                    x += 32;
                    y += 32;
                }
            }

            foreach (VerticalHitbox vertHitbox in level.vertList)
            {
                if (HitTest(vertHitbox))
                {
                    //Console.WriteLine("Vertical hit.");
                    //Console.WriteLine(3);
                    if (z >= vertHitbox.depth - level.tileSize / 16 - width && z <= vertHitbox.depth)
                    {
                        collision = true;
                        //calculateZ = false;
                        zSpeed = 0;
                        return true;
                        //z -= 10;
                        //Console.WriteLine(5);
                        //foreach (Line line in verHitbox.listOfLines)
                        //{
                        //    collision = true;
                        //    RodrigoCollision(line, false, 0.4f);
                        //    if (NewCollision(line, false, 0.4f)) collision = true;
                        //}
                    }

                    //else if (hitbox is HorizontalHitbox)
                    //{
                    //    //collision = false;
                    //    //Console.WriteLine(2);
                    //    //HorizontalHitbox horHitbox = hitbox as HorizontalHitbox;
                    //    //if (z >= horHitbox.startDepth && z < horHitbox.endDepth)
                    //    //{
                    //    //    Console.WriteLine("Horizontal hit.");
                    //    //    foreach (Line line in horHitbox.listOfLines)
                    //    //    {
                    //    //        if (z >= line.startDepth - 8 && z < line.endDepth + 8)
                    //    //        {
                    //    //            //Console.WriteLine("Line: " + line);
                    //    //            //Console.WriteLine("Package: " + position + ", z: " + z);
                    //    //            //NewCollision(line, _old_position, false, 0.05f);
                    //    //        }
                    //    //        //NewCollision(line, oldPos, true, 0.05f);
                    //    //    }
                    //    //}
                    //}

                    ////else if (HitTest(level.skybox) || scale < 0.33f) collision = true;
                    //else Console.WriteLine("Well then.");
                }
            }

            //foreach (HitBox hitbox in level.listHitbox)
            //{
            //    if (HitTest(hitbox))
            //    {
            //        if (hitbox is VerticalHitbox)
            //        {
            //            //Console.WriteLine("Vertical hit.");
            //            //Console.WriteLine(3);
            //            VerticalHitbox verHitbox = hitbox as VerticalHitbox;
            //            if (z >= verHitbox.depth - level.tileSize / 1 - width && z <= verHitbox.depth)
            //            {
            //                collision = true;
            //                calculateZ = false;
            //                zSpeed = 0;
            //                return true;
            //                //z -= 10;
            //                //Console.WriteLine(5);
            //                //foreach (Line line in verHitbox.listOfLines)
            //                //{
            //                //    collision = true;
            //                //    RodrigoCollision(line, false, 0.4f);
            //                //    if (NewCollision(line, false, 0.4f)) collision = true;
            //                //}
            //            }
            //        }

            //        //else if (hitbox is HorizontalHitbox)
            //        //{
            //        //    //collision = false;
            //        //    //Console.WriteLine(2);
            //        //    //HorizontalHitbox horHitbox = hitbox as HorizontalHitbox;
            //        //    //if (z >= horHitbox.startDepth && z < horHitbox.endDepth)
            //        //    //{
            //        //    //    Console.WriteLine("Horizontal hit.");
            //        //    //    foreach (Line line in horHitbox.listOfLines)
            //        //    //    {
            //        //    //        if (z >= line.startDepth - 8 && z < line.endDepth + 8)
            //        //    //        {
            //        //    //            //Console.WriteLine("Line: " + line);
            //        //    //            //Console.WriteLine("Package: " + position + ", z: " + z);
            //        //    //            //NewCollision(line, _old_position, false, 0.05f);
            //        //    //        }
            //        //    //        //NewCollision(line, oldPos, true, 0.05f);
            //        //    //    }
            //        //    //}
            //        //}

            //        ////else if (HitTest(level.skybox) || scale < 0.33f) collision = true;
            //        //else Console.WriteLine("Well then.");
            //    }
            //}
        }
        return collision;
    }

    private void LayerDetection()
    {
        if (z > level.frontFenceDepth - 16 && z <= level.houseDepth) //&& parent != level.packageLayer)
        {
            //Console.WriteLine(1);
            level.packageLayer.RemoveChild(this);
            level.behindFenceLayer.AddChild(this);
        }
        else if (z > level.houseDepth && z <= level.backFenceDepth)
        {
            //Console.WriteLine(2);
            level.behindFenceLayer.RemoveChild(this);
            level.behindHouseLayer.AddChild(this);
        }
        else if (z > level.backFenceDepth)
        {
            //Console.WriteLine(3);
            //Console.WriteLine(z + " ? " + level.backFenceDepth);
            level.behindHouseLayer.RemoveChild(this);
            level.behindBackFenceLayer.AddChild(this);
        }
    }

    private bool NewCollision(Line line, bool flipNormal, float bounciness = 1, bool vert = false)
    {
        //Console.WriteLine("NERD");
        //Vec2 lineToBall = line.start.Clone().Subtract(position);
        Vec2 lineToBall = this.position.Clone().Subtract(line.start);
        Vec2 lineVector = line.end.Clone().Subtract(line.start);
        Vec2 lineVectorNormal = lineVector.Normal();
        lineVectorNormal.Scale(flipNormal ? (-1) : 1);

        //shortest distance from the ball to the line
        //distance from ball to line along line normal
        //Console.WriteLine("lineToBall: " + lineToBall.length);
        float bitA = lineToBall.Dot(lineVectorNormal) - this.radius;

        //difference between old position and new position
        Vec2 old_to_new_position = this.position.Clone().Subtract(_old_position);

        //distance from old to new position along normal
        float bitB = old_to_new_position.Dot(lineVectorNormal);

        //Console.WriteLine("bitA: " + bitA);
        //Console.WriteLine("-bitB: " + -bitB);
        bool crossedTheLine = (bitA >= 0 && -bitB > bitA);

        //Start + (-A/B * velocity)
        Vec2 allowedMovement = this.position.Clone().Add(this.velocity.Clone().Scale(-bitA / bitB));
        //Vec2 allowedMovement = oldPos.Clone().Subtract(_ball.velocity.Clone().Scale(bitA / bitB));

        //difference between point of allowed movement or impact and start of the line
        Vec2 delta = allowedMovement.Subtract(line.start);

        //impact distance
        float impactDistance = delta.Dot(lineVector.Clone().Normalize());
        //Console.WriteLine(impactDistance);

        //Console.WriteLine("crossed " + crossedTheLine);
        //Console.WriteLine("impact distance " + impactDistance);
        //Console.WriteLine("linevector length" + lineVector.length);

        if (crossedTheLine && impactDistance >= 0 && impactDistance < lineVector.length)
        {
            position.SetXY(allowedMovement.Add(line.start));
            this.x = this.position.x;
            this.y = this.position.y;
            velocity.Reflect(lineVectorNormal, bounciness);
            zSpeed = 0;
            z -= 10;
            flying = false;
            return true;
        }

        return false;

        //Console.WriteLine("missed collision.");
        //else BumperCollision(line);
        //Console.WriteLine(lineVectorNormal);
        //Console.WriteLine(lineVector);
        //Console.WriteLine(lineToBall);
    }

    private void Fall()
    {
        float angle;
        if (shootingAngle >= 0 && shootingAngle < 360)
        {
            if (shootingAngle <= 90)
            {
                angle = shootingAngle / 2;
            }
            else //if (shootingAngle > 90)
            {
                angle = 90 - shootingAngle / 2;
            }
        }
        else if (shootingAngle < 0 && shootingAngle >= -360)
        {
            if (shootingAngle > -90)
            {
                angle = shootingAngle / 2;
            }
            else //if (shootingAngle <= -90)
            {
                angle = -90 - shootingAngle / 2;
            }
        }
        else angle = 0;

        float localX = position.x - startX;
        if (localX < 0) localX *= -1;

        float localY = Mathf.Tan(Vec2.Deg2Rad(shootingAngle)) * localX;
        heightOffset = Mathf.Tan(Vec2.Deg2Rad(angle)) * localX;
        personalHeight = (maxHeight - heightOffset) - position.y;
        //ListHeightStats();
        //if (CollisionDetection())
        //{
        //    Console.WriteLine("HIT");
        //}
        //else 
        if (!CollisionDetection() && personalHeight < 0)
        {
            List<GameObject> children = level.GetChildren().ToList();
            foreach (GameObject gobject in children)
            {
                if (!(gobject is Pivot)) Console.WriteLine("Collision with " + gobject.GetType());
            }

            FullStop();

            AwardPoints();

            SFX.PlaySound((MyUtils.RandomBool()) ? SFX.packageDrop : SFX.packageDrop2);
        }
    }

    private void ListHeightStats()
    {
        if (personalHeight != 0)
        {
            Console.WriteLine("SA: " + shootingAngle / 2);
            Console.WriteLine("\tX: " + (position.x - startX));
            Console.WriteLine("\tMH: " + maxHeight);
            Console.WriteLine("\tHO: " + heightOffset);
            Console.WriteLine("\tPH: " + personalHeight);
        }
    }

    private void FullStop()
    {
        //Full stop.
        Console.WriteLine("FULL STOP");
        zSpeed = 0;
        flying = false;
        calculateZ = false;
        velocity = Vec2.zero;
        gravity = Vec2.zero;
        wind = Vec2.zero;
        combinedForces = Vec2.zero;
    }

    private void AwardPoints()
    {
        if (!pointsAwarded)
        {
            Console.WriteLine("POINTS NOT AWARDED");
            foreach (HorizontalHitbox hitbox in level.horzList)
            {
                if (HitTest(hitbox))
                {
                    Console.WriteLine(GetType() + " COLLISION WITH: " + hitbox.GetType());
                    if (hitbox is DoorArea)
                    {
                        //Console.WriteLine(hitbox.GetType());
                        DoorArea door = hitbox as DoorArea;
                        door.gotHit = true;
                        level.truck.player.score += PointManager.pointsTable[ID_TYPE, door.ID_TYPE];
                        pointsAwarded = true;
                    }
                    else if (hitbox is Garden)
                    {
                        //Console.WriteLine(hitbox.GetType());
                        Garden garden = hitbox as Garden;
                        level.truck.player.score += PointManager.pointsTable[ID_TYPE, garden.ID_TYPE];
                        pointsAwarded = true;
                    }
                    else if (hitbox is Street)
                    {
                        //Console.WriteLine(hitbox.GetType());
                        Street street = hitbox as Street;
                        level.truck.player.score += PointManager.pointsTable[ID_TYPE, street.ID_TYPE];
                        pointsAwarded = true;
                    }
                }
            }
        }
    }
}

public class Letter : Package
{
    public Letter(Level pLevel, Vec2 pPosition = null, Vec2 pVelocity = null) : base("assets/items/packages/Letter.png", pLevel, pPosition, pVelocity)
    {
        ID_TYPE = 0;
        weight = 1;
        depth = 2;
        SetCombinedForces();
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        UpdateCombinedForces();
        base.Update();
    }
}

public class BigLetter : Package
{
    public BigLetter(Level pLevel, Vec2 pPosition = null, Vec2 pVelocity = null) : base("assets/items/packages/Letter_Big.png", pLevel, pPosition, pVelocity)
    {
        ID_TYPE = 1;
        weight = 2;
        depth = 4;
        SetCombinedForces();
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        UpdateCombinedForces();
        base.Update();
    }
}

public class Parcel : Package
{
    public Parcel(Level pLevel, Vec2 pPosition = null, Vec2 pVelocity = null) : base("assets/items/packages/Package_small.png", pLevel, pPosition, pVelocity)
    {
        ID_TYPE = 2;
        weight = 3;
        depth = 16;
        SetCombinedForces();
    }

    protected override void Update()
    {
        UpdateCombinedForces();
        base.Update();
    }
}

public class BigParcel : Package
{
    public BigParcel(Level pLevel, Vec2 pPosition = null, Vec2 pVelocity = null) : base("assets/items/packages/Package_Big.png", pLevel, pPosition, pVelocity)
    {
        ID_TYPE = 3;
        weight = 4;
        depth = 32;
        SetCombinedForces();
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        UpdateCombinedForces();
        base.Update();
    }
}

public class Potato : Package
{
    public Potato(Level pLevel, Vec2 pPosition = null, Vec2 pVelocity = null) : base("assets/items/packages/Potato.png", pLevel, pPosition, pVelocity)
    {
        ID_TYPE = 4;
        weight = 2.5f;
        depth = 6;
        SetCombinedForces();
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        UpdateCombinedForces();
        base.Update();
    }
}

public class RoundBox : Package
{
    public RoundBox(Level pLevel, Vec2 pPosition = null, Vec2 pVelocity = null) : base("assets/items/packages/Round_Box.png", pLevel, pPosition, pVelocity)
    {
        ID_TYPE = 5;
        weight = 3.5f;
        depth = 16;
        SetCombinedForces();
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        UpdateCombinedForces();
        base.Update();
    }
}

public class Clothes : Package
{
    public Clothes(Level pLevel, Vec2 pPosition = null, Vec2 pVelocity = null) : base("assets/items/packages/Clothes.png", pLevel, pPosition, pVelocity)
    {
        ID_TYPE = 6;
        weight = 3;
        depth = 8;
        SetCombinedForces();
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        UpdateCombinedForces();
        base.Update();
    }
}
