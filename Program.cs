using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using xadrez;
using XadrezConsole.tabuleiro;

namespace XadrezConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Tabuleiro tab = new Tabuleiro(8, 8);
                //tab.colocarPeca(new Torre(tab, Cor.Black), new Posicao(0, 0));
                //tab.colocarPeca(new Torre(tab, Cor.Black), new Posicao(1, 3));
                //tab.colocarPeca(new Rei(tab, Cor.Black), new Posicao(2, 4));

                //Tela.imprimirTabuleiro(tab);

                PosicaoXadrez posicao = new PosicaoXadrez('a', 1);
                Console.WriteLine(posicao);
                Console.WriteLine(posicao.toPosicao());
            }
            catch(TabuleiroException e) 
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
