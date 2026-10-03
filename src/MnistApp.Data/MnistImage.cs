namespace MnistApp.Data;

// Recebe dados que represeta uma imagem
public class MnistImage
{
    private const int Tpx = 28 * 28; 
    
    public float[] Pixels {get; set;}
    public int Label {get; set;}

    public MnistImage(int label)
    {
        this.Pixels = new float[Tpx];
        this.Label = label;
    }
}
