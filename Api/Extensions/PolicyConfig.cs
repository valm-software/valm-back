using Microsoft.AspNetCore.Authorization;

namespace Api.Extensions
{
    public static class PolicyConfig
    {
        public static void AddPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy("Gestionar Usuarios", policy => policy.RequireClaim("policy", "Gestionar Usuarios"));
            options.AddPolicy("Gestionar Roles y Politicas", policy => policy.RequireClaim("policy", "Gestionar Roles y Politicas"));
            options.AddPolicy("Realizar Ventas", policy => policy.RequireClaim("policy", "Realizar Ventas"));
            options.AddPolicy("Visualizar Ventas", policy => policy.RequireClaim("policy", "Visualizar Ventas"));
            options.AddPolicy("Gestionar Inventario", policy => policy.RequireClaim("policy", "Gestionar Inventario"));
            options.AddPolicy("Visualizar Inventario", policy => policy.RequireClaim("policy", "Visualizar Inventario"));
            options.AddPolicy("Realizar Cobros", policy => policy.RequireClaim("policy", "Realizar Cobros"));
            options.AddPolicy("Visualizar Cobros", policy => policy.RequireClaim("policy", "Visualizar Cobros"));
            options.AddPolicy("Realizar Entregas", policy => policy.RequireClaim("policy", "Realizar Entregas"));
            options.AddPolicy("Visualizar Entregas", policy => policy.RequireClaim("policy", "Visualizar Entregas"));
            options.AddPolicy("Registrar Gastos", policy => policy.RequireClaim("policy", "Registrar Gastos"));
            options.AddPolicy("Visualizar Gastos", policy => policy.RequireClaim("policy", "Visualizar Gastos"));
            options.AddPolicy("Generar Reportes Financieros", policy => policy.RequireClaim("policy", "Generar Reportes Financieros"));
            options.AddPolicy("Visualizar Reportes Financieros", policy => policy.RequireClaim("policy", "Visualizar Reportes Financieros"));
            options.AddPolicy("Gestionar Clientes", policy => policy.RequireClaim("policy", "Gestionar Clientes"));
            options.AddPolicy("Visualizar Clientes", policy => policy.RequireClaim("policy", "Visualizar Clientes"));
            options.AddPolicy("Gestionar Soporte", policy => policy.RequireClaim("policy", "Gestionar Soporte"));
            options.AddPolicy("Visualizar Soporte", policy => policy.RequireClaim("policy", "Visualizar Soporte"));
            options.AddPolicy("Gestionar Catalogo", policy => policy.RequireClaim("policy", "Gestionar Catalogo"));
            options.AddPolicy("Visualizar Catalogo", policy => policy.RequireClaim("policy", "Visualizar Catalogo"));
        }
    }
}
