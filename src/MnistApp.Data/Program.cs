using System;

namespace MnistApp.Data
{
    class Program
    {
        static void Main(string[] args)
        {
            var loader = new MnistLoader();
            loader.CarregarDataset();
        }
    }
}
