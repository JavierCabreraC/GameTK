using OpenTK.Mathematics;
using GameTK.Shapes.Base;

namespace GameTK.Shapes.Components
{
    public class Keyboard : Objeto
    {
        private float width;
        private float height;

        public Keyboard(Vector3 position, Vector3 color, float width = 0.8f, float height = 0.2f)
            : base(position, color)
        {
            this.width = width;
            this.height = height;
        }

        public override void GenerateVertices()
        {
            vertices = new float[]
            {
                // Marco exterior (0-3)
                -width/2,  height/2,
                 width/2,  height/2,
                 width/2, -height/2,
                -width/2, -height/2,

                // Fila 1 (4-7)
                -width/2 + 0.05f,  height/2 - 0.05f,
                 width/2 - 0.05f,  height/2 - 0.05f,
                 width/2 - 0.05f,  height/2 - 0.08f,
                -width/2 + 0.05f,  height/2 - 0.08f,

                // Fila 2 (8-11)
                -width/2 + 0.05f,  height/2 - 0.12f,
                 width/2 - 0.05f,  height/2 - 0.12f,
                 width/2 - 0.05f,  height/2 - 0.15f,
                -width/2 + 0.05f,  height/2 - 0.15f,

                // Barra espaciadora (12-15)
                -0.25f, -0.02f,
                 0.25f, -0.02f,
                 0.25f, -0.06f,
                -0.25f, -0.06f,
            };

            indices = new uint[]
            {
                // Marco externo
                0,1, 1,2, 2,3, 3,0,
                // Fila 1
                4,5, 5,6, 6,7, 7,4,
                // Fila 2
                8,9, 9,10, 10,11, 11,8,
                // Barra espaciadora
                12,13, 13,14, 14,15, 15,12
            };
        }

        // Permite cambiar tamaño
        public void Resize(float newWidth, float newHeight)
        {
            width = newWidth;
            height = newHeight;

            vertices = null; // forzar regeneración
            if (isInitialized)
            {
                Dispose();
                isInitialized = false;
                Initialize();
            }
        }
    }
}
