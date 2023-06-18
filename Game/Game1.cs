using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.XInput;
using SpaceGame.Code;
using System.Diagnostics.Contracts;

namespace SpaceGame
{
    public enum Stat
    {
        StartScreen,
        Information,
        Level1,
        Level2,
        Lose,
        Final
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static Stat Stat = Stat.StartScreen;
        public static Song death;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.PreferredBackBufferHeight = 650;
            _graphics.PreferredBackBufferWidth = 840;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            death = Content.Load<Song>("death1");
            StartScreen.Background = Content.Load<Texture2D>("background");
            StartScreen.Font = Content.Load<SpriteFont>("StartFont");
            StartScreen.Font2 = Content.Load<SpriteFont>("Text");

            Information.Background = Content.Load<Texture2D>("background");
            Information.Font1 = Content.Load<SpriteFont>("Text");
            Information.Font2 = Content.Load<SpriteFont>("StartFont");

            Level1Screen.Background = Content.Load<Texture2D>("backgrLevel2");
            Level1Screen.Font = Content.Load<SpriteFont>("Level1Font");
            Level1Screen.FinalDoor = Content.Load<Texture2D>("finalDoor");

            Level2Screen.Background = Content.Load<Texture2D>("backLevel2");
            Level2Screen.Font = Content.Load<SpriteFont>("Level1Font");
            Level2Screen.FinalDool = Content.Load<Texture2D>("finalDoor");

            LoseScreen.Background = Content.Load<Texture2D>("background");
            LoseScreen.Font = Content.Load<SpriteFont>("StartFont");

            FinalScreen.Background = Content.Load<Texture2D>("final2");
            FinalScreen.Font = Content.Load<SpriteFont>("StartFont");

            PlayerLives.Init(_spriteBatch, PlayerLives.allHearts);

            foreach (var couple in PlayerLives.allHearts)
            {
                couple.FullHeart = Content.Load<Texture2D>("Full1");
                couple.EmptyHeart = Content.Load<Texture2D>("Empty1");
            }

            Portals.Init(_spriteBatch, Portals.ports1, Portals.ports2);

            Portals.ports1[0].Port = new Texture2D[] { Content.Load<Texture2D>("port11"),
                                                      Content.Load<Texture2D>("port12"),
                                                      Content.Load<Texture2D>("port13")};

            Portals.ports1[1].Port = new Texture2D[] { Content.Load<Texture2D>("portBlue1"),
                                                      Content.Load<Texture2D>("portBlue2"),
                                                      Content.Load<Texture2D>("portBlue3")};

            Portals.ports1[2].Port = new Texture2D[] { Content.Load<Texture2D>("portGreen1"),
                                                      Content.Load<Texture2D>("portGreen2"),
                                                      Content.Load<Texture2D>("portGreen3")};
            Portals.ports2[0].Port = Portals.ports1[0].Port;
            Portals.ports2[1].Port = Portals.ports1[1].Port;
            Portals.ports2[2].Port = Portals.ports1[2].Port;

            MovedWalls.Init(_spriteBatch);
            MovedWalls.MovedWall.Img = Content.Load<Texture2D>("wallKnife1");
            StaticWalls.Init(_spriteBatch);
            StaticWalls.staticWall.Img = Content.Load<Texture2D>("staticWall");

            Switches.Init(_spriteBatch, Switches.Button1, Switches.Button2);
            Code.Buttons.ButtonNon = Content.Load<Texture2D>("click_r");
            Code.Buttons.ButtonPress = Content.Load<Texture2D>("click2");

            Player.Init(_spriteBatch);
            Player.PlayerMoves.Stay = new Texture2D[] { Content.Load<Texture2D>("v0"),
                                                        Content.Load<Texture2D>("v01")};
            Player.PlayerMoves.MoveToRight = new Texture2D[] { Content.Load<Texture2D>("r1"),
                                                            Content.Load<Texture2D>("r2"),
                                                            Content.Load<Texture2D>("r3"),
                                                            Content.Load<Texture2D>("r4"),
                                                            Content.Load<Texture2D>("r5"),
                                                            Content.Load<Texture2D>("r6"),
                                                            Content.Load<Texture2D>("r7"),
                                                            Content.Load<Texture2D>("r8")};
            
            Player.PlayerMoves.MoveToLeft = new Texture2D[] { Content.Load<Texture2D>("l1"),
                                                            Content.Load<Texture2D>("l2"),
                                                            Content.Load<Texture2D>("l3"),
                                                            Content.Load<Texture2D>("l4"),
                                                            Content.Load<Texture2D>("l5"),
                                                            Content.Load<Texture2D>("l6"),
                                                            Content.Load<Texture2D>("l7"),
                                                            Content.Load<Texture2D>("l8")};

            Enemy.Init(_spriteBatch, Enemy.Bomb);
            Enemy.Bomb.Boom = new Texture2D[] { Content.Load<Texture2D>("boom1"),
                                          Content.Load<Texture2D>("boom2"),
                                          Content.Load<Texture2D>("boom3"),
                                          Content.Load<Texture2D>("boom4"),
                                          Content.Load<Texture2D>("boom5"),
                                          Content.Load<Texture2D>("boom6"),
                                          Content.Load<Texture2D>("boom7"),
                                          Content.Load<Texture2D>("boom6"),
                                          Content.Load<Texture2D>("boom7"),
                                          Content.Load<Texture2D>("boom7"),
                                          Content.Load<Texture2D>("boom6"),
                                          Content.Load<Texture2D>("boom8")};
        }

