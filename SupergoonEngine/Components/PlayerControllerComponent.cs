using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.SupergoonEngine.Input;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class PlayerControllerComponent : Component
{
    public PlayerController PlayerController;
    public List<ControllerButtonAndAction> ButtonsThisFrame;
    


    public PlayerControllerComponent(GameObject parent, int playerNumber = 0) : base(parent)
    {
        PlayerController = new PlayerController(playerNumber);
        UpdateOrder = EngineTags.ComponentTags.PlayerController;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        PlayerController.Update(gameTime);
    }
}