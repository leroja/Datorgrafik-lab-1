using Microsoft.Xna.Framework;
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
        //Holds a model and the data transforms for its meshes.
        public Model Model { get; set; }
        
        public Matrix[] MeshWorldMatrices { get; set; }
        
        public ModelComponent(Model model)
        {
            this.Model = model;
        }
    }
}
