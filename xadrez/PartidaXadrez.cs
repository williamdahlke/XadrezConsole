﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using XadrezConsole.tabuleiro;

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
            colocarPecas();
            Terminada = false;
            Xeque = false;
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.retirarPeca(origem);
            p.incrementarQuantidadeMovimentos();
            
            Peca pecaCapturada = Tab.retirarPeca(destino);

            if (pecaCapturada != null)
            {
                _capturadas.Add(pecaCapturada);
            }

            Tab.colocarPeca(p, destino);

            return pecaCapturada;
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
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

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in _pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            
            Peca pecacapturada = executaMovimento(origem, destino);
            if (estaEmXeque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecacapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if (estaEmXeque(adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque=false;
            }

            if (testeXequeMate(adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                mudaJogador();
            }
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecacapturada)
        {
            Peca p = Tab.retirarPeca(destino);
            p.DecrementarQuantidadeMovimentos();

            if (pecacapturada != null)
            {
                Tab.colocarPeca(pecacapturada, destino);
                _capturadas.Remove(pecacapturada);
            }
            Tab.colocarPeca(p, origem);
        }

        private void mudaJogador()
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

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            _pecas.Add(peca);
        }

        private void colocarPecas()
        {
            colocarNovaPeca('c', 1, new Torre(Tab, Cor.White));
            colocarNovaPeca('c', 2, new Torre(Tab, Cor.White));
            colocarNovaPeca('d', 2, new Torre(Tab, Cor.White));
            colocarNovaPeca('e', 2, new Torre(Tab, Cor.White));
            colocarNovaPeca('e', 1, new Torre(Tab, Cor.White));
            colocarNovaPeca('d', 1, new Rei(Tab, Cor.White));

            colocarNovaPeca('c', 7, new Torre(Tab, Cor.Black));
            colocarNovaPeca('c', 8, new Torre(Tab, Cor.Black));
            colocarNovaPeca('d', 7, new Torre(Tab, Cor.Black));
            colocarNovaPeca('e', 7, new Torre(Tab, Cor.Black));
            colocarNovaPeca('e', 8, new Torre(Tab, Cor.Black));
            colocarNovaPeca('d', 8, new Rei(Tab, Cor.Black));
        }

        public void validarPosicaoOrigem(Posicao posicao)
        {
            if (Tab.peca(posicao) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }

            if (JogadorAtual != Tab.peca(posicao).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }

            if (!Tab.peca(posicao).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.peca(origem).podeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private Cor adversaria(Cor cor)
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

        private Peca rei (Cor cor)
        {
            foreach(Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca r = rei(cor);
            if (r == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }

            foreach(Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[r.Posicao.linha, r.Posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }

            foreach(Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < Tab.linhas; i++)
                {
                    for(int j =0; j < Tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            
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
