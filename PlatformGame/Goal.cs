﻿
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using BrownieEngine;

namespace BrownieEngine
{
    class Goal : BrownieSprite
    {
        public new void LoadContent(ContentManager theContentManager, string asset)
        {
            changeColour(Color.White);
            base.LoadContent(theContentManager, filename);
        }
        

        
    }
}
