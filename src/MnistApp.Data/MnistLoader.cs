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
                string[] dataset = Directory.GetFiles(caminho, "*");
                foreach(string dados in dataset)
                {
                    string pular = Path.GetFileName(dados);
                    if(pular.Equals(".gitkeep")){continue;}
                    
                    byte[] bytes = new byte[4];
                    using(var stream = File.OpenRead(dados))
                    {
                        stream.ReadExactly(bytes,0,4);
                    }        
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
