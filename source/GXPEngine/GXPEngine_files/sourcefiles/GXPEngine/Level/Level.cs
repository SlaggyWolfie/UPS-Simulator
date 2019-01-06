using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;

public partial class Level : GameObject
{
    public Vec2 gravity;
    public float gravityVerticalPower;
    public Vec2 wind;
    public float windHorizontalPower;
    public Vec2 newWindDirection;

    private float _lastUpdateWind;
    private bool _windIsDoneChanging;
    private bool _typeOfWind; //true is Slow, false is Fast
    private bool _fastWindCheck; //goddamnit

    //Level and level building data
    public float width;
    public float height;
    public float depth;
    public readonly float tiltAngle = 45;
    public int tileSize;
    public string filename;

    public float frontFenceDepth;
    public float backFenceDepth;
    public float houseDepth;

    private Dictionary<string, string> _properties = new Dictionary<string, string> { };
    private bool _hasProperties;
    public float timeToBack;
    public int scoreToWin;

    //Drawing stuff
    private Canvas _myCanvas;

    //Camera
    private float _cameraOffsetLeft;
    private float _cameraOffsetBottom;
    private float _cameraOffsetTop;

    private HUD _hud;

    public Truck truck;
    private float _windowWidth;
    private float _windowHeight;

    //
    public List<Package> packageWaitingList = new List<Package> { };

    //Collision stuff
    public Skybox skybox;
    public List<Package> listPackage = new List<Package> { };
    public List<HitBox> listHitbox = new List<HitBox> { };
    public List<VerticalHitbox> vertList = new List<VerticalHitbox> { };
    public List<HorizontalHitbox> horzList = new List<HorizontalHitbox> { };

    //Layers
    public Pivot packageLayer;
    public Pivot playerLayer;
    public Pivot HUD_Layer;
    public Pivot backgroundLayer;
    public Pivot hitboxLayer;
    public Pivot behindFenceLayer;
    public Pivot fenceLayer;
    public Pivot houseLayer;
    public Pivot behindHouseLayer;
    public Pivot backFenceLayer;
    public Pivot behindBackFenceLayer;

    //Bird Chirp sound
    private readonly int _cooldownChirp = 6000; //milliseconds
    private int _lastUpdateChirp;
    private int _chirpLength = 27000; //ms

    public Level(string filename)
    {
        MyGame.currentLevel = this;
        Console.WriteLine(filename);
        MyGame.gameHasStarted = true;
        PauseManager.UnPause();
        gravityVerticalPower = 3;
        windHorizontalPower = -1;
        gravity = new Vec2(0, gravityVerticalPower);
        wind = new Vec2(windHorizontalPower, 0);
        newWindDirection = Vec2.zero;

        _windIsDoneChanging = true;
        _lastUpdateWind = Time.time;

        //Level stuff
        TMXParser tmxParser = new TMXParser();
        Map map = tmxParser.Parse(filename);

        this.filename = filename;
        SetTileSize(map);

        width = map.Width * tileSize;
        height = map.Height * tileSize;

        if (map.Properties != null && map.Properties.Property.Length > 0)
        {
            for (int i = 0; i < map.Properties.Property.Length; i++)
            {
                //if (map.Properties.Property[i].Type == "float" && map.Properties.Property[i].Value != Mathf.Floor(map.Properties.Property[i].Value)) map.Properties.Property[i].Value += "f"; 
                _properties[map.Properties.Property[i].Name.ToLower()] = map.Properties.Property[i].Value;
                //float testFloat;
                //bool floatParseSuccess = float.TryParse(_properties[map.Properties.Property[i].Name.ToLower()], out testFloat);
                //if (!floatParseSuccess)
                //{
                //    //_properties[map.Properties.Property[i].Name.ToLower()] += "f";
                //}
                ////Console.WriteLine(_properties[map.Properties.Property[i].Name.ToLower()]);
                ////Console.WriteLine(testFloat);
            }
            _hasProperties = true;
        }

        depth = 1024;
        timeToBack = 1000000;
        scoreToWin = 100;

        if (_hasProperties)
        {
            if (_properties.ContainsKey("depth")) depth = float.Parse(_properties["depth"]);
            if (_properties.ContainsKey("time to get back")) timeToBack = float.Parse(_properties["time to get back"]);
            if (_properties.ContainsKey("score to win")) scoreToWin = int.Parse(_properties["score to win"]);
        }

        //Console.WriteLine(depth + " " + timeToBack + " " + scoreToWin);

        //Camera setup
        _cameraOffsetLeft = game.width / 3;
        _cameraOffsetBottom = game.height / 3;
        _cameraOffsetTop = game.height * 5 / 6;

        //Possible future use
        _windowWidth = game.width;
        _windowHeight = game.height;

        //_myCanvas = new Canvas(_width, _height);
        //AddChild(_myCanvas);

        SetUpLayers();

        ParseMap(map);
        ParseObjects(map);

        _hud = new HUD(this);
        HUD_Layer.AddChild(_hud);

        //Initialize sounds
        SFX.PlaySound(SFX.backgroundMusic);
        _lastUpdateChirp = Time.time - _chirpLength - _cooldownChirp;
    }

    private void Update()
    {
        if (Time.time > _lastUpdateChirp + _chirpLength + _cooldownChirp)
        {
            SFX.PlaySound(SFX.birdChirpingBackground);
            _lastUpdateChirp = Time.time;
        }

        if (PauseManager.GetPause())
        {
            if (SFX.soundChannelLibrary.ContainsKey(SFX.birdChirpingBackground)) SFX.soundChannelLibrary[SFX.birdChirpingBackground].IsPaused = false;
            return;
        }

        RandomWind();

        Camera();
    }

