using ContaCorrente.ApiConta.Data;
using ContaCorrente.ApiConta.Enums;
using ContaCorrente.ApiConta.Models;
using ContaCorrente.ApiConta.ViewModels;
using ContaCorrente.ApiConta.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ContaCorrente.ApiConta.Controllers
{

    [ApiController]
    public class ContaController : ControllerBase
    {
        [HttpGet("v1/conta")]
        public async Task<IActionResult> Get(
            [FromServices] IMemoryCache cache,
            [FromServices] ContaDataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {

            var contas = cache.GetOrCreate("ContasCache", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return GetContas(context);

            });

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = contas.Count,
                page,
                pageSize,
                contas
            }));

        }

        private List<Contas> GetContas(ContaDataContext context)
        {
            return context.Conta.AsNoTracking().ToList();
        }

        [HttpGet("v1/conta/{agencia}/{conta}")]
        public async Task<IActionResult> Get(
            [FromServices] ContaDataContext context,
            [FromRoute] string agencia,
            [FromRoute] string conta
        )
        {
            try
            {
                var count = await context.Conta.AsNoTracking().CountAsync();
                var contas = await context
                                .Conta
                                .AsNoTracking()
                                .Where(x => x.Agencia == agencia
                                        && x.Conta == conta)
                                .Select(x => new ListContaViewModel
                                {
                                    Agencia = x.Agencia,
                                    Conta = x.Conta,
                                    Saldo = x.Saldo,
                                    DataCriacao = x.DataCriacao
                                })
                                .FirstOrDefaultAsync();

                return Ok(new ResultViewModel<dynamic>(new
                    {
                        Agencia = contas.Agencia,
                        Conta = contas.Conta,
                        Saldo = contas.Saldo,
                        DataCriacao = contas.DataCriacao
                    }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna servidor"));

            }
        }

        [HttpGet("v1/saldo/{agencia}/{conta}")]
        public async Task<IActionResult> GetAgenciaConta(
            [FromServices] ContaDataContext context,
            [FromRoute] string agencia,
            [FromRoute] string conta
        )
        {
            try
            {
                var contas = await context
                                .Conta
                                .AsNoTracking()
                                .Where(x => x.Agencia == agencia
                                        && x.Conta == conta)
                                .Select(x => new ListContaViewModel
                                {
                                    Agencia = x.Agencia,
                                    Conta = x.Conta,
                                    Saldo = x.Saldo,
                                    DataCriacao = x.DataCriacao
                                })
                                .FirstOrDefaultAsync();

                if (contas == null)
                    return NotFound(new ResultViewModel<string>("Conta não encontrado."));

                return Ok(new ResultViewModel<dynamic>(new
                    {
                        Agencia = contas.Agencia,
                        Conta = contas.Conta,
                        Saldo = contas.Saldo,
                        DataCriacao = contas.DataCriacao
                    }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna servidor"));

            }
        }

         [HttpPost("v1/conta/")]
        public async Task<IActionResult> PostAsync(
            [FromBody] CreateContaViewModel contaviewmodel,
            [FromServices] ContaDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

            try
            {
                var QConta = await context
                                .Conta
                                .AsNoTracking()
                                .Where(x => x.Agencia ==contaviewmodel.Agencia
                                        && x.Conta == contaviewmodel.Conta)
                                .CountAsync();
                                
                if(QConta > 0)
                    return BadRequest(new ResultViewModel<string>("Conta informada já existe."));


                var conta = contaviewmodel.ToCreateConta();
                await context.Conta.AddAsync(conta);
                context.SaveChanges();

                return Created($"v1/conta/{conta.Id}", new ResultViewModel<dynamic>(new
                {
                    Agencia = conta.Agencia,
                    Conta = conta.Conta,
                    Saldo = conta.Saldo,
                    DataCriacao = conta.DataCriacao
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Falha ao incluir a conta"));
            }
        }

    }
}