using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.Actors;
using SupergoonDashCrossPlatform.SupergoonEngine.Cameras;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.Tags;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform
{
    public class SupergoonDashGameWorld : GameWorld
    {
        //ui stuff
        private Texture2D rect;
        private Color[] data;
        private Vector2 coor = new(30, 30);
        private SpriteFont font;
        private Vector2 coinTextLoc = new(33, 35);
        private Vector2 deathTextLoc = new(33, 50);
        private Vector2 timeTextLoc = new(33, 65);

        public static int CoinAmount = 0;
        public static int MaxSpeed = 0;
        public static int Attempts = 0;
        public static TimeSpan TimeThisLevel = TimeSpan.Zero;

        protected override void Initialize()
        {
            base.Initialize();
            TiledActorFactory.NameToSpawnFunction["player"] = Player.FactoryFunction;
            TiledActorFactory.NameToSpawnFunction["coin"] = Coin.FactoryFunction;
            TiledActorFactory.NameToSpawnFunction["spikel"] = Spike.FactoryFunction;
            TiledActorFactory.NameToSpawnFunction["end"] = LevelEnd.FactoryFunction;

            //Load ui stuff
            rect = new Texture2D(GraphicsDevice, 160, 50);
            data = new Color[160 * 50];
            for (int i = 0; i < data.Length; ++i) data[i] = new Color(20, 20, 20, 175);
            rect.SetData(data);
            font = Content.Load<SpriteFont>("Fonts/ui");
        }

        protected override void BeginRun()
        {
            base.BeginRun();
            _graphicsGameComponent.ApplyResolutionSettings(false);

            var level1 = new Level("level1");
            level1.AddTag(LevelTags.Level1);
            AddLevels(level1);
            InitializeLevels();
            //TODO remove this, just there to start the music on restart and start currently.
            Reset();
            ChangeLevel(LevelTags.Level1);
        }

        public override void Reset()
        {
            base.Reset();
            GC.Collect();
            CameraGameComponent.MainCamera.Location = Vector3.Zero;
            _soundGameComponent.PlayBgm("level1");
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                null,
                _graphicsGameComponent.SpriteScale);

            //DrawUI components
            //Transparent box
            _spriteBatch.Draw(rect, coor, null, Color.White, 0.0f, new Vector2(), new Vector2(1, 1), SpriteEffects.None,
                0.9f);
            var stingText = $"Total Coins: {CoinAmount.ToString()} Max Speed: {MaxSpeed.ToString()} ";
            var deathText = $"Total Attempts: {Attempts.ToString()}";
            var minutes = TimeThisLevel.Minutes;
            var seconds = TimeThisLevel.TotalSeconds - (minutes * 60);
            seconds = Math.Truncate(10000 * seconds) / 10000;
            var timeText = $"Total Level Time: {minutes}:{seconds} ";
            _spriteBatch.DrawString(font, stingText,coinTextLoc, Color.DarkBlue );
            _spriteBatch.DrawString(font, deathText,deathTextLoc, Color.Azure );
            _spriteBatch.DrawString(font, timeText,timeTextLoc, Color.Chartreuse );


            _spriteBatch.End();
        }
    }
}