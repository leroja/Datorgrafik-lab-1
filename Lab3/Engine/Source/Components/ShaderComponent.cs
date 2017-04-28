using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class ShaderComponent : IComponent
    {
        public Effect ShaderEffect { private set; get; }

        public ShaderComponent(Effect effect)
        {
            ShaderEffect = effect;
        }
    }
}
