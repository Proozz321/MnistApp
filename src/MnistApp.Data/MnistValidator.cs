using System;
using System.IO;
using System.Text;

namespace MnistApp.Data
{
    public class MnistValidator
    {
        private byte[] Bytes {get; set;}
        private const int MagicImagens  = 2051; // 0x00000803
        private const int MagicEtiquetas = 2049; // 0x00000801
        
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
            
            //comparar
            if(MagicLido == MagicImagens || MagicLido == MagicEtiquetas)
            {
                return true;
            }
            return false; //valor incorreto
        }
    }
}
