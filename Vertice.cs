using OpenTK.Mathematics;

public class Vertice
{
    public Vector3 Posicion { get; set; }
    
    public Vertice(float x, float y, float z)
    {
        Posicion = new Vector3(x, y, z);
    }
    
    public Vertice(Vector3 posicion)
    {
        Posicion = posicion;
    }
}