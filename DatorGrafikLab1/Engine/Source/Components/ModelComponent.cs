using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class ModelComponent : IComponent
    {
        public Model model { get; set; }
        

        public ModelComponent(Model model)
        {
            this.model = model;

        }
    }
}
