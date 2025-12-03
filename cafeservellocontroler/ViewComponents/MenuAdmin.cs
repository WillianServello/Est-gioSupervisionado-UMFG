using cafeservellocontroler.Models.Pessoa;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace cafeservellocontroler.ViewComponents
{
    public class MenuAdmin : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessaoUsuario = HttpContext.Session.GetString("UsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario))
                return null;

            ModelUsuario usuario = JsonConvert.DeserializeObject<ModelUsuario>(sessaoUsuario);
            return View("AdminArea", usuario);
        }
    }
}
