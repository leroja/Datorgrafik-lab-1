using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class TransformComponent : IComponent
    {
        //Holds data such as position, rotation and scaling.

        public Vector3 Position { get; set; }
        public Matrix Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Matrix ObjectMatrix { get; set; }
        public TransformComponent(Vector3 position, Vector3 scale)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = Matrix.Identity;
        }




    }
}
