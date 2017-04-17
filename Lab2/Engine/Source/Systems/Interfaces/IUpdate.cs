using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine.Source.Systems.Interfaces
{
    public abstract class IUpdate : ISystem
    {
        public abstract void Update(GameTime gameTime);
    }
}
