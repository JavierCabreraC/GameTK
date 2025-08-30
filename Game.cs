using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GameTK
{
    public class Game : GameWindow
    {

        private Escenario escenario;

        public Game(int width, int height) : base(width, height, GraphicsMode.Default, "Diseño Computadora - 3D")
        {
            var monitorScreenFront = new Poligono(new Color4(0.1f, 0.1f, 0.1f, 1.0f)); // Dark gray for screen
            monitorScreenFront.addVertice(-0.8f, 0.4f, 0.1f);
            monitorScreenFront.addVertice(0.8f, 0.4f, 0.1f);
            monitorScreenFront.addVertice(0.8f, -0.2f, 0.1f);
            monitorScreenFront.addVertice(-0.8f, -0.2f, 0.1f);
            monitorScreenFront.setCentro(new Punto(0.0f, 0.4f, 0.0f)); // Raised up

            var monitorScreenBack = new Poligono(new Color4(0.2f, 0.2f, 0.2f, 1.0f));
            monitorScreenBack.addVertice(-0.8f, 0.4f, -0.1f);
            monitorScreenBack.addVertice(0.8f, 0.4f, -0.1f);
            monitorScreenBack.addVertice(0.8f, -0.2f, -0.1f);
            monitorScreenBack.addVertice(-0.8f, -0.2f, -0.1f);
            monitorScreenBack.setCentro(new Punto(0.0f, 0.4f, 0.0f));

            var monitorScreenLeft = new Poligono(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            monitorScreenLeft.addVertice(-0.8f, 0.4f, 0.1f);
            monitorScreenLeft.addVertice(-0.8f, 0.4f, -0.1f);
            monitorScreenLeft.addVertice(-0.8f, -0.2f, -0.1f);
            monitorScreenLeft.addVertice(-0.8f, -0.2f, 0.1f);
            monitorScreenLeft.setCentro(new Punto(0.0f, 0.4f, 0.0f));

            var monitorScreenRight = new Poligono(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            monitorScreenRight.addVertice(0.8f, 0.4f, 0.1f);
            monitorScreenRight.addVertice(0.8f, 0.4f, -0.1f);
            monitorScreenRight.addVertice(0.8f, -0.2f, -0.1f);
            monitorScreenRight.addVertice(0.8f, -0.2f, 0.1f);
            monitorScreenRight.setCentro(new Punto(0.0f, 0.4f, 0.0f));

            var monitorScreenTop = new Poligono(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            monitorScreenTop.addVertice(-0.8f, 0.4f, 0.1f);
            monitorScreenTop.addVertice(-0.8f, 0.4f, -0.1f);
            monitorScreenTop.addVertice(0.8f, 0.4f, -0.1f);
            monitorScreenTop.addVertice(0.8f, 0.4f, 0.1f);
            monitorScreenTop.setCentro(new Punto(0.0f, 0.4f, 0.0f));

            var monitorScreenBottom = new Poligono(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            monitorScreenBottom.addVertice(-0.8f, -0.2f, 0.1f);
            monitorScreenBottom.addVertice(-0.8f, -0.2f, -0.1f);
            monitorScreenBottom.addVertice(0.8f, -0.2f, -0.1f);
            monitorScreenBottom.addVertice(0.8f, -0.2f, 0.1f);
            monitorScreenBottom.setCentro(new Punto(0.0f, 0.4f, 0.0f));

            var monitorScreenPart = new Partes();
            monitorScreenPart.add("Monitor Screen Front", monitorScreenFront);
            monitorScreenPart.add("Monitor Screen Back", monitorScreenBack);
            monitorScreenPart.add("Monitor Screen Left", monitorScreenLeft);
            monitorScreenPart.add("Monitor Screen Right", monitorScreenRight);
            monitorScreenPart.add("Monitor Screen Top", monitorScreenTop);
            monitorScreenPart.add("Monitor Screen Bottom", monitorScreenBottom);
            monitorScreenPart.setCentro(new Punto(0.0f, 0.4f, 0.0f));

            // Monitor Stand (narrow, short prism below screen)
            var monitorStandFront = new Poligono(new Color4(0.4f, 0.4f, 0.4f, 1.0f));
            monitorStandFront.addVertice(-0.2f, -0.2f, 0.15f);
            monitorStandFront.addVertice(0.2f, -0.2f, 0.15f);
            monitorStandFront.addVertice(0.2f, -0.5f, 0.15f);
            monitorStandFront.addVertice(-0.2f, -0.5f, 0.15f);
            monitorStandFront.setCentro(new Punto(0.0f, -0.15f, 0.0f)); // Positioned below screen

            var monitorStandBack = new Poligono(new Color4(0.4f, 0.4f, 0.4f, 1.0f));
            monitorStandBack.addVertice(-0.2f, -0.2f, -0.15f);
            monitorStandBack.addVertice(0.2f, -0.2f, -0.15f);
            monitorStandBack.addVertice(0.2f, -0.5f, -0.15f);
            monitorStandBack.addVertice(-0.2f, -0.5f, -0.15f);
            monitorStandBack.setCentro(new Punto(0.0f, -0.15f, 0.0f));

            var monitorStandLeft = new Poligono(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            monitorStandLeft.addVertice(-0.2f, -0.2f, 0.15f);
            monitorStandLeft.addVertice(-0.2f, -0.2f, -0.15f);
            monitorStandLeft.addVertice(-0.2f, -0.5f, -0.15f);
            monitorStandLeft.addVertice(-0.2f, -0.5f, 0.15f);
            monitorStandLeft.setCentro(new Punto(0.0f, -0.15f, 0.0f));

            var monitorStandRight = new Poligono(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            monitorStandRight.addVertice(0.2f, -0.2f, 0.15f);
            monitorStandRight.addVertice(0.2f, -0.2f, -0.15f);
            monitorStandRight.addVertice(0.2f, -0.5f, -0.15f);
            monitorStandRight.addVertice(0.2f, -0.5f, 0.15f);
            monitorStandRight.setCentro(new Punto(0.0f, -0.15f, 0.0f));

            var monitorStandTop = new Poligono(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            monitorStandTop.addVertice(-0.2f, -0.2f, 0.15f);
            monitorStandTop.addVertice(-0.2f, -0.2f, -0.15f);
            monitorStandTop.addVertice(0.2f, -0.2f, -0.15f);
            monitorStandTop.addVertice(0.2f, -0.2f, 0.15f);
            monitorStandTop.setCentro(new Punto(0.0f, -0.15f, 0.0f));

            var monitorStandBottom = new Poligono(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            monitorStandBottom.addVertice(-0.2f, -0.5f, 0.15f);
            monitorStandBottom.addVertice(-0.2f, -0.5f, -0.15f);
            monitorStandBottom.addVertice(0.2f, -0.5f, -0.15f);
            monitorStandBottom.addVertice(0.2f, -0.5f, 0.15f);
            monitorStandBottom.setCentro(new Punto(0.0f, -0.15f, 0.0f));

            var monitorStandPart = new Partes();
            monitorStandPart.add("Monitor Stand Front", monitorStandFront);
            monitorStandPart.add("Monitor Stand Back", monitorStandBack);
            monitorStandPart.add("Monitor Stand Left", monitorStandLeft);
            monitorStandPart.add("Monitor Stand Right", monitorStandRight);
            monitorStandPart.add("Monitor Stand Top", monitorStandTop);
            monitorStandPart.add("Monitor Stand Bottom", monitorStandBottom);
            monitorStandPart.setCentro(new Punto(0.0f, -0.15f, 0.0f));

            // Monitor as a single Objeto with two parts
            var monitor = new Objeto();
            monitor.addParte("Monitor Screen", monitorScreenPart);
            monitor.addParte("Monitor Stand", monitorStandPart);
            monitor.setCentro(new Punto(0.0f, 0.0f, 0.0f)); // Centered in scene

            // Keyboard (wide, flat, shallow prism, positioned in front)
            var keyboardFront = new Poligono(new Color4(0.2f, 0.2f, 0.2f, 1.0f));
            keyboardFront.addVertice(-0.7f, -0.45f, 0.6f); // Moved forward on Z
            keyboardFront.addVertice(0.7f, -0.45f, 0.6f);
            keyboardFront.addVertice(0.7f, -0.55f, 0.6f);
            keyboardFront.addVertice(-0.7f, -0.55f, 0.6f);
            keyboardFront.setCentro(new Punto(0.0f, -0.5f, 0.5f));

            var keyboardBack = new Poligono(new Color4(0.2f, 0.2f, 0.2f, 1.0f));
            keyboardBack.addVertice(-0.7f, -0.45f, 0.3f);
            keyboardBack.addVertice(0.7f, -0.45f, 0.3f);
            keyboardBack.addVertice(0.7f, -0.55f, 0.3f);
            keyboardBack.addVertice(-0.7f, -0.55f, 0.3f);
            keyboardBack.setCentro(new Punto(0.0f, -0.5f, 0.5f));

            var keyboardLeft = new Poligono(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            keyboardLeft.addVertice(-0.7f, -0.45f, 0.6f);
            keyboardLeft.addVertice(-0.7f, -0.45f, 0.3f);
            keyboardLeft.addVertice(-0.7f, -0.55f, 0.3f);
            keyboardLeft.addVertice(-0.7f, -0.55f, 0.6f);
            keyboardLeft.setCentro(new Punto(0.0f, -0.5f, 0.5f));

            var keyboardRight = new Poligono(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            keyboardRight.addVertice(0.7f, -0.45f, 0.6f);
            keyboardRight.addVertice(0.7f, -0.45f, 0.3f);
            keyboardRight.addVertice(0.7f, -0.55f, 0.3f);
            keyboardRight.addVertice(0.7f, -0.55f, 0.6f);
            keyboardRight.setCentro(new Punto(0.0f, -0.5f, 0.5f));

            var keyboardTop = new Poligono(new Color4(0.1f, 0.1f, 0.1f, 1.0f)); // Dark top for keys
            keyboardTop.addVertice(-0.7f, -0.45f, 0.6f);
            keyboardTop.addVertice(-0.7f, -0.45f, 0.3f);
            keyboardTop.addVertice(0.7f, -0.45f, 0.3f);
            keyboardTop.addVertice(0.7f, -0.45f, 0.6f);
            keyboardTop.setCentro(new Punto(0.0f, -0.5f, 0.5f));

            var keyboardBottom = new Poligono(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            keyboardBottom.addVertice(-0.7f, -0.55f, 0.6f);
            keyboardBottom.addVertice(-0.7f, -0.55f, 0.3f);
            keyboardBottom.addVertice(0.7f, -0.55f, 0.3f);
            keyboardBottom.addVertice(0.7f, -0.55f, 0.6f);
            keyboardBottom.setCentro(new Punto(0.0f, -0.5f, 0.5f));

            var keyboardPart = new Partes();
            keyboardPart.add("Keyboard Front", keyboardFront);
            keyboardPart.add("Keyboard Back", keyboardBack);
            keyboardPart.add("Keyboard Left", keyboardLeft);
            keyboardPart.add("Keyboard Right", keyboardRight);
            keyboardPart.add("Keyboard Top", keyboardTop);
            keyboardPart.add("Keyboard Bottom", keyboardBottom);
            keyboardPart.setCentro(new Punto(0.0f, -0.5f, 0.5f));

            var keyboard = new Objeto();
            keyboard.addParte("Keyboard", keyboardPart);
            keyboard.setCentro(new Punto(0.0f, -0.5f, 0.5f)); // In front of monitor

            // CPU (tall tower prism, positioned to the side)
            var cpuFront = new Poligono(new Color4(0.4f, 0.4f, 0.4f, 1.0f));
            cpuFront.addVertice(1.0f, 0.5f, 0.3f); // Shifted right on X
            cpuFront.addVertice(1.5f, 0.5f, 0.3f);
            cpuFront.addVertice(1.5f, -0.6f, 0.3f);
            cpuFront.addVertice(1.0f, -0.6f, 0.3f);
            cpuFront.setCentro(new Punto(1.25f, 0.0f, 0.0f));

            var cpuBack = new Poligono(new Color4(0.4f, 0.4f, 0.4f, 1.0f));
            cpuBack.addVertice(1.0f, 0.5f, -0.3f);
            cpuBack.addVertice(1.5f, 0.5f, -0.3f);
            cpuBack.addVertice(1.5f, -0.6f, -0.3f);
            cpuBack.addVertice(1.0f, -0.6f, -0.3f);
            cpuBack.setCentro(new Punto(1.25f, 0.0f, 0.0f));

            var cpuLeft = new Poligono(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            cpuLeft.addVertice(1.0f, 0.5f, 0.3f);
            cpuLeft.addVertice(1.0f, 0.5f, -0.3f);
            cpuLeft.addVertice(1.0f, -0.6f, -0.3f);
            cpuLeft.addVertice(1.0f, -0.6f, 0.3f);
            cpuLeft.setCentro(new Punto(1.25f, 0.0f, 0.0f));

            var cpuRight = new Poligono(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            cpuRight.addVertice(1.5f, 0.5f, 0.3f);
            cpuRight.addVertice(1.5f, 0.5f, -0.3f);
            cpuRight.addVertice(1.5f, -0.6f, -0.3f);
            cpuRight.addVertice(1.5f, -0.6f, 0.3f);
            cpuRight.setCentro(new Punto(1.25f, 0.0f, 0.0f));

            var cpuTop = new Poligono(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            cpuTop.addVertice(1.0f, 0.5f, 0.3f);
            cpuTop.addVertice(1.0f, 0.5f, -0.3f);
            cpuTop.addVertice(1.5f, 0.5f, -0.3f);
            cpuTop.addVertice(1.5f, 0.5f, 0.3f);
            cpuTop.setCentro(new Punto(1.25f, 0.0f, 0.0f));

            var cpuBottom = new Poligono(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            cpuBottom.addVertice(1.0f, -0.6f, 0.3f);
            cpuBottom.addVertice(1.0f, -0.6f, -0.3f);
            cpuBottom.addVertice(1.5f, -0.6f, -0.3f);
            cpuBottom.addVertice(1.5f, -0.6f, 0.3f);
            cpuBottom.setCentro(new Punto(1.25f, 0.0f, 0.0f));

            var cpuPart = new Partes();
            cpuPart.add("CPU Front", cpuFront);
            cpuPart.add("CPU Back", cpuBack);
            cpuPart.add("CPU Left", cpuLeft);
            cpuPart.add("CPU Right", cpuRight);
            cpuPart.add("CPU Top", cpuTop); 
            cpuPart.add("CPU Bottom", cpuBottom);
            cpuPart.setCentro(new Punto(1.25f, 0.0f, 0.0f));

            var cpu = new Objeto();
            cpu.addParte("CPU Tower", cpuPart);
            cpu.setCentro(new Punto(1.25f, 0.0f, 0.0f)); // To the right

            // Escenario:
            this.escenario = new Escenario(new Vector3(0, 0, 0));
            this.escenario.addObjeto("Monitor", monitor);
            this.escenario.addObjeto("Keyboard", keyboard);
            this.escenario.addObjeto("CPU", cpu);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.0f, 0.5f, 0.0f, 1.0f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
            float aspectRatio = (float)Width / Height;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f), aspectRatio, 0.1f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }



        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            // Configura la cámara
            Matrix4 modelview = Matrix4.LookAt(
                new Vector3(0.0f, 2f, 3.5f), // Posición de la cámara
                new Vector3(0.0f, 0.1f, 0.0f), // Punto de mira
                Vector3.UnitY); // Vector arriba
            GL.LoadMatrix(ref modelview);
            
            escenario.dibujar(new Vector3(0, 0, 0));
            GL.End();
            SwapBuffers();
        }
        private void DibujarEjes()
        {
            GL.Begin(PrimitiveType.Lines);

            GL.Color3(1.0f, 0.0f, 0.0f); // X
            GL.Vertex3(-2.0f, 0.0f, 0.0f);
            GL.Vertex3(2.0f, 0.0f, 0.0f);

            GL.Color3(0.0f, 1.0f, 0.0f); // Y
            GL.Vertex3(0.0f, -2.0f, 0.0f);
            GL.Vertex3(0.0f, 2.0f, 0.0f);

            GL.Color3(0.0f, 0.0f, 1.0f); // Z
            GL.Vertex3(0.0f, 0.0f, -2.0f);
            GL.Vertex3(0.0f, 0.0f, 2.0f);

            GL.End();
        }
    }
}
