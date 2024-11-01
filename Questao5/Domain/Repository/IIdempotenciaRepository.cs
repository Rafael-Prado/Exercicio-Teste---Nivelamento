namespace Questao5.Domain.Repository
{
    public interface IIdempotenciaRepository
    {
        Task<bool> VerificarIdempotenciaAsync(Guid chaveIdempotencia);
        Task RegistrarIdempotenciaAsync(Guid chaveIdempotencia, string requisicao, string resultado);
    }
}
