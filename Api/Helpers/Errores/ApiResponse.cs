using AspNetCoreRateLimit;

namespace Api.Helpers.Errores
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string SeriousMessage { get; set; }
        public string FunMessage { get; set; }

        public ApiResponse()
        {

        }

        public ApiResponse(int statusCode, string seriousMessage = null, string funMessage = null)
        {
            StatusCode = statusCode;
            SeriousMessage = seriousMessage ?? GetDefaultSeriousMessage(statusCode);
            FunMessage = funMessage ?? GetDefaultFunMessage(statusCode);
        }

        private string GetDefaultSeriousMessage(int statusCode)
        {
            return statusCode switch
            {
                100 => "Continuar",
                101 => "Cambiando protocolos",
                200 => "Correcto",
                201 => "Creado",
                202 => "Aceptado",
                204 => "Sin contenido",
                206 => "Contenido parcial",
                300 => "Múltiples opciones",
                301 => "Movido permanentemente",
                302 => "Encontrado",
                304 => "No modificado",
                400 => "Petición incorrecta",
                401 => "No autorizado",
                403 => "Prohibido",
                404 => "No encontrado",
                405 => "Método no permitido",
                406 => "No aceptable",
                408 => "Tiempo de espera agotado",
                409 => "Conflicto",
                410 => "Gone",
                415 => "Tipo de medio no soportado",
                426 => "Actualización requerida",
                429 => "Demasiadas peticiones",
                500 => "Error interno del servidor",
                501 => "No implementado",
                502 => "Puerta de enlace incorrecta",
                503 => "Servicio no disponible",
                504 => "Tiempo de espera agotado en la puerta de enlace",
                505 => "Versión HTTP no soportada",
                _ => "Código de estado no mapeado"
            };
        }

        private string GetDefaultFunMessage(int statusCode)
        {
            return statusCode switch
            {
                100 => "Espera un momentito, todavía estoy pensando 🤔",
                101 => "Ok, cambiando protocolos. ¡Sujétate!",
                200 => "¡Yay! Todo salió como lo planeamos 🎉",
                201 => "¡Voila! Acabo de crear algo mágico 🎩",
                202 => "Recibido, voy a pensarlo. Dame un segundo.",
                204 => "Hecho. Nada más que decir. Micrófono apagado 🎤",
                206 => "¡Aquí tienes, pero solo una parte! 🙌",
                300 => "Tengo varias opciones, pero no sé cuál escoger 🤷‍♀️",
                301 => "Nos mudamos. Ve al nuevo lugar, por favor 🚚",
                302 => "Mira, he encontrado algo. Pero está en otro lugar 👉",
                304 => "Nada nuevo que ver aquí, sigue adelante 👀",
                400 => "Lo siento, creo que no entendí eso. ¿Puedes reformularlo? 🤔",
                401 => "¡Alto ahí! Necesitas un pase mágico para entrar 🎟️",
                //403 => "Nop, no puedes hacer eso. No insistas 🚫",
                403 => "No tio, No 🚫!!!",
                404 => "Busqué por todas partes, pero no encontré nada 🤷‍♂️",
                405 => "Ese movimiento no está permitido en este juego 🙅‍❌",
                406 => "Lo siento, no puedo entregar lo que estás pidiendo 🙅‍♀️",
                408 => "Se acabó el tiempo. Tardaste demasiado ⏲️",
                409 => "Estamos teniendo un pequeño desacuerdo 🙅‍♂️",
                410 => "Llegaste tarde, ya se ha ido 🏃‍♂️",
                415 => "No puedo trabajar con este formato 🤮",
                426 => "Necesitarás actualizarte para seguir 🆙",
                429 => "Woah, cálmate. Estás pidiendo demasiado rápido ⚠️",
                //500 => "¡Oops! Algo explotó en el servidor 🧨",
                500 => "Tío Javi algo hiciste mal! 🧨",
                501 => "Esa característica aún está en mi lista de tareas pendientes 📝",
                502 => "Mis amigos servidores están dando malas respuestas 🙄",
                503 => "Dame un descanso, estoy un poco saturado 😓",
                504 => "Se agotó el tiempo y no pude terminar 🕰️",
                505 => "No soporto esa versión 🚫",
                _ => "Algo raro pasó y no sé qué es 🤷‍♀️"
            };
        }

        public static implicit operator QuotaExceededResponse(ApiResponse v)
        {
            throw new NotImplementedException();
        }
    }
}
