using MnistApp.Data;

string pasta = "/home/proozz/MnistApp/datasets";
var loader = new MnistLoader();

try
{
    // 1. Confirmar que os 4 ficheiros existem e têm magic number válido
    loader.ValidarDataset(pasta);

    // 2. Carregar 5 imagens já com etiqueta e píxeis normalizados (0 a 1)
    List<MnistImage> lista = loader.CarregarImagens(
        Path.Combine(pasta, "emnist-balanced-train-images-idx3-ubyte"),
        Path.Combine(pasta, "emnist-balanced-train-labels-idx1-ubyte"),
        5);

    // 3. Tabela que converte o número da etiqueta no caractere
    Dictionary<int, char> mapa = CarregarMapa(
        Path.Combine(pasta, "emnist-balanced-mapping.txt"));

    // 4. Mostrar cada imagem no terminal
    for (int i = 0; i < lista.Count; i++)
    {
        int etiqueta = lista[i].Label;
        string caractere = mapa.TryGetValue(etiqueta, out char c) ? c.ToString() : "?";

        Console.WriteLine($"\nImagem {i}: etiqueta {etiqueta} = '{caractere}'");
        Desenhar(lista[i].Pixels);
    }
}
catch (Exception erro) when (erro is IOException || erro is InvalidDataException)
{
    // Ficheiro em falta ou ficheiro que não é IDX
    Console.WriteLine("Erro ao carregar o dataset: " + erro.Message);
}

// Os píxeis são float entre 0 e 1, por isso o limite do '#' é 0.25f
static void Desenhar(float[] pixeis)
{
    const int Lado = 28;

    for (int linha = 0; linha < Lado; linha++)
    {
        for (int coluna = 0; coluna < Lado; coluna++)
        {
            Console.Write(pixeis[linha * Lado + coluna] > 0.25f ? '#' : ' ');
        }
        Console.WriteLine();
    }
}

// Lê o mapeamento: cada linha tem "etiqueta códigoASCII" (ex.: "45 114" = 'r')
static Dictionary<int, char> CarregarMapa(string caminho)
{
    var mapa = new Dictionary<int, char>();

    if (!File.Exists(caminho))
    {
        Console.WriteLine("Aviso: ficheiro de mapeamento não encontrado, os caracteres aparecem como '?'.");
        return mapa;
    }

    foreach (string linha in File.ReadAllLines(caminho))
    {
        string[] partes = linha.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (partes.Length == 2
            && int.TryParse(partes[0], out int etiqueta)
            && int.TryParse(partes[1], out int codigo))
        {
            mapa[etiqueta] = (char)codigo;
        }
    }

    return mapa;
}
