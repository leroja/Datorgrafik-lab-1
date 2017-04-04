using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Source.Components;
using Engine.Source.Managers;

namespace Engine.Source.Systems
{
    public class TransformSystem : IUpdate
    {

        /// <summary>
        /// the system for this component should handle the objects world matrix calculation
        /// i.e objectWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
        /// </summary>
        public void update(GameTime gameTime)
        {
            //Hämta ut Modelcomponenten
            Dictionary<int, IComponent> mc = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<TransformComponent>();
            foreach (var entity in mc)
            {
                TransformComponent tfc = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity.Key);
                tfc.objectMatrix = Matrix.CreateScale(tfc.scale) * tfc.rotation * Matrix.CreateTranslation(tfc.position);
            }
        }
    }
}
