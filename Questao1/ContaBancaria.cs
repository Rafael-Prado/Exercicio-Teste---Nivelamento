using System;
using System.Globalization;

namespace Questao1
{
  
    public class ContaBancaria {

        private const double taxaSaque = 3.50;
        public ContaBancaria(int numero, string titular, double depositoInicial =  0)
        {
            Numero = numero;
            Titular = titular;
            DepositoInicial = depositoInicial;
        }

        public int Numero { get; set; }
        public string Titular { get; set; }
        public double DepositoInicial { get; set; }


        public void Deposito(double quantia)
        {
            this.DepositoInicial += quantia;
        }

        public void Saque(double quantia)
        {
            this.DepositoInicial -= quantia;
            this.DepositoInicial -= taxaSaque;
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: R$ {DepositoInicial:F2}";
        }

    }
}
