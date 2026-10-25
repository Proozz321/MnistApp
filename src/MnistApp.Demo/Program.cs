using MnistApp.Core;
using MnistApp.Data;
using Microsoft.ML;
using Microsoft.ML.Data;

// O caminho aponta para a pasta datasets na raiz da solução.
// Se o executável estiver a correr a partir de bin/Debug/net10.0, 
// o caminho relativo pode precisar de ser ajustado (ex: ../../../../../datasets)
string pastaDataset = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../../datasets"));

var loader = new MnistLoader();

// Vamos começar com um subset pequeno para testar rápido
const int QtdTreino = 60000;
const int QtdTeste = 1000;

var mlContext = new MLContext();

Console.WriteLine($"A carregar treino de: {pastaDataset}");
var imagensTreino = loader.CarregarImagens(
    Path.Combine(pastaDataset, "emnist-balanced-train-images-idx3-ubyte"),
    Path.Combine(pastaDataset, "emnist-balanced-train-labels-idx1-ubyte"),
    QtdTreino);

Console.WriteLine($"A carregar teste de: {pastaDataset}");
var imagensTeste = loader.CarregarImagens(
    Path.Combine(pastaDataset, "emnist-balanced-test-images-idx3-ubyte"),
    Path.Combine(pastaDataset, "emnist-balanced-test-labels-idx1-ubyte"),
    QtdTeste);

Console.WriteLine($"Treino: {imagensTreino.Count} imagens | Teste: {imagensTeste.Count} imagens");

Console.WriteLine($"Primeiros 10 pixeis da imagem 0: {string.Join(", ", imagensTreino[0].Pixels.Take(10))}");
Console.WriteLine($"Label da imagem 0: {imagensTreino[0].Label}");
Console.WriteLine($"Label da imagem 1: {imagensTreino[1].Label}");
Console.WriteLine($"Label da imagem 2: {imagensTreino[2].Label}");

// Converter para o formato do ML.NET
var amostrasTreino = ConversorAmostras.DevolverAmostras(imagensTreino);
var amostrasTeste = ConversorAmostras.DevolverAmostras(imagensTeste);

Console.WriteLine("=== DIAGNÓSTICO DE DADOS ===");
for (int i = 0; i < 3; i++)
{
    var amostra = amostrasTreino[i];
    Console.WriteLine($"Amostra {i}:");
    Console.WriteLine($"  Label: {amostra.Label}");
    Console.WriteLine($"  Tamanho do array de pixeis: {amostra.Pixels.Length}");
    Console.WriteLine($"  Soma de todos os pixeis: {amostra.Pixels.Sum()}");
    Console.WriteLine($"  Primeiros 5 pixeis: {string.Join(", ", amostra.Pixels.Take(5))}");
}
Console.WriteLine("===========================");

// Treinar
var treinador = new TreinarModelo();
Console.WriteLine("A treinar o modelo (pode demorar uns segundos)...");
var modelo = treinador.Treinar(amostrasTreino);

var motor = mlContext.Model.CreatePredictionEngine<AmostraEmnist, PrevisaoEmnist>(modelo);
var previsao = motor.Predict(amostrasTreino[0]);
Console.WriteLine($"[TESTE] Label real: {amostrasTreino[0].Label} | Previsto: {previsao.PredictedLabel}");

// Avaliar
Console.WriteLine("A avaliar...");
var metricas = treinador.Avaliar(modelo, amostrasTeste);
Console.WriteLine();
Console.WriteLine($"--- RESULTADOS ---");
Console.WriteLine($"MicroAccuracy: {metricas.MicroAccuracy:P2}");
Console.WriteLine($"MacroAccuracy: {metricas.MacroAccuracy:P2}");

// Guardar (vai criar o ficheiro na pasta bin/Debug/net10.0 do Demo)
treinador.Guardar(modelo, "modelo-emnist.zip");
Console.WriteLine("✔ Fim.");
