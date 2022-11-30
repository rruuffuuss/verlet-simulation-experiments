using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace verlet_idk
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<Body> bodies;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            _graphics.PreferredBackBufferWidth = display.X;
            _graphics.PreferredBackBufferHeight = display.Y;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var _nodeTexture = Content.Load<Texture2D>("node");
            var _arcTexture = Content.Load<Texture2D>("arc");


            var b = new Body(
                segNo: 24,

                segStructure: new int[][] {
                    new int[]{ 0 , 1 },
                    new int[]{ 0, 1, 2 },
                    new int[]{ 1, 2 } },
                    

                segRad: new Single[] { 80, 90, 100 },

                centre: new Vector2(350, 350),

                vel: new Vector2(3f, 0)
                )
            {
                nodeTexture = _nodeTexture,
                arcTexture = _arcTexture
            };
            /*
            var b = new Body(
                segNo: 3,

                segStructure: new int[][] {
                    new int[]{ 0 }
                },

                segRad: new Single[] {200},

                centre: new Vector2(350, 350),

                vel: new Vector2(8f, 0)
                )
            {
                nodeTexture = _nodeTexture,
                arcTexture = _arcTexture
            };
            */

            bodies = new List<Body>
            {
                b
            };
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            foreach(Body b in bodies)
            {
                b.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here

            foreach (Body b in bodies)
            {
                b.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
