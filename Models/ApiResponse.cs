namespace WebApi_Coris.Models
{
    public class ApiResponse<T>
    {
        public T? Dados { get; set; }
        public string Mensagem { get; set; } = String.Empty;
        public bool Sucesso { get; set; } = true;

        public static ApiResponse<T> Success(T data, string mensagem = "")
        => new ApiResponse<T>
        {
            Sucesso = true,
            Mensagem = mensagem,
            Dados = data
        };

        public static ApiResponse<T> Fail(string mensagem)
            => new ApiResponse<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Dados = default!
            };
    }


}
