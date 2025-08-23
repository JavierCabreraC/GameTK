using OpenTK.Mathematics;
using GameTK.Shapes.Base;

namespace GameTK.Shapes.Components
{
    public class Monitor : Objeto
    {
        private float width;
        private float height;
        private float baseHeight;
        private float baseWidth;
        private bool includeStand;

        // Constructor principal con parámetros personalizables
        public Monitor(Vector3 position, Vector3 color, float width = 0.6f, float height = 0.4f, 
                      bool includeStand = true, float baseHeight = 0.05f, float baseWidth = 0.2f) 
            : base(position, color)
        {
            this.width = width;
            this.height = height;
            this.includeStand = includeStand;
            this.baseHeight = baseHeight;
            this.baseWidth = baseWidth;
        }

        // Constructor simplificado
        public Monitor(Vector3 position, Vector3 color) 
            : this(position, color, 0.6f, 0.4f, true, 0.05f, 0.2f) { }

        // Constructor con coordenadas específicas (para máximo control)
        public Monitor(Vector3 position, Vector3 color, float[] customVertices) 
            : base(position, color)
        {
            // Si se proporcionan vértices custom, usarlos directamente
            if (customVertices != null && customVertices.Length > 0)
            {
                vertices = customVertices;
                return;
            }
            
            // Si no, usar valores por defecto
            width = 0.6f;
            height = 0.4f;
            includeStand = true;
            baseHeight = 0.05f;
            baseWidth = 0.2f;
        }

        public override void GenerateVertices()
        {
            // Si ya se asignaron vértices personalizados, no regenerar
            if (vertices != null) return;

            if (includeStand)
            {
                GenerateMonitorWithStand();
            }
            else
            {
                GenerateScreenOnly();
            }
        }

        private void GenerateScreenOnly()
        {
            // Vértices para un monitor rectangular simple
            // Coordenadas relativas que serán transformadas por posición y escala
            vertices = new float[]
            {
                // Pantalla principal (rectángulo)
                -width/2,  height/2,   // Esquina superior izquierda
                 width/2,  height/2,   // Esquina superior derecha  
                 width/2, -height/2,   // Esquina inferior derecha
                -width/2, -height/2,   // Esquina inferior izquierda
            };

            // Índices para dibujar el contorno
            indices = new uint[] { 0, 1, 2, 3 };
        }

        private void GenerateMonitorWithStand()
        {
            float bezel = 0.02f; // grosor del marco

            vertices = new float[]
            {
                // Pantalla externa (0-3)
                -width/2,  height/2,
                width/2,  height/2,
                width/2, -height/2,
                -width/2, -height/2,

                // Pantalla interna (4-7)
                -width/2 + bezel,  height/2 - bezel,
                width/2 - bezel,  height/2 - bezel,
                width/2 - bezel, -height/2 + bezel,
                -width/2 + bezel, -height/2 + bezel,

                // Poste (8-11)
                -0.02f, -height/2,
                0.02f, -height/2,
                0.02f, -height/2 - 0.15f,
                -0.02f, -height/2 - 0.15f,

                // Base (12-15)
                -baseWidth/2, -height/2 - 0.15f,
                baseWidth/2, -height/2 - 0.15f,
                baseWidth/2, -height/2 - 0.15f - baseHeight,
                -baseWidth/2, -height/2 - 0.15f - baseHeight
            };

            indices = new uint[]
            {
                // Marco externo
                0,1, 1,2, 2,3, 3,0,
                // Marco interno
                4,5, 5,6, 6,7, 7,4,
                // Poste
                8,9, 9,10, 10,11, 11,8,
                // Base
                12,13, 13,14, 14,15, 15,12,
                // Conexiones
                2,8,
                3,11,
                10,13,
                11,12
            };
        }
        
        // Método para cambiar el tamaño del monitor dinámicamente
        public void Resize(float newWidth, float newHeight)
        {
            width = newWidth;
            height = newHeight;
            
            // Regenerar vértices con nuevas dimensiones
            vertices = null; // Forzar regeneración
            
            if (isInitialized)
            {
                // Actualizar buffers si ya están inicializados
                Dispose();
                isInitialized = false;
                Initialize();
            }
        }

        // Método para alternar el soporte
        public void ToggleStand()
        {
            includeStand = !includeStand;
            vertices = null; // Forzar regeneración
            
            if (isInitialized)
            {
                Dispose();
                isInitialized = false;
                Initialize();
            }
        }

        // Método estático para crear configuraciones predefinidas
        public static Monitor CreateLaptopScreen(Vector3 position, Vector3 color)
        {
            return new Monitor(position, color, 0.8f, 0.5f, false);
        }

        public static Monitor CreateDesktopMonitor(Vector3 position, Vector3 color)
        {
            return new Monitor(position, color, 0.6f, 0.4f, true, 0.05f, 0.25f);
        }

        public static Monitor CreateUltrawide(Vector3 position, Vector3 color)
        {
            return new Monitor(position, color, 1.0f, 0.3f, true, 0.06f, 0.3f);
        }

        // Propiedades para acceso a las dimensiones
        public float Width => width;
        public float Height => height;
        public bool HasStand => includeStand;
    }
}
