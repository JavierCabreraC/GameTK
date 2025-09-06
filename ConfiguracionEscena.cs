using System.Text.Json.Serialization;
using OpenTK.Mathematics;

public class ConfiguracionEscena
{
    [JsonPropertyName("colorFondo")]
    public Color4Config ColorFondo { get; set; }
    
    [JsonPropertyName("configuracionCamara")]
    public ConfiguracionCamara Camara { get; set; }
    
    [JsonPropertyName("objetos")]
    public List<ConfiguracionObjeto> Objetos { get; set; }
}

public class ConfiguracionCamara
{
    [JsonPropertyName("rotacionInicialX")]
    public float RotacionInicialX { get; set; }
    
    [JsonPropertyName("rotacionInicialY")]
    public float RotacionInicialY { get; set; }
    
    [JsonPropertyName("distanciaInicial")]
    public float DistanciaInicial { get; set; }
    
    [JsonPropertyName("campoVision")]
    public float CampoVision { get; set; }
    
    [JsonPropertyName("nearPlane")]
    public float NearPlane { get; set; }
    
    [JsonPropertyName("farPlane")]
    public float FarPlane { get; set; }
}

public class ConfiguracionObjeto
{
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }
    
    [JsonPropertyName("tipo")]
    public string Tipo { get; set; }
    
    [JsonPropertyName("centroMasa")] // NUEVO: Centro de masa del objeto
    public Vector3Config CentroMasa { get; set; }
    
    [JsonPropertyName("posicion")]
    public Vector3Config Posicion { get; set; }
    
    [JsonPropertyName("dimensiones")]
    public Vector3Config Dimensiones { get; set; }
    
    [JsonPropertyName("color")]
    public Color4Config Color { get; set; }
    
    [JsonPropertyName("partes")]
    public List<ConfiguracionParte> Partes { get; set; }
}

public class ConfiguracionParte
{
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }
    
    [JsonPropertyName("dimensiones")]
    public Vector3Config Dimensiones { get; set; }
    
    [JsonPropertyName("posicionRelativa")] // Relativa al centro de masa del objeto
    public Vector3Config PosicionRelativa { get; set; }
    
    [JsonPropertyName("color")]
    public Color4Config Color { get; set; }
}

public class Vector3Config
{
    [JsonPropertyName("x")]
    public float X { get; set; }
    
    [JsonPropertyName("y")]
    public float Y { get; set; }
    
    [JsonPropertyName("z")]
    public float Z { get; set; }

    public Vector3 ToVector3() => new Vector3(X, Y, Z);
}

public class Color4Config
{
    [JsonPropertyName("r")]
    public float R { get; set; }
    
    [JsonPropertyName("g")]
    public float G { get; set; }
    
    [JsonPropertyName("b")]
    public float B { get; set; }
    
    [JsonPropertyName("a")]
    public float A { get; set; }

    public Color4 ToColor4() => new Color4(R, G, B, A);
}