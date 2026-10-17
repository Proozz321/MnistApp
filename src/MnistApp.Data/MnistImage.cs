namespace MnistApp.Data;

// Recebe dados que represeta uma imagem
public class MnistImage
{
    public const int TotalPixeis = 28 * 28; 
    
    public float[] Pixels {get;}
    public int Label {get;}

    public MnistImage(float[] pixels, int label)
    {
        ArgumentNullException.ThrowIfNull(pixels);

        if(pixels.Length != TotalPixeis)
        {
            throw new ArgumentException("pixeis não corresponde ao total esperado");
        }

        Pixels = pixels;
        Label = label;
    }
}
