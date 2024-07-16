using IntegracaoKafka.Services;
using IntegracaoKafka.Services.Util;

namespace IntegracaoKafka.Api
{
    public static class DependencyInjection
    {
        public static WebApplicationBuilder ConfigureDI(this WebApplicationBuilder builder)
        {
            Settings.IS_DESENV = builder.Configuration["Ambiente"] == "2";
            Settings.URL_SERVIDOR_KAFKA = builder.Configuration["UrlServidorKafka"];

            #region Application Services
            builder.Services.AddTransient<MensagemService>();
            #endregion

            return builder;
        }
    }
}
