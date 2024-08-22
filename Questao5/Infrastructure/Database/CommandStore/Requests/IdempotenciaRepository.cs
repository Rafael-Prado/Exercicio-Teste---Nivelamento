using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }
        public async Task<Idempotencia> GetIdempotenciaIdRequisicao(string idRequisicao)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var idempotencia = await connection.QueryFirstOrDefaultAsync<Idempotencia>(
               "SELECT * FROM idempotencia WHERE chave_idempotencia = @IdRequisicao",
               new { IdRequisicao = idRequisicao });
            return idempotencia;
        }

        public async void InserirIdempotencia(string idMovimento, MovimentoCommand request)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            await connection.ExecuteAsync(@"
            INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
            VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)",
                 new
                 {
                     ChaveIdempotencia = request.IdRequisicao,
                     Requisicao = request.ToString(),
                     Resultado = idMovimento
                 });

        }
    }
}
