using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks.Dataflow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Scissors_Paper_Stone_Simulation;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    List<Item> Items;
    float ScissorsScale = 0.035f;
    float PaperScale = 0.3f;
    float StoneScale = 0.15f;

    public class Item
    {
        public Texture2D objectTexture;
        public Vector2 objectPosition;
        public float objectSpeed;
        public string objectType;
    }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Items = new List<Item>();

        for (int i = 0; i < 20; i++)
        {
            Items.Add(new Item());
            Items[0 + 3*i].objectType = "Scissors";

            Items.Add(new Item());
            Items[1 + 3*i].objectType = "Paper";

            Items.Add(new Item());
            Items[2 + 3*i].objectType = "Stone";
        }
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = true;
        _graphics.PreferredBackBufferWidth = 2560;
        _graphics.PreferredBackBufferHeight = 1600;
        _graphics.ApplyChanges();


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].objectType == "Scissors")
            {
                Items[i].objectTexture = Content.Load<Texture2D>("Scissors");
                
                Items[i].objectPosition = new Vector2 (
                    new Random().Next(
                        (int)(Items[i].objectTexture.Width * ScissorsScale / 2),
                        (int)(_graphics.PreferredBackBufferWidth - (Items[i].objectTexture.Width * ScissorsScale / 2))
                    ),
                    new Random().Next(
                        (int)(Items[i].objectTexture.Height * ScissorsScale / 2),
                        (int)(_graphics.PreferredBackBufferHeight - (Items[i].objectTexture.Height * ScissorsScale / 2))
                    )
                );
            }
            else if (Items[i].objectType == "Paper")
            {
                Items[i].objectTexture = Content.Load<Texture2D>("Paper");

                Items[i].objectPosition = new Vector2 (
                    new Random().Next(
                        (int)(Items[i].objectTexture.Width * PaperScale / 2),
                        (int)(_graphics.PreferredBackBufferWidth - (Items[i].objectTexture.Width * PaperScale / 2))
                    ),
                    new Random().Next(
                        (int)(Items[i].objectTexture.Height * PaperScale / 2),
                        (int)(_graphics.PreferredBackBufferHeight - (Items[i].objectTexture.Height * PaperScale / 2))
                    )
                );
            }
            else if (Items[i].objectType == "Stone")
            {
                Items[i].objectTexture = Content.Load<Texture2D>("Stone");

                Items[i].objectPosition = new Vector2 (
                    new Random().Next(
                        (int)(Items[i].objectTexture.Width * StoneScale / 2),
                        (int)(_graphics.PreferredBackBufferWidth - (Items[i].objectTexture.Width * StoneScale / 2))
                    ),
                    new Random().Next(
                        (int)(Items[i].objectTexture.Height * StoneScale / 2),
                        (int)(_graphics.PreferredBackBufferHeight - (Items[i].objectTexture.Height * StoneScale / 2))
                    )
                );
            }
        }
    }   

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].objectType == "Scissors")
            {
                _spriteBatch.Draw(Items[i].objectTexture, Items[i].objectPosition, null, Color.White,
                                  0, new Vector2 (0, 0), new Vector2 (ScissorsScale, ScissorsScale), SpriteEffects.None, 1);
            }
            else if (Items[i].objectType == "Paper")
            {
                _spriteBatch.Draw(Items[i].objectTexture, Items[i].objectPosition, null, Color.White,
                                  0, new Vector2 (0, 0), new Vector2 (PaperScale, PaperScale), SpriteEffects.None, 0);
            }
            else if (Items[i].objectType == "Stone")
            {
                _spriteBatch.Draw(Items[i].objectTexture, Items[i].objectPosition, null, Color.White,
                                  0, new Vector2 (0, 0), new Vector2 (StoneScale, StoneScale), SpriteEffects.None, 0);
            }
        }

        base.Draw(gameTime);

        _spriteBatch.End();
    }
}
