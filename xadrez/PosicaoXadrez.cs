using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez
{
    internal class PosicaoXadrez
    {
        public PosicaoXadrez(char coluna, int linha)
        {
            this.coluna = coluna;
            this.linha = linha;
        }

        public char coluna { get; set; }
        public int linha { get; set; }

        public override string ToString()
        {
            return "" + coluna + linha;
        }

        public Posicao toPosicao()
        {
            return new Posicao(8 - linha, coluna - 'a');
        }

    }
}
