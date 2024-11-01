using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public MovimentoRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<IEnumerable<Movimento>> ObterPorContaIdAsync(string contaId)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var query = "SELECT * FROM movimento WHERE idcontacorrente = @ContaId";
            return await connection.QueryAsync<Movimento>(query, new { ContaId = contaId });
        }

        public async Task InserirAsync(Movimento movimento)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var command = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                        VALUES (@Id, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            await connection.ExecuteAsync(command, new
            {
                Id = movimento.IdMovimento,
                IdContaCorrente = movimento.IdContaCorrente,
                DataMovimento = movimento.DataMovimento.ToString("dd/MM/yyyy"),
                TipoMovimento = movimento.TipoMovimento,
                Valor = movimento.Valor
            });
        }
    }
}
