using System;

namespace ContaCorrente.ApiConta.ViewModels
{
    public class ListContaViewModel
    {
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public decimal Saldo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
