using CalazansUdemy.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace CalazansUdemy.Controllers
{
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProdutosController: ControllerBase
    {

        private readonly IProdutoRepository Repositorio;

        public ProdutosController(IProdutoRepository repositorio)
        {
            Repositorio = repositorio;
        }


        [HttpPost]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Criar([FromBody] Produto produto)
        {
            if (string.IsNullOrWhiteSpace(produto.Codigo))
                return BadRequest("Código não informado");

            if(string.IsNullOrWhiteSpace(produto.Descricao))
                return BadRequest("Descrição não informado");

            Repositorio.Inserir(produto);
            return Created(nameof(Criar), produto);
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json", "application/xml")]
        public IActionResult Obter()
        {
            var lista = Repositorio.Obter();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Obter(int id)
        {
            var produto = Repositorio.Obter(id);

            if(produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPut]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult Atualizar([FromBody] Produto produto)
        {
            var prod = Repositorio.Obter(produto.Id);

            if (produto == null) return NotFound();

            if (string.IsNullOrWhiteSpace(produto.Codigo))
                return BadRequest("Código não informado");

            if (string.IsNullOrWhiteSpace(produto.Descricao))
                return BadRequest("Descrição não informado");

            Repositorio.Editar(produto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Apagar(int id)
        {
            var produto = Repositorio.Obter(id);

            if (produto == null) return NotFound();
            Repositorio.Excluir(produto);

            return Ok();
        }

        [HttpGet("{codigo}")]
        [ApiVersion("2.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult ObterPorCodigo(string codigo)
        {
            return Ok("metodo na versao 2");
        }

        [HttpGet("")]
        [ApiVersion("3.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult ObterTodos(string codigo)
        {
            List<string> lista = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                lista.Add($"indica {i}");
            }

            return Ok(string.Join(",", lista));
        }
    }
}
