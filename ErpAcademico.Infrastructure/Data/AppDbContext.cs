using ErpAcademico.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ErpAcademico.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();

    public DbSet<Venda> Vendas => Set<Venda>();

    public DbSet<ItemVenda> ItensVenda => Set<ItemVenda>();

    public DbSet<Orcamento> Orcamentos { get; set; }

    public DbSet<ItemOrcamento> ItensOrcamento { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<MovimentacaoEstoque>
    MovimentacoesEstoque { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // PRODUTO
        // =========================

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Descricao)
                .HasMaxLength(1000);

            entity.Property(p => p.PrecoVenda)
                .HasPrecision(18, 2);

            entity.Property(p => p.PrecoCusto)
                .HasPrecision(18, 2);
        });

        // =========================
        // VENDA
        // =========================

        modelBuilder.Entity<Venda>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.ValorTotal)
                .HasPrecision(18, 2);

            entity.Property(v => v.Status)
                .HasConversion<int>();
        });

        // =========================
        // ITEM VENDA
        // =========================

        modelBuilder.Entity<ItemVenda>(entity =>
        {
            entity.HasKey(iv => iv.Id);

            entity.Property(iv => iv.PrecoUnitario)
                .HasPrecision(18, 2);

            // Relacionamento Venda 1:N Itens
            entity.HasOne(iv => iv.Venda)
                .WithMany(v => v.Itens)
                .HasForeignKey(iv => iv.VendaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento Produto 1:N Itens
            entity.HasOne(iv => iv.Produto)
                .WithMany()
                .HasForeignKey(iv => iv.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // =======================
            // Orçamento
            //========================

        modelBuilder.Entity<Orcamento>()
            .Property(o => o.ValorTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ItemOrcamento>()
            .Property(i => i.PrecoUnitario)
            .HasPrecision(18, 2);

         modelBuilder.Entity<Orcamento>()
            .HasMany(o => o.Itens)
            .WithOne(i => i.Orcamento)
            .HasForeignKey(i => i.OrcamentoId);

        modelBuilder.Entity<Orcamento>()
            .Property(o => o.Status)
            .HasConversion<int>();

            
            //========================
            // User
            //========================
        modelBuilder.Entity<Usuario>()
           .Property(u => u.Nome)
           .HasMaxLength(100);

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Email)
            .HasMaxLength(150);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();


            //========================
            // Estoque
            //========================

modelBuilder.Entity<MovimentacaoEstoque>()
    .HasOne(m => m.Produto)
    .WithMany(p => p.Movimentacoes)
    .HasForeignKey(m => m.ProdutoId)
    .OnDelete(DeleteBehavior.Restrict);        
    });
    }
}