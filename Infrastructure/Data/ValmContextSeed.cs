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


            if (!context.AuthUsuarios.Any()) 
            {
                using (var readerAuthUsuarios = new StreamReader(ruta + @"/Data/Csvs/AuthUsuarios.csv"))
                {
                    using (var csvAuthUsuarios = new CsvReader(readerAuthUsuarios, CultureInfo.InvariantCulture)) {

                        var AuthUsuarios = csvAuthUsuarios.GetRecords<AuthUsuario>();
                        context.AuthUsuarios.AddRange(AuthUsuarios);
                        await context.SaveChangesAsync();
                    }
                }            
            }


            if (!context.AuthRoles.Any()) 
            {
                using (var readerAuthRoles = new StreamReader(ruta + @"/Data/Csvs/AuthRoles.csv"))
                {
                    using (var csvAuthRoles = new CsvReader(readerAuthRoles, CultureInfo.InvariantCulture))
                    {
                        var AuthRoles = csvAuthRoles.GetRecords<AuthRol>();
                        context.AuthRoles.AddRange(AuthRoles);
                        await context.SaveChangesAsync();
                    }
                }            
            }

            if (!context.AuthPermisos.Any())
            {
                using (var readerAuthPermisos = new StreamReader(ruta + @"/Data/Csvs/AuthPermisos.csv"))
                {
                    using (var csvAuthPermisos = new CsvReader(readerAuthPermisos, CultureInfo.InvariantCulture))
                    {
                        var AuthPermisos = csvAuthPermisos.GetRecords<AuthPermiso>();
                        context.AuthPermisos.AddRange(AuthPermisos);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.AuthRolesPermisos.Any())
            {
                using (var readerAuthRolesPermisos = new StreamReader(ruta + @"/Data/Csvs/AuthRolesPermisos.csv"))
                {
                    using (var csvAuthRolesPermisos = new CsvReader(readerAuthRolesPermisos, CultureInfo.InvariantCulture))
                    {
                        var AuthRolesPermisosRecords = csvAuthRolesPermisos.GetRecords<AuthRolPermisoDto>(); // Utilizamos el DTO aquí

                        List<AuthRolPermiso> AuthRolesPermisosList = new List<AuthRolPermiso>();
                        foreach (var record in AuthRolesPermisosRecords)
                        {
                            AuthRolesPermisosList.Add(new AuthRolPermiso
                            {
                                RolId = record.RolId,
                                PermisoId = record.PermisoId
                            });
                        }

                        context.AuthRolesPermisos.AddRange(AuthRolesPermisosList);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.AuthUsuariosRoles.Any()) // Verifica si ya hay registros en la tabla AuthUsuarioRol
            {
                using (var readerAuthUsuariosRoles = new StreamReader(ruta + @"/Data/Csvs/AuthUsuariosRoles.csv"))
                {
                    using (var csvAuthUsuariosRoles = new CsvReader(readerAuthUsuariosRoles, CultureInfo.InvariantCulture))
                    {
                        var AuthUsuariosRolesRecords = csvAuthUsuariosRoles.GetRecords<AuthUsuarioRolDto>(); // Utilizamos el DTO aquí

                        List<AuthUsuarioRol> AuthUsuariosRolesList = new List<AuthUsuarioRol>();
                        foreach (var record in AuthUsuariosRolesRecords)
                        {
                            AuthUsuariosRolesList.Add(new AuthUsuarioRol
                            {
                                UsuarioId = record.UsuarioId,
                                RolId = record.RolId
                            });
                        }

                        context.AuthUsuariosRoles.AddRange(AuthUsuariosRolesList); // Agrega todos los registros a la tabla AuthUsuarioRol
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
