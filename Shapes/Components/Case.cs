using OpenTK.Mathematics;
using GameTK.Shapes.Base;

namespace GameTK.Shapes.Components
{
    public class Case : Objeto
    {
        private float width;
        private float height;

        public Case(Vector3 position, Vector3 color, float width = 0.25f, float height = 0.7f)
            : base(position, color)
        {
            this.width = width;
            this.height = height;
        }

        public override void GenerateVertices()
        {
            // Grosor de marco
            float bezel = 0.02f;

            vertices = new float[]
            {
                // Torre exterior (0-3)
                -width/2,  height/2,
                 width/2,  height/2,
                 width/2, -height/2,
                -width/2, -height/2,

                // Bandeja 1 (4-7)
                -width/2 + bezel,  height/2 - 0.15f,
                 width/2 - bezel,  height/2 - 0.15f,
                 width/2 - bezel,  height/2 - 0.25f,
                -width/2 + bezel,  height/2 - 0.25f,

                // Bandeja 2 (8-11)
                -width/2 + bezel,  height/2 - 0.30f,
                 width/2 - bezel,  height/2 - 0.30f,
                 width/2 - bezel,  height/2 - 0.40f,
                -width/2 + bezel,  height/2 - 0.40f,

                // Botón de encendido (círculo simulado con 4 lados, 12-15)
                -0.03f, -height/2 + 0.1f,
                 0.03f, -height/2 + 0.1f,
                 0.03f, -height/2 + 0.16f,
                -0.03f, -height/2 + 0.16f
            };

            indices = new uint[]
            {
                // Torre externa
                0,1, 1,2, 2,3, 3,0,
                // Bandeja 1
                4,5, 5,6, 6,7, 7,4,
                // Bandeja 2
                8,9, 9,10, 10,11, 11,8,
                // Botón de encendido
                12,13, 13,14, 14,15, 15,12
            };
        }

        // Permite cambiar tamaño dinámicamente
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
