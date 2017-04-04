﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Systems.Interfaces
{
    public interface IRender : ISystem
    {
        void Draw(GameTime gameTime);
        
    }
}
