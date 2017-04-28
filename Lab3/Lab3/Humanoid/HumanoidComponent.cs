using Engine.Source.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Humanoid
{
    public class HumanoidComponent : IComponent
    {
        public IGameObject Humanoid { get; set; }
        public BasicEffect Effect { get; set; }

        public HumanoidComponent(IGameObject Humanoid, BasicEffect effect)
        {
            this.Humanoid = Humanoid;
            this.Effect = effect;
        }
    }
}
