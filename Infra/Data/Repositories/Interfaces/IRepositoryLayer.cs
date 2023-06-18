namespace LocalStore.Infra.Data.Repositories.Interfaces
{
    public interface IRepositoryLayer
    {
        Task BeginTransaction();
        Task RollBackTransaction();
        Task CommitTransaction();
        UserRepository User { get; }
        EstabelecimentoRepository Estabelecimento { get; }
        ClienteRepository Cliente { get; }
        ProdutoRepository Produto { get; }
        PedidoRepository Pedido { get; }
    }
}
