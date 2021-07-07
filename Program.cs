using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_GP5
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.Write("---------------------MENU--------------------\n" +
                        "=                                           =\n" +
                        "=               1 -CRIPTOGRAFAR             =\n" +
                        "=                                           =\n" +
                        "=              2 -DESCRIPTOGRAFAR           =\n" +
                        "=                                           =\n" +
                        "=                  3 -SAIR                  =\n" +
                        "=                                           =\n" +
                        "---------------------------------------------\n" +
                        "");
                string Strcaso = Console.ReadLine();
                int caso;
                if (int.TryParse(Strcaso, out caso))
                {
                    switch (caso)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("----------Digite a mensagem em claro------------");
                            string msgClaro = Console.ReadLine().Replace(" ", "");
                            Console.WriteLine("----------Digite a senha de criptografia------------");
                            string senha = Console.ReadLine();
                            Console.WriteLine("----------Digite caracter para letra morta------------");
                            char Letramorta = char.Parse(Console.ReadLine());
                            GetShiftIndexes(senha);
                            Console.Clear();
                            Console.WriteLine("---------MENSAGEM CRIPTOGRAFADA------------");
                            string msgCript = Encipher(msgClaro, senha, Letramorta);
                            Console.WriteLine(msgCript);
                            Console.ReadKey();
                            break;
                        case 2:
                            Console.WriteLine("----------Digite a senha de criptografia------------");
                            senha = Console.ReadLine().Replace(" ", "");
                            Console.WriteLine("----------Digite a mensagem criptografada------------");
                            msgCript = Console.ReadLine().Replace(" ", "");
                            Console.WriteLine(Decipher(msgCript, senha));
                            Console.ReadKey();
                            break;
                        case 3:
                            Environment.Exit(1);
                            break;
                        default:
                            Console.WriteLine("Opcao invalida");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Apenas numeros");
                    Console.ReadKey();
                }
            } while (true);
        }
        private static int[] GetShiftIndexes(string key)
        {
            int keyLength = key.Length;
            int[] indexes = new int[keyLength];
            List<KeyValuePair<int, char>> sortedKey = new List<KeyValuePair<int, char>>();
            int i;

            for (i = 0; i < keyLength; ++i)
                sortedKey.Add(new KeyValuePair<int, char>(i, key[i]));

            sortedKey.Sort(
                delegate (KeyValuePair<int, char> pair1, KeyValuePair<int, char> pair2)
                {
                    return pair1.Value.CompareTo(pair2.Value);
                }
            );

            for (i = 0; i < keyLength; ++i)
                indexes[sortedKey[i].Key] = i;

            return indexes;
        }

        public static string Encipher(string input, string key, char padChar)
        {
            input = (input.Length % key.Length == 0) ? input : input.PadRight(input.Length - (input.Length % key.Length) + key.Length, padChar);
            StringBuilder output = new StringBuilder();
            int totalChars = input.Length;
            int totalColumns = key.Length;
            int totalRows = (int)Math.Ceiling((double)totalChars / totalColumns);
            char[,] rowChars = new char[totalRows, totalColumns];
            char[,] colChars = new char[totalColumns, totalRows];
            char[,] sortedColChars = new char[totalColumns, totalRows];
            int currentRow, currentColumn, i, j;
            int[] shiftIndexes = GetShiftIndexes(key);

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalColumns;
                currentColumn = i % totalColumns;
                rowChars[currentRow, currentColumn] = input[i];
            }

            for (i = 0; i < totalRows; ++i)
                for (j = 0; j < totalColumns; ++j)
                    colChars[j, i] = rowChars[i, j];

            for (i = 0; i < totalColumns; ++i)
                for (j = 0; j < totalRows; ++j)
                    sortedColChars[shiftIndexes[i], j] = colChars[i, j];

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalRows;
                currentColumn = i % totalRows;
                output.Append(sortedColChars[currentRow, currentColumn]);
            }

            return output.ToString();
        }

        public static string Decipher(string input, string key)
        {
            StringBuilder output = new StringBuilder();
            int totalChars = input.Length;
            int totalColumns = (int)Math.Ceiling((double)totalChars / key.Length);
            int totalRows = key.Length;
            char[,] rowChars = new char[totalRows, totalColumns];
            char[,] colChars = new char[totalColumns, totalRows];
            char[,] unsortedColChars = new char[totalColumns, totalRows];
            int currentRow, currentColumn, i, j;
            int[] shiftIndexes = GetShiftIndexes(key);

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalColumns;
                currentColumn = i % totalColumns;
                rowChars[currentRow, currentColumn] = input[i];
            }

            for (i = 0; i < totalRows; ++i)
                for (j = 0; j < totalColumns; ++j)
                    colChars[j, i] = rowChars[i, j];

            for (i = 0; i < totalColumns; ++i)
                for (j = 0; j < totalRows; ++j)
                    unsortedColChars[i, j] = colChars[i, shiftIndexes[j]];

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalRows;
                currentColumn = i % totalRows;
                output.Append(unsortedColChars[currentRow, currentColumn]);
            }

            return output.ToString();
        }
    }
}
