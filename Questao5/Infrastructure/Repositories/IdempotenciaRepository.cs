using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Repository;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<bool> VerificarIdempotenciaAsync(Guid chaveIdempotencia)
        {
            using var connection = new SqliteConnection(databaseConfig.Name); 
            var query = "SELECT COUNT(1) FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
            var result = await connection.ExecuteScalarAsync<int>(query, new { ChaveIdempotencia = chaveIdempotencia });
            return result > 0;
        }

        public async Task RegistrarIdempotenciaAsync(Guid chaveIdempotencia, string requisicao, string resultado)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var command = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) 
                        VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";

            await connection.ExecuteAsync(command, new
            {
                ChaveIdempotencia = chaveIdempotencia,
                Requisicao = requisicao,
                Resultado = resultado
            });
        }
    }
}
