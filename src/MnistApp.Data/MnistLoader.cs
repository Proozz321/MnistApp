using System;
using System.IO;
using System.Text;
using System.Buffers.Binary;

namespace MnistApp.Data
{
    class MnistLoader
    {
        private static readonly string[] Dataset =
        {
            "emnist-balanced-train-images-idx3-ubyte",
            "emnist-balanced-train-labels-idx1-ubyte",
            "emnist-balanced-test-images-idx3-ubyte",
            "emnist-balanced-test-labels-idx1-ubyte"
        };

        private static BinaryReader AbrirFicheiro(string ficheiro) => new BinaryReader(File.OpenRead(ficheiro));        
        private static int LerInt32(BinaryReader reader) => BinaryPrimitives.ReadInt32BigEndian(reader.ReadBytes(4));

        public void ValidarDataset(string pasta)
        {
            foreach(string nome in Dataset)
            {
                string caminho = Path.Combine(pasta,nome);
                using var reader = AbrirFicheiro(caminho);
                byte[] magic = reader.ReadBytes(4);
               
                if(!new MnistValidator(magic).Comparar())
                {
                    throw new FileNotFoundException("Ficheiro em falta no dataset.", caminho);
                }

                Console.WriteLine("Valido: " + nome);
            }
        }
        
        public byte[][] LerImagens(string ficheiro, int itens)
        {
            using var reader = AbrirFicheiro(ficheiro);

            byte[] magic = reader.ReadBytes(4);
            int total = LerInt32(reader);
            int linhas = LerInt32(reader);
            int colunas = LerInt32(reader);

            int quantidade = Math.Min(total, itens);
            byte[][] imagens = new byte[quantidade][];

            for(int i = 0; i < quantidade; i++)
            {
                byte[] bruta = reader.ReadBytes(linhas * colunas);
                imagens[i] = Transpor(bruta,linhas,colunas);
            }
            
            return imagens;
        }

        public byte[] LerEtiquetas(string ficheiro, int itens)
        {
            using var reader = AbrirFicheiro(ficheiro);
            byte[] magic = reader.ReadBytes(4);

            if(!new MnistValidator(magic).Etiquetas())
            {
                throw new InvalidDataException("ficheiro de Etiquetas Invalido");
            }

            int total = LerInt32(reader);
            return reader.ReadBytes(Math.Min(total,itens));
        }

        private static byte[] Transpor(byte[] bruta, int linhas, int colunas)
        {
            byte[] resultado = new byte[bruta.Length];

            for(int L = 0; L < linhas; L++)
            {
                for(int C = 0; C < colunas; C++)
                {
                    resultado[C * linhas + L] = bruta[L * colunas + C];
                }
            }
            return resultado;
        }
    }
}

