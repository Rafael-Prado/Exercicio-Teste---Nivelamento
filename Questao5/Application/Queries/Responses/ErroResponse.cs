namespace Questao5.Application.Queries.Responses
{
    public class ErroResponse
    {
        public string Mensagem { get; set; }

        public ErroResponse(string mensagem)
        {
            Mensagem = mensagem;
        }
    }
}
