using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.IO;

using BrownieEngine;

namespace PlatformGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LevelManager lman;
        MainMenu mainmenu;

        SpriteFont font;

        Texture2D bg;
        Texture2D bg2;
        Texture2D endingpic;

        enum gameStates { intro, menu, game, ending }
        gameStates gameStat = gameStates.intro;
        float introTimer = 0;
        float bestTime;
        float introPos = 0;

        SoundEffect gameMus;
        List<SoundEffect> sfx = new List<SoundEffect>();
        

        Color colur = new Color();

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            colur.A = 0;
            BrownieEngnies.initializeContent(this.Content);
            mainmenu = new MainMenu();
            lman = BrownieEngnies.lman;
        }

        protected override void LoadContent()
        {
            addSong("catwalk theme");
            playSong();

            font = Content.Load<SpriteFont>("font");
            bg = Content.Load<Texture2D>("BG city");
            bg2 = Content.Load<Texture2D>("logo");
            endingpic = Content.Load<Texture2D>("ending screeen");

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        void addSong(string val)
        {
            if (sfx.Count > 0) { sfx[0].Dispose(); }
            sfx.Clear();
            gameMus = Content.Load<SoundEffect>(val);
            sfx.Add(gameMus);
        }

        public void playSong()
        {
            var music = sfx[0].CreateInstance();
            music.IsLooped = true;

            music.Play();
        }

        public void LoadTimes()
        {
            SaveLoad load = new SaveLoad();

            SaveData btm = load.loadDat();
            bestTime = btm.time;
        }
        
        protected override void Update(GameTime gameTime)
        {
            
            BrownieEngnies.kbdStat = Keyboard.GetState();

            if (BrownieEngnies.kbdStat.IsKeyDown(Keys.Escape))
                Exit();

            if (BrownieEngnies.playerChar != null) {
                BrownieEngnies.playerChar.Update(gameTime);
            }

            switch (gameStat)
            {
                case gameStates.menu:
                    if (!mainmenu.inGame)
                    {
                        mainmenu.Update(gameTime);
                    }
                    else
                    {
                        BrownieEngnies.lman.levelStart();
                        gameStat = gameStates.game;
                    }

                    break;

                case gameStates.game:

                    BrownieEngnies.lman.Update(gameTime);
                    

                    if (BrownieEngnies.charactersOnScreen.Count > 0)
                    {
                        for (int i = 0; BrownieEngnies.charactersOnScreen.Count > i; i++)
                        {
                            if (BrownieEngnies.charactersOnScreen[i].isTouchingPlayer())
                            {
                                BrownieEngnies.lman.restartLevel();
                            }
                        }
                    }

                    break;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.DarkViolet);

            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            spriteBatch.Draw(bg, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            
            switch (gameStat)
            {
                case gameStates.intro:

                    spriteBatch.DrawString(font, "PrownieBrownieSoft presents...", new Vector2(introPos, 160), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                    
                        if (introTimer < 1.7f)
                        {
                            introPos += 5;
                            introTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            gameStat = gameStates.menu;
                        }
                    
                    break;

                case gameStates.menu:

                    spriteBatch.DrawString(font, "Press 'Z' to play", new Vector2(530, 320), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.Draw(bg2, new Vector2(490,150), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                    break;

                case gameStates.game:
                    spriteBatch.DrawString(font, "Pro Tip: " + BrownieEngnies.lman.proTips[BrownieEngnies.lman.currentLevel], new Vector2(20, 20), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                    spriteBatch.DrawString(font, "Timer: " + LevelManager.timer.ToString(), new Vector2(20, 40), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                    spriteBatch.DrawString(font, "Level: " + (BrownieEngnies.lman.currentLevel+1), new Vector2(600, 40), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);

                    foreach (BrownieSprite character in BrownieEngnies.charactersOnScreen)
                    {
                        character.Draw(this.spriteBatch);
                    }
                    if (BrownieEngnies.lman.isGameEnding)
                    {
                        addSong("catwalk ending");
                        playSong();
                        gameStat = gameStates.ending;
                    }
                    break;

                case gameStates.ending:
                    spriteBatch.DrawString(font, "The End...?", new Vector2(490, 500), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(font, "Programming, Art, Music:", new Vector2(20, 70), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(font, "PrownieBronieSoft", new Vector2(20, 90), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                    spriteBatch.DrawString(font, "Time taken: " + LevelManager.timer.ToString(), new Vector2(460, 550), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.Draw(endingpic, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                    break;
            }
            
            
            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
