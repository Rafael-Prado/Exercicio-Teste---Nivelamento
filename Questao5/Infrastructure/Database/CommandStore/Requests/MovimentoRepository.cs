using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public MovimentoRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<ContaCorrente> GetContaConrrenteId(string idContaCorrente)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            connection.Open();
            var conta = await connection.QueryFirstOrDefaultAsync<ContaCorrente>(
           "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
           new { IdContaCorrente = idContaCorrente });
            connection.Close();
            return conta;
        }

        public async Task<decimal> GetSaldoAtual(string idContaCorrente)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var saldo = await connection.QuerySingleOrDefaultAsync<decimal>(@"
                        SELECT 
                            SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END) 
                        FROM movimento 
                        WHERE idcontacorrente = @IdContaCorrente",
            new { IdContaCorrente = idContaCorrente });

            return saldo;
        }

        public async void InserirMovimento(string idMovimento, MovimentoCommand request)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            await connection.ExecuteAsync(@"
                        INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                        VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
            new
            {
                IdMovimento = idMovimento,
                request.IdContaCorrente,
                request.DataMovimento,
                request.TipoMovimento,
                request.Valor
            });
        }
    }
}
