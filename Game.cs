using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Text.Json;

public class Game : GameWindow
{
    // === CONFIGURACIÓN Y ESCENARIO ===
    private Escenario _escenario;
    private ConfiguracionEscena _configuracion;
    
    // === CÁMARA ===
    private float _rotationX;
    private float _rotationY;
    private float _cameraDistance;
    private float _campoVision;
    private float _nearPlane;
    private float _farPlane;
    private Color4 _colorFondo;
    
    // === SELECCIÓN DE OBJETOS ===
    private List<string> _nombresObjetos;
    private int _indiceSeleccion = 0;
    private string _objetoSeleccionado;
    
    // === MODO PARTES ===
    private bool _modoPartes = false;
    private int _parteSeleccionada = 0;
    
    // === CONTROLES DE TRANSFORMACIÓN ===
    private float _incrementoMovimiento = 0.05f;
    private float _incrementoRotacion = 2.0f;
    private float _incrementoEscala = 0.02f;

    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _escenario = new Escenario();
        _nombresObjetos = new List<string>();
        
        CargarConfiguracion("configuracion.json");
        CrearSetupDesdeConfiguracion();
    }

    private void CargarConfiguracion(string rutaArchivo)
    {
        try
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException($"Archivo no encontrado: {rutaArchivo}");

            string json = File.ReadAllText(rutaArchivo);
            _configuracion = JsonSerializer.Deserialize<ConfiguracionEscena>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (_configuracion?.Camara == null || _configuracion?.Objetos == null)
                throw new Exception("Configuración JSON inválida");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar configuración: {ex.Message}");
            throw;
        }

        // Aplicar configuración de cámara
        _rotationX = _configuracion.Camara.RotacionInicialX;
        _rotationY = _configuracion.Camara.RotacionInicialY;
        _cameraDistance = _configuracion.Camara.DistanciaInicial;
        _campoVision = _configuracion.Camara.CampoVision;
        _nearPlane = _configuracion.Camara.NearPlane;
        _farPlane = _configuracion.Camara.FarPlane;
        _colorFondo = _configuracion.ColorFondo?.ToColor4() ?? new Color4(0.65f, 0.5f, 0.35f, 1.0f);
    }

    private void CrearSetupDesdeConfiguracion()
    {
        foreach (var configObjeto in _configuracion.Objetos)
        {
            if (string.IsNullOrEmpty(configObjeto.Nombre))
                continue;

            var objeto = new Objeto(configObjeto.Nombre);

            // Establecer centro de masa
            if (configObjeto.CentroMasa != null)
                objeto.CentroMasa = configObjeto.CentroMasa.ToVector3();

            // Crear partes del objeto
            if (configObjeto.Partes != null && configObjeto.Partes.Count > 0)
            {
                foreach (var configParte in configObjeto.Partes)
                {
                    if (configParte.Dimensiones == null) continue;

                    var posicionRelativa = configParte.PosicionRelativa?.ToVector3() ?? Vector3.Zero;
                    var parte = objeto.CrearParte(configParte.Nombre ?? "sin_nombre", posicionRelativa);
                    var vertices = CrearVerticesCubo(configParte.Dimensiones.ToVector3());
                    var colorParte = configParte.Color?.ToColor4() ?? new Color4(0.5f, 0.5f, 0.5f, 1.0f);

                    foreach (var caraVertices in vertices)
                        parte.AgregarCara(caraVertices, colorParte);
                }
            }
            else if (configObjeto.Dimensiones != null)
            {
                // Objeto simple sin partes
                var parte = objeto.CrearParte("principal", Vector3.Zero);
                var vertices = CrearVerticesCubo(configObjeto.Dimensiones.ToVector3());
                var colorObjeto = configObjeto.Color?.ToColor4() ?? new Color4(0.5f, 0.5f, 0.5f, 1.0f);

                foreach (var caraVertices in vertices)
                    parte.AgregarCara(caraVertices, colorObjeto);
            }

            // Calcular centro de masa si no se especificó
            if (configObjeto.CentroMasa == null)
                objeto.CalcularCentroMasa();

            // Establecer posición del objeto
            if (configObjeto.Posicion != null)
                objeto.Mover(configObjeto.Posicion.ToVector3());

            // CORRECCIÓN: Agregar solo UNA vez al escenario
            _escenario.AgregarObjeto(configObjeto.Nombre, objeto);
            _nombresObjetos.Add(configObjeto.Nombre);
        }

        // Seleccionar primer objeto
        if (_nombresObjetos.Count > 0)
            _objetoSeleccionado = _nombresObjetos[0];
    }

    private List<List<Vertice>> CrearVerticesCubo(Vector3 dim)
    {
        float x = dim.X, y = dim.Y, z = dim.Z;
        return new List<List<Vertice>>
        {
            new List<Vertice> { new(-x,-y, z), new( x,-y, z), new( x, y, z), new(-x, y, z) }, // Frente
            new List<Vertice> { new(-x,-y,-z), new(-x, y,-z), new( x, y,-z), new( x,-y,-z) }, // Atrás
            new List<Vertice> { new(-x, y,-z), new(-x, y, z), new( x, y, z), new( x, y,-z) }, // Arriba
            new List<Vertice> { new(-x,-y,-z), new( x,-y,-z), new( x,-y, z), new(-x,-y, z) }, // Abajo
            new List<Vertice> { new( x,-y,-z), new( x, y,-z), new( x, y, z), new( x,-y, z) }, // Derecha
            new List<Vertice> { new(-x,-y,-z), new(-x,-y, z), new(-x, y, z), new(-x, y,-z) }  // Izquierda
        };
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(_colorFondo);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Back);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.X, Size.Y);

        var projection = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(_campoVision),
            (float)Size.X / Size.Y,
            _nearPlane,
            _farPlane);

        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref projection);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape)) Close();

        // === CÁMARA ===
        if (input.IsKeyDown(Keys.W)) _rotationX += 1f;
        if (input.IsKeyDown(Keys.S)) _rotationX -= 1f;
        if (input.IsKeyDown(Keys.A)) _rotationY += 1f;
        if (input.IsKeyDown(Keys.D)) _rotationY -= 1f;
        if (input.IsKeyDown(Keys.Q)) _cameraDistance -= 0.1f;
        if (input.IsKeyDown(Keys.E)) _cameraDistance += 0.1f;

        // === SELECCIÓN ===
        if (input.IsKeyPressed(Keys.Tab)) CambiarObjetoSeleccionado();
        if (input.IsKeyPressed(Keys.Space)) ResetearObjeto();

        var objeto = _escenario.ObtenerObjeto(_objetoSeleccionado);
        if (objeto == null) return;

        // === TRANSFORMACIONES DE OBJETO ===
        if (!_modoPartes)
        {
            // Movimiento
            if (input.IsKeyDown(Keys.Up)) objeto.Trasladar(new Vector3(0, _incrementoMovimiento, 0));
            if (input.IsKeyDown(Keys.Down)) objeto.Trasladar(new Vector3(0, -_incrementoMovimiento, 0));
            if (input.IsKeyDown(Keys.Left)) objeto.Trasladar(new Vector3(-_incrementoMovimiento, 0, 0));
            if (input.IsKeyDown(Keys.Right)) objeto.Trasladar(new Vector3(_incrementoMovimiento, 0, 0));
            if (input.IsKeyDown(Keys.PageUp)) objeto.Trasladar(new Vector3(0, 0, _incrementoMovimiento));
            if (input.IsKeyDown(Keys.PageDown)) objeto.Trasladar(new Vector3(0, 0, -_incrementoMovimiento));

            // Rotación (Shift)
            if (input.IsKeyDown(Keys.LeftShift) || input.IsKeyDown(Keys.RightShift))
            {
                if (input.IsKeyDown(Keys.Up)) objeto.Rotar(new Vector3(_incrementoRotacion, 0, 0));
                if (input.IsKeyDown(Keys.Down)) objeto.Rotar(new Vector3(-_incrementoRotacion, 0, 0));
                if (input.IsKeyDown(Keys.Left)) objeto.Rotar(new Vector3(0, _incrementoRotacion, 0));
                if (input.IsKeyDown(Keys.Right)) objeto.Rotar(new Vector3(0, -_incrementoRotacion, 0));
            }

            // Escalado (Ctrl)
            if (input.IsKeyDown(Keys.LeftControl) || input.IsKeyDown(Keys.RightControl))
            {
                if (input.IsKeyDown(Keys.Up)) objeto.Escalar(new Vector3(1 + _incrementoEscala, 1 + _incrementoEscala, 1 + _incrementoEscala));
                if (input.IsKeyDown(Keys.Down)) objeto.Escalar(new Vector3(1 - _incrementoEscala, 1 - _incrementoEscala, 1 - _incrementoEscala));
            }

            // Reflexión (Alt)
            if (input.IsKeyDown(Keys.LeftAlt) || input.IsKeyDown(Keys.RightAlt))
            {
                if (input.IsKeyPressed(Keys.Up)) objeto.Reflejar(false, true, false);
                if (input.IsKeyPressed(Keys.Left)) objeto.Reflejar(true, false, false);
                if (input.IsKeyPressed(Keys.PageUp)) objeto.Reflejar(false, false, true);
            }
        }

        // === MODO PARTES ===
        if (input.IsKeyPressed(Keys.P))
        {
            _modoPartes = !_modoPartes;
            _parteSeleccionada = 0;
        }

        if (_modoPartes && objeto.Partes.Count > 0)
        {
            if (input.IsKeyPressed(Keys.N))
                _parteSeleccionada = (_parteSeleccionada + 1) % objeto.Partes.Count;

            var parte = objeto.Partes[_parteSeleccionada];

            if (input.IsKeyDown(Keys.D1)) // Mover
            {
                if (input.IsKeyDown(Keys.Up)) parte.Trasladar(new Vector3(0, _incrementoMovimiento, 0));
                if (input.IsKeyDown(Keys.Down)) parte.Trasladar(new Vector3(0, -_incrementoMovimiento, 0));
                if (input.IsKeyDown(Keys.Left)) parte.Trasladar(new Vector3(-_incrementoMovimiento, 0, 0));
                if (input.IsKeyDown(Keys.Right)) parte.Trasladar(new Vector3(_incrementoMovimiento, 0, 0));
            }
            if (input.IsKeyDown(Keys.D2)) // Rotar
            {
                if (input.IsKeyDown(Keys.Up)) parte.Rotar(new Vector3(_incrementoRotacion, 0, 0));
                if (input.IsKeyDown(Keys.Down)) parte.Rotar(new Vector3(-_incrementoRotacion, 0, 0));
                if (input.IsKeyDown(Keys.Left)) parte.Rotar(new Vector3(0, _incrementoRotacion, 0));
                if (input.IsKeyDown(Keys.Right)) parte.Rotar(new Vector3(0, -_incrementoRotacion, 0));
            }
            if (input.IsKeyDown(Keys.D3)) // Escalar
            {
                if (input.IsKeyDown(Keys.Up)) parte.Escalar(new Vector3(1 + _incrementoEscala, 1 + _incrementoEscala, 1 + _incrementoEscala));
                if (input.IsKeyDown(Keys.Down)) parte.Escalar(new Vector3(1 - _incrementoEscala, 1 - _incrementoEscala, 1 - _incrementoEscala));
            }
        }

        // === ESCENARIO ===
        if (input.IsKeyDown(Keys.R)) _escenario.Rotar(new Vector3(0, _incrementoRotacion, 0));
        if (input.IsKeyDown(Keys.T)) _escenario.Escalar(new Vector3(1 + _incrementoEscala, 1 + _incrementoEscala, 1 + _incrementoEscala));
        if (input.IsKeyDown(Keys.Y)) _escenario.Escalar(new Vector3(1 - _incrementoEscala, 1 - _incrementoEscala, 1 - _incrementoEscala));

        // === AJUSTES ===
        if (input.IsKeyPressed(Keys.F1)) _incrementoMovimiento = Math.Max(0.01f, _incrementoMovimiento - 0.01f);
        if (input.IsKeyPressed(Keys.F2)) _incrementoMovimiento = Math.Min(0.5f, _incrementoMovimiento + 0.01f);
        if (input.IsKeyPressed(Keys.F3)) _incrementoRotacion = Math.Max(0.5f, _incrementoRotacion - 0.5f);
        if (input.IsKeyPressed(Keys.F4)) _incrementoRotacion = Math.Min(10f, _incrementoRotacion + 0.5f);
        if (input.IsKeyPressed(Keys.F5)) _incrementoEscala = Math.Max(0.005f, _incrementoEscala - 0.005f);
        if (input.IsKeyPressed(Keys.F6)) _incrementoEscala = Math.Min(0.1f, _incrementoEscala + 0.005f);

        if (input.IsKeyPressed(Keys.F12)) MostrarAyuda();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var view = Matrix4.LookAt(
            new Vector3(0.0f, 0.5f, _cameraDistance),
            Vector3.Zero,
            Vector3.UnitY);

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref view);
        GL.Rotate(_rotationX, Vector3.UnitX);
        GL.Rotate(_rotationY, Vector3.UnitY);

        _escenario.Dibujar();

        string modoInfo = _modoPartes && _escenario.ObtenerObjeto(_objetoSeleccionado)?.Partes.Count > 0
            ? $" - MODO PARTES: {_escenario.ObtenerObjeto(_objetoSeleccionado).Partes[_parteSeleccionada].Nombre}"
            : "";
        Title = $"Setup de Computadora - {_objetoSeleccionado}{modoInfo}";

        SwapBuffers();
    }

    private void CambiarObjetoSeleccionado()
    {
        if (_nombresObjetos.Count == 0) return;
        _indiceSeleccion = (_indiceSeleccion + 1) % _nombresObjetos.Count;
        _objetoSeleccionado = _nombresObjetos[_indiceSeleccion];
    }

    private void ResetearObjeto()
    {
        var objeto = _escenario.ObtenerObjeto(_objetoSeleccionado);
        if (objeto != null)
        {
            objeto.Posicion = Vector3.Zero;
            objeto.Rotacion = Vector3.Zero;
            objeto.Escala = Vector3.One;
            objeto.Reflexion = Vector3.One;
        }
    }

    private void MostrarAyuda()
    {
        Console.WriteLine("\n=== CONTROLES ===");
        Console.WriteLine("CÁMARA: WASD (rotar), Q/E (zoom)");
        Console.WriteLine("SELECCIÓN: TAB (objeto), P (modo partes), N (siguiente parte), ESPACIO (resetear)");
        Console.WriteLine("OBJETO: Flechas (mover), Shift+Flechas (rotar), Ctrl+Flechas (escalar), Alt+Flechas (reflejar)");
        Console.WriteLine("PARTES: 1-4 + Flechas (mover/rotar/escalar/reflejar parte)");
        Console.WriteLine("ESCENARIO: R (rotar), T/Y (escalar)");
        Console.WriteLine("AJUSTES: F1-F6 (velocidades)\n");
    }
}