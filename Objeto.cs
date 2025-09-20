using OpenTK.Mathematics;

public class Objeto
{
    public string Nombre { get; set; }
    public List<Parte> Partes { get; set; }
    public Vector3 CentroMasa { get; set; } 
    public Vector3 Posicion { get; set; }
    public Vector3 Rotacion { get; set; }
    public Vector3 Escala { get; set; }
    public Vector3 Reflexion { get; set; } // 1 o -1 para cada eje

    public Objeto(string nombre)
    {
        Nombre = nombre;
        Partes = new List<Parte>();
        CentroMasa = Vector3.Zero; 
        Posicion = Vector3.Zero;
        Rotacion = Vector3.Zero;
        Escala = Vector3.One;
        Reflexion = Vector3.One; // Sin reflexión por defecto
    }

    public Parte CrearParte(string nombreParte, Vector3 posicionRelativaAlCentroMasa)
    {
        var parte = new Parte(nombreParte, posicionRelativaAlCentroMasa);
        Partes.Add(parte);
        return parte;
    }

    public Parte ObtenerParte(string nombre)
    {
        return Partes.Find(p => p.Nombre == nombre);
    }

    // Calcular el centro de masa automáticamente basado en las partes
    public void CalcularCentroMasa()
    {
        if (Partes.Count == 0)
        {
            CentroMasa = Vector3.Zero;
            return;
        }

        Vector3 sumaPosiciones = Vector3.Zero;
        foreach (var parte in Partes)
        {
            sumaPosiciones += parte.PosicionRelativaAlCentroMasa;
        }
        CentroMasa = sumaPosiciones / Partes.Count;
    }

    // Métodos de transformación
    public void Trasladar(Vector3 desplazamiento)
    {
        Posicion += desplazamiento;
    }

    public void Mover(Vector3 nuevaPosicion)
    {
        Posicion = nuevaPosicion;
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

    // Mover una parte relativa al centro de masa
    public void MoverParte(string nombreParte, Vector3 nuevaPosicionRelativa)
    {
        var parte = ObtenerParte(nombreParte);
        if (parte != null)
        {
            parte.PosicionRelativaAlCentroMasa = nuevaPosicionRelativa;
            CalcularCentroMasa(); // Recalcular centro de masa después de mover una parte
        }
    }

    // Aplicar transformaciones a partes específicas
    public void TrasladarParte(string nombreParte, Vector3 desplazamiento)
    {
        var parte = ObtenerParte(nombreParte);
        parte?.Trasladar(desplazamiento);
    }

    public void EscalarParte(string nombreParte, Vector3 factor)
    {
        var parte = ObtenerParte(nombreParte);
        parte?.Escalar(factor);
    }

    public void RotarParte(string nombreParte, Vector3 rotacion)
    {
        var parte = ObtenerParte(nombreParte);
        parte?.Rotar(rotacion);
    }

    public void ReflejarParte(string nombreParte, bool reflejarX, bool reflejarY, bool reflejarZ)
    {
        var parte = ObtenerParte(nombreParte);
        parte?.Reflejar(reflejarX, reflejarY, reflejarZ);
    }

    // Obtener matriz de transformación del objeto
    public Matrix4 ObtenerMatrizTransformacion()
    {
        var escala = Matrix4.CreateScale(Escala.X * Reflexion.X, Escala.Y * Reflexion.Y, Escala.Z * Reflexion.Z);
        var rotacionX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X));
        var rotacionY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y));
        var rotacionZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z));
        var traslacion = Matrix4.CreateTranslation(Posicion);

        return escala * rotacionX * rotacionY * rotacionZ * traslacion;
    }

    // Método para dibujar SIN operaciones GL (solo pasa matrices)
    public void Dibujar(Matrix4 matrizPadre = default)
    {
        if (matrizPadre == default)
            matrizPadre = Matrix4.Identity;

        var matrizObjeto = ObtenerMatrizTransformacion() * matrizPadre;
        
        foreach (var parte in Partes)
        {
            parte.Dibujar(matrizObjeto);
        }
    }
}