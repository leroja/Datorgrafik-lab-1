using Engine.Source.Components;
using Engine.Source.Managers;
using Engine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DatorGrafikLab1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model plane;
        GraphicsDevice device;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Entitet för planet
            int entityID = ComponentManager.Instance.CreateID();
            List<IComponent> componentList = new List<IComponent>();
            //Skapa och lägg till alla komponenter som vi behöver för modellen
            componentList.Add(new ModelComponent(Content.Load<Model>("Chopper")));
            componentList.Add(new TransformComponent(new Vector3(0, 0, -50), new Vector3(5, 5, 5)));
            componentList.Add(new CameraComponent(graphics.GraphicsDevice));
            ComponentManager.Instance.AddAllComponents(entityID, componentList);

            SystemManager.Instance.AddSystem(new ModelSystem());
            
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            plane = Content.Load<Model>("Chopper");
            device = graphics.GraphicsDevice;

            // TODO: use this.Content to load your game content here
        }
        

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SystemManager.Instance.RunRenderSystems();
           

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
