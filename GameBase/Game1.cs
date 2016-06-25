using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace cellgame
{
    /// <summary>
    /// This is the main type for your game
    /// プログラム自動生成のゲームの中核のクラス。あんまり触らない。
    /// タイトルとかいろいろいじりたいならここ
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SceneManager scenem;

        public const int WindowSizeX = 1280;
        public const int WindowSizeY = 960;
        internal static readonly Vector WindowSize = new Vector(WindowSizeX, WindowSizeY);

        public static bool AvailbleSpeedup = true;

        //倍率込みのサイズ　ふつうは扱わなくてよい　staticなのは苦しまぎれ
        public static int _WindowSizeX;
        public static int _WindowSizeY;
        

        public Game1()
        {
            //タイトル
            this.Window.Title = "WAR in vivo";

            graphics = new GraphicsDeviceManager(this);

            ChangeWindowSize(Settings.WindowStyle);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            
            Settings.WindowStyle = 1;

            scenem = new SceneManager(new Drawing(spriteBatch, new Drawing3D(GraphicsDevice), this));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            DataBase.hex1 = Content.Load<Texture2D>("hex1.png");
            DataBase.box_flame = new List<Texture2D>();
            for (int i = 0; i < 9; i++) {
                DataBase.box_flame.Add(Content.Load<Texture2D>(string.Format("box_flame{0}.png", i)));
            }
            // TODO: use this.Content to load your game content here
            TextureManager.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            SoundManager.Music.Close();
        }

        bool exited;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            // TODO: Add your update logic here
            if (!scenem.Update() && !exited) { this.Exit(); SoundManager.Music.Close(); exited = true; }
            base.Update(gameTime);
            SoundManager.Update();
        }

        void SetFPS(double value)
        {
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / value);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            scenem.Draw();

            base.Draw(gameTime);
        }

        public void ChangeWindowSize(int style)
        {
            _WindowSizeX = WindowSizeX;
            if (style == 1) _WindowSizeY = WindowSizeY;
            else _WindowSizeY = WindowSizeY - 240;

            graphics.PreferredBackBufferWidth = _WindowSizeX;
            graphics.PreferredBackBufferHeight = _WindowSizeY;
            graphics.ApplyChanges();
        }
    }
}
