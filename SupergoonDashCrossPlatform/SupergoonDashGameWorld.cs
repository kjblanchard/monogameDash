using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.Actors;
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
        public static int CurrentLevel = 1;
        public static int MaxLevel = 2;

        protected override void Initialize()
        {
            base.Initialize();
            TiledActorFactory.NameToSpawnFunction["player"] = Player.FactoryFunction;
            TiledActorFactory.NameToSpawnFunction["coin"] = Coin.FactoryFunction;
            TiledActorFactory.NameToSpawnFunction["spike"] = Spike.FactoryFunction;
            TiledActorFactory.NameToSpawnFunction["end"] = LevelEnd.FactoryFunction;

            //Load ui stuff
            rect = new Texture2D(GraphicsDevice, 160, 50);
            data = new Color[160 * 50];
            for (int i = 0; i < data.Length; ++i) data[i] = new Color(20, 20, 20, 200);
            rect.SetData(data);
            font = Content.Load<SpriteFont>("Fonts/ui");
        }

        protected override void BeginRun()
        {
            base.BeginRun();
            _graphicsGameComponent.ApplyResolutionSettings(false);

            var level1 = new Level("level1", "level1");
            var level2 = new Level("level2", "level2");
            level1.AddTag(LevelTags.Level1);
            level2.AddTag(LevelTags.Level2);
            AddLevels(level1, level2);
            ChangeLevel(LevelTags.Level1);
        }


        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                null,
                _graphicsGameComponent.SpriteScale);

            DrawUi();


            _spriteBatch.End();
        }

        private void DrawUi()
        {
            //Draw UI components.
            _spriteBatch.Draw(rect, coor, null, Color.White, 0.0f, new Vector2(), new Vector2(1, 1), SpriteEffects.None,
                0.89f);
            var stingText = $"Total Coins: {CoinAmount.ToString()} Max Speed: {MaxSpeed.ToString()} ";
            var deathText = $"Total Attempts: {Attempts.ToString()}";
            var minutes = TimeThisLevel.Minutes;
            var seconds = TimeThisLevel.TotalSeconds - (minutes * 60);
            seconds = Math.Truncate(10000 * seconds) / 10000;
            var timeText = $"Total Level Time: {minutes}:{seconds} ";
            _spriteBatch.DrawString(font, stingText, coinTextLoc, Color.Orange, 0.0f, new Vector2(), 1.0f,
                SpriteEffects.None, 0.9f);
            _spriteBatch.DrawString(font, deathText, deathTextLoc, Color.Azure, 0.0f, new Vector2(), 1.0f,
                SpriteEffects.None, 0.9f);
            _spriteBatch.DrawString(font, timeText, timeTextLoc, Color.Chartreuse, 0.0f, new Vector2(), 1.0f,
                SpriteEffects.None, 0.9f);
        }

        public void RestartLevel()
        {
            CoinAmount = 0;
            LevelStateMachine.RestartLevel();
        }

        public void NextLevel()
        {
            //Gather the proper tag to use for the level.
            var nextLevel = CurrentLevel + 1;
            if (nextLevel > MaxLevel)
                nextLevel = 1;

            CurrentLevel = nextLevel;
            Attempts = 0;
            CoinAmount = 0;
            TimeThisLevel = TimeSpan.Zero;
            LevelStateMachine.ChangeState(CurrentLevel);
        }
    }
}