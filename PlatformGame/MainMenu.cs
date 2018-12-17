using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace BrownieEngine
{
    class MainMenu
    {
        public bool inGame = false;
        

        public void Update(GameTime gametim) {
            KeyboardState kybd = BrownieEngnies.kbdStat;

            

            if (kybd.IsKeyDown(Keys.Z))
            {
                inGame = true;
            }

            //BrownieEngnies.lman.levelSelection(currentSelection);           

        }

    }
}
