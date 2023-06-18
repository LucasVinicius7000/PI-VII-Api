namespace LocalStore.Application.Requests
{
    public class AddProdutoRequest
    {
        public int ClienteId { get; set; }
        public double QuantidadeUnidadeKgLitro { get; set; }
        public double ValorUnitarioKgL { get; set; }
        public int ProdutoIdOriginal { get; set; }
        public string? Observacao { get; set; }
    }
}
