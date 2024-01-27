using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using XadrezConsole.tabuleiro;
using XadrezConsole.xadrez;

namespace xadrez
{
    internal class PartidaXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        public bool Xeque { get; private set; }

        private HashSet<Peca> _pecas;
        private HashSet<Peca> _capturadas;

        public PartidaXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.White;
            _pecas = new HashSet<Peca>();
            _capturadas = new HashSet<Peca>();
            ColocarPecas();
            Terminada = false;
            Xeque = false;
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQuantidadeMovimentos();
            
            Peca pecaCapturada = Tab.RetirarPeca(destino);

            if (pecaCapturada != null)
            {
                _capturadas.Add(pecaCapturada);
            }

            Tab.ColocarPeca(p, destino);

            return pecaCapturada;
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach(Peca x in _capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in _pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            
            Peca pecacapturada = ExecutaMovimento(origem, destino);
            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecacapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if (EstaEmXeque(Adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque=false;
            }

            if (TesteXequeMate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecacapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQuantidadeMovimentos();

            if (pecacapturada != null)
            {
                Tab.ColocarPeca(pecacapturada, destino);
                _capturadas.Remove(pecacapturada);
            }
            Tab.ColocarPeca(p, origem);
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Cor.White)
            {
                JogadorAtual = Cor.Black;
            }
            else
            {
                JogadorAtual = Cor.White;
            }
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            _pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Tab, Cor.White));
            ColocarNovaPeca('b', 1, new Cavalo(Tab, Cor.White));
            ColocarNovaPeca('c', 1, new Bispo(Tab, Cor.White));
            ColocarNovaPeca('d', 1, new Dama(Tab, Cor.White));
            ColocarNovaPeca('e', 1, new Rei(Tab, Cor.White));
            ColocarNovaPeca('f', 1, new Bispo(Tab, Cor.White));
            ColocarNovaPeca('g', 1, new Cavalo(Tab, Cor.White));
            ColocarNovaPeca('h', 1, new Torre(Tab, Cor.White));
            ColocarNovaPeca('a', 2, new Peao(Tab, Cor.White));
            ColocarNovaPeca('b', 2, new Peao(Tab, Cor.White));
            ColocarNovaPeca('c', 2, new Peao(Tab, Cor.White));
            ColocarNovaPeca('d', 2, new Peao(Tab, Cor.White));
            ColocarNovaPeca('e', 2, new Peao(Tab, Cor.White));
            ColocarNovaPeca('f', 2, new Peao(Tab, Cor.White));
            ColocarNovaPeca('g', 2, new Peao(Tab, Cor.White));
            ColocarNovaPeca('h', 2, new Peao(Tab, Cor.White));

            ColocarNovaPeca('a', 8, new Torre(Tab, Cor.Black));
            ColocarNovaPeca('b', 8, new Cavalo(Tab, Cor.Black));
            ColocarNovaPeca('c', 8, new Bispo(Tab, Cor.Black));
            ColocarNovaPeca('d', 8, new Dama(Tab, Cor.Black));
            ColocarNovaPeca('e', 8, new Rei(Tab, Cor.Black));
            ColocarNovaPeca('f', 8, new Bispo(Tab, Cor.Black));
            ColocarNovaPeca('g', 8, new Cavalo(Tab, Cor.Black));
            ColocarNovaPeca('h', 8, new Torre(Tab, Cor.Black));
            ColocarNovaPeca('a', 7, new Peao(Tab, Cor.Black));
            ColocarNovaPeca('b', 7, new Peao(Tab, Cor.Black));
            ColocarNovaPeca('c', 7, new Peao(Tab, Cor.Black));
            ColocarNovaPeca('d', 7, new Peao(Tab, Cor.Black));
            ColocarNovaPeca('e', 7, new Peao(Tab, Cor.Black));
            ColocarNovaPeca('f', 7, new Peao(Tab, Cor.Black));
            ColocarNovaPeca('g', 7, new Peao(Tab, Cor.Black));
            ColocarNovaPeca('h', 7, new Peao(Tab, Cor.Black));
        }

        public void ValidarPosicaoOrigem(Posicao posicao)
        {
            if (Tab.Peca(posicao) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }

            if (JogadorAtual != Tab.Peca(posicao).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }

            if (!Tab.Peca(posicao).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.Peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private Cor Adversaria(Cor cor)
        {
            if (cor == Cor.White)
            {
                return Cor.Black;
            }
            else
            {
                return Cor.White;
            }

        }

        private Peca Rei (Cor cor)
        {
            foreach(Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca r = Rei(cor);
            if (r == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }

            foreach(Peca x in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[r.Posicao.Linha, r.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }

            foreach(Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for(int j =0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
