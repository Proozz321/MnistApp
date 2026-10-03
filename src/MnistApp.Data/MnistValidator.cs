using System;
using System.IO;
using System.Text;

namespace MnistApp.Data
{
    public class MnistValidator
    {
        private byte[] Bytes {get; set;}
        private string caminho = "/home/proozz/MnistApp/src/MnistApp.Data/number.txt";
        
        //recebe bytes 
        public MnistValidator(byte[] bytes)
        {
            this.Bytes = bytes;
        }

        private void Reverse(){ if(BitConverter.IsLittleEndian){  Array.Reverse(Bytes); }}

        public bool Comparar()
        {

            //faz o reverse nos bytes e converte para int32
            Reverse();
            int MagicLido = BitConverter.ToInt32(Bytes,0);

            //confirma se o caminho existe
            if(!File.Exists(caminho))
            {
                throw new FileNotFoundException($"O arquivo number.txt não foi encontrado em: {caminho}");
            }
            
            //lê linha por linha e compara com o numero magico
            foreach(string linha in File.ReadAllLines(caminho))
            {
                //transforma em string em int e cria variavel valorlido
                if(int.TryParse(linha.Trim(), out int ValorLido))
                {
                    //comparar
                    if(MagicLido == ValorLido)
                    {
                        return true;
                    }
                }
            }
            return false; //valor incorreto
        }
    }
}
