using Engine.Source.Enums;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class KeyBoardComponent : IComponent
    {
        public Dictionary<ActionsEnum, Keys> KeyBoardActions { get; set; }
        public Dictionary<ActionsEnum, ButtonStates> State { get; set; }

        public KeyBoardComponent()
        {
            KeyBoardActions = new Dictionary<ActionsEnum, Keys>();
            State = new Dictionary<ActionsEnum, ButtonStates>();
        }
    }
}
