using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class Timer : GameObject
{
    private int _time;
    private Action _onTimeOut;
    private Action _onTimeOut0;
    public Timer(int timeOut, Action onTimeOut) : base()
    {
        _onTimeOut = onTimeOut;
        _time = timeOut;
    }
    public Timer(int timeOut, Action onTimeOut, Action onTimeOut0) : base()
    {
        _onTimeOut = onTimeOut;
        _onTimeOut0 = onTimeOut0;
        _time = timeOut;
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            Destroy();
            _onTimeOut();
            _onTimeOut0?.Invoke();
        }
    }

}