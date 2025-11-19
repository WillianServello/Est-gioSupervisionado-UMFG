using cafeservellocontroler.Models.Pessoa;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace cafeservellocontroler.Helper
{
    public class Sessao : ISessao
    {

        private readonly IHttpContextAccessor _httpContext;


        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public ModelUsuario BuscarSessaoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("UsuarioLogado");

            if(string.IsNullOrEmpty(sessaoUsuario))
                return null;

            return JsonConvert.DeserializeObject<ModelUsuario>(sessaoUsuario);
        }

        public void CriarSessaoUsuario(ModelUsuario modelUsuario)
        {
            string valor = JsonConvert.SerializeObject(modelUsuario);
            _httpContext.HttpContext.Session.SetString("UsuarioLogado", valor);
        }

        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("UsuarioLogado");
        }
    }
}