    private void Camera()
    {
        //Camera
        //if (x > _cameraOffsetLeft - truck.x) x = _cameraOffsetLeft - truck.x;
        //else if (x < _cameraOffsetLeft) x = 0;
        if (truck.x + (game.width - _cameraOffsetLeft) < width) x = _cameraOffsetLeft - truck.x;
        if (truck.x - _cameraOffsetLeft < 0) x = 0;
        //else 
        if (y >= _cameraOffsetTop - truck.y) y = _cameraOffsetTop - truck.y;
        else if (y < -_cameraOffsetTop) y = _cameraOffsetTop;

        //HUD related
        HUD_Layer.x = -x;
        HUD_Layer.y = -y;

        //MyUtils.ControlObject(this);
    }

    public void ShootPackage(Package pPackage, float pX, float pY, float z_speed = 0)
    {
        Package package = pPackage.CloneAt(pX - truck.width * 3 / 4, pY - truck.height / 2);
        if (z_speed != 0) package.zSpeed = z_speed;
        pPackage.Destroy();
        packageLayer.AddChild(package);
        listPackage.Add(package);

        truck.player.slingshotIsEmpty = true;
        package.flying = true;
        package.SetOrigin(package.width / 2, package.height);
    }

    public void ShootPackage(Package pPackage, float pX, float pY, float z_speed = 0, float shootingAngle = 0)
    {
        Package package = pPackage.CloneAt(pX - truck.width * 3 / 4, pY - truck.height / 2);
        if (z_speed != 0) package.zSpeed = z_speed;
        pPackage.Destroy();
        packageLayer.AddChild(package);
        listPackage.Add(package);

        //Falling stuff
        package.maxHeight = package.position.y + truck.height / 2;
        package.startX = package.position.x;
        package.shootingAngle = shootingAngle;

        truck.player.slingshotIsEmpty = true;
        package.flying = true;
        package.calculateZ = true;
        package.SetOrigin(package.width / 2, package.height / 2);
    }

    public void FastWind()
    {
        wind.SetAngleDegrees(Utils.Random(0, 180));
    }

    public void SlowWind(Vec2 newWindDirection)
    {
        float angleTo0 = wind.GetAngleDegrees();

        wind.SetAngleDegrees(wind.GetAngleDegrees() - angleTo0);
        newWindDirection.SetAngleDegrees(newWindDirection.GetAngleDegrees() - angleTo0);

        float differenceAngle = newWindDirection.GetAngleDegrees() - wind.GetAngleDegrees(); //wind degrees not necesary - but better safe than sorry!
        float rotationalSpeed = Mathf.Max(Mathf.Abs(differenceAngle / 100), 0.25f);
        if (differenceAngle < 0) rotationalSpeed *= -1; 
        wind.SetAngleDegrees(wind.GetAngleDegrees() + angleTo0 + rotationalSpeed);
        newWindDirection.SetAngleDegrees(newWindDirection.GetAngleDegrees() + angleTo0);

        if (Mathf.Abs(wind.GetAngleDegrees() - newWindDirection.GetAngleDegrees()) < 0.25f)
        {
            this.newWindDirection = Vec2.zero;
            _lastUpdateWind = Time.time;
            _windIsDoneChanging = true;
        }
    }

    private void RandomWind()
    {
        //if (Input.GetKeyDown(Key.SPACE)) Console.WriteLine(_windIsDoneChanging);
        if (_windIsDoneChanging)
        {
            if (Time.time > _lastUpdateWind + Utils.Random(12, 18) * 1000)
            {
                _windIsDoneChanging = false;
                _typeOfWind = MyUtils.RandomBool();
                if (!_typeOfWind) _fastWindCheck = false;
                _lastUpdateWind = Time.time;
            }
        }

        if (!_windIsDoneChanging)
        {
            if (_typeOfWind)
            {
                if (newWindDirection.x == 0 && newWindDirection.y == 0)
                {
                    newWindDirection = new Vec2(1, 0).SetAngleDegrees(Utils.Random(0, 180));
                }
                SlowWind(newWindDirection);
            }
            else if (!_typeOfWind && !_fastWindCheck)
            {
                int numberOfTimes = Utils.Random(1, 5);
                int timeDifference = Utils.Random(1000, 5000);
                bool timeVariation = MyUtils.RandomBool();
                for (int i = 0; i < numberOfTimes; i++)
                {
                    int timeVar = timeDifference * i + ((timeVariation) ? Utils.Random(timeDifference / 4, timeDifference / 2) : 0);
                    Timer timer = new Timer(timeVar, FastWind);
                    _fastWindCheck = true;
                    if (i >= numberOfTimes - 1)
                    {
                        Timer windTimer = new Timer(timeVar, SetWindDone);
                    }
                }
            }
        }
    }

    private void SetWindDone()
    {
        _fastWindCheck = true;
        _windIsDoneChanging = true;
        _lastUpdateWind = Time.time;
    }

    //protected override void OnDestroy()
    //{
    //    base.OnDestroy();
    //    foreach (GameObject go in GetChildren())
    //    {
    //        //if (go.GetChildren().Count > 0)
    //        //{
    //        //    foreach (GameObject go_ in go.GetChildren())
    //        //    {
    //        //        RemoveChild(go_);
    //        //        go_.Destroy();
    //        //    }
    //        //}
    //        RemoveChild(go);
    //        go.Destroy();
    //    }
    //}
}