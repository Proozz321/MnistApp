using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MnistApp.Data
{
    public class MnistValidator
    {
        private byte[] _Bytes;
        private const int MagicImagens  = 2051; // 0x00000803
        private const int MagicEtiquetas = 2049; // 0x00000801
        
        //recebe bytes 
        public MnistValidator(byte[] bytes)
        {
            _Bytes = bytes;
        }

        private int LerMagic(){
            
            byte[] copia = _Bytes.ToArray(); 

            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(copia); 
            }
            return BitConverter.ToInt32(copia,0);
        }

        public bool Comparar()
        {
            //faz o reverse nos bytes e converte para int32
            int MagicLido = LerMagic(); 
            return MagicLido == MagicImagens || MagicLido == MagicEtiquetas;
        }

        public bool Imagens() => LerMagic() == MagicImagens;
        public bool Etiquetas() => LerMagic() == MagicEtiquetas;
    }
}
