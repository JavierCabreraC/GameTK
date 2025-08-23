using GameTK.Shapes.Base;
using GameTK.Shapes.Components;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Monitor = GameTK.Shapes.Components.Monitor;
using Case = GameTK.Shapes.Components.Case;

namespace GameTK
{
    internal class Game : GameWindow
    {
        private List<Objeto> shapes;
        private double _time = 0.0;

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            // this.CenterWindow(new Vector2i(1280, 760));
            this.Title = "Dibujo Vectorial - Computadora";
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.DepthTest);
            GL.LineWidth(2.0f);

            InitializeShapes();
        }

        private void InitializeShapes()
        {
            shapes = new List<Objeto>();

            // Monitor en el centro-izquierda
            var monitor = new Monitor(
                position: new Vector3(-0.3f, 0.2f, 0.0f),
                color: new Vector3(0.0f, 1.0f, 0.0f),
                width: 0.6f,
                height: 0.4f,
                includeStand: true,
                baseHeight: 0.05f,
                baseWidth: 0.25f
            );

            // Case a la derecha
            var caseTower = new Case(
                position: new Vector3(0.6f, 0.0f, 0.0f),
                color: new Vector3(0.0f, 0.5f, 1.0f),
                width: 0.25f,
                height: 0.7f
            );

            // Teclado debajo del monitor
            var keyboard = new Teclado(
                position: new Vector3(-0.3f, -0.3f, 0.0f), 
                color: new Vector3(1.0f, 1.0f, 1.0f),
                width: 0.8f,
                height: 0.2f
            );

            shapes.Add(monitor);
            shapes.Add(caseTower);
            shapes.Add(keyboard);

            foreach (var shape in shapes)
                shape.Initialize();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            _time += args.Time;

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var shape in shapes)
                shape.Render();

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            foreach (var shape in shapes)
                shape.Dispose();

            base.OnUnload();
        }
        
        
    }
}
