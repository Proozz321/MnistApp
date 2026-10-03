using System;
using System.IO;
using System.Text;

namespace MnistApp.Data
{
    class MnistLoader
    {
        public void CarregarDataset(){
            
            string caminho = "/home/proozz/MnistApp/datasets/";

            if(Directory.Exists(caminho))
            {
                string[] dataset = Directory.GetFiles(caminho, "");
                foreach(string dados in dataset)
                {
                    string pular = Path.GetFileName(dados);
                    if(pular.Equals(".gitkeep")){continue;}
                    
                    byte[] bytes = new byte[4];
                    using(var stream = File.OpenRead(dados))
                    {
                        stream.ReadExactly(bytes,0,4);
                    }
                    Console.WriteLine($"Ficheiro: {pular}");
                    Console.WriteLine($"Bytes: {bytes[0]:X2} {bytes[1]:X2} {bytes[2]:X2} {bytes[3]:X2}");

                    var val = new MnistValidator(bytes);
                    if(!val.Comparar())
                    {
                        throw new InvalidDataException("Arquivo inválido");
                    }

                    Console.WriteLine("valido");
                }
            }
        }
    }
}
