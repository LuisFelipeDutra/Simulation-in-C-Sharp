using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class Evento
    {
        private int tipo;
        private double tempo;

        public Evento(int tipo, double tempo) : base()
        {
            this.tipo = tipo;
            this.tempo = tempo;
        }

        public int getTipo()
        {
            return tipo;
        }

        public void setTipo(int tipo)
        {
            this.tipo = tipo;
        }

        public double getTempo()
        {
            return tempo;
        }

        public void setTempo(double tempo)
        {
            this.tempo = tempo;
        }
    }
}
