using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks.Dataflow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Scissors_Paper_Stone_Simulation;

public class ScissorsPaperStone : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    List<Item> Items;
    float ScissorsScale = 0.035f;
    float PaperScale = 0.3f;
    float StoneScale = 0.15f;
    float speed = 300f;
    string ScissorsTexture = "Scissors";
    string PaperTexture = "Paper";
    string StoneTexture = "Stone";

    public class Item
    {
        public Texture2D Texture;
        public Vector2 Position;
        public string Type;
        public Vector2 Direction = new Vector2(
            new Random().Next(-10000, 10000) / 10000f,
            new Random().Next(-10000, 10000) / 10000f
            );
        public float Scale;
        public string TextureName;
    }

    public ScissorsPaperStone()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Items = new List<Item>();

        for (int i = 0; i < 20; i++)
        {
            Items.Add(new Item());
            Items[0 + 3*i].Type = "Scissors";

            Items.Add(new Item());
            Items[1 + 3*i].Type = "Paper";

            Items.Add(new Item());
            Items[2 + 3*i].Type = "Stone";
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

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Type == "Scissors")
            {
                Items[i].Texture = Content.Load<Texture2D>("Scissors");
                Items[i].Scale = ScissorsScale;
                Items[i].Position = new Vector2 (
                    new Random().Next(
                        (int)(Items[i].Texture.Width * ScissorsScale / 2),
                        (int)(_graphics.PreferredBackBufferWidth - (Items[i].Texture.Width * ScissorsScale / 2))
                    ),
                    new Random().Next(
                        (int)(Items[i].Texture.Height * ScissorsScale / 2),
                        (int)(_graphics.PreferredBackBufferHeight - (Items[i].Texture.Height * ScissorsScale / 2))
                    )
                );
                Items[i].TextureName = ScissorsTexture;
            }
            else if (Items[i].Type == "Paper")
            {
                Items[i].Texture = Content.Load<Texture2D>("Paper");
                Items[i].Scale = PaperScale;
                Items[i].Position = new Vector2 (
                    new Random().Next(
                        (int)(Items[i].Texture.Width * PaperScale / 2),
                        (int)(_graphics.PreferredBackBufferWidth - (Items[i].Texture.Width * PaperScale / 2))
                    ),
                    new Random().Next(
                        (int)(Items[i].Texture.Height * PaperScale / 2),
                        (int)(_graphics.PreferredBackBufferHeight - (Items[i].Texture.Height * PaperScale / 2))
                    )
                );
                Items[i].TextureName = PaperTexture;
            }
            else if (Items[i].Type == "Stone")
            {
                Items[i].Texture = Content.Load<Texture2D>("Stone");
                Items[i].Scale = StoneScale;
                Items[i].Position = new Vector2 (
                    new Random().Next(
                        (int)(Items[i].Texture.Width * StoneScale / 2),
                        (int)(_graphics.PreferredBackBufferWidth - (Items[i].Texture.Width * StoneScale / 2))
                    ),
                    new Random().Next(
                        (int)(Items[i].Texture.Height * StoneScale / 2),
                        (int)(_graphics.PreferredBackBufferHeight - (Items[i].Texture.Height * StoneScale / 2))
                    )
                );
                Items[i].TextureName = StoneTexture;
            }
        }
    }   

    protected override void Update(GameTime gameTime)
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        for (int i = 0; i < Items.Count; i++)
        {
            // Movement and Collision with wall checks
            if (Items[i].Position.X + speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Items[i].Direction.X >
                _graphics.PreferredBackBufferWidth - (Items[i].Texture.Width * Items[i].Scale / 2) ||
                Items[i].Position.X + speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Items[i].Direction.X <
                Items[i].Texture.Width * Items[i].Scale / 2)
                {
                    Items[i].Direction.X = -Items[i].Direction.X;
                }
            Items[i].Position.X = Items[i].Position.X + speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Items[i].Direction.X;

            if (Items[i].Position.Y + speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Items[i].Direction.Y >
                _graphics.PreferredBackBufferHeight - (Items[i].Texture.Height * Items[i].Scale / 2) ||
                Items[i].Position.Y + speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Items[i].Direction.Y <
                Items[i].Texture.Height * Items[i].Scale / 2)
                {
                    Items[i].Direction.Y = -Items[i].Direction.Y;
                }
            Items[i].Position.Y = Items[i].Position.Y + speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Items[i].Direction.Y;

            // Collision Checks
            for (int x = i + 1; x < Items.Count; x++)
            {
                if (Items[i].Position.X < Items[x].Position.X &&
                    Items[i].Position.X + Items[i].Texture.Width * Items[i].Scale > Items[x].Position.X &&
                    Items[i].Position.Y < Items[x].Position.Y &&
                    Items[i].Position.Y + Items[i].Texture.Height * Items[i].Scale > Items[x].Position.Y ||

                    Items[i].Position.X < Items[x].Position.X + Items[x].Texture.Width * Items[x].Scale &&
                    Items[i].Position.X + Items[i].Texture.Width * Items[i].Scale > Items[x].Position.X + Items[x].Texture.Width * Items[x].Scale &&
                    Items[i].Position.Y < Items[x].Position.Y &&
                    Items[i].Position.Y + Items[i].Texture.Height * Items[i].Scale > Items[x].Position.Y ||

                    Items[i].Position.X < Items[x].Position.X &&
                    Items[i].Position.X + Items[i].Texture.Width * Items[i].Scale > Items[x].Position.X &&
                    Items[i].Position.Y < Items[x].Position.Y + Items[x].Texture.Height * Items[x].Scale &&
                    Items[i].Position.Y + Items[i].Texture.Height * Items[i].Scale > Items[x].Position.Y + Items[x].Texture.Height * Items[x].Scale ||

                    Items[i].Position.X < Items[x].Position.X + Items[x].Texture.Width * Items[x].Scale &&
                    Items[i].Position.X + Items[i].Texture.Width * Items[i].Scale > Items[x].Position.X + Items[x].Texture.Width * Items[x].Scale &&
                    Items[i].Position.Y < Items[x].Position.Y + Items[x].Texture.Height * Items[x].Scale &&
                    Items[i].Position.Y + Items[i].Texture.Height * Items[i].Scale > Items[x].Position.Y + Items[x].Texture.Height * Items[x].Scale
                    )
                    {
                        // Condition 1: Items[i] wins
                        if (Items[i].Type == "Scissors" && Items[x].Type == "Paper" ||
                            Items[i].Type == "Paper" && Items[x].Type == "Stone" ||
                            Items[i].Type == "Stone" && Items[x].Type == "Scissors")
                        {
                            Items[x].Type = Items[i].Type;
                            Items[x].Texture = Content.Load<Texture2D>(Items[i].TextureName);
                            Items[x].Scale = Items[i].Scale;
                            Items[x].TextureName = Items[i].TextureName;
                        }
                        // Condition 2: Items[i] Loses
                        else if (Items[x].Type == "Scissors" && Items[i].Type == "Paper" ||
                                 Items[x].Type == "Paper" && Items[i].Type == "Stone" ||
                                 Items[x].Type == "Stone" && Items[i].Type == "Scissors")
                        {
                            Items[i].Type = Items[x].Type;
                            Items[i].Texture = Content.Load<Texture2D>(Items[x].TextureName);
                            Items[i].Scale = Items[x].Scale;
                            Items[i].TextureName = Items[x].TextureName;
                        }
                        // Condition 3: Draw - Do nothing
                    }
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Type == "Scissors")
            {
                _spriteBatch.Draw(Items[i].Texture, Items[i].Position, null, Color.White,
                                  0, new Vector2 (0, 0), new Vector2 (ScissorsScale, ScissorsScale), SpriteEffects.None, 1);
            }
            else if (Items[i].Type == "Paper")
            {
                _spriteBatch.Draw(Items[i].Texture, Items[i].Position, null, Color.White,
                                  0, new Vector2 (0, 0), new Vector2 (PaperScale, PaperScale), SpriteEffects.None, 0);
            }
            else if (Items[i].Type == "Stone")
            {
                _spriteBatch.Draw(Items[i].Texture, Items[i].Position, null, Color.White,
                                  0, new Vector2 (0, 0), new Vector2 (StoneScale, StoneScale), SpriteEffects.None, 0);
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
