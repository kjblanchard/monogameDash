using System;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Cameras;

public class CameraBoom
{
    private Camera _camera;
    private GameObject _target;

    public CameraBoom(GameObject target, Camera camera = null)
    {
        _camera = camera ?? CameraGameComponent.MainCamera;
        _target = target;
    }


    public void KeepCameraAtHalfScreen()
    {
        var diff = _target.Location.X - _camera.Location.X;
        var middle_screen_x = _camera.GetWorldSize().X / 2;
        //Try moving Right if needed if the camera has room
        var noRoom = _camera.LevelWidth - _camera.GetWorldSize().X;
        if (_camera.Location.X < noRoom)
        {
            if (diff >= middle_screen_x)
            {
                var offset = diff - middle_screen_x;
                _camera.Location.X += offset;
            }
        }

        //Try moving left if needed if the camera has room
        if (_camera.Location.X > 0)
        {
            if (diff < middle_screen_x)
            {
                var offset = middle_screen_x - diff;
                _camera.Location.X -= offset;
            }
        }
        
        ReduceJitter();


    }

    /// <summary>
    /// Without this, occasionally there will be a large jitter when camera is moving.
    /// </summary>
    private void ReduceJitter()
    {
        _camera.Location.X = (float)Math.Floor(_camera.Location.X);
        _camera.Location.Y = (float)Math.Floor(_camera.Location.Y);
        
    }
}