using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez
{
    internal class Torre : Peca
    {
        public Torre(Tabuleiro tab, Cor cor) : base(tab, cor)
        { 
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = Tab.peca(pos);
            return p == null || p.Cor != this.Cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.linhas, Tab.colunas];
            Posicao pos = new Posicao(0, 0);

            //acima
            pos.definirValores(this.Posicao.linha - 1, this.Posicao.coluna);
            while (this.Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;

                if (Tab.peca(pos) != null && Tab.peca(pos).Cor != this.Cor) 
                {
                    break;
                }
                pos.linha = pos.linha - 1;
            }

            //abaixo
            pos.definirValores(this.Posicao.linha + 1, this.Posicao.coluna);
            while (this.Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;

                if (Tab.peca(pos) != null && Tab.peca(pos).Cor != this.Cor)
                {
                    break;
                }
                pos.linha = pos.linha + 1;
            }

            //direita
            pos.definirValores(this.Posicao.linha, this.Posicao.coluna + 1);
            while (this.Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;

                if (Tab.peca(pos) != null && Tab.peca(pos).Cor != this.Cor)
                {
                    break;
                }
                pos.coluna = pos.coluna + 1;
            }

            //esquerda
            pos.definirValores(this.Posicao.linha, this.Posicao.coluna - 1);
            while (this.Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;

                if (Tab.peca(pos) != null && Tab.peca(pos).Cor != this.Cor)
                {
                    break;
                }
                pos.coluna = pos.coluna - 1;
            }

            return mat;
        }

        public override string ToString()
        {
            return "T";
        }


    }
}
