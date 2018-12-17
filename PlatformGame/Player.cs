using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using BrownieEngine;

namespace BrownieEngine
{
    class Player : BrownieSprite
    {
        public enum animstate { moving, jumping, falling }
        public enum animdirection { left, right }

        public animdirection directionOfAnims;
        public animstate animation;
        const float gravity = 0.98f;

        Texture2D idleright, idleleft;
        Texture2D falling;

        Texture2D[] animations = new Texture2D[10];
        public int currentFrame = 0;

        double frameInterval;
        public double animTimer;

        public new void LoadContent(ContentManager theContentManager, string asset)
        {
            changeColour(Color.White);
            idleright = theContentManager.Load<Texture2D>("player");
            idleleft = theContentManager.Load<Texture2D>("playerleft");
            falling = theContentManager.Load<Texture2D>("playerjump");

            animTimer = 0f;
            frameInterval = 0.09f;

            animations[0] = theContentManager.Load<Texture2D>("player");
            animations[1] = theContentManager.Load<Texture2D>("playerwalk1");
            animations[2] = theContentManager.Load<Texture2D>("playerwalk2");
            animations[3] = theContentManager.Load<Texture2D>("playerwalk3");
            animations[4] = theContentManager.Load<Texture2D>("playerwalk4");

            animations[5] = theContentManager.Load<Texture2D>("playerleft");
            animations[6] = theContentManager.Load<Texture2D>("playerwalkleft1");
            animations[7] = theContentManager.Load<Texture2D>("playerwalkleft2");
            animations[8] = theContentManager.Load<Texture2D>("playerwalkleft3");
            animations[9] = theContentManager.Load<Texture2D>("playerwalkleft4");


            base.LoadContent(theContentManager, filename);
        }

        public void Update(GameTime gamTime)
        {
            updateMovenemt(BrownieEngnies.kbdStat);
            updatePhysics();
            updateAnims(BrownieEngnies.kbdStat, gamTime);
        }

        public void updateAnims(KeyboardState kybd, GameTime gamTime)
        {

            if (kybd.IsKeyDown(Keys.Left) || kybd.IsKeyDown(Keys.Right))
            {

                animTimer += gamTime.ElapsedGameTime.TotalSeconds;
                switch (directionOfAnims)
                {
                    case animdirection.left:

                        if (currentFrame >= 9)
                        {
                            currentFrame = 6;
                        }
                        break;

                    case animdirection.right:

                        if (currentFrame >= 4)
                        {
                            currentFrame = 1;
                        }
                        break;
                }

                if (animTimer >= frameInterval)
                {
                    currentFrame++;
                    animTimer = 0;
                }
               

            }
            else
            {
                animTimer = 0;
                switch (directionOfAnims)
                {
                    case animdirection.left:

                        currentFrame = 5;

                        break;

                    case animdirection.right:

                        currentFrame = 0;

                        break;
                }
            }
            spriteTexture = animations[currentFrame];
        }

        public void updateMovenemt(KeyboardState kybd) {


            if (kybd.IsKeyDown(Keys.Left))
            {
                directionOfAnims = animdirection.left;
                velocity.X += -3.1f;
            }

            if (kybd.IsKeyDown(Keys.Right))
            {
                directionOfAnims = animdirection.right;
                velocity.X += 3.1f;
            }
            if (kybd.IsKeyDown(Keys.Up) && isGrounded)
            {
                if ((kybd.IsKeyDown(Keys.Right)) || (kybd.IsKeyDown(Keys.Left)))
                {
                    velocity.Y += -13f;
                }
                else {

                    velocity.Y += -11f;
                }
            }
            
        }

        public bool isColliding()
        {
            List<BrownieSprite> sprites = BrownieEngnies.charactersOnScreen;

            #region debugcollsion
            /*
            charrect = sprites[32].rect;
            Rectangle intersection = Rectangle.Intersect(this.rect, charrect);

            if (this.iskinematic)
            {
                if (this.rect.Intersects(charrect))
                {
                    //right collision
                    if (this.rect.Right > charrect.Left &&
                        this.rect.Top + 10 < charrect.Bottom &&
                        this.rect.Bottom - 10 > charrect.Top)
                    {
                        spriteColour = changeColour(Color.LightSalmon);
                        position.X -= intersection.Width + velocity.X;
                        velocity.X = 0;
                    }

                    //left collision
                    if (this.rect.Left < charrect.Right && 
                        this.rect.Top + 10 < charrect.Bottom && 
                        this.rect.Bottom - 10 > charrect.Top)
                    {
                        spriteColour = changeColour(Color.Crimson);
                        position.X += intersection.Width;
                        velocity.X = 0;
                        //&& this.rect.Right + 5 > character.rect.Right
                    }


                    //bottom collision
                    if (this.rect.Bottom > charrect.Top && 
                        this.rect.Top + 10 < charrect.Bottom &&
                        this.rect.Left + 10 < charrect.Right &&
                        this.rect.Right - 10 > charrect.Left)
                    {
                        velocity.Y = 0;
                        position.Y -= intersection.Height;
                        isGrounded = true;
                    }

                    if (this.rect.Top < charrect.Bottom && 
                        this.rect.Left + 10 < charrect.Right &&
                        this.rect.Right - 10 > charrect.Left)
                    {
                        position.Y += intersection.Height;
                        velocity.Y = 0;
                    }
                    return true;
                }
            }*/

            #endregion debugcollision

            #region collision
            foreach (BrownieSprite character in sprites)
            {
                if (character == this || character.issolid == false) { continue; }

                Rectangle intersection = Rectangle.Intersect(this.rect, character.rect);

                if (this.rect.Intersects(character.rect))
                {
                    charrect = character.rect;
                    BrownieSprite currentcol = character;
                    currentcol.changeColour(Color.Red);
                    
                    //bottom collision
                    if (this.rect.Bottom  >= character.rect.Top)
                    {
                        velocity.Y = Math.Sign(velocity.Y);
                        position.Y -= intersection.Height - velocity.Y;

                        return true;
                    }

                    return false;
                }

            }
            #endregion collision

            this.changeColour(Color.White);
            return false;
        }

        public void updatePhysics()
        {
            //velocity.Y *= 0.65f;

            if (velocity.Y < -25)
            {
                velocity.Y = -25;
            }
            if (velocity.Y > 16)
            {
                velocity.Y = 16;
            }

            if (isColliding())
            {
                animation = animstate.moving;
                isGrounded = true;
            }
            else
            {
                velocity.Y += gravity; isGrounded = false;
            }

            velocity.X *= 0.65f;
            position += new Vector2((int)velocity.X, (int)velocity.Y);

            if (this.position.X <= BrownieEngnies.lman.goalpos.X + 20 && this.position.X >= BrownieEngnies.lman.goalpos.X - 20 && this.position.Y <= BrownieEngnies.lman.goalpos.Y + 20 && this.position.Y >= BrownieEngnies.lman.goalpos.Y - 20)
            {
                BrownieEngnies.lman.completeLevel();
                BrownieEngnies.lman.levelStart();
            }
            if (this.position.Y > 900)
            {
                BrownieEngnies.lman.restartLevel();
            }

        }
    }
}
