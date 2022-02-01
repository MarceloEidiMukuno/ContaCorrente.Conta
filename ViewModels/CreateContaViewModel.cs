using System;
using System.ComponentModel.DataAnnotations;
using ContaCorrente.ApiConta.Models;

namespace ContaCorrente.ApiConta.ViewModels
{
    public class CreateContaViewModel
    {
        
        [Required(ErrorMessage = "O campo agencia é obrigatorio")]
        public string Agencia { get; set; }
        
        [Required(ErrorMessage = "O campo conta é obrigatorio")]
        public string Conta { get; set; }

        public Contas ToCreateConta() => new(
            0,
            Agencia,
            Conta,
            0,
            DateTime.Now,
            0
        );
    }
}
