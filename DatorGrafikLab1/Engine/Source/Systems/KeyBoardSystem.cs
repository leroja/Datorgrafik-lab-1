using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.Source.Managers;
using Engine.Source.Systems.Interfaces;
using Engine.Source.Components;
using Engine.Source.Enums;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// a system that updates the state of the keyboard and the updates the states of the keyboard components
    /// </summary>
    public class KeyBoardSystem : IUpdate
    {
        public KeyboardState PrevState { get; set; }
        public KeyboardState CurState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void update(GameTime gameTime)
        {
            updateStates();

            List<int> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<KeyBoardComponent>();

            if (entities != null)
            {
                foreach (var item in entities)
                {
                    KeyBoardComponent kbc = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(item);
                    UpdateActionStates(kbc);
                }
            }
        }

        /// <summary>
        /// updates the previous & current State of the keyboard
        /// </summary>
        private void updateStates()
        {
            PrevState = CurState;
            CurState = Keyboard.GetState();
        }

        /// <summary>
        /// updates the states of all the Keyboard actions in a keyboard Component
        /// </summary>
        /// <param name="keyboardComp"></param>
        public void UpdateActionStates(KeyBoardComponent keyboardComp)
        {

            foreach (ActionsEnum action in keyboardComp.keyBoardActions.Keys)
            {
                Keys key = keyboardComp.keyBoardActions[action];
                bool newState = CurState.IsKeyDown(key);
                bool oldState = PrevState.IsKeyDown(key);

                if (newState && !oldState)
                {
                    keyboardComp.state[action] = ButtonStates.Pressed;
                }
                else if (newState && oldState)
                {
                    keyboardComp.state[action] = ButtonStates.Hold;
                }
                else if (!newState && oldState)
                {
                    keyboardComp.state[action] = ButtonStates.Released;
                }
                else
                {
                    keyboardComp.state[action] = ButtonStates.Not_Pressed;
                }
            }
        }
    }
}