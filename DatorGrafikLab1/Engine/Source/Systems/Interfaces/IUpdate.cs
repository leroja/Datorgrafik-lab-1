using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine.Source.Systems.Interfaces
{
    interface IUpdate : ISystem
    {
        void update(GameTime gameTime);
    }
}