        protected override void Update(GameTime gameTime)
        {
            switch (Stat)
            {
                case Stat.StartScreen:
                    StartScreen.Update(_spriteBatch);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && StartScreen.Game)
                    {
                        Stat = Stat.Level1;
                        _graphics.PreferredBackBufferHeight = 720;
                        _graphics.PreferredBackBufferWidth = 1280;
                        _graphics.ApplyChanges();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !StartScreen.Game 
                        && StartScreen.TimeCounter > 15)
                    {
                        Stat = Stat.Information;
                        StartScreen.TimeCounter = 0;
                        _graphics.PreferredBackBufferHeight = 650;
                        _graphics.PreferredBackBufferWidth = 840;
                        _graphics.ApplyChanges();
                    }
                    break;
                case Stat.Information:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && Information.TimeCounter > 15)
                        {
                            Stat = Stat.StartScreen;
                            Information.TimeCounter = 0;
                        }
                    }
                    break;
                case Stat.Level1:
                    {
                        MovedWalls.Update();
                        StaticWalls.Update();
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            Stat = Stat.StartScreen;
                            _graphics.PreferredBackBufferHeight = 650;
                            _graphics.PreferredBackBufferWidth = 840;
                            _graphics.ApplyChanges();
                        }
                        if ((Keyboard.GetState().IsKeyDown(Keys.Space) &&
                            PlayerRun.PlayerPos.X >= Level1Screen.PosFinal.X - 35 &&
                            PlayerRun.PlayerPos.X <= Level1Screen.PosFinal.X + 115 &&
                            PlayerRun.PlayerPos.Y == Level1Screen.PosFinal.Y + 10))     
                        {
                            Stat = Stat.Level2;
                            _graphics.PreferredBackBufferHeight = 720;
                            _graphics.PreferredBackBufferWidth = 1280;
                            _graphics.ApplyChanges();
                        }
                        if (PlayerLives.CurHeart >= 2 && PlayerLives.allHearts[PlayerLives.CurHeart].Enemy)
                        {
                            Stat = Stat.Lose;
                            _graphics.PreferredBackBufferHeight = 650;
                            _graphics.PreferredBackBufferWidth = 840;
                            _graphics.ApplyChanges();
                        }
                    }
                    break;
                case Stat.Level2:
                    {
                        MovedWalls.Update();
                        StaticWalls.Update();
                        if (PlayerLives.CurHeart >= 2 && PlayerLives.allHearts[PlayerLives.CurHeart].Enemy)
                        {
                            Stat = Stat.Lose;
                            _graphics.PreferredBackBufferHeight = 650;
                            _graphics.PreferredBackBufferWidth = 840;
                            _graphics.ApplyChanges();
                        }
                        if ((Keyboard.GetState().IsKeyDown(Keys.Space) &&
                            PlayerRun.PlayerPos.X >= Level2Screen.PosFinal.X - 35 &&
                            PlayerRun.PlayerPos.X <= Level2Screen.PosFinal.X + 115 &&
                            PlayerRun.PlayerPos.Y == Level2Screen.PosFinal.Y + 10))
                        {
                            Stat = Stat.Final;
                            _graphics.PreferredBackBufferHeight = 650;
                            _graphics.PreferredBackBufferWidth = 840;
                            _graphics.ApplyChanges();
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            Stat = Stat.StartScreen;
                            _graphics.PreferredBackBufferHeight = 650;
                            _graphics.PreferredBackBufferWidth = 840;
                            _graphics.ApplyChanges();
                        }
                    }
                    break;
                case Stat.Lose:
                    {
                        if ((Keyboard.GetState().IsKeyDown(Keys.Enter)) && LoseScreen.TimeCounter > 15)
                        {
                            Stat = Stat.Level1;
                            _graphics.PreferredBackBufferHeight = 720;
                            _graphics.PreferredBackBufferWidth = 1280;
                            _graphics.ApplyChanges();
                            PlayerLives.CurHeart = 0;
                            PlayerLives.allHearts[0].Enemy = false;
                            PlayerLives.allHearts[1].Enemy = false;
                            PlayerLives.allHearts[2].Enemy = false;
                            PlayerRun.PlayerPos = new Vector2(60, 544);
                            LoseScreen.TimeCounter = 0;
                        }
                    }
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            switch (Stat)
            {
                case Stat.StartScreen:
                    StartScreen.Draw(_spriteBatch);
                    break;
                case Stat.Information:
                    Information.Draw(_spriteBatch);
                    break;
                case Stat.Level1:
                    Level1Screen.Draw(_spriteBatch);
                    PlayerLives.Draw();
                    Portals.Draw();
                    MovedWalls.Draw();
                    StaticWalls.Draw(_spriteBatch);
                    Switches.Draw();
                    Player.Draw();
                    Enemy.Draw();
                    break;
                case Stat.Level2:
                    Level2Screen.Draw(_spriteBatch);
                    PlayerLives.Draw();
                    Portals.Draw();
                    MovedWalls.Draw();
                    StaticWalls.Draw(_spriteBatch);
                    Switches.Draw();
                    Player.Draw();
                    Enemy.Draw();
                    break;
                case Stat.Lose:
                    LoseScreen.Draw(_spriteBatch);
                    break;
                case Stat.Final:
                    FinalScreen.Draw(_spriteBatch);
                    break;
            }            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}