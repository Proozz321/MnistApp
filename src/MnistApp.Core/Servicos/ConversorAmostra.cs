using MnistApp.Data;

namespace MnistApp.Core
{

    public static class ConversorAmostras
    {
        public static List<AmostraEmnist> DevolverAmostras(List<MnistImage> imagens)
        {
            List<AmostraEmnist> resultado = new List<AmostraEmnist>();

            foreach(MnistImage imagem in imagens)
            {
                AmostraEmnist amostra = new AmostraEmnist
                {
                    Pixels = imagem.Pixels,
                    Label = imagem.Label
                };
                resultado.Add(amostra);
            }
            return resultado;
        }
    }
}
