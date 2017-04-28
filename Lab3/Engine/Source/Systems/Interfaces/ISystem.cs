using Engine.Source.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Systems.Interfaces
{
    public abstract class ISystem
    {
        protected ComponentManager ComponentManager { get; } = ComponentManager.Instance;
    }
}
