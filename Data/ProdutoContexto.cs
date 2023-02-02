using Microsoft.EntityFrameworkCore;

namespace CalazansUdemy.Data
{
    public class ProdutoContexto: DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        public ProdutoContexto(DbContextOptions options)
            : base(options)
        {

        }
    }
}
