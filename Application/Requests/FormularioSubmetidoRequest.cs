using LocalStore.Domain.DTO;
using LocalStore.Domain.Enum;

namespace LocalStore.Application.Requests
{
    public class FormularioSubmetidoRequest
    {
        public Coordinates Coordenadas { get; set; }
        public string NomeArquivo { get; set; }
        public string ExtensaoArquivo { get; set; }
        public string ConteudoArquivo { get; set; }
        public MetodoCompra MetodoCompra { get; set; }
        public string FormasPagamento { get; set; }
        public string NomeProprietario { get; set; }
        public double ValorPorKmRodado { get; set; }
        public double TaxaMinima { get; set; }
        public string CPFProprietario { get; set; }
        public int EstabelecimentoId { get; set; }

    }
}
