using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContaCorrente.ApiConta.Enums;

namespace ContaCorrente.ApiConta.Models
{
    [Table("Contas")]
    public class Contas
    {
        public Contas(int Id, string Agencia, string Conta, decimal Saldo, DateTime DataCriacao, int UserID){
            this.Id = Id;
            this.Agencia = Agencia;
            this.Conta = Conta;
            this.Saldo = Saldo;
            this.DataCriacao = DataCriacao;
            this.UserID = UserID;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public decimal Saldo { get; set; }
        public DateTime DataCriacao { get; set; }
        public int UserID { get; set; }


        public void AtualizaSaldo(ETipoTransacao enuTipoTransacao, decimal valorTransacao)
        {

            switch ((ETipoTransacao)enuTipoTransacao)
            {
                case ETipoTransacao.Credito:
                    Saldo += valorTransacao;
                    break;
                case ETipoTransacao.Debito:
                    Saldo -= valorTransacao;
                    break;
                default:
                    break;
            }
        }
    }
}