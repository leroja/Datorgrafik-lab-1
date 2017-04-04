using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine.Source.Systems
{
    class TransformSystem : IUpdate
    {

        /// <summary>
        /// the system for this component should handle the objects world matrix calculation
        /// i.e objectWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
        /// </summary>
        public void update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
