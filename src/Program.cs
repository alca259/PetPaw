using PetPaw.Infrastructure;

namespace PetPaw;

class Program
{
    static Point _initialPosition;
    static Random _random = new();
    static System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
    static ApplicationContext _context;

    static void Main(string[] args)
    {
        // Obtiene el tamaño y la posición de la pantalla primaria
        var workArea = Screen.PrimaryScreen.WorkingArea;

        var x = 0;
        var y = workArea.Height - 20;

        _initialPosition = new Point(x, y);

        // Crea la ventana de la mascota
        _context = PetForm.CreateForm(_initialPosition);

        // Agrega un temporizador para mover la ventana cada 2 segundos
        _timer.Interval = 50;
        _timer.Tick += Timer_Tick;
        _timer.Start();

        // Ejecuta la aplicación
        Application.Run(_context);
    }

    static void Timer_Tick(object sender, EventArgs e)
    {
        // Obtiene el tamaño y la posición de la pantalla primaria
        var workArea = Screen.PrimaryScreen.WorkingArea;

        var x = _context.MainForm.Location.X + 1;

        // Si la ventana ha llegado al final de la pantalla, la mueve al inicio de la pantalla
        if (x > workArea.Width - _context.MainForm.Width)
        {
            x = 0;
        }

        // Mueve la ventana a la nueva posición aleatoria
        _context.MainForm.Location = new Point(x, _context.MainForm.Location.Y);

        // Actualiza la posición actual
        _initialPosition = _context.MainForm.Location;
    }
}