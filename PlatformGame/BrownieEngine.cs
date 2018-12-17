using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BrownieEngine
{

    class BrownieEngnies {

        public static List<BrownieSprite> charactersOnScreen = new List<BrownieSprite>();
        public static LevelManager lman;
        public static Player playerChar = null;
        public static KeyboardState kbdStat;

        public enum objectType { wall, foregroundWall, player, goal, spikes };
        public objectType objType = objectType.wall;
        BrownieSprite gamebj = new BrownieSprite();

        public static void initializeContent(ContentManager content) {
            lman = new LevelManager(content);
        }

        public static void createObject(objectType objTyp, Vector2 pos, ContentManager cont)
        {

            if (objTyp == objectType.wall)
            {
                BrownieSprite obj = new BrownieSprite();

                obj.chooseLayer = BrownieSprite.layer.bgLayer;
                obj.position = pos;
                obj.filename = "tileset";
                obj.ishazard = false;
                obj.issolid = true;
                obj.LoadContent(cont, obj.filename);
                charactersOnScreen.Add(obj);
            }

            if (objTyp == objectType.spikes)
            {
                BrownieSprite obj = new BrownieSprite();

                obj.chooseLayer = BrownieSprite.layer.bgLayer;
                obj.position = pos;
                obj.filename = "spikes";
                obj.ishazard = true;
                obj.issolid = true;
                obj.LoadContent(cont, obj.filename);
                charactersOnScreen.Add(obj);
            }

            if (objTyp == objectType.foregroundWall)
            {
                BrownieSprite obj = new BrownieSprite();

                obj.chooseLayer = BrownieSprite.layer.bgLayer;
                obj.position = pos;
                obj.filename = "tileset2";
                obj.ishazard = false;
                obj.issolid = false;
                obj.LoadContent(cont, obj.filename);
                charactersOnScreen.Add(obj);
            }

            if (objTyp == objectType.player)
            {
                
                Player obj = new Player();

                obj.chooseLayer = BrownieSprite.layer.playerlayer;
                obj.position = pos;
                obj.filename = "placehodler";
                obj.ishazard = false;
                obj.LoadContent(cont, obj.filename);
                playerChar = obj;
                charactersOnScreen.Add(obj);
            }

            if (objTyp == objectType.goal)
            {
                Goal obj = new Goal();

                obj.chooseLayer = BrownieSprite.layer.objectLayer;
                obj.position = pos;
                obj.filename = "flag";
                obj.ishazard = false;
                obj.issolid = false;
                obj.LoadContent(cont, obj.filename);
                charactersOnScreen.Add(obj);
            }

        }
    }

    class LevelManager
    {
        public Color[,] thing;
        public Texture2D[] levels = new Texture2D[20];
        public string[] proTips = new string[20];
        public int currentLevel = -1;
        public static float timer = 0f;
        ContentManager stuff;

        public bool isGameEnding= false;

        public Vector2 goalpos;

        public LevelManager(ContentManager content)
        {
            for (int i = BrownieEngnies.charactersOnScreen.Count - 1; i >= 0; i--)
            {
                BrownieEngnies.charactersOnScreen.RemoveAt(i);
            }
            stuff = content;
            levels[0] = stuff.Load<Texture2D>("level 1");
            proTips[0] = "Welcome to Catwalk! Don't jump on that first building, the inhabitants don't seem to like cats...";
            levels[1] = stuff.Load<Texture2D>("level 2");
            proTips[1] = "Here's something I found out about cats: They jump lower when they stand and the 'Up' arrow is pressed";
            levels[2] = stuff.Load<Texture2D>("level 3");
            proTips[2] = "Beleive me, you will really need the advice of te last level to complete this game.";
            levels[3] = stuff.Load<Texture2D>("level 4");
            proTips[3] = "Thought you could just fall to the lower platforms and complete the level quickly? " +
                "In your dreams looser...";
            levels[4] = stuff.Load<Texture2D>("level 5");
            proTips[4] = "Who designed this level?! Anyways, you can climb up these platforms like you can with stairs, quite cool eh?";
            levels[5] = stuff.Load<Texture2D>("level 6");
            proTips[5] = "This is the equivelent of what game developers go through when making games...";
            levels[6] = stuff.Load<Texture2D>("level 7");
            proTips[6] = "Ooh!You're now going into a flat eh? Well... it dosen't seem like they welcome cats at all. Remember to do those little jumps now, hey?";
           levels[7] = stuff.Load<Texture2D>("level 8");
            proTips[7] = "...";
            levels[8] = stuff.Load<Texture2D>("level 9");
            proTips[8] = "Be careful now... You know that if you don't get off the black poles at the right time, the spikes will...";
            levels[9] = stuff.Load<Texture2D>("level 10");
            proTips[9] = "There is an easier but longer path.";
            levels[10] = stuff.Load<Texture2D>("level 11");
            proTips[10] = "...";
            levels[11] = stuff.Load<Texture2D>("level 12");
            proTips[11] = "Now remember, just ride the black slopes and be cool with it!";
            levels[12] = stuff.Load<Texture2D>("level 13");
            proTips[12] = "Wow... Las Vegas' luxor hotel is quite unsafe... FOR CATS!";
            levels[13] = stuff.Load<Texture2D>("level 14");
            proTips[13] = "I'm pretty sure you know that this game is forcing you to take long paths.";
            levels[14] = stuff.Load<Texture2D>("level 15");
            proTips[14] = "This is where timing and run-jumps come hand-in-hand.";
            levels[15] = stuff.Load<Texture2D>("level 16");
            proTips[15] = "Pick the floor which isn't blocked by spikes.";
            levels[16] = stuff.Load<Texture2D>("level 17");
            proTips[16] = "W-wait... is this a city or a parkour course? Maybe you should just slip down some platforms.";
            levels[17] = stuff.Load<Texture2D>("level 18");
            proTips[17] = "When going down the slide, be sure to hold the right button when it turns... It'll take practice...";
            levels[18] = stuff.Load<Texture2D>("level 19");
            proTips[18] = "Watch your step now my friend. Sometimes legging it is always the best idea.";
            levels[19] = stuff.Load<Texture2D>("level 20");
            proTips[19] = "This is the last level... Remember what you have learned throughout your journey like timing, small jumps etc.";

        }

        public void Update(GameTime gamtime)
        {
            timer += (float)gamtime.ElapsedGameTime.TotalSeconds;
        }

        public void levelSelection(int level) {
            currentLevel = level;
        }

        public void restartLevel()
        {
            for (int i = BrownieEngnies.charactersOnScreen.Count - 1; i >= 0; i--)
            {
                BrownieEngnies.charactersOnScreen.RemoveAt(i);
            }
            loadLevel(levels[currentLevel], stuff);
        }

        public void completeLevel()
        {
            if (BrownieEngnies.lman.currentLevel == 19)
            {
               // SaveLoad sav = new SaveLoad();
               // sav.saveDat(currentLevel, timer);
            }
        }

        public void levelStart()
        {
            for (int i = BrownieEngnies.charactersOnScreen.Count - 1; i >= 0; i--) {
                BrownieEngnies.charactersOnScreen.RemoveAt(i);
            }
            if (currentLevel < 19)
            {
                currentLevel++;
                loadLevel(levels[currentLevel], stuff);
            }
            else
            {
                isGameEnding = true;
            }
        }

        public void loadLevel(Texture2D level, ContentManager cont)
        {
            float tilesize = 20;
            thing = texture2arrray(level);

            for (int x = 0; x < level.Width; x++)
            {
                for (int y = 0; y < level.Height; y++)
                {
                    if (thing[x, y].B == 36 && thing[x, y].R == 237 && thing[x, y].G == 28) {

                        BrownieEngnies.createObject(BrownieEngnies.objectType.spikes, new Vector2(x * tilesize, y * tilesize), cont);
                    }

                    if (thing[x, y].B == 127 && thing[x, y].R == 127 && thing[x, y].G == 127)
                    {
                        BrownieEngnies.createObject(BrownieEngnies.objectType.foregroundWall, new Vector2(x * tilesize, y * tilesize), cont);
                    }

                    if (thing[x, y].B == 29 && thing[x, y].R == 181 && thing[x, y].G == 230)
                    {
                        BrownieEngnies.createObject(BrownieEngnies.objectType.player, new Vector2(x * tilesize, y * tilesize), cont);
                    }

                    if (thing[x, y].B == 0 && thing[x, y].R == 0 && thing[x, y].G == 0)
                    {
                        BrownieEngnies.createObject(BrownieEngnies.objectType.wall, new Vector2(x * tilesize, y * tilesize), cont);
                    }

                    if (thing[x, y].B == 204 && thing[x, y].R == 63 && thing[x, y].G == 72)
                    {
                        BrownieEngnies.createObject(BrownieEngnies.objectType.goal, new Vector2(x * tilesize, y * tilesize), cont);
                        goalpos = new Vector2(x * tilesize, y * tilesize);
                    }
                }
            }

        }

        Color[,] texture2arrray(Texture2D level)
        {
            Color[] colour = new Color[level.Width * level.Height];

            level.GetData(colour);
            Color[,] rawDat = new Color[level.Width, level.Height];
            for (int x = 0; x < level.Width; x++)
                for (int y = 0; y < level.Height; y++)
                    rawDat[x, y] = colour[x + y * level.Width];

            return rawDat;
        }
    }

    class BrownieSprite
    {

        public const float guiLayer = 1f;
        public const float objLayer = 0f;
        public const float bgLayer = 0.4f;

        const float gravity = 0.98f;

        public enum layer { objectLayer, guiLayer, bgLayer, playerlayer }
        public layer chooseLayer;

        public bool issolid;
        public bool ishazard;
        public Vector2 velocity;
        public Color spriteColour;
        public Texture2D spriteTexture;
        public Vector2 position;

        public bool isGrounded;

        public string filename;

        public Rectangle rect = new Rectangle();
        public Rectangle charrect;

        protected SpriteFont debugtxt;

        public void LoadContent(ContentManager theContentManager, string asset)
        {
            this.changeColour(Color.White);
            spriteTexture = theContentManager.Load<Texture2D>(asset);
        }

        public bool isTouchingPlayer()
        {
            if (this.ishazard)
            {
                Player player = BrownieEngnies.playerChar;


                Rectangle intersection = Rectangle.Intersect(this.rect, player.rect);

                if (this.rect.Intersects(player.rect))
                {

                    return true;
                }

                return false;
            }
            return false;

        }


        public void loadBrownieSprite(SpriteBatch sp) {


            switch (chooseLayer)
            {
                case layer.playerlayer:
                    this.rect.X = (int)this.position.X + 12;
                    this.rect.Y = (int)this.position.Y + 7;
                    this.rect.Width = 20;
                    this.rect.Height = 27;
                    sp.Draw(spriteTexture, position, null, spriteColour, 0, Vector2.Zero, 1, SpriteEffects.None,0.99f);
                    break;
                case layer.objectLayer:
                    this.rect.Width = this.spriteTexture.Width;
                    this.rect.Height = this.spriteTexture.Height;
                    this.rect.X = (int)this.position.X;
                    this.rect.Y = (int)this.position.Y;
                    sp.Draw(spriteTexture, position, null, spriteColour, 0, Vector2.Zero, 1, SpriteEffects.None, 0.1f);
                    break;
                case layer.bgLayer:
                    this.rect.Width = this.spriteTexture.Width;
                    this.rect.Height = this.spriteTexture.Height;
                    this.rect.X = (int)this.position.X;
                    this.rect.Y = (int)this.position.Y;
                    sp.Draw(spriteTexture, position, null, spriteColour, 0, Vector2.Zero, 1, SpriteEffects.None, bgLayer);
                    break;

            }
        }
        
        public void setFont(SpriteFont font) {
            debugtxt = font;
        }

        public Color changeColour(Color colour)
        {
            spriteColour = colour;
            return colour;
        }

        public void Draw(SpriteBatch sp)
        {
            loadBrownieSprite(sp);
        }

       
    }
}
