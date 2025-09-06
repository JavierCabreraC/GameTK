using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;


public class Cara
{
    public List<Vertice> Vertices { get; set; }
    public Color4 Color { get; set; }
    public PrimitiveType Tipo { get; set; }

    public Cara(List<Vertice> vertices, Color4 color)
    {
        Vertices = vertices;
        Color = color;

        // Determinar automáticamente el tipo de primitiva basado en la cantidad de vértices
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
            _ => PrimitiveType.Polygon // Para 5+ vértices
        };
    }

    public void Dibujar()
    {
        GL.Begin(Tipo);
        GL.Color4(Color);
        foreach (var vertice in Vertices)
        {
            GL.Vertex3(vertice.Posicion);
        }
        GL.End();
    }
}