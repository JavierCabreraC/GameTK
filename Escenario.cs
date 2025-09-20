using OpenTK.Mathematics;

public class Escenario
{
    public Dictionary<string, Objeto> Objetos { get; set; }
    public Vector3 Posicion { get; set; }
    public Vector3 Rotacion { get; set; }
    public Vector3 Escala { get; set; }
    public Vector3 Reflexion { get; set; } // 1 o -1 para cada eje

    public Escenario()
    {
        Objetos = new Dictionary<string, Objeto>();
        Posicion = Vector3.Zero;
        Rotacion = Vector3.Zero;
        Escala = Vector3.One;
        Reflexion = Vector3.One; // Sin reflexión por defecto
    }

    // Métodos de gestión de objetos
    public void AgregarObjeto(string nombre, Objeto objeto)
    {
        if (!Objetos.ContainsKey(nombre))
        {
            Objetos[nombre] = objeto;
        }
    }

    public Objeto ObtenerObjeto(string nombre)
    {
        return Objetos.ContainsKey(nombre) ? Objetos[nombre] : null;
    }

    public void EliminarObjeto(string nombre)
    {
        if (Objetos.ContainsKey(nombre))
        {
            Objetos.Remove(nombre);
        }
    }

    public void LimpiarEscenario()
    {
        Objetos.Clear();
    }

    // Métodos de transformación para todo el escenario
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

    // Obtener matriz de transformación del escenario
    public Matrix4 ObtenerMatrizTransformacion()
    {
        var escala = Matrix4.CreateScale(Escala.X * Reflexion.X, Escala.Y * Reflexion.Y, Escala.Z * Reflexion.Z);
        var rotacionX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X));
        var rotacionY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y));
        var rotacionZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z));
        var traslacion = Matrix4.CreateTranslation(Posicion);

        return escala * rotacionX * rotacionY * rotacionZ * traslacion;
    }

    // Aplicar transformaciones a objetos específicos
    public void TrasladarObjeto(string nombreObjeto, Vector3 desplazamiento)
    {
        var objeto = ObtenerObjeto(nombreObjeto);
        objeto?.Trasladar(desplazamiento);
    }

    public void EscalarObjeto(string nombreObjeto, Vector3 factor)
    {
        var objeto = ObtenerObjeto(nombreObjeto);
        objeto?.Escalar(factor);
    }

    public void RotarObjeto(string nombreObjeto, Vector3 rotacion)
    {
        var objeto = ObtenerObjeto(nombreObjeto);
        objeto?.Rotar(rotacion);
    }

    public void ReflejarObjeto(string nombreObjeto, bool reflejarX, bool reflejarY, bool reflejarZ)
    {
        var objeto = ObtenerObjeto(nombreObjeto);
        objeto?.Reflejar(reflejarX, reflejarY, reflejarZ);
    }

    // Método para dibujar todo el escenario
    public void Dibujar()
    {
        var matrizEscenario = ObtenerMatrizTransformacion();
        
        foreach (var kvp in Objetos)
        {
            kvp.Value.Dibujar(matrizEscenario);
        }
    }
}