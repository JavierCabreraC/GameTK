using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class Cara
{
    public List<Vertice> Vertices { get; set; }
    public Color4 Color { get; set; }
    public PrimitiveType Tipo { get; set; }
    public Vector3 Posicion { get; set; }
    public Vector3 Rotacion { get; set; }
    public Vector3 Escala { get; set; }
    public Vector3 Reflexion { get; set; }

    public Cara(List<Vertice> vertices, Color4 color)
    {
        Vertices = vertices;
        Color = color;
        Posicion = Vector3.Zero;
        Rotacion = Vector3.Zero;
        Escala = Vector3.One;
        Reflexion = Vector3.One;
        
        Tipo = DeterminarTipoPrimitiva(vertices.Count);
    }

    private PrimitiveType DeterminarTipoPrimitiva(int cantidadVertices)
    {
        return cantidadVertices switch
        {
            1 => PrimitiveType.Points,
            2 => PrimitiveType.Lines,
            3 => PrimitiveType.Triangles,
            4 => PrimitiveType.Quads,
            _ => PrimitiveType.Polygon
        };
    }

    public void Trasladar(Vector3 desplazamiento)
    {
        Posicion += desplazamiento;
    }

    public void Escalar(Vector3 factor)
    {
        Escala = new Vector3(Escala.X * factor.X, Escala.Y * factor.Y, Escala.Z * factor.Z);
    }

    public void Rotar(Vector3 rotacion)
    {
        Rotacion += rotacion;
    }

    public void Reflejar(bool reflejarX, bool reflejarY, bool reflejarZ)
    {
        Reflexion = new Vector3(
            reflejarX ? -Reflexion.X : Reflexion.X,
            reflejarY ? -Reflexion.Y : Reflexion.Y,
            reflejarZ ? -Reflexion.Z : Reflexion.Z
        );
    }

    // Obtener matriz de transformación de la cara
    public Matrix4 ObtenerMatrizTransformacion()
    {
        var escala = Matrix4.CreateScale(Escala.X * Reflexion.X, Escala.Y * Reflexion.Y, Escala.Z * Reflexion.Z);
        var rotacionX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X));
        var rotacionY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y));
        var rotacionZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z));
        var traslacion = Matrix4.CreateTranslation(Posicion);

        return escala * rotacionX * rotacionY * rotacionZ * traslacion;
    }

    // AQUÍ ESTÁN TODAS LAS OPERACIONES GL:
    public void Dibujar(Matrix4 matrizPadre)
    {
        // Combinar matriz de la cara con la matriz padre
        var matrizFinal = ObtenerMatrizTransformacion() * matrizPadre;
        
        // Aplicar la matriz de transformación completa
        GL.PushMatrix();
        GL.MultMatrix(ref matrizFinal);
        
        // Dibujar la cara
        GL.Begin(Tipo);
        GL.Color4(Color);
        
        foreach (var vertice in Vertices)
        {
            GL.Vertex3(vertice.Posicion);
        }
        
        GL.End();
        GL.PopMatrix();
    }

    // Método alternativo para dibujar con transformación de vértices
    public void DibujarConVerticesTransformados(Matrix4 matrizPadre)
    {
        var matrizFinal = ObtenerMatrizTransformacion() * matrizPadre;
        
        GL.Begin(Tipo);
        GL.Color4(Color);
        
        foreach (var vertice in Vertices)
        {
            // Transformar cada vértice manualmente
            var verticeTransformado = new Vector4(vertice.Posicion, 1.0f) * matrizFinal;
            GL.Vertex3(verticeTransformado.X, verticeTransformado.Y, verticeTransformado.Z);
        }
        
        GL.End();
    }
}