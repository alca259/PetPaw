namespace PetPaw.Models;

public sealed class Pet
{
    private readonly Bitmap _image;
    private Point _position;

    public Pet(Bitmap image, Point position)
    {
        _image = image;
        _position = position;
    }

    public void Draw(IntPtr handle)
    {
        using Graphics g = Graphics.FromHdc(handle);
        g.DrawImage(_image, _position);
    }

    public void Move(Point position)
    {
        _position = position;
    }
}