using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using WhatsappApi.Models;

namespace WhatsappApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasUsuarioController : ControllerBase
    {

        public const string ACCOUNT_SID = "";
        public const string AUTH_TOKEN = "";
        public const string NUMERO_ORIGEN = "";

        [HttpPost]
        [Route("EnviarCodigo")]
        public async Task<IActionResult> EnviarCodigo(SendCodeModel data)
        {
            try
            {

                var codigo_generado = new Random().Next(1000, 9999);
                String mensaje = String.Format("Your login code for {0} is {1}.", "Api Test Production",
                                                +codigo_generado
                                                + " *(if you didn't request this message please ignore it)*");


                TwilioClient.Init(ACCOUNT_SID, AUTH_TOKEN);


                var message = await MessageResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber("whatsapp:" + NUMERO_ORIGEN),
                    body: mensaje,
                    to: new Twilio.Types.PhoneNumber("whatsapp:" + data.cod_pais + data.num_destino)
                );

                if (message.Status == MessageResource.StatusEnum.Queued)
                {

                    return Ok(new { resultado = true, msg = "El mensaje se enviará." });
                }
                else
                {
                    return Ok(new { resultado = false, msg = "El mensaje no se enviará. (" + message.ErrorMessage + ")" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { resultado = false, msg = "Ocurrió un problema en el proceso de envío. (Ex: " + ex.Message + ")" });
            }
        }

    }
}