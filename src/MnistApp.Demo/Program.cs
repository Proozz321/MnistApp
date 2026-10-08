using MnistApp.Data;

string pasta = "/home/marcus/MnistApp/datasets";
var loader = new MnistLoader();

loader.ValidarDataset(pasta);

byte[][] imagens = loader.LerImagens(Path.Combine(pasta, "emnist-balanced-train-images-idx3-ubyte"), 10);
byte[] etiquetas = loader.LerEtiquetas(Path.Combine(pasta, "emnist-balanced-train-labels-idx1-ubyte"), 10);

// Lê o mapeamento: "etiqueta código_ASCII" por linha
var mapa = new Dictionary<int, char>();
foreach (string linha in File.ReadAllLines(Path.Combine(pasta, "emnist-balanced-mapping.txt")))
{
    string[] partes = linha.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (partes.Length == 2)
    {
        mapa[int.Parse(partes[0])] = (char)int.Parse(partes[1]);
    }
}

for (int i = 0; i < imagens.Length; i++)
{
    Console.WriteLine($"\nImagem {i}: etiqueta {etiquetas[i]} = '{mapa[etiquetas[i]]}'");
    Desenhar(imagens[i]);
}

// Usa tons de cinzento: quanto mais claro o píxel, mais "forte" o símbolo
static void Desenhar(byte[] pixeis)
{
    const string Tons = " .:-=+*#%@";

    for (int linha = 0; linha < 28; linha++)
    {
        for (int coluna = 0; coluna < 28; coluna++)
        {
            byte p = pixeis[linha * 28 + coluna];
            char simbolo = Tons[p * (Tons.Length - 1) / 255];
            Console.Write(simbolo);
            Console.Write(simbolo); // duplica para a imagem não ficar esticada na vertical
        }
        Console.WriteLine();
    }
}

