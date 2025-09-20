using OpenTK.Mathematics;

public class Parte
{
    public string Nombre { get; set; }
    public List<Cara> Caras { get; set; }
    public Vector3 PosicionRelativaAlCentroMasa { get; set; }
    public Vector3 Rotacion { get; set; }
    public Vector3 Escala { get; set; }
    public Vector3 Reflexion { get; set; } // 1 o -1 para cada eje

    public Parte(string nombre, Vector3 posicionRelativaAlCentroMasa)
    {
        Nombre = nombre;
        Caras = new List<Cara>();
        PosicionRelativaAlCentroMasa = posicionRelativaAlCentroMasa;
        Rotacion = Vector3.Zero;
        Escala = Vector3.One;
        Reflexion = Vector3.One; // Sin reflexión por defecto
    }

    public void AgregarCara(List<Vertice> vertices, Color4 color)
    {
        Caras.Add(new Cara(vertices, color));
    }

    // Métodos de transformación
    public void Trasladar(Vector3 desplazamiento)
    {
        PosicionRelativaAlCentroMasa += desplazamiento;
    }

    public void Mover(Vector3 nuevaPosicionRelativa)
    {
        PosicionRelativaAlCentroMasa = nuevaPosicionRelativa;
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

    // Aplicar transformaciones a caras específicas
    public void TrasladarCara(int indiceCara, Vector3 desplazamiento)
    {
        if (indiceCara >= 0 && indiceCara < Caras.Count)
        {
            Caras[indiceCara].Trasladar(desplazamiento);
        }
    }

    public void EscalarCara(int indiceCara, Vector3 factor)
    {
        if (indiceCara >= 0 && indiceCara < Caras.Count)
        {
            Caras[indiceCara].Escalar(factor);
        }
    }

    public void RotarCara(int indiceCara, Vector3 rotacion)
    {
        if (indiceCara >= 0 && indiceCara < Caras.Count)
        {
            Caras[indiceCara].Rotar(rotacion);
        }
    }

    public void ReflejarCara(int indiceCara, bool reflejarX, bool reflejarY, bool reflejarZ)
    {
        if (indiceCara >= 0 && indiceCara < Caras.Count)
        {
            Caras[indiceCara].Reflejar(reflejarX, reflejarY, reflejarZ);
        }
    }

    // Obtener matriz de transformación de la parte
    public Matrix4 ObtenerMatrizTransformacion()
    {
        var escala = Matrix4.CreateScale(Escala.X * Reflexion.X, Escala.Y * Reflexion.Y, Escala.Z * Reflexion.Z);
        var rotacionX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X));
        var rotacionY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y));
        var rotacionZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z));
        var traslacion = Matrix4.CreateTranslation(PosicionRelativaAlCentroMasa);

        return escala * rotacionX * rotacionY * rotacionZ * traslacion;
    }

    // Método para dibujar SIN operaciones GL (solo pasa matrices)
    public void Dibujar(Matrix4 matrizPadre)
    {
        var matrizParte = ObtenerMatrizTransformacion() * matrizPadre;
        
        foreach (var cara in Caras)
        {
            cara.Dibujar(matrizParte);
        }
    }
}