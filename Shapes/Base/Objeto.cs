using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace GameTK.Shapes.Base
{
    public abstract class VectorShape : IDisposable
    {
        protected float[] vertices;
        protected uint[] indices;
        protected Vector3 position;
        protected Vector3 color;
        protected Vector3 scale;
        
        // OpenGL objects
        protected int VAO, VBO, EBO;
        protected bool isInitialized = false;
        
        // Shader program (compartido entre todas las formas)
        protected static int shaderProgram = -1;
        protected static bool shaderCreated = false;

        public VectorShape(Vector3 position, Vector3 color, Vector3 scale)
        {
            this.position = position;
            this.color = color;
            this.scale = scale;
            
            // Crear shaders si no existen
            if (!shaderCreated)
            {
                CreateShaderProgram();
                shaderCreated = true;
            }
        }

        // Constructor con escala por defecto
        public VectorShape(Vector3 position, Vector3 color) 
            : this(position, color, Vector3.One) { }

        // Método abstracto que cada forma debe implementar
        public abstract void GenerateVertices();

        // Inicializar los buffers de OpenGL
        public virtual void Initialize()
        {
            if (isInitialized) return;

            GenerateVertices();
            SetupBuffers();
            isInitialized = true;
        }

        protected virtual void SetupBuffers()
        {
            // Generar VAO
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // Generar VBO
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Si hay índices, generar EBO
            if (indices != null && indices.Length > 0)
            {
                EBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            }

            // Configurar atributos de vértices (posición: location 0)
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Desvincular
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public virtual void Render()
        {
            if (!isInitialized) Initialize();

            GL.UseProgram(shaderProgram);

            // Pasar uniforms al shader
            SetUniforms();

            // Renderizar
            GL.BindVertexArray(VAO);

            if (indices != null && indices.Length > 0)
            {
                // Usar índices para dibujar
                GL.DrawElements(PrimitiveType.LineLoop, indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                // Dibujar directamente los vértices
                GL.DrawArrays(PrimitiveType.LineLoop, 0, vertices.Length / 2);
            }

            GL.BindVertexArray(0);
        }

        protected virtual void SetUniforms()
        {
            // Pasar posición
            int posLocation = GL.GetUniformLocation(shaderProgram, "uPosition");
            GL.Uniform2(posLocation, position.X, position.Y);

            // Pasar color
            int colorLocation = GL.GetUniformLocation(shaderProgram, "uColor");
            GL.Uniform3(colorLocation, color.X, color.Y, color.Z);

            // Pasar escala
            int scaleLocation = GL.GetUniformLocation(shaderProgram, "uScale");
            GL.Uniform2(scaleLocation, scale.X, scale.Y);
        }

        // Métodos para modificar propiedades
        public virtual void SetPosition(Vector3 newPosition)
        {
            position = newPosition;
        }

        public virtual void SetPosition(float x, float y)
        {
            position = new Vector3(x, y, position.Z);
        }

        public virtual void SetColor(Vector3 newColor)
        {
            color = newColor;
        }

        public virtual void SetColor(float r, float g, float b)
        {
            color = new Vector3(r, g, b);
        }

        public virtual void SetScale(Vector3 newScale)
        {
            scale = newScale;
        }

        public virtual void SetScale(float uniformScale)
        {
            scale = new Vector3(uniformScale, uniformScale, 1.0f);
        }

        // Crear programa de shaders compartido
        private static void CreateShaderProgram()
        {
            string vertexShaderSource = 
                "#version 330 core\n" +
                "layout (location = 0) in vec2 aPosition;\n" +
                "uniform vec2 uPosition;\n" +
                "uniform vec2 uScale;\n" +
                "void main()\n" +
                "{\n" +
                "    vec2 scaledPos = aPosition * uScale;\n" +
                "    vec2 finalPos = scaledPos + uPosition;\n" +
                "    gl_Position = vec4(finalPos, 0.0, 1.0);\n" +
                "}";

            string fragmentShaderSource = 
                "#version 330 core\n" +
                "out vec4 FragColor;\n" +
                "uniform vec3 uColor;\n" +
                "void main()\n" +
                "{\n" +
                "    FragColor = vec4(uColor, 1.0);\n" +
                "}";

            // Compilar shaders
            int vertexShader = CompileShader(ShaderType.VertexShader, vertexShaderSource);
            int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);

            // Crear programa
            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);

            // Verificar enlazado
            GL.GetProgram(shaderProgram, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(shaderProgram);
                throw new Exception($"Error enlazando programa de shader: {infoLog}");
            }

            // Limpiar
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        private static int CompileShader(ShaderType type, string source)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error compilando {type}: {infoLog}");
            }

            return shader;
        }

        // Implementación de IDisposable
        public virtual void Dispose()
        {
            if (isInitialized)
            {
                GL.DeleteBuffer(VBO);
                GL.DeleteVertexArray(VAO);
                if (EBO != 0) GL.DeleteBuffer(EBO);
                isInitialized = false;
            }
        }

        // Propiedades públicas para acceso
        public Vector3 Position => position;
        public Vector3 Color => color;
        public Vector3 Scale => scale;
    }
}
