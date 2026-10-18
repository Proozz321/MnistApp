using MnistApp.Core;
using MnistApp.Data;


string pasta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"MnistApp", "datasets");

var loader = new MnistLoader();
loader.ValidarDataset(pasta);

// 1. Carregar 5 imagens já com etiqueta (o que o Data entrega)
List<MnistImage> imagens = loader.CarregarImagens(
    Path.Combine(pasta, "emnist-balanced-train-images-idx3-ubyte"),
    Path.Combine(pasta, "emnist-balanced-train-labels-idx1-ubyte"),
    5);

// 2. Converter para o formato do ML.NET (o que o Core faz)
List<AmostraEmnist> amostras = ConversorAmostras.DevolverAmostras(imagens);

Console.WriteLine($"\nImagens: {imagens.Count} | Amostras: {amostras.Count}");

// 3. Comparar cada amostra com a imagem de origem
for (int i = 0; i < amostras.Count; i++)
{
    bool etiquetaIgual = amostras[i].Label == imagens[i].Label;
    bool tamanhoCerto = amostras[i].Pixels.Length == MnistImage.TotalPixeis;

    Console.WriteLine(
        $"Amostra {i}: Label {amostras[i].Label}, píxeis {amostras[i].Pixels.Length}, " +
        (etiquetaIgual && tamanhoCerto ? "OK" : "ERRO"));
}

// 4. Cada amostra deve ter o seu array de píxeis (se der True, o array foi partilhado entre imagens)
Console.WriteLine("\nAmostras 0 e 1 partilham o array? " +
    ReferenceEquals(amostras[0].Pixels, amostras[1].Pixels));

// 5. A amostra guarda a referência do MnistImage, sem copiar (decisão tomada)
Console.WriteLine("Amostra 0 usa o array do MnistImage 0? " +
    ReferenceEquals(amostras[0].Pixels, imagens[0].Pixels));
