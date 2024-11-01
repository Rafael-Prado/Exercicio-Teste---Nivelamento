using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContaCorrenteRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<ContaCorrente> ObterPorIdAsync(string id)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var query = "SELECT * FROM contacorrente WHERE idcontacorrente = @Id";
            return await connection.QuerySingleOrDefaultAsync<ContaCorrente>(query, new { Id = id });
        }

        public async Task<ContaCorrente> ObterPorNumeroAsync(int numero)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var query = "SELECT * FROM contacorrente WHERE numero = @Numero";
            return await connection.QuerySingleOrDefaultAsync<ContaCorrente>(query, new { Numero = numero });
        }
    }
}
