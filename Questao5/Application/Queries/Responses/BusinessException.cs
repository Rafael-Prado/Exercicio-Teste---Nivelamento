namespace Questao5.Application.Queries.Responses
{
    public class BusinessException : Exception
    {
        public string Tipo { get; }

        public BusinessException(string message, string tipo) : base(message)
        {
            Tipo = tipo;
        }
    }
}
