using System;

namespace ContaCorrente.ApiConta.Models
{
    public class Transacao
    {
        public int TransacaoId { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Descricao { get; set; }
        public int TipoTransacao { get; set; }

    }
}