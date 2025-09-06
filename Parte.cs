using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Generic;

public class Parte
{
    public string Nombre { get; set; }
    public List<Cara> Caras { get; set; }
    public Vector3 PosicionRelativaAlCentroMasa { get; set; } // Cambiado: posición relativa al centro de masa
    public Vector3 Rotacion { get; set; }
    public Vector3 Escala { get; set; }

    public Parte(string nombre, Vector3 posicionRelativaAlCentroMasa)
    {
        Nombre = nombre;
        Caras = new List<Cara>();
        PosicionRelativaAlCentroMasa = posicionRelativaAlCentroMasa;
        Rotacion = Vector3.Zero;
        Escala = Vector3.One;
    }

    public void AgregarCara(List<Vertice> vertices, Color4 color)
    {
        Caras.Add(new Cara(vertices, color));
    }

    public void Dibujar()
    {
        GL.PushMatrix();
        
        // Aplicar transformaciones relativas al centro de masa del objeto
        GL.Translate(PosicionRelativaAlCentroMasa);
        GL.Rotate(Rotacion.X, Vector3.UnitX);
        GL.Rotate(Rotacion.Y, Vector3.UnitY);
        GL.Rotate(Rotacion.Z, Vector3.UnitZ);
        GL.Scale(Escala);
        
        foreach (var cara in Caras)
        {
            cara.Dibujar();
        }
        
        GL.PopMatrix();
    }

    // Mover esta parte relativa al centro de masa
    public void Mover(Vector3 nuevaPosicionRelativa)
    {
        PosicionRelativaAlCentroMasa = nuevaPosicionRelativa;
    }
}