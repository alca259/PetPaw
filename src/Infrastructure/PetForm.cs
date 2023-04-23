namespace PetPaw.Infrastructure;

public sealed class PetForm : Form
{
    private readonly Bitmap _buffer;

    private PetForm()
    {
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.White;
        TransparencyKey = Color.White;
        DoubleBuffered = true;

        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PetPaw.Resources.Media.Pet.png");
        var image = new Bitmap(stream);

        var width = image.Width + 25;
        var height = image.Height + 21;


        Padding = new Padding(0);
        FormBorderStyle = FormBorderStyle.None;
        Size = new Size(width, height);
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        Location = Point.Empty;
        TopMost = true;

        _buffer = new Bitmap(width, height);

        // Dibuja la mascota en el buffer
        using (var g = Graphics.FromImage(_buffer))
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, Point.Empty);
        }

        // Registra el método OnPaint para que se llame cuando sea necesario dibujar en pantalla
        Paint += OnPaint;
    }

    void OnPaint(object sender, PaintEventArgs e)
    {
        // Obtiene el contexto de dibujo de la pantalla
        var hdc = WindowsHelper.GetDC(IntPtr.Zero);

        // Selecciona el objeto del buffer en el contexto de dibujo
        var hBitmap = _buffer.GetHbitmap();
        var hOldBitmap = WindowsHelper.SelectObject(hdc, hBitmap);

        // Copia el contenido del buffer al contexto de dibujo
        var dstRect = new Rectangle(Point.Empty, _buffer.Size);
        var srcRect = new Rectangle(Point.Empty, _buffer.Size);
        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImage(_buffer, dstRect, srcRect, GraphicsUnit.Pixel);

        // Limpia el contexto de dibujo
        WindowsHelper.SelectObject(hdc, hOldBitmap);
        WindowsHelper.DeleteObject(hBitmap);
        WindowsHelper.ReleaseDC(IntPtr.Zero, hdc);
    }

    public static ApplicationContext CreateForm(Point initialPosition)
    {
        // Creamos una instancia de la clase ApplicationContext para que se ejecute nuestro programa
        var context = new ApplicationContext();
        var form = new PetForm { Location = initialPosition };
        // A la posicion inicial debemos eliminar el alto del formulario
        var newPosition = new Point(initialPosition.X, initialPosition.Y - form.Height + 21);
        form.Location = newPosition;
        // Agregamos el evento de cierre de la ventana para cerrar la aplicación
        form.FormClosed += (sender, args) => context.ExitThread();
        form.Show();

        context.MainForm = form;
        return context;
    }
}