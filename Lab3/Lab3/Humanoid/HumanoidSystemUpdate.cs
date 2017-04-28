using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lab3.Humanoid
{
    public class HumanoidSystemUpdate : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            var ids = ComponentManager.GetAllEntitiesWithComponentType<HumanoidComponent>();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var humanoidComp = ComponentManager.GetEntityComponent<HumanoidComponent>(id);
                    humanoidComp.Humanoid.Update(gameTime);
                }
            }
        }
    }
}
