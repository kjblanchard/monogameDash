using System;
using Microsoft.Xna.Framework;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Cameras;

public class Camera
{
    public Vector3 Location = Vector3.Zero;
    private static CameraGameComponent _cameraGameComponent;


    /// <summary>
    /// The main Game camera.  This is used to move around the current map.
    /// </summary>
    /// <param name="resolutionHelper">The resolution help that it works with</param>
    /// <param name="graphicsDevice">The graphics device that is used for calculations</param>
    public Camera(CameraGameComponent cameraGameComponent)
    {
        _cameraGameComponent = cameraGameComponent;
    }

    public Point GetWorldSize() => _cameraGameComponent.GetWorldSize();

    public Point GetWindowSize() => _cameraGameComponent.GetWindowSize();
    

    /// <summary>
    /// Returns the resolution world coordinates for the current screen position, probably used for UI
    /// </summary>
    /// <param name="screenPosition">The current position on the screen</param>
    /// <returns>The position in the world</returns>
    public Vector2 ScreenToWorldResolution(Vector2 screenPosition)
    {
        var viewportTopLeft = _cameraGameComponent.GetTopLeftOfViewport();
        var screenToWorldScale = _cameraGameComponent.GetScreenToWorldScale();
        return (screenPosition - viewportTopLeft) * screenToWorldScale;
    }

    /// <summary>
    /// Probably used for rendering most game objects, this will add in the camera offset to the gameObject so that it's in the proper location
    /// </summary>
    /// <param name="currentLocation">The current location of the object in the game world</param>
    /// <returns>The current location of the object in the gameworld in regards to the camera</returns>
    public Vector2 CalculateCameraOffset(Vector2 currentLocation)
    {
        currentLocation.X -= Location.X;
        currentLocation.Y -= Location.Y;
        return currentLocation;
    }

    /// <summary>
    /// Calculates the current position given translated from Screen to world, with the camera offset
    /// </summary>
    /// <param name="currentScreenLocation">The current screen location</param>
    /// <returns></returns>
    public Vector2 ScreenToWorldAndCamOffset(Vector2 currentScreenLocation)
    {
        var locAndCamOffset = CalculateCameraOffset(currentScreenLocation);

        return ScreenToWorldResolution(locAndCamOffset);
    }

    /// <summary>
    /// Gets the transform matrix of the CameraTransform matrix
    /// </summary>
    /// <returns></returns>
    public Matrix GetCameraTransformMatrix()
    {
        // var matrix = Matrix.CreateScale(
            // (float) _graphicsDevice.Viewport.Width / _graphicsGameComponent.WorldSize.X,
            // (float) _graphicsDevice.Viewport.Height / _graphicsGameComponent.WorldSize.Y, 1);
            var location = Location;
            location.X = (float)Math.Round(location.X);
            location.Y = (float)Math.Round(location.Y);
            location.X = -location.X;
        // Matrix.CreateTranslation(ref Location, out var matrix);
        Matrix.CreateTranslation(ref location, out var matrix);
        return matrix;
    }


    public int LevelWidth => _cameraGameComponent.GetCurrentLevelWidth();
}