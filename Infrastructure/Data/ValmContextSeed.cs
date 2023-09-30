using Core.Entities;
using CsvHelper;
using Infrastructure.Data.Dtos;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;

namespace Infrastructure.Data;
public class ValmContextSeed
{
    public static async Task SeedAsync(ValmContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            var ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


            if (!context.Auth_Usuarios.Any()) 
            {
                using (var readerAuth_Usuarios = new StreamReader(ruta + @"/Data/Csvs/Auth_Usuarios.csv"))
                {
                    using (var csvAuth_Usuarios = new CsvReader(readerAuth_Usuarios, CultureInfo.InvariantCulture)) {

                        var Auth_Usuarios = csvAuth_Usuarios.GetRecords<Auth_Usuario>();
                        context.Auth_Usuarios.AddRange(Auth_Usuarios);
                        await context.SaveChangesAsync();
                    }
                }            
            }


            if (!context.Auth_Roles.Any()) 
            {
                using (var readerAuth_Roles = new StreamReader(ruta + @"/Data/Csvs/Auth_Roles.csv"))
                {
                    using (var csvAuth_Roles = new CsvReader(readerAuth_Roles, CultureInfo.InvariantCulture))
                    {
                        var Auth_Roles = csvAuth_Roles.GetRecords<Auth_Rol>();
                        context.Auth_Roles.AddRange(Auth_Roles);
                        await context.SaveChangesAsync();
                    }
                }            
            }

            if (!context.Auth_Permisos.Any())
            {
                using (var readerAuth_Permisos = new StreamReader(ruta + @"/Data/Csvs/Auth_Permisos.csv"))
                {
                    using (var csvAuth_Permisos = new CsvReader(readerAuth_Permisos, CultureInfo.InvariantCulture))
                    {
                        var Auth_Permisos = csvAuth_Permisos.GetRecords<Auth_Permiso>();
                        context.Auth_Permisos.AddRange(Auth_Permisos);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Auth_RolesPermisos.Any())
            {
                using (var readerAuth_RolesPermisos = new StreamReader(ruta + @"/Data/Csvs/Auth_RolesPermisos.csv"))
                {
                    using (var csvAuth_RolesPermisos = new CsvReader(readerAuth_RolesPermisos, CultureInfo.InvariantCulture))
                    {
                        var Auth_RolesPermisosRecords = csvAuth_RolesPermisos.GetRecords<DtoAuth_RolPermiso>(); // Utilizamos el DTO aquí

                        List<Auth_RolPermiso> Auth_RolesPermisosList = new List<Auth_RolPermiso>();
                        foreach (var record in Auth_RolesPermisosRecords)
                        {
                            Auth_RolesPermisosList.Add(new Auth_RolPermiso
                            {
                                RolId = record.RolId,
                                PermisoId = record.PermisoId
                            });
                        }

                        context.Auth_RolesPermisos.AddRange(Auth_RolesPermisosList);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Auth_UsuariosRoles.Any()) // Verifica si ya hay registros en la tabla Auth_UsuarioRol
            {
                using (var readerAuth_UsuariosRoles = new StreamReader(ruta + @"/Data/Csvs/Auth_UsuariosRoles.csv"))
                {
                    using (var csvAuth_UsuariosRoles = new CsvReader(readerAuth_UsuariosRoles, CultureInfo.InvariantCulture))
                    {
                        var Auth_UsuariosRolesRecords = csvAuth_UsuariosRoles.GetRecords<DtoAuth_UsuarioRol>(); // Utilizamos el DTO aquí

                        List<Auth_UsuarioRol> Auth_UsuariosRolesList = new List<Auth_UsuarioRol>();
                        foreach (var record in Auth_UsuariosRolesRecords)
                        {
                            Auth_UsuariosRolesList.Add(new Auth_UsuarioRol
                            {
                                UsuarioId = record.UsuarioId,
                                RolId = record.RolId
                            });
                        }

                        context.Auth_UsuariosRoles.AddRange(Auth_UsuariosRolesList); // Agrega todos los registros a la tabla Auth_UsuarioRol
                        await context.SaveChangesAsync(); // Guarda los cambios en la base de datos
                    }
                }
            }





            if (!context.Marcas.Any())
            {
                using (var readerMarcas = new StreamReader(ruta + @"/Data/Csvs/marcas.csv"))
                {
                    using (var csvMarcas = new CsvReader(readerMarcas, CultureInfo.InvariantCulture))
                    {
                        var marcas = csvMarcas.GetRecords<Marca>();
                        context.Marcas.AddRange(marcas);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Categorias.Any())
            {
                using (var readerCategorias = new StreamReader(ruta + @"/Data/Csvs/categorias.csv"))
                {
                    using (var csvCategorias = new CsvReader(readerCategorias, CultureInfo.InvariantCulture))
                    {
                        var categorias = csvCategorias.GetRecords<Categoria>();
                        context.Categorias.AddRange(categorias);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Productos.Any())
            {
                using (var readerProductos = new StreamReader(ruta + @"/Data/Csvs/productos.csv"))
                {
                    using (var csvProductos = new CsvReader(readerProductos, CultureInfo.InvariantCulture))
                    {
                        var listadoProductosCsv = csvProductos.GetRecords<Producto>();

                        List<Producto> productos = new List<Producto>();
                        foreach (var item in listadoProductosCsv)
                        {
                            productos.Add(new Producto
                            {
                                Id = item.Id,
                                Nombre = item.Nombre,
                                Precio = item.Precio,
                                FechaCreacion = item.FechaCreacion,
                                CategoriaId = item.CategoriaId,
                                MarcaId = item.MarcaId                        
                            });
                        }

                        context.Productos.AddRange(productos);
                        await context.SaveChangesAsync();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ValmContextSeed>();
            logger.LogError(ex.Message);
        }
    }
}
