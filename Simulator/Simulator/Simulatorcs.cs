using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class Simulatorcs
    {
        static GeraAleatorios ger = new GeraAleatorios();
        static Fila fila;
        static List<double> estadoFila = new List<double>();
        static List<Evento> listaEvento = new List<Evento>();
        static List<Double> numerosAleatorios = new List<double>();
        static double tempoTotal = 0; // diferenca entre tempos
        static double tempoDecorrido = 0; // tempo real
        static int perda = 0; // qnts chegadas cairam
        static void Main(string[] args)
        {
            Console.WriteLine("Digite a quantidade de servidores da fila: ");
            int serv = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Digite a capacidade maxima da fila: ");
            int capa = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Digite o tempo de chegada minimo: ");
            int cheMin = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Digite o tempo de chegada maximo: ");
            int cheMax = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Digite o tempo de atendimento min: ");
            int ateMin = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Digite o tempo de atendimento max: ");
            int ateMax = Convert.ToInt32(Console.ReadLine());

            fila = new Fila(serv, capa, cheMin, cheMax, ateMin, ateMax);

            Console.WriteLine(fila.exibir());

            //setup das demais configs
            Console.WriteLine("Digite o estado inicial de chegada: ");
            double estIni = Convert.ToDouble(Console.ReadLine());

            for (int i = 0; i < 100000; i++)
            {
                numerosAleatorios.Add(ger.receiveRandomIn(0, 1));
            }

            for (int i = 0; i < capa + 1; i++)
            {
                estadoFila.Add(0.0);
            }

            filaSimples(fila, estIni);

            for (int i = 0; i <= fila.getCap(); i++)
            {
                Console.WriteLine("Tempo total que " + i + " ficaram na fila: " + estadoFila.ElementAt(i));
            }

            Console.WriteLine("perdas: " + perda);
            Console.WriteLine("tempo total: " + tempoDecorrido);

        }

        private static void filaSimples(Fila fi, double inicio)
        {
            tempoDecorrido = 0;
            chegada(inicio);
            double menorTempo = 0;
            int posMenor = 0;

            // enquanto existirem numeros aleatorios na lista
            while (!numerosAleatorios.Any())
            {
                if (fila.getAtual() == fila.getCap())
                {
                    menorTempo = listaEvento.ElementAt(0).getTempo();
                    posMenor = 0;
                    for (int i = 0; i < listaEvento.Count; i++)
                    {
                        if (listaEvento.ElementAt(posMenor).getTempo() > listaEvento.ElementAt(i).getTempo())
                        {
                            menorTempo = listaEvento.ElementAt(i).getTempo();
                            posMenor = i;
                        }
                    }

                    // se evento não for saida
                    if (listaEvento.ElementAt(posMenor).getTipo() == 1)
                    {
                        perda++;
                        listaEvento.RemoveAt(posMenor);
                        chegada(menorTempo);
                    }
                    else
                    {
                        saida(menorTempo);
                        listaEvento.RemoveAt(posMenor);
                    }
                }
                else
                {
                    //caso fila não cheia
                    menorTempo = listaEvento.ElementAt(0).getTempo();
                    posMenor = 0;
                    for (int i = 0; i < listaEvento.Count; i++)
                    {
                        if (listaEvento.ElementAt(posMenor).getTempo() > listaEvento.ElementAt(i).getTempo())
                        {
                            menorTempo = listaEvento.ElementAt(i).getTempo();
                            posMenor = i;
                        }
                    }

                    //Chegada
                    if (listaEvento.ElementAt(posMenor).getTipo() == 1)
                    {
                        chegada(menorTempo);
                        listaEvento.RemoveAt(posMenor);
                    }
                    //Saida
                    else if (listaEvento.ElementAt(posMenor).getTipo() == 0)
                    {
                        saida(menorTempo);
                        listaEvento.RemoveAt(posMenor);
                    }
                    menorTempo = 0;
                    posMenor = 0;
                }
            }
        }

        private static void chegada(double tempo)
        {
            int posFila = fila.getAtual();
            contabilizaTempo(tempo);

            if (fila.getAtual() < fila.getCap())
            {
                fila.setAtual(posFila + 1);
                if (fila.getAtual() <= fila.getServidores())
                {
                    agendaSaida();
                }
            }
            agendaChegada();
        }

        private static void saida(double tempo)
        {
            contabilizaTempo(tempo);
            int posFila = fila.getAtual();
            fila.setAtual(posFila - 1);
            if(fila.getAtual() >= fila.getServidores())
            {
                agendaSaida();
            }
        }
        private static void agendaChegada()
        {
            double aux = numerosAleatorios.ElementAt(0); //Antes era .RemoveAt(0)
            double result = tempoDecorrido
                            + (((fila.getTempoChegadaMax() - fila.getTempoChegadaMin()) * aux) 
                            + fila.getTempoChegadaMin());
        }
        private static void agendaSaida()
        {
            double aux = numerosAleatorios.ElementAt(0); //Antes era .RemoveAt(0)
            double result = tempoDecorrido + (((fila.getTempoAtendimentoMax()
                                           - fila.getTempoAtendimentoMin()) * aux)
                                           + fila.getTempoAtendimentoMin());
        }

        private static void contabilizaTempo(double tempo)
        {
            int aux = fila.getAtual();

            double tempoAnterior = tempoDecorrido;
            tempoDecorrido = tempo;
            double posTemAux = tempoDecorrido - tempoAnterior;
            double tempoAux = estadoFila.ElementAt(aux) + posTemAux;
            estadoFila.Add(tempoAux); //Era estadoFila.set(aux, tempoAux);
        }
    }
}
